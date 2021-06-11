using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHit : MonoBehaviour
{
    [Header("Target Moving Setting")]
    public float minMoveLength = -2.0f;
    public float maxMoveLength = 2.0f;
    [HideInInspector] public bool targetMove = false;
    [Range(0.01f, 0.05f)] public float minMoveSpeed = 0.03f;
    [Range(0.01f, 0.05f)] public float maxMoveSpeed = 0.05f;

    float moveSpeed = 0;

    [Header("Sounds")]
    public AudioSource hitSound;
    public AudioSource respawnSound;
    public AudioSource gameStart;

    [Header("Other")]
    public float respawnTime = 2.0f;

    // Classes
    PlayerGun script;
    GameManager gameManager;
    Score score;

    [HideInInspector]
    public Vector3 originalPosition;


    // Respawn Time
    float maxDuration = 100.0f;
    float currentDuration;
    bool isTargetActive = true;

    // Game Start Delay
    float maxGameStartDuration = 100.0f;
    float currentGameStartDuration;
    bool startGame = false;

    void Start()
    {
        originalPosition = transform.position;

        score = GameObject.Find("Score Text").GetComponent<Score>();
        script = GameObject.Find("Gun").GetComponent<PlayerGun>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);

        // Random direction
        if (Random.value <= 0.5f)
            moveSpeed = -moveSpeed;

        // Get max/min based on targets position
        maxMoveLength += transform.position.x;
        minMoveLength += transform.position.x;

        // If target is not round starter and game is not active
        if (gameManager.roundStarter != transform.gameObject && !gameManager.isRoundActive)
        {
            // Disable Target
            DisableTarget();
        }
    }

    public void DisableTarget()
    {
        // If target is hit disable target
        GameObject go = transform.gameObject;
        go.GetComponent<MeshRenderer>().enabled = false;

        // Disable collider
        go.GetComponents<BoxCollider>()[0].enabled = false;
        go.GetComponents<BoxCollider>()[1].enabled = false;

        isTargetActive = false;
    }

    public void EnableTarget()
    {
        // Enable target after set amount of time
        GameObject go = transform.gameObject;
        go.GetComponent<MeshRenderer>().enabled = true;

        go.GetComponents<BoxCollider>()[0].enabled = true;
        go.GetComponents<BoxCollider>()[1].enabled = true;

        isTargetActive = true;
    }

    void OnTriggerEnter(Collider other)
    {
        // Handles starting rounds (peter)
        if (!gameManager.isRoundActive)
        {
            startGame = true;
            gameStart.Play();

            DisableTarget();

            // Delay spawn targets
            currentGameStartDuration = 0.0f;
            maxGameStartDuration = Time.deltaTime + 3.0f;

            // Remove bullet
            script.RemoveBulletFromList(other.gameObject);

            // Reset Score
            score.currentScore = 0;
        }
        else if (other.tag == "Bullet")
        {
            DisableTarget();

            // Duration being disabled
            currentDuration = 0.0f;
            maxDuration = Time.deltaTime + respawnTime;

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
        if (startGame && currentGameStartDuration > maxGameStartDuration)
        {
            startGame = false;

            //start round
            gameManager.isRoundActive = true;
            gameManager.minutes = gameManager.roundTimerMinutes;
            gameManager.seconds = gameManager.roundTimerSeconds;

            foreach (GameObject target in gameManager.targetArray)
            {
                target.GetComponent<TargetHit>().EnableTarget();
            }
        }
        else if (startGame)
        {
            currentGameStartDuration += Time.deltaTime;
        }

        if (!isTargetActive && gameManager.isRoundActive && currentDuration > maxDuration)
        {
            EnableTarget();

            // Play sound
            respawnSound.Play();
        }
        else
        {
            currentDuration += Time.deltaTime;
        }

        if (targetMove && isTargetActive)
        {
            // Get target position
            Vector3 targetPosition = transform.position;
            targetPosition = new Vector3(targetPosition.x + moveSpeed, targetPosition.y, targetPosition.z);

            // If target reaches end of length go opposite direction
            if (targetPosition.x > maxMoveLength)
                moveSpeed = -moveSpeed;
            else if (targetPosition.x < minMoveLength)
                moveSpeed = -moveSpeed;

            // Set position
            transform.position = targetPosition;
        }
    }
}
