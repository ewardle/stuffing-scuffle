using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
	// Use this for initialization
	public void Start()
	{
	}
	
	// Update is called once per frame
	public void Update()
	{
		if (Input.GetKey("escape"))
		{
			Application.Quit();
			Debug.Log("Closing Stuffing Scuffle.");
		}
	}
}
