using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMaster : Singleton<MusicMaster>
{
    private float SecondsPerBeat;
    public float LoopTimeBegin { get; private set; }
    public float LoopTimeEnd { get; private set; }
    public bool Initialized = false;
    private FMOD.Studio.EventInstance musicEvent;

    void Start()
    {
        if (!Initialized)
        {
            Initialize();
        }

        DontDestroyOnLoad(gameObject);
    }

    //Initializes the object's variables
    public void Initialize()
    {
        SecondsPerBeat = 60f / 110f;
        LoopTimeBegin = 36 * SecondsPerBeat;
        LoopTimeEnd = 71 * SecondsPerBeat;

        musicEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Argula");
        musicEvent.start();
        Initialized = true;
    }

    //Returns the amount of seconds into the song the soundtrack currently is.
    public float GetPlaybackTime()
    {
        int timeMs = 0;
        musicEvent.getTimelinePosition(out timeMs);
        return timeMs / 1000.0f;
    }

    //Returns the amount of seconds per beat, initializing if uninitialized.
    public float GetSecondsPerBeat()
    {
        if (!Initialized)
        {
            Initialize();
        }
        return SecondsPerBeat;
    }

    //Play the sound for opening and closing a menu.
    public void PlayMenuSound()
    {
        FMOD.Studio.EventInstance audioEvent = FMODUnity.RuntimeManager.CreateInstance("event:/SoundEffects/Menu");
        audioEvent.start();
    }

    //Play the sound for successfully hitting a beat.
    public void PlayBeatHitSound()
    {
        FMOD.Studio.EventInstance audioEvent = FMODUnity.RuntimeManager.CreateInstance("event:/SoundEffects/Beat");
        audioEvent.start();
    }

    //Play the sound for missing a beat.
    public void PlayBeatMissSound()
    {
        FMOD.Studio.EventInstance audioEvent = FMODUnity.RuntimeManager.CreateInstance("event:/SoundEffects/BeatMiss");
        audioEvent.start();
    }

    //Play the sound for blocking.
    public void PlayBlockSound()
    {
        FMOD.Studio.EventInstance audioEvent = FMODUnity.RuntimeManager.CreateInstance("event:/SoundEffects/Block");
        audioEvent.start();
    }

    //Play the sound for being hurt.
    public void PlayHurtSound()
    {
        FMOD.Studio.EventInstance audioEvent = FMODUnity.RuntimeManager.CreateInstance("event:/SoundEffects/Hurt");
        audioEvent.start();
    }
}
