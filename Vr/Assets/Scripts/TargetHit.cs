using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHit : MonoBehaviour
{
    bool isTargetActive = true;
    float maxDuration;
    float currentDuration;

    AudioSource hitSound;

    Score score;

    void Start()
    {
        score = GameObject.Find("Score Text").GetComponent<Score>();

        hitSound = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            GameObject go = transform.parent.gameObject;
            go.GetComponentsInChildren<MeshRenderer>()[0].enabled = false;
            go.GetComponentsInChildren<MeshRenderer>()[1].enabled = false;


            go.GetComponentsInChildren<BoxCollider>()[0].enabled = false;

            isTargetActive = false;

            currentDuration = 0.0f;
            maxDuration = Time.deltaTime + 2.0f;

            score.AddScore();

            hitSound.Play();
        }
    }

    void Update()
    {
        if (!isTargetActive && currentDuration > maxDuration)
        {
            GameObject go = transform.parent.gameObject;
            go.GetComponentsInChildren<MeshRenderer>()[0].enabled = true;
            go.GetComponentsInChildren<MeshRenderer>()[1].enabled = true;

            go.GetComponentsInChildren<BoxCollider>()[0].enabled = true;

            isTargetActive = true;
        }
        else
        {
            currentDuration += Time.deltaTime;
        }
    }
}
