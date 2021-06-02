using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPusher : MonoBehaviour
{
    public GameObject bullet;
    public Transform pointer;

    List<GameObject> bulletsFired;

    void Start()
    {
        bulletsFired = new List<GameObject>();
    }

    void Update()
    {
        Ray ray = new Ray(pointer.position, pointer.forward);

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("button pressed");

            GameObject go = Instantiate(bullet, ray.origin, Quaternion.LookRotation(ray.direction));
            go.GetComponent<Rigidbody>().AddForce(10000.0f * ray.direction);

            bulletsFired.Add(go);
        }

        for (int i = 0; i < bulletsFired.Count; i++)
        {
            GameObject bullet = bulletsFired[i];

            float distance = Vector3.Distance(pointer.position, bullet.transform.position);

            if (distance >= 100.0f)
            {
                Destroy(bullet);
                bulletsFired.Remove(bullet);
            }
        }
    }
}
