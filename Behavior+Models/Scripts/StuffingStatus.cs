using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffingStatus : MonoBehaviour {

	private List<Draggable> Handles;
	private List<Rippable> Seams;
	private int numInitialSeams;

	// Use this for initialization
	void Start () {
		Handles = new List<Draggable> (GetComponentsInChildren<Draggable> ());
		Seams = new List<Rippable> (Handles.Capacity);
		foreach (Draggable part in Handles) {
			Rippable seam = part.GetComponentInParent<Rippable> ();
			if (seam != null) {
				Seams.Add (seam);
			}
		}
		numInitialSeams = Seams.Count;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Called when object is loaded
	public void OnEnable()
	{
		// Event listeners enabled
		Rippable.OnRipped += this.OnRip;
		Draggable.OnDamaged += this.OnDamage;
	}

	// Called when script or object is disabled or destroyed
	public void OnDisable()
	{
		// Event listeners disabled
		Rippable.OnRipped -= this.OnRip;
		Draggable.OnDamaged -= this.OnDamage;
	}


	// Called at a set time interval regardless of framerate
	void FixedUpdate() {
		// TODO: Reduce stuffing incrementally, proportional to limb damage sustained
	}

	void OnRip(Rippable sender) {
		// TODO: immediate loss of remaining stuffing from that limb if it belongs to us
		if (Seams.Contains (sender)) {
		}
	}
	void OnDamage(Draggable sender, float intensity) {
		// TODO: immediate loss of some stuffing if it belongs to us
		if (Handles.Contains (sender)) {
		}
	}
}
