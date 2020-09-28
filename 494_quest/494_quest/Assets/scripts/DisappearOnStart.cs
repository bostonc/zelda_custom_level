/*
 * It may often be useful to show objects while in the Unity Editor, yet hide them during gameplay.
 * For example, CameraPoint objects are used to control Camera movement from room to room in this example game.
 * Having them be visible in the Editor allows us to conveniently move them and plan the world layout.
 * But they are quite ugly during gameplay, so when the game starts, this component will disable their renderer.
 */

using UnityEngine;
using System.Collections;

public class DisappearOnStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().enabled = false;
	}
}
