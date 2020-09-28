/*
 * The GameOver component manages the end-screen of the game.
 * 
 * It is responsible for completing the game-loop. It allows Players to restart the game.
 * It is important to clean up any static data, as it will carry over between scenes, and
 * may lead to odd game-states when a game is restarted.
 */
using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Reset the static data.
		// If we don't, we will continue to build up rupees, even as we continuously die, for example.
		CameraPoint.cameraPoints.Clear();
		Player.rupeeCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		// When the user presses the space-bar, we should return to gameplay.
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Application.LoadLevel("Gameplay");
		}
	}
}
