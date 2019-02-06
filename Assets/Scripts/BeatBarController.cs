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

    private int beatIndex;
    private float timer;
    private float[] beatTimes = new float[] { 1.176f, 1.764f, .588f*4, .588f*5, .588f*6 };
    private Transform Mark;
    private Transform SpawnPoint;
    private Transform DespawnPoint;
    private List<GameObject> beats = new List<GameObject>();
    private List<GameObject> beatsToRemove = new List<GameObject>();

    void Start()
    {
        Mark = transform.Find("Mark");
        SpawnPoint = transform.Find("SpawnPoint");
        DespawnPoint = transform.Find("DespawnPoint");
        beatIndex = 0;
    }

    void Update()
    {
        timer = Time.time;
        manaCounter.text = manaCount + " Mana";
        if (beatIndex < beatTimes.Length && timer >= beatTimes[beatIndex] - beatWaitTime)
        {
            GameObject obj = Instantiate(Beat, SpawnPoint);
            beats.Add(obj);
            obj.GetComponent<Beat>().distancePerSecond = (SpawnPoint.transform.position.x - Mark.transform.position.x)/beatWaitTime;
            beatIndex++;
        }

        //Did the player press space and miss?
        bool punish = false;
        if (Input.GetKeyDown(KeyCode.Space))
            punish = true;
        foreach (GameObject beat in beats)
        {
            if (beat.transform.position.x <= DespawnPoint.position.x)
            {
                beatsToRemove.Add(beat);
                Destroy(beat);
            }
            if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(beat.transform.position.x - Mark.transform.position.x) <= beatMargin)
            {
                punish = false;
                beatsToRemove.Add(beat);
                manaCount = Mathf.Min(manaCount + 1, manaMax);
                Destroy(beat);
            }
        }
        //Now remove the beats in a separate enumeration
        foreach (GameObject beat in beatsToRemove)
            beats.Remove(beat);

        if (punish)
            manaCount = Mathf.Max(manaCount - 1, 0);
       
        beatsToRemove.Clear();
    }
}
