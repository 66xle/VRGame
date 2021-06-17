using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHit : MonoBehaviour
{
    [Header("Target Moving Setting")]
    public float minMoveLength = -2.0f;
    public float maxMoveLength = 2.0f;
    [Range(0.01f, 0.05f)] public float minMoveSpeed = 0.03f;
    [Range(0.01f, 0.05f)] public float maxMoveSpeed = 0.05f;

    [Header("Sounds")]
    public AudioSource hitSound;
    public AudioSource respawnSound;
    public AudioSource gameStartSound;

    [Header("Other")]
    public float respawnTime = 2.0f;

    #region Internal Variables

    // Target Moving
    [HideInInspector] public bool targetMove = false;
    float moveSpeed = 0;

    // Other
    [HideInInspector] public Vector3 originalPosition;

    // Respawn Time
    bool isTargetActive = true;

    // Game Start Delay
    [HideInInspector] public bool startGameDelay = false;

    // Classes
    PlayerGun script;
    GameManager gameManager;
    Score score;

    #endregion

    void Start()
    {
        // Set Values
        originalPosition = transform.position;
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);

        // Get References
        score = GameObject.Find("Score Text").GetComponent<Score>();
        script = GameObject.Find("Gun").GetComponent<PlayerGun>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        #region Target stuff

        // Random target direction
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

        #endregion
    }

    void Update()
    {
        #region Moving Target

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

        #endregion
    }

    public void DisableTarget()
    {
        // If target is hit disable target
        GameObject go = transform.gameObject;
        go.GetComponent<MeshRenderer>().enabled = false;

        // Disable collider
        go.GetComponent<BoxCollider>().enabled = false;

        isTargetActive = false;
    }

    public void EnableTarget()
    {
        // Enable target after set amount of time
        GameObject go = transform.gameObject;
        go.GetComponent<MeshRenderer>().enabled = true;
        go.GetComponent<BoxCollider>().enabled = true;

        isTargetActive = true;
    }

    void OnTriggerEnter(Collider other)
    {
        // Player shoots starting round target
        if (!gameManager.isRoundActive)
        {
            StartCoroutine(GameStart(other));
        }
        else if (other.tag == "Bullet")
        {
            StartCoroutine(RespawnTarget(other));
        }
    }

    IEnumerator RespawnTarget(Collider other)
    {
        DisableTarget();

        // Score
        score.AddScore();

        // Sound
        hitSound.Play();

        // Remove bullet
        script.RemoveBulletFromList(other.gameObject);


        // Delay
        yield return new WaitForSeconds(respawnTime);


        if (!isTargetActive && gameManager.isRoundActive)
        {
            EnableTarget();

            // Play sound
            respawnSound.Play();
        }
    }

    IEnumerator GameStart(Collider other)
    {
        startGameDelay = true;

        gameStartSound.Play();

        DisableTarget();

        // Remove bullet
        script.RemoveBulletFromList(other.gameObject);

        // Reset Score
        score.currentScore = 0;

        // Disable UI
        gameManager.shootTargetText.SetActive(false);

        
        // Delay
        yield return new WaitForSeconds(3f);


        startGameDelay = false;

        //start round
        gameManager.isRoundActive = true;
        gameManager.minutes = gameManager.roundTimerMinutes;
        gameManager.seconds = gameManager.roundTimerSeconds;

        foreach (GameObject target in gameManager.targetArray)
        {
            target.GetComponent<TargetHit>().EnableTarget();
        }
    }
}
