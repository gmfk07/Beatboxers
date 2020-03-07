using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour
{
    public bool TalkedTo;

    //What should be done when the E button is pressed near the NPC?
    public abstract void HandleButtonPress();
}
