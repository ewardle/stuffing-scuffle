//////////////////////
///	for Stuffing Scuffle
///	by Eliana Wardle
//////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rippable : MonoBehaviour
{
	public Joint Bones;
	public GameObject Mesh;
	public Renderer Hole;

	private bool isRipped = false;

	public bool Ripped
	{
		get
		{
			return isRipped;
		}

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
		
	// Use this for initialization
	public void Start()
	{
		if (Bones == null || Mesh == null)
		{
			Debug.Log("on " + gameObject.name + ": Cannot use Rippable without a limb mesh and bone set");
			Destroy(this);
		}

		// Testing: rip immediately
		////this.Rip();

		return;
	}
	
	// Update is called once per frame
	public void Update()
	{
		// TODO: update joint's Break Force and Break Torque according to damage sustained
		return;
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

		// If applicable: accelerate rate of stuffing loss and remove remaining stuffing that was contained in the arm
	}
}
