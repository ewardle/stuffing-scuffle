//////////////////////
///	for Stuffing Scuffle
///	by Eliana Wardle
/// (partially adapted from Unity wiki's DontGoThroughThings script)
//////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public class Draggable : NetworkBehaviour 
{
	// 

	/* TODO: (general) - Make the draggable areas easier to see (show a texture circle on mouseover or something) */

	// Event broadcasting for when damage is sustained by this limb
	public delegate void DamageAction(Draggable sender, float intensity);
	public static event DamageAction OnDamaged;

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

	private StuffingStatus character;

	// Publicly visible latest change in velocity (impulse) for player-collision-related calculations
	private float dV;
	public float Velocity {
		get { return dV; }
		private set { dV = value; }
	}

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

		character = GetComponentInParent<StuffingStatus> ();
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
		RaycastHit hit;
		if (sqrDD > sqrMinExtent)
		{
			float dD = Mathf.Sqrt(sqrDD);

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
			dV = (vCurr - vPrev).magnitude;
			/*if (!isImmune && dV >= 15.0f)
			{
				// Light damage for general strong impulse
				float intensity = dV;
				intensity *= GameRules.DamageScale.Impulse;
				OnDamaged (this, intensity);
				Debug.Log ("Damaged " + this.name + " with impulse; intensity " + intensity);
				// Turn on temporary immunity
				isImmune = true;
			}*/
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

	public void OnCollisionEnter(Collision hit) {
		if (!isImmune && hit.impulse.magnitude > 7.0f) {
			// Damage calculation
			float intensity = dV;
			Draggable other = hit.collider.GetComponent<Draggable> ();
			if (other && other.character != this.character) {
				// High damage from hitting other player (more if you were going faster, less if going slower)
				float ddV = Mathf.Abs (dV - other.dV);
				intensity += (ddV < 0) ? ddV : -ddV;
				intensity *= GameRules.DamageScale.PlayerCollide;
				Debug.Log ("Damaged " + this.name + " with other player hit; intensity " + intensity);

				OnDamaged (this, intensity);
			} else if (hit.collider.GetComponent<CharacterJoint>() == null) {
				// Moderate damage from hitting non-player object
				intensity *= GameRules.DamageScale.StaticCollide;
				Debug.Log ("Damaged " + this.name + " with static collision; intensity " + intensity);

				OnDamaged (this, intensity);
			}
			// Turn on temporary immunity
			isImmune = true;
		}
	}
}
