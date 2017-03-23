//////////////////////
///	for Stuffing Scuffle
///	by Eliana Wardle
/// (partially adapted from Unity wiki's DontGoThroughThings script)
//////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public class Draggable : MonoBehaviour 
{
	/* TODO: (general) 
	 * - Make the draggable areas easier to see (show a texture circle on mouseover or something)
	*/

	// Don't worry about damage during immunity frames
	private int immuneI = 0;
	private bool isImmune = false;

	// Track velocity every few physics steps, and position every step
	private Vector3 vCurr, vPrev, pPrev;
	private short physicsI = 0;

	// Extra information needed for raycast-based collision detection
	private float minExtent, partExtent, sqrMinExtent, skinWidth;
	private LayerMask layer = -1;
	private Collider col;
	private Rigidbody body;

	// TODO: character object hash or reference or something for when we do networked
	// (so player can only move their own character's parts)

	// Use this for initialization
	public void Start()
	{
		vCurr = new Vector3(0, 0, 0);
		vPrev = new Vector3(0, 0, 0);
		pPrev = transform.position;
		skinWidth = 0.01f;
		body = GetComponent<Rigidbody>();
		col = GetComponent<Collider>();
		minExtent = Mathf.Min(Mathf.Min(col.bounds.extents.x, col.bounds.extents.y), col.bounds.extents.z);
		partExtent = minExtent * (1.0f - skinWidth);
		sqrMinExtent = minExtent * minExtent; // (makes for faster calculations later)
	}
	
	// Update is called once per frame
	public void Update()
	{
	}

	// Physics update function
	public void FixedUpdate()
	{
		// Block further movement if going fast enough to "skip" collider
		Vector3 dP = body.position - pPrev;
		float sqrDD = dP.sqrMagnitude;
		if (sqrDD > sqrMinExtent)
		{
			float dD = Mathf.Sqrt(sqrDD);
			RaycastHit hit;

			// Check for obstructions within past movement step
			if (Physics.Raycast(pPrev, dP, out hit, dD, layer.value))
			{
				if (hit.collider && hit.collider.GetComponent<Rigidbody>() == null)
				{
					if (hit.collider.isTrigger)
					{
						////hit.collider.SendMessage ("OnTriggerEnter", col);
					}
					else
					{
						// non-trigger collider
						body.position = hit.point - ((dP / dD) * partExtent);
					}
				}
			}

			pPrev = body.position;
		}

		// Calculate change in velocity (approximate "impulse") every few frames
		physicsI++;
		if (physicsI >= 5)
		{
			physicsI = 0;
			vPrev = vCurr;
			vCurr = body.velocity;
			float dV = (vCurr - vPrev).magnitude;
			if (!isImmune && dV >= 15.0f && gameObject.name == "Head")
			{
				Debug.Log("Strong impulse of " + dV + " sustained; temporary immunity starting");

				// TODO: do damage stuff either here or another script:
				// - light limb damage on general strong impulse, 
				// - medium overall damage on strong collision,
				// - high overall damage if other collider was a rigidbody/other player,
				//		proportional to velocity difference
				//		(low damage if you were going faster than them, high damage if you were slower)
				// (meanwhile, just start immunity frames)
				isImmune = true;
			}
		}

		if (isImmune)
		{
			immuneI++;
			if (immuneI >= GameRules.ImmunityTime)
			{
				immuneI = 0;
				isImmune = false;

				// TODO: visual/audio cues as to immunity being on and off
				Debug.Log("Immunity over");
			}
		}
	}
}
