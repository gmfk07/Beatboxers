using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatBarController : MonoBehaviour
{
    private const int SECONDS_PER_MINUTE = 60;

    public float beatWaitTime; //How long (in secs) between a beat appearing on the screen and the beat reaching the marker
    public float beatMargin; //How many seconds before or after a beat you can act and still be considered on beat
    public float gracePeriod;
    public float loopTimeBegin = 60f * 2f + 21.682f;
    public float loopTimeEnd = 151.119f;
    public GameObject Beat;
    public int manaCount = 0;
    public int manaMax = 10;
    public int safeBeats = 3;
    public Text manaCounter;
    public EnemyController target;

    private int beatIndex;
    private float startTimer;
    private int safeBeatsLeft;

    private float bpm = 102;
    private float songLength = 197;
    private List<float> beatTimes = new List<float>();
    private string beatShapes = "tdccdtsdt";
    private List<float> beatPotentials = new List<float>();
    private bool canLoopBeats;

    private Transform Mark;
    private Transform SpawnPoint;
    private Transform DespawnPoint;
    private List<GameObject> beats = new List<GameObject>();
    private List<GameObject> beatsToRemove = new List<GameObject>();

    private float previousSongTime;

    void Start()
    {
        InitializeBeatmap();

        safeBeatsLeft = safeBeats;
        previousSongTime = MusicMaster.Instance.GetPlaybackTime();

        Mark = transform.Find("Mark");
        SpawnPoint = transform.Find("SpawnPoint");
        DespawnPoint = transform.Find("DespawnPoint");
        beatIndex = GetBeatIndex(true, GetTimer());

        float timer = GetTimer();

        previousSongTime = timer;
        canLoopBeats = timer < loopTimeEnd - beatWaitTime;
    }

    //Initializes beatTimes based on bpm, initialize beatShapes randomly, initialize beatPotentials as a repeated pattern.
    private void InitializeBeatmap()
    {
        float secondsPerBeat = SECONDS_PER_MINUTE / bpm;
        float beatsInSong = songLength / secondsPerBeat;

        char[] potentialShapeChars = new char[] { 't', 'd', 's', 'c' };
        float[] beatPotentialsLoop = new float[] { 0, 0, 0, 1f, .25f };

        for (int i = 1; i < beatsInSong; i++)
        {
            beatTimes.Add(secondsPerBeat * i);
            beatShapes += potentialShapeChars[UnityEngine.Random.Range(0, potentialShapeChars.Length)];
            beatPotentials.Add(beatPotentialsLoop[(i - 1) % beatPotentialsLoop.Length]);
        }
    }

    //Returns what the current beat index should be, using songTime as the current time in the song to calculate beatTimer and (optionally)
    //grace period length to only include beats that should reasonably be spawned.
    private int GetBeatIndex(bool useGracePeriod, float songTime)
    {
        int result = 0;

        foreach (float beatTime in beatTimes)
        {
            if (useGracePeriod)
            {
                if (songTime + gracePeriod + beatWaitTime > beatTime)
                {
                    result++;
                }
            }
            else
            {
                if (songTime + beatWaitTime > beatTime)
                {
                    result++;
                }
            }
        }
        return result;
    }

    void Update()
    {
        manaCounter.text = manaCount + " Mana\nHealth: " + PlayerStats.health;

        TryBeatSpawn();
        bool punish = TryBeatDespawn();

        //Remove the beats in a separate enumeration
        foreach (GameObject beat in beatsToRemove)
        {
            beats.Remove(beat);
        }

        if (punish)
        {
            manaCount = Mathf.Max(manaCount - 1, 0);
        }

        HandleLooping();

        beatsToRemove.Clear();

        previousSongTime = GetTimer();
    }
    
    //Handles beat despawning if applicable, returning true if the player should be punished for a missed beat and false otherwise.
    private bool TryBeatDespawn()
    {
        bool punish = false;
        bool buttonJustPressed = CheckButtonPressed();
        //Did the player press a key and miss?
        if (buttonJustPressed)
        {
            punish = true;
        }
        foreach (GameObject beat in beats)
        {
            if (beat.transform.position.x <= DespawnPoint.position.x)
            {
                beatsToRemove.Add(beat);
                Destroy(beat);
            }
            if (buttonJustPressed && Mathf.Abs(beat.transform.position.x - Mark.transform.position.x) <= beatMargin)
            {
                Beat beatComponent = beat.GetComponent<Beat>();

                //This beat is in range and something was pressed
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    punish = false;
                    manaCount = Mathf.Min(manaCount + 1, manaMax);
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                    punish = !ResolveAttack(ref manaCount, PlayerStats.upAttack, beatComponent);
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                    punish = !ResolveAttack(ref manaCount, PlayerStats.downAttack, beatComponent);
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    punish = !ResolveAttack(ref manaCount, PlayerStats.leftAttack, beatComponent);
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                    punish = !ResolveAttack(ref manaCount, PlayerStats.rightAttack, beatComponent);
                else if (Input.GetKeyDown(KeyCode.W))
                    punish = !ResolveDefense(ref manaCount, PlayerStats.upDefense, beatComponent);
                else if (Input.GetKeyDown(KeyCode.S))
                    punish = !ResolveDefense(ref manaCount, PlayerStats.downDefense, beatComponent);
                else if (Input.GetKeyDown(KeyCode.A))
                    punish = !ResolveDefense(ref manaCount, PlayerStats.leftDefense, beatComponent);
                else if (Input.GetKeyDown(KeyCode.D))
                    punish = !ResolveDefense(ref manaCount, PlayerStats.rightDefense, beatComponent);

                if (!punish)
                {
                    beatsToRemove.Add(beat);
                    Destroy(beat);
                }
            }
        }

        return punish;
    }

    //Checks to see if currentBeat should be updated due to an impending loop, and also resets looking for a loop after looping.
    private void HandleLooping()
    {
        float timer = GetTimer();

        if (canLoopBeats && timer >= loopTimeEnd - beatWaitTime)
        {
            beatIndex = GetBeatIndex(false, loopTimeBegin - (loopTimeEnd - timer));
            canLoopBeats = false;
        }

        //If we've looped, we can reset the loop clock!
        if (timer < previousSongTime)
        {
            canLoopBeats = true;
        }
    }

    //Checks if a beat should be spawned, and if so, spawns it with the appropriate characteristics.
    private void TryBeatSpawn()
    {
        float timer = GetTimer();
        bool beatBackInTime = beatTimes[beatIndex] < timer;
        bool beatShouldSpawnNormal = timer >= beatTimes[beatIndex] - beatWaitTime;
        bool beatShouldSpawnLoop = loopTimeBegin - (loopTimeEnd - timer) >= beatTimes[beatIndex] - beatWaitTime;
        if (beatIndex < beatTimes.Count && ((!beatBackInTime && beatShouldSpawnNormal) || (beatBackInTime && beatShouldSpawnLoop)))
        {
            GameObject obj = Instantiate(Beat, SpawnPoint);
            Beat bt = obj.GetComponent<Beat>();
            SetBeatShape(bt);

            beats.Add(obj);
            bt.DistancePerSecond = (SpawnPoint.transform.position.x - Mark.transform.position.x) / beatWaitTime;
            if (safeBeatsLeft > 0)
            {
                safeBeatsLeft--;
                bt.IsAttackBeat = false;
            }
            else
            {
                if (beatPotentials[beatIndex] >= target.AttackMinimum)
                {
                    float danger = beatPotentials[beatIndex];

                    bt.IsAttackBeat = true;
                    bt.Attack = target.GetAttack(danger);
                    bt.Redness = target.GetAttack(danger).Redness;
                }
                else
                {
                    bt.IsAttackBeat = false;
                }
            }

            if (safeBeatsLeft > 0)
            {
                safeBeatsLeft--;
            }

            beatIndex++;
        }
    }

    //Set the given beat's shape based on the given char in beatShapes.
    private void SetBeatShape(Beat beat)
    {
        switch (beatShapes[beatIndex])
        {
            case 's':
                beat.Shape = Shape.Square;
                break;
            case 't':
                beat.Shape = Shape.Triangle;
                break;
            case 'd':
                beat.Shape = Shape.Diamond;
                break;
            case 'c':
                beat.Shape = Shape.Circle;
                break;
        }
    }

    //Returns true if an attack or defense or mana gain was input, false otherwise.
    private bool CheckButtonPressed()
    {
        return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow)
                    || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) ||
                    Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D);
    }

    //Returns false if manaAmount is less than the provided amount's cost or there's a shape mismatch, otherwise reduces manaAmount by the cost, attacks, and returns true
    bool ResolveAttack(ref int manaAmount, Attack attack, Beat beat)
    {
        if (manaAmount < attack.manaCost)
        {
            return false;
        }
        if (attack.Shape != Shape.Any && attack.Shape != beat.Shape)
        {
            return false;
        }
        manaAmount -= attack.manaCost;
        target.Hit(attack);
        AttackAnimationController.Instance.PlayPlayerAttackAnimation(attack.itemName);
        return true;
    }

    //Returns false if manaAmount is less than the provided amount's cost or there's a shape mismatch, otherwise reduces manaAmount by the cost, defends, and returns true
    bool ResolveDefense(ref int manaAmount, Defense defense, Beat beat)
    {
        if (manaAmount < defense.manaCost)
        {
            return false;
        }
        if (defense.Shape != Shape.Any && defense.Shape != beat.Shape)
        {
            return false;
        }
        manaAmount -= defense.manaCost;
        PlayerStats.Defend(defense);
        return true;
    }

    //Returns the amount of seconds that have passed since the battle started
    float GetTimer()
    {
        return MusicMaster.Instance.GetPlaybackTime();
    }
}
