using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPusher : MonoBehaviour
{
    public Transform pointer;

    LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        Ray ray = new Ray(pointer.position, pointer.forward);
        RaycastHit hit;

        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            Debug.Log("button pressed");

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, ray.origin + 100 * ray.direction);

            if (Physics.Raycast(ray, out hit))
            {
                Rigidbody body = hit.collider.GetComponent<Rigidbody>();
                if (body)
                {
                    body.AddForce(10000.0f * ray.direction);
                }
            }
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }
}
