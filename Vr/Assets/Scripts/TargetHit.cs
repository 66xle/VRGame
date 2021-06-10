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

    [Range(0.05f, 0.5f)]
    public float moveSpeed = 0.05f;

    PlayerGun script;

    AudioSource hitSound;
    Score score;

    public bool targetMove = false;

    void Start()
    {
        score = GameObject.Find("Score Text").GetComponent<Score>();
        script = GameObject.Find("Gun").GetComponent<PlayerGun>();

        hitSound = GetComponent<AudioSource>();

        // Random direction
        if (Random.value <= 0.5f)
            moveSpeed = -moveSpeed;

        // Get max/min based on targets position
        maxMoveLength += transform.parent.position.x;
        minMoveLength += transform.parent.position.x;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            // If target is hit disable target
            GameObject go = transform.parent.gameObject;
            go.GetComponentsInChildren<MeshRenderer>()[0].enabled = false;
            go.GetComponentsInChildren<MeshRenderer>()[1].enabled = false;

            // Disable collider
            go.GetComponentsInChildren<BoxCollider>()[0].enabled = false;

            isTargetActive = false;

            // Duration being disabled
            currentDuration = 0.0f;
            maxDuration = Time.deltaTime + 2.0f;

            // Score
            score.AddScore();

            // Sound
            hitSound.Play();

            // Remove bullet
            script.RemoveBulletFromList(other.gameObject);
        }
    }

    void Update()
    {
        if (!isTargetActive && currentDuration > maxDuration)
        {
            // Enable target after set amount of time
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

        if (targetMove && isTargetActive)
        {
            // Get target position
            Vector3 targetPosition = transform.parent.position;
            targetPosition = new Vector3(targetPosition.x + moveSpeed, targetPosition.y, targetPosition.z);

            // If target reaches end of length go opposite direction
            if (targetPosition.x > maxMoveLength)
                moveSpeed = -moveSpeed;
            else if (targetPosition.x < minMoveLength)
                moveSpeed = -moveSpeed;

            // Set position
            transform.parent.position = targetPosition;
        }
    }
}
