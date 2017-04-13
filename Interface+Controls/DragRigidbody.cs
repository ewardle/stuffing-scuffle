//////////////////////
///	for Stuffing Scuffle
/// (modified from standard asset version)
//////////////////////

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DragRigidbody : NetworkBehaviour
{
	public Camera PlayerCamera;

    private float kSpring = 75.0f; // 50.0f default
    private float kDamper = 5.0f; // 5.0f default
    private float kDrag = 100.0f; // 10.0f default
    private float kAngularDrag = 500.0f; // 5.0f default
    private float kDistance = 0.0f; // 0.2f default

    private SpringJoint mSpringJoint;

	private void Start() {
		if (PlayerCamera == null) {
			PlayerCamera = FindCamera ();
		}
	}

    private void Update()
    {
        // Make sure the user pressed the mouse down
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        // We need to actually hit an object
        RaycastHit hit = new RaycastHit();
        if (!Physics.Raycast(
				PlayerCamera.ScreenPointToRay(Input.mousePosition).origin, 
				PlayerCamera.ScreenPointToRay(Input.mousePosition).direction, 
				out hit, 
				100,
				Physics.DefaultRaycastLayers))
        {
            return;
        }

        // We need to hit a rigidbody that is not kinematic
        if (!hit.rigidbody || hit.rigidbody.isKinematic)
        {
            return;
        }

		// We only can drag Draggable parts as well, not just any rigidbody
		if (!hit.rigidbody.GetComponent<Draggable>())
		{
			return;
		}

        if (!mSpringJoint)
        {
            var go = new GameObject("Rigidbody Dragger");
            Rigidbody body = go.AddComponent<Rigidbody>();
            mSpringJoint = go.AddComponent<SpringJoint>();
            body.isKinematic = true;
        }

        mSpringJoint.transform.position = hit.point;
        mSpringJoint.anchor = Vector3.zero;

        mSpringJoint.spring = kSpring;
        mSpringJoint.damper = kDamper;
        mSpringJoint.maxDistance = kDistance;
        mSpringJoint.connectedBody = hit.rigidbody;

        StartCoroutine("DragObject", hit.distance);
    }

    private IEnumerator DragObject(float distance)
    {
        var oldDrag = mSpringJoint.connectedBody.drag;
        var oldAngularDrag = mSpringJoint.connectedBody.angularDrag;
        mSpringJoint.connectedBody.drag = kDrag;
        mSpringJoint.connectedBody.angularDrag = kAngularDrag;
        while (Input.GetMouseButton(0))
        {
			var ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);

            mSpringJoint.transform.position = ray.GetPoint(distance);
            yield return null;
        }

        if (mSpringJoint.connectedBody)
        {
            mSpringJoint.connectedBody.drag = oldDrag;
            mSpringJoint.connectedBody.angularDrag = oldAngularDrag;
            mSpringJoint.connectedBody = null;
        }
    }

    private Camera FindCamera()
    {
        if (GetComponentInChildren<Camera>())
        {
            return GetComponentInChildren<Camera>();
        }

        return Camera.main;
    }
}
