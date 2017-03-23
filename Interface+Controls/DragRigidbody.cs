//////////////////////
///	for Stuffing Scuffle
/// (modified from standard asset version)
//////////////////////

using System;
using System.Collections;
using UnityEngine;

public class DragRigidbody : MonoBehaviour
{
    private float kSpring = 75.0f; // 50.0f default
    private float kDamper = 5.0f; // 5.0f default
    private float kDrag = 100.0f; // 10.0f default
    private float kAngularDrag = 500.0f; // 5.0f default
    private float kDistance = 0.0f; // 0.2f default

    private SpringJoint mSpringJoint;

    private void Update()
    {
        // Make sure the user pressed the mouse down
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        var mainCamera = FindCamera();

        // We need to actually hit an object
        RaycastHit hit = new RaycastHit();
        if (!Physics.Raycast(
				mainCamera.ScreenPointToRay(Input.mousePosition).origin, 
				mainCamera.ScreenPointToRay(Input.mousePosition).direction, 
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
        var mainCamera = FindCamera();
        while (Input.GetMouseButton(0))
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

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
        if (GetComponent<Camera>())
        {
            return GetComponent<Camera>();
        }

        return Camera.main;
    }
}
