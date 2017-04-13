using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LimbImage {
	public Image Icon;
	public Rippable Limb;
	public Text NumberField;
}

public class StuffingStatusUI : MonoBehaviour {

	public StuffingStatus Character;
	public Slider Bar;
	public Text HealthNumber;
	public LimbImage[] LimbImages;

	public StuffingStatusUI OpponentStatus;

	// Use this for initialization
	void Start () {
		if (Character == null) {
			Character = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameRules>().Players [0];
		}
	}

	// Update is called once per frame
	void Update () {
		// Update health bars
		Bar.value = Mathf.MoveTowards (Bar.value, Character.stuffingLevel, 1.0f);
		HealthNumber.text = Mathf.CeilToInt(Character.stuffingLevel).ToString ();


	}

	// Called when object is loaded
	public void OnEnable()
	{
		// Event listeners enabled
		Rippable.OnRipped += this.OnRip;
	}

	// Called when script or object is disabled or destroyed
	public void OnDisable()
	{
		// Event listeners disabled
		Rippable.OnRipped -= this.OnRip;
	}

	public void OnRip(Rippable sender) {
		foreach (LimbImage limb in LimbImages) {
			if (sender == limb.Limb) {
				limb.Icon.color = new Color (200.0f, 100.0f, 100.0f);
			}
		}
	}


}
