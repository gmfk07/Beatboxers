using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GlobalStats
{
    public enum Shape { CIRCLE, SQUARE, TRIANGLE, DIAMOND };

    public static Sprite circleBeat = Resources.Load<Sprite>("Circle");
    public static Sprite squareBeat = Resources.Load<Sprite>("Square");
    public static Sprite diamondBeat = Resources.Load<Sprite>("Diamond");
    public static Sprite triangleBeat = Resources.Load<Sprite>("Triangle");
}
