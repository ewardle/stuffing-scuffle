//////////////////////
///	for Stuffing Scuffle
///	by Eliana Wardle
//////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rippable : MonoBehaviour
{
	// Event broadcasting for when this limb gets ripped off
	public delegate void RipAction(Rippable sender);
	public static event RipAction OnRipped;

	public Joint Bones;
	public GameObject Mesh;
	public Renderer Hole;

	private bool isRipped = false;
	private float integrity = 100.0f;

	// Reference to the draggable limb which calculates hits
	private Draggable handle;

	public bool Ripped
	{
		get { return isRipped; }

		set
		{
			// Cannot "unrip" a limb, but check if possible to rip
			if (value == true && !isRipped) 
			{
				// To ensure consistency, rip function sets isRipped instead
				this.Rip();
			}
		}
	}

	public float Integrity
	{
		get { return integrity; }
		private set { integrity = value; }
	}
		
	// Use this for initialization
	public void Start()
	{
		if (Bones == null || Mesh == null)
		{
			Debug.Log("on " + gameObject.name + ": Cannot use Rippable without a limb mesh and bone set");
			Destroy(this);
		}

		handle = GetComponentInChildren<Draggable> ();

		// Testing: rip immediately
		////this.Rip();

		return;
	}
	
	// Update is called once per frame
	public void Update()
	{
		return;
	}

	// Called when object is loaded
	public void OnEnable()
	{
		// Event listeners enabled
		Draggable.OnDamaged += this.OnDamage;
	}

	// Called when script or object is disabled or destroyed
	public void OnDisable()
	{
		// Event listeners disabled
		Draggable.OnDamaged -= this.OnDamage;
	}

	public void OnJointBreak(float breakForce)
	{
		Debug.Log("Broke joint: " + gameObject.name + ", force: " + breakForce);
		this.Rip();
	}

	private void Rip()
	{
		Debug.Log("Tore off limb: " + Mesh.name);

		// Ensure the limb is marked as ripped (without calling this method automatically by it)
		this.isRipped = true;

		 // Make some stuffing particle effect related stuff (maybe later)

		 // Unparent the arm bone chain and arm object from basic doll, and hide the hole object
		Bones.transform.parent = null;
		Mesh.transform.parent = null;

		if (Hole != null)
		{
			Hole.enabled = false;
		}
			
		// Destroy Joint component to detach and let fall (does nothing if joint already broken through physics)
		Destroy(Bones);

		// Broadcast event that this limb was ripped off
		OnRipped(this);
	}

	private void OnDamage(Draggable sender, float intensity) {
		// TODO: reduce integrity, proportional to hit strength, IF this limb was the one damaged
		if (sender == handle) {
		}
	}

}
