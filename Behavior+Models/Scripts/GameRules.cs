using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
	// Milliseconds of immunity time given to a limb when it takes damaging impulse
	public static int ImmunityTime = 50;

	// Scaling factors to apply to different damage types for: impulse, collided impulse, player-collided impulse
	public static float ScaleImpulse = 0.05f, ScaleCollide = 0.5f, ScaleRigidbodyCollide = 1.0f;

	// Objects for the players
	public StuffingStatus player0, player1;

	// Use this for initialization
	public void Start()
	{
	}
	
	// Update is called once per frame
	public void Update()
	{
	}
}
