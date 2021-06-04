using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHit : MonoBehaviour
{
    bool isTargetActive = true;
    float maxDuration;
    float currentDuration;

    public float minMoveLength = -2.0f;
    public float maxMoveLength = 2.0f;
    public float moveIncrement = 1.0f;

    AudioSource hitSound;

    Score score;

    public bool targetMove = false;

    void Start()
    {
        score = GameObject.Find("Score Text").GetComponent<Score>();

        hitSound = GetComponent<AudioSource>();

        moveIncrement = Random.Range(-moveIncrement, moveIncrement);

        maxMoveLength += transform.parent.position.x;
        minMoveLength += transform.parent.position.x;
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

        if (targetMove)
        {
            Vector3 targetPosition = transform.parent.position;
            targetPosition = new Vector3(targetPosition.x + moveIncrement, targetPosition.y, targetPosition.z);

            if (targetPosition.x > maxMoveLength)
                moveIncrement = -moveIncrement;
            else if (targetPosition.x < minMoveLength)
                moveIncrement = -moveIncrement;

            transform.parent.position = targetPosition;
        }
    }
}
