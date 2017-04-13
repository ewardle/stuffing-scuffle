using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuFunctions : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Quit() {
		Debug.Log ("Quitting application");
		Application.Quit ();
	}

	public void LoadScene(string name) {
		Debug.Log ("Loading scene: " + name);
		SceneManager.LoadScene (name);
	}
}
