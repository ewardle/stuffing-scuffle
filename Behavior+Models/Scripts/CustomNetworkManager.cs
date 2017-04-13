using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
		GameObject player = (GameObject)Instantiate (playerPrefab, Vector3.zero, Quaternion.identity);
		GameRules controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameRules> ();
		if (controller.Players [0] == null) {
			controller.Players [0] = player.GetComponent<StuffingStatus>();
		} else {
			controller.Players [1] = player.GetComponent<StuffingStatus>();

			// Since second player showed up, enable UI segment dedicated to opposing player (right-side) and hook up to appropriate parts
			controller.Players [0].GetComponentInChildren<StuffingStatusUI> ().OpponentStatus.gameObject.SetActive (true);
			controller.Players [1].GetComponentInChildren<StuffingStatusUI> ().OpponentStatus.gameObject.SetActive (true);

			StuffingStatusUI[] hostUIs = controller.Players[0].GetComponentsInChildren<StuffingStatusUI>();
			StuffingStatusUI[] guestUIs = controller.Players[1].GetComponentsInChildren<StuffingStatusUI>();

			hostUIs[1].Character = controller.Players [1];
			guestUIs[1].Character = controller.Players [0];

			for (int i = 0; i < hostUIs[1].LimbImages.Length; i++) {
				hostUIs [1].LimbImages [i].Limb = guestUIs [0].LimbImages [i].Limb;
				guestUIs [1].LimbImages [i].Limb = hostUIs [0].LimbImages [i].Limb;
			}
		}

		// Add player to game
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
