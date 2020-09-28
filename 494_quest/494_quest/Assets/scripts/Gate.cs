/*
 * A "Gate" is an object that disappears when a certain number of Rupees are delivered to it.
 * It is used in Gameplay to make Rupees valuable, and to supply a motive for combating the enemies.
 */

using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {


	public int rupeeCost = 30;
	public GameObject rupeeCostText;

	// Use this for initialization
	void Start () {
		// Ensure that the TextMesh displays the proper rupee count.
		// We don't want to confuse players with incorrect prices!
		rupeeCostText.GetComponent<TextMesh>().text = "x " + rupeeCost.ToString();
	}

	// This function is called whenever an object with a collider and Rigidbody begins a collision with this object.
	// Note that the spelling and argument, "OnCollisionEnter(Collision <arg name>)", must be used exactly.
	void OnCollisionEnter(Collision coll)
	{
		// We only care about collisions with Player objects.
		// Note that gameobject tags are set at the top of the inspector.
		if(coll.gameObject.tag == "Player")
		{
			// Can the player afford to lift the gate?
			if(Player.rupeeCount >= rupeeCost)
			{
				// Cash out the player's rupees.
				Player.rupeeCount -= rupeeCost;

				// Refresh the hud to reflect this rupee-count change.
				Hud.RefreshDisplay();

				// Remove the Gate from existence.
				Destroy(gameObject);
			}
		}
	}
}
