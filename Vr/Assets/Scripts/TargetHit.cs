using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHit : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Destroy(transform.gameObject);
        }
    }
}
