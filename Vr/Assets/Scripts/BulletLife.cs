using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLife : MonoBehaviour
{
    [HideInInspector] public float lifeLeft = 0.0f;

    // Update is called once per frame
    void Update()
    {
        lifeLeft--;
    }
}
