using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatBarController : MonoBehaviour
{
    private const int SECONDS_PER_MINUTE = 60;

    public float beatWaitTime;
    public float beatMargin;
    public float gracePeriod;
    public GameObject Beat;
    public int manaCount = 0;
    public int manaMax = 10;
    public int safeBeats = 3;
    public Text manaCounter;
    public EnemyController target;

    private int beatIndex;
    private float startTimer;
    private float timer;
    private int safeBeatsLeft;

    private float bpm = 102;
    private float songLength = 197;
    private List<float> beatTimes = new List<float>();
    private string beatShapes = "tdccdtsdt";
    private List<float> beatPotentials = new List<float>();

    private Transform Mark;
    private Transform SpawnPoint;
    private Transform DespawnPoint;
    private List<GameObject> beats = new List<GameObject>();
    private List<GameObject> beatsToRemove = new List<GameObject>();

    void Start()
    {
        InitializeBeatmap();
        safeBeatsLeft = safeBeats;

        Mark = transform.Find("Mark");
        SpawnPoint = transform.Find("SpawnPoint");
        DespawnPoint = transform.Find("DespawnPoint");
        beatIndex = GetInitialBeatIndex();
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

    //Returns what the initial beat index should be, using the current time in the song and grace period length to only include beats
    //that should reasonably be spawned.
    private int GetInitialBeatIndex()
    {
        int result = 0;
        float songTime = GetTimer();
        foreach (float beatTime in beatTimes)
        {
            if (songTime + gracePeriod + beatMargin > beatTime)
                result++;
        }
        return result;
    }

    void Update()
    {
        timer = GetTimer();
        manaCounter.text = manaCount + " Mana\nHealth: " + PlayerStats.health;

        BeatSpawn();
        bool punish = BeatDespawn();

        //Remove the beats in a separate enumeration
        foreach (GameObject beat in beatsToRemove)
            beats.Remove(beat);

        if (punish)
            manaCount = Mathf.Max(manaCount - 1, 0);

        beatsToRemove.Clear();
    }
    
    //Handles beat despawning if applicable, returning true if the player should be punished for a missed beat and false otherwise.
    private bool BeatDespawn()
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
                //This beat is in range and something was pressed
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    punish = false;
                    manaCount = Mathf.Min(manaCount + 1, manaMax);
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                    punish = !ResolveAttack(ref manaCount, PlayerStats.upAttack);
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                    punish = !ResolveAttack(ref manaCount, PlayerStats.downAttack);
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    punish = !ResolveAttack(ref manaCount, PlayerStats.leftAttack);
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                    punish = !ResolveAttack(ref manaCount, PlayerStats.rightAttack);
                else if (Input.GetKeyDown(KeyCode.W))
                    punish = !ResolveDefense(ref manaCount, PlayerStats.upDefense);
                else if (Input.GetKeyDown(KeyCode.S))
                    punish = !ResolveDefense(ref manaCount, PlayerStats.downDefense);
                else if (Input.GetKeyDown(KeyCode.A))
                    punish = !ResolveDefense(ref manaCount, PlayerStats.leftDefense);
                else if (Input.GetKeyDown(KeyCode.D))
                    punish = !ResolveDefense(ref manaCount, PlayerStats.rightDefense);

                if (!punish)
                {
                    beatsToRemove.Add(beat);
                    Destroy(beat);
                }
            }
        }

        return punish;
    }

    //Checks if a beat should be spawned, and if so, spawns it with the appropriate characteristics.
    private void BeatSpawn()
    {
        if (beatIndex < beatTimes.Count && timer >= beatTimes[beatIndex] - beatWaitTime)
        {
            GameObject obj = Instantiate(Beat, SpawnPoint);
            Beat bt = obj.GetComponent<Beat>();
            SetBeatShape(bt);

            beats.Add(obj);
            bt.distancePerSecond = (SpawnPoint.transform.position.x - Mark.transform.position.x) / beatWaitTime;
            if (safeBeatsLeft > 0)
            {
                safeBeatsLeft--;
                bt.isAttackBeat = false;
            }
            else
            {
                if (beatPotentials[beatIndex] >= target.AttackMinimum)
                {
                    bt.isAttackBeat = true;
                    bt.attack = target.GetAttack(beatPotentials[beatIndex]);
                }
                else
                {
                    bt.isAttackBeat = false;
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
                beat.shape = GlobalStats.Shape.SQUARE;
                break;
            case 't':
                beat.shape = GlobalStats.Shape.TRIANGLE;
                break;
            case 'd':
                beat.shape = GlobalStats.Shape.DIAMOND;
                break;
            case 'c':
                beat.shape = GlobalStats.Shape.CIRCLE;
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

    //Returns false if manaAmount is less than the provided amount's cost, otherwise reduces manaAmount by the cost, attacks, and returns true
    bool ResolveAttack(ref int manaAmount, Attack attack)
    {
        if (manaAmount < attack.manaCost)
            return false;
        manaAmount -= attack.manaCost;
        target.Hit(attack);
        AttackAnimationController.Instance.PlayPlayerAttackAnimation(attack.itemName);
        return true;
    }

    //Returns false if manaAmount is less than the provided amount's cost, otherwise reduces manaAmount by the cost, defends, and returns true
    bool ResolveDefense(ref int manaAmount, Defense defense)
    {
        if (manaAmount < defense.manaCost)
            return false;
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
