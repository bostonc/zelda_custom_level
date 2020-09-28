/*
 * Rupees are collectables!
 * 
 * They respond to collisions to Player objects, incrementing the Player's rupee count.
 */

using UnityEngine;
using System.Collections;

public class Rupee : MonoBehaviour {

	void OnCollisionEnter(Collision coll)
	{
		// We only care if we collide with a player.
		// Gameobjects may be given a tag via a slot at the top of the inspector.
		if(coll.gameObject.tag == "Player")
		{
			// Increment the globally accessible rupee count.
			Player.rupeeCount ++;

			// Refresh the display to reflect this change.
			Hud.RefreshDisplay();

			// Stop the rupee from moving.
			GetComponent<Rigidbody>().velocity = Vector3.zero;

			// Destroy the Rupee's collider, so it can no longer collide.
			// Without this, one rupee may collide with a player many times,
			// resulting in an incorrect incrementation of the player's rupee count.
			Destroy(GetComponent<Collider>());

			// Give the rupee a Limited Lifetime component so it disappears with style.
			LimitedLifetime l = gameObject.AddComponent<LimitedLifetime>();

			// It is difficult to initialize components with parameters 
			// (in constrast with traditional object constructors, which allow parameters easily)
			// We edit the component's "framesToLive" property to make the rupee disappear quickly.
			l.framesToLive = 30;
		}
	}
}
