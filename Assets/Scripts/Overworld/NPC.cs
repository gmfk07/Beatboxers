using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public List<string> Dialog = new List<string>();
    public Item ItemToGive;
    public bool HasRepeatDialog;
    public List<string> RepeatDialog = new List<string>();
}
