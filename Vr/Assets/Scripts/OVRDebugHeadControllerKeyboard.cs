using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRDebugHeadControllerKeyboard : MonoBehaviour
{
	[SerializeField]
	public bool AllowPitchLook = false;
	[SerializeField]
	public bool AllowYawLook = true;
	[SerializeField]
	public bool InvertPitch = false;
	[SerializeField]
	public float Keyboard_PitchDegreesPerSec = 90.0f;
	[SerializeField]
	public float Keyboard_YawDegreesPerSec = 90.0f;
	[SerializeField]
	public bool AllowMovement = false;
	[SerializeField]
	public float ForwardSpeed = 2.0f;
	[SerializeField]
	public float StrafeSpeed = 2.0f;

	protected OVRCameraRig CameraRig = null;

	void Awake()
	{
		// locate the camera rig so we can use it to get the current camera transform each frame
		OVRCameraRig[] CameraRigs = gameObject.GetComponentsInChildren<OVRCameraRig>();

		if (CameraRigs.Length == 0)
			Debug.LogWarning("OVRCamParent: No OVRCameraRig attached.");
		else if (CameraRigs.Length > 1)
			Debug.LogWarning("OVRCamParent: More then 1 OVRCameraRig attached.");
		else
			CameraRig = CameraRigs[0];
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (AllowMovement)
		{
			float gamePad_FwdAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y;
			float gamePad_StrafeAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x;

			Vector3 fwdMove = (CameraRig.centerEyeAnchor.rotation * Vector3.forward) * gamePad_FwdAxis * Time.deltaTime * ForwardSpeed;
			Vector3 strafeMove = (CameraRig.centerEyeAnchor.rotation * Vector3.right) * gamePad_StrafeAxis * Time.deltaTime * StrafeSpeed;
			transform.position += fwdMove + strafeMove;
		}

#if UNITY_2017_2_OR_NEWER
		if (!UnityEngine.XR.XRDevice.isPresent && (AllowYawLook || AllowPitchLook))
#else
		if ( !UnityEngine.VR.VRDevice.isPresent && ( AllowYawLook || AllowPitchLook ) )
#endif
		{
			Quaternion r = transform.rotation;
			if (AllowYawLook)
			{
				float keyboardYaw = Input.GetAxisRaw("Horizontal");
				float yawAmount = keyboardYaw * Time.deltaTime * Keyboard_YawDegreesPerSec;
				Quaternion yawRot = Quaternion.AngleAxis(yawAmount, Vector3.up);
				r = yawRot * r;
			}
			if (AllowPitchLook)
			{
				float gamePadPitch = Input.GetAxisRaw("Vertical");
				if (Mathf.Abs(gamePadPitch) > 0.0001f)
				{
					if (InvertPitch)
					{
						gamePadPitch *= -1.0f;
					}
					float pitchAmount = gamePadPitch * Time.deltaTime * Keyboard_PitchDegreesPerSec;
					Quaternion pitchRot = Quaternion.AngleAxis(pitchAmount, Vector3.left);
					r = r * pitchRot;
				}
			}

			transform.rotation = r;
		}
	}
}
