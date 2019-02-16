using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatBarController : MonoBehaviour
{
    public float beatWaitTime;
    public float beatMargin;
    public GameObject Beat;
    public int manaCount = 0;
    public int manaMax = 10;
    public Text manaCounter;
    public Enemy target;

    //Just here for testing, please remove later
    public Attack testUpAttack;
    public Defense testUpDefense;

    private int beatIndex;
    private float timer;
    private float[] beatTimes = new float[] { 1.176f, 1.764f, .588f * 4, .588f * 5, .588f * 6, .588f * 7, .588f * 8, .588f * 9, .588f * 10, .588f * 11 };
    private float[] beatPotential = new float[] {0, 0, 0, 1, .5f, 0, 0, 0, 1, .5f};
    private Transform Mark;
    private Transform SpawnPoint;
    private Transform DespawnPoint;
    private List<GameObject> beats = new List<GameObject>();
    private List<GameObject> beatsToRemove = new List<GameObject>();

    void Start()
    {
        PlayerStats.upAttack = testUpAttack;
        PlayerStats.upDefense = testUpDefense;
        Mark = transform.Find("Mark");
        SpawnPoint = transform.Find("SpawnPoint");
        DespawnPoint = transform.Find("DespawnPoint");
        beatIndex = 0;
    }

    void Update()
    {
        timer = Time.time;
        manaCounter.text = manaCount + " Mana\nHealth: " + PlayerStats.health;
        if (beatIndex < beatTimes.Length && timer >= beatTimes[beatIndex] - beatWaitTime)
        {
            GameObject obj = Instantiate(Beat, SpawnPoint);
            Beat bt = obj.GetComponent<Beat>();

            beats.Add(obj);
            bt.distancePerSecond = (SpawnPoint.transform.position.x - Mark.transform.position.x)/beatWaitTime;

            if (beatPotential[beatIndex] >= target.attackMinimum)
            {
                bt.isAttackBeat = true;
                bt.attack = target.GetAttack(beatPotential[beatIndex]);
            } else
                bt.isAttackBeat = false;

            beatIndex++;
        }

        bool punish = false;
        bool buttonJustPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow)
            || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D);
        //Did the player press a key and miss?
        if (buttonJustPressed)
            punish = true;
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
        //Now remove the beats in a separate enumeration
        foreach (GameObject beat in beatsToRemove)
            beats.Remove(beat);

        if (punish)
            manaCount = Mathf.Max(manaCount - 1, 0);
       
        beatsToRemove.Clear();
    }

    //Returns false if manaAmount is less than the provided amount's cost, otherwise reduces manaAmount by the cost, attacks, and returns true
    bool ResolveAttack(ref int manaAmount, Attack attack)
    {
        if (manaAmount < attack.manaCost)
            return false;
        manaAmount -= attack.manaCost;
        target.Hit(attack);
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
}
