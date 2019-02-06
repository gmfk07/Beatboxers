using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour
{
    public float distancePerSecond = 0;

    void Update()
    {
        float movement = distancePerSecond * Time.deltaTime;
        gameObject.transform.position += new Vector3(-movement, 0);
    }
}
