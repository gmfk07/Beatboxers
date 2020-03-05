using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour
{
    public bool TalkedTo;

    public abstract void HandleButtonPress();
}
