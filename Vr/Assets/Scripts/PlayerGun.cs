using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public Transform cameraRig;
    public GameObject bullet;
    public GameManager gameManager;

    [Header("Gun Values")]
    public float bulletSpeed = 5.0f;
    public float bulletLife = 100.0f;
    public float smoothTime = 0.3f;
    public float aimDistance = 10.0f;
    public float aimYOffset = -0.05f;
    public float aimXOffset = 0.0f;
    public float shootDelay = 2.0f;

    #region Internal variables

    bool canShoot = true;

    bool isAimTrigged = false;
    bool allowKMDebug;

    LineRenderer lr;
    Vector3 velocity = Vector3.zero;

    // Sound
    AudioSource gunSound;

    List<GameObject> bulletsFired;
    GameObject inputInstruction;
    Vector3 controllerPos;
    #endregion

    void Start()
    {
        // Check if game is running in unity
        allowKMDebug = Application.isEditor; 

        // Set line distance to 0
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, new Vector3(0, 0, 0));
        lr.SetPosition(1, new Vector3(0, 0, 0));

        // Get references
        inputInstruction = GameObject.Find("Input Instruction");
        gunSound = GetComponent<AudioSource>();

        bulletsFired = new List<GameObject>();
    }

    void Update()
    {
        SetVRRotations();
        Aiming();
        Shoot();

        #region Remove/Show UI
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.One) || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            inputInstruction.SetActive(false);

            if (!gameManager.roundStarter.GetComponent<TargetHit>().startGameDelay && !gameManager.endGameDelay && !gameManager.isRoundActive)
            {
                gameManager.shootTargetText.SetActive(true);
            }
        }
        #endregion

        #region Remove Bullet
        // Remove Bullet if out of range or lifeLeft is 0
        for (int i = 0; i < bulletsFired.Count; i++)
        {
            GameObject bullet = bulletsFired[i];

            float distance = Vector3.Distance(controllerPos, bullet.transform.position);

            if (distance >= 100.0f)
            {
                // If bullet is far away delete
                Destroy(bullet);
                bulletsFired.Remove(bullet);
            }
            else if (bullet.GetComponent<BulletLife>().lifeLeft <= 0)
            {
                // Time until bullet gets deleted
                Destroy(bullet);
                bulletsFired.Remove(bullet);
            }
        }
        #endregion
    }
    void SetVRRotations()
    {
        // Set rotation of camera
        if (allowKMDebug)
            transform.localRotation = cameraRig.rotation;
        else
            transform.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);

        // Set position of controller
        if (allowKMDebug)
            controllerPos = new Vector3(0.3f, -0.2f, 0.3f);
        else
            controllerPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

        // Move controller towards position
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, controllerPos, ref velocity, smoothTime);
    }

    void Aiming()
    {
        // Toggle Aiming
        if (OVRInput.GetDown(OVRInput.Button.One) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!isAimTrigged)
                isAimTrigged = true;
            else
                isAimTrigged = false;
        }

        // Update Laser
        if (isAimTrigged)
        {
            lr.SetPosition(0, controllerPos);
            lr.SetPosition(1, transform.forward * 5000);
        }
        else
        {
            lr.SetPosition(0, controllerPos);
            lr.SetPosition(1, controllerPos);
        }
    }

    void Shoot()
    {
        // Bullet direction
        Ray ray = new Ray(controllerPos, transform.forward);

        if (canShoot)
        {
            // Shoot Bullet
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                //Removed if statment to allow shooting the round starter
                GameObject go = Instantiate(bullet, ray.origin + (ray.direction * 0.5f), Quaternion.LookRotation(ray.direction));
                go.GetComponent<Rigidbody>().AddForce(bulletSpeed * 100.0f * ray.direction);
                go.GetComponent<BulletLife>().lifeLeft = bulletLife;

                // Add bullet to list
                bulletsFired.Add(go);

                // Shoot sound
                gunSound.Play();

                // Delay Shooting
                StartCoroutine(DelayShoot());
            }
        }
    }

    IEnumerator DelayShoot()
    {
        canShoot = false;

        yield return new WaitForSeconds(shootDelay);

        canShoot = true;
    }

    public void RemoveBulletFromList(GameObject bullet)
    {
        bulletsFired.Remove(bullet);
    }
}
