/*
 * The Enemy component manages the Element Queue for an enemy.
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Entity {

	public List<Element> elementQueue = new List<Element>();

	public GameObject rupeePrefab;

	// Use this for initialization
	void Start () {
		// An enemy should begin its life wanting to chase the player.
		Element.addElement(elementQueue, new ElementChase(this, Player.instance, 0.02f));
	}
	
	// Update is called once per frame
	void Update()
	{
		// If the enemy has no more elements to process, give it an element that
		// makes it chase the player.
		if(elementQueue.Count == 0)
			Element.addElement(elementQueue, new ElementChase(this, Player.instance, 0.02f));
		
		Element.updateElements(elementQueue);
	}

	void OnCollisionEnter(Collision coll)
	{
		// We only care about collisions with bubble projectiles.
		// Tags may be set for gameobjects near the top of the inspector.
		if(coll.gameObject.tag != "Bubble")
			return;

		// If this enemy is invincible and we've collided with a bubble, destroy the bubble.
		if(currentState == EntityState.INVINCIBLE && coll.gameObject.tag == "Bubble")
		{
			Destroy(coll.gameObject);
			return;
		}

		if(currentState == EntityState.NORMAL)
		{
			// Spew Rupees from the enemy.
			for(int i = 0; i < 5; i++)
			{
				Vector3 newVelocity = UnityEngine.Random.onUnitSphere * 10;
				newVelocity = new Vector3(newVelocity.x, newVelocity.y, 0);
				GameObject newRupee = Instantiate(rupeePrefab, transform.position, Quaternion.identity) as GameObject;
				newRupee.GetComponent<Rigidbody>().velocity = newVelocity;
			}

			// The enemy has been hit by a bubble, so stun the enemy.
			Element.disruptElement(elementQueue, new ElementStunned(this, 60, true));

			// Add a Spin Attack element, so the enemy will attack when the stunned element finishes.
			Element.addElement(elementQueue, new ElementSpinAttack(this, Player.instance, 0.05f));

			health --;
			if(health <= 0)
			{
				// The enemy has been killed. Give it a death animation via the
				// Limited Lifetime component.
				LimitedLifetime l = gameObject.AddComponent<LimitedLifetime>();
				l.framesToLive = 30;
			}			
		}
	}
}