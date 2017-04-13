﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffingStatus : MonoBehaviour {

	public ParticleSystem Leak;
	public ParticleSystem Burst;

	private List<Draggable> Handles;
	private List<Rippable> Seams;

	private float stuffingLevel = 100.0f;

	public float Health {
		get { return stuffingLevel; }
		private set{
			if (value < 0.0f && Leak.isPlaying) {
				Leak.Stop ();
			}
			stuffingLevel = Mathf.Max (0.0f, value); }
	}

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
		float totalIntegrityLoss = 0.0f;
		// Reduce stuffing incrementally, proportional to limb damage sustained (worse if lost limb)
		foreach (Rippable limb in Seams) {
			float limbDamage = 100.0f - limb.Integrity;
			totalIntegrityLoss += limbDamage;
			limbDamage *= (limb.Ripped) ? limb.HoleFactor : limb.LeakFactor;
			Health -= limbDamage;
		}
		ParticleSystem.EmissionModule emit = Leak.emission;
		emit.rateOverDistance = 0.2f + (totalIntegrityLoss / Seams.Count)/100;
		emit.rateOverTime = (100.0f - Health) / 50;
	}

	void OnRip(Rippable sender) {
		// Immediate loss of stuffing from that limb if it belongs to us
		if (Seams.Contains (sender)) {
			if (Health > 0.0f) {
				// Big burst of stuffing flies out
				Burst.Play ();
				Burst.Play ();
			}
			Health -= 10 * sender.HoleFactor;

		}
	}
	void OnDamage(Draggable sender, float intensity) {
		// Immediate loss of some stuffing if it belongs to us
		if (Handles.Contains (sender)) {
			if (Health > 0.0f) {
				// Small burst of stuffing flies out
				Burst.Play ();
			}
			Health -= intensity;

		}
	}
}
