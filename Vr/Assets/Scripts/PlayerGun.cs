using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public Transform cameraRig;
    public Transform pointer;
    public GameObject bullet;

    [Header("Gun Values")]
    public float bulletSpeed = 5.0f;
    public float bulletLife = 100.0f;
    public float smoothTime = 0.3f;
    public float aimDistance = 10.0f;
    public float aimYOffset = -0.05f;
    public float aimXOffset = 0.0f;

    Vector3 velocity = Vector3.zero;
    bool aimTriggered = false;
    

    List<GameObject> bulletsFired;

    void Start()
    {
        bulletsFired = new List<GameObject>();
    }

    void Update()
    {
        Vector3 position = new Vector3();
        transform.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);

        // Toggle Aiming
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            if (!aimTriggered)
                aimTriggered = true;
            else
                aimTriggered = false;
        }

        // Set gun position
        if (aimTriggered)
        {
            position = cameraRig.TransformPoint(new Vector3(aimXOffset, aimYOffset, aimDistance));

            transform.position = Vector3.SmoothDamp(transform.position, position, ref velocity, smoothTime);
        }
        else
        {
            position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, position, ref velocity, smoothTime);
        }
        
        // Bullet direction
        Ray ray = new Ray(position, transform.forward);

        // Shoot Bullet
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            GameObject go = Instantiate(bullet, ray.origin + (ray.direction * 2.0f), Quaternion.LookRotation(ray.direction));
            go.GetComponent<Rigidbody>().AddForce(bulletSpeed * 100.0f * ray.direction);
            go.GetComponent<BulletLife>().lifeLeft = 100.0f;

            bulletsFired.Add(go);
        }

        // Remove Bullet if out of range or lifeLeft is 0
        for (int i = 0; i < bulletsFired.Count; i++)
        {
            GameObject bullet = bulletsFired[i];

            float distance = Vector3.Distance(pointer.position, bullet.transform.position);

            if (distance >= 100.0f)
            {
                Destroy(bullet);
                bulletsFired.Remove(bullet);
            }
            else if (bullet.GetComponent<BulletLife>().lifeLeft <= 0)
            {
                Destroy(bullet);
                bulletsFired.Remove(bullet);
            }
        }
    }
}
