using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour
{
    public Transform cameraRig;
    public float CameraDistance = 3.0f;
    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;
    private Transform target;


    // Start is called before the first frame update
    void Start()
    {
        target = cameraRig;
    }

    // Update is called once per frame
    void Update()
    {
        // Get target position in front of the camera ->
        Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, CameraDistance));

        // Smoothly move my canvas towards target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // version 1: canvas looks at camera
        transform.LookAt(transform.position + cameraRig.rotation * Vector3.forward, cameraRig.rotation * Vector3.up);

        //// version 2 : my object's rotation isn't finished synchronously with the position smooth.damp ->
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, 35 * Time.deltaTime);
    }
}
