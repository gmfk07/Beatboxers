using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMaster : Singleton<MusicMaster>
{
    private FMOD.Studio.EventInstance musicEvent;

    void Start()
    {
        musicEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Jump");
        musicEvent.start();
        DontDestroyOnLoad(this.gameObject);
    }

    //Returns the amount of seconds into the song the soundtrack currently is.
    public float GetPlaybackTime()
    {
        int timeMs = 0;
        musicEvent.getTimelinePosition(out timeMs);
        return timeMs / 1000.0f;
    }
}
