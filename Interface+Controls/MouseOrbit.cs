//////////////////////
/// (adapted from Unity wiki's MouseOrbitImproved script)
//////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOrbit : MonoBehaviour {

	public Transform Target;
	public float Distance = 5.0f;
	public float xSpeed = 40.0f;
	public float ySpeed = 120.0f;

	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;

	public float DistanceMin = 0.5f;
	public float DistanceMax = 15f;

	private float x = 0.0f;
	private float y = 0.0f;

	public void Start()
	{
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
	}

	public void LateUpdate()
	{
		bool rightClicked = Input.GetMouseButton (1);

		if (Target) {
			Quaternion rotation;
			if (rightClicked) {
				x += Input.GetAxis ("Mouse X") * xSpeed * Distance * 0.02f;
				y -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;
				y = ClampAngle (y, yMinLimit, yMaxLimit);

				rotation = Quaternion.Euler (y, x, 0);
			} else {
				rotation = transform.rotation;
			}
			Distance = Mathf.Clamp (Distance - Input.GetAxis ("Mouse ScrollWheel") * 5, DistanceMin, DistanceMax);

			RaycastHit hit;
			if (Physics.Linecast (transform.position, Target.position, out hit))
			{
				Distance -= hit.distance;
			}
			Vector3 negDistance = new Vector3 (0.0f, 0.0f, -Distance);
			Vector3 position = rotation * negDistance + Target.position;

			transform.rotation = rotation;
			transform.position = position;
		}
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f) {
			angle += 360f;
		}
		if (angle > 360F) {
			angle -= 360f;
		}
		return Mathf.Clamp (angle, min, max);
	}
}
