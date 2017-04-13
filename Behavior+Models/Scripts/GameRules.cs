using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
	// Milliseconds of immunity time given to a limb when it takes damaging impulse
	public static int ImmunityTime = 50;

	// Scaling factors to apply to different damage types for: impulse, collided impulse, player-collided impulse
	public struct DamageScale {
		public const float Impulse = 0.05f;
		public const float StaticCollide = 0.5f;
		public const float PlayerCollide = 1.0f;
	}

	// Objects for the players
	[HideInInspector]
	public StuffingStatus[] Players;

	// Use this for initialization
	public void Start()
	{
		Players = GameObject.FindObjectsOfType<StuffingStatus> ();
	}
	
	// Update is called once per frame
	public void Update()
	{
	}
}
