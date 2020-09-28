/*
 * Entity is a base class for "living" objects in the game.
 * 
 * "Living" objects have health, may be stunned, or made invincible.
 */

using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

	//public int initialHealth = 3;
	//public int health;

	public EntityState currentState = EntityState.NORMAL;

	/*// This function provides a way for Derived objects run logic during Awake(),
	// While reserving Awake() itself for this base Entity object.
	protected virtual void PostAwake() {}

	// Use this for initialization
	void Awake () {
		health = initialHealth;
		PostAwake();
	}*/
}

/*
 * This element is responsible for handling an entity in its "Stunned" state.
 */
public class ElementStunned : Element
{
	Entity entity;
	float life = 0.0f;
	bool shouldSpin;

	public ElementStunned(Entity entity, float stunTicks, bool shouldSpin)
	{ 
		this.entity = entity;
		life = stunTicks;
		//this.shouldSpin = shouldSpin;
	}
	
	public override void onActive()
	{
		entity.currentState = EntityState.STUNNED;
		// Color the object Red to indicate damage.
		entity.GetComponent<Renderer>().material.color = new Color(1, 0, 0);

		// If the stunned entity should spin, adjust constraints, and provide an angular velocity
		// for a fun physics effect.
		/*if(shouldSpin)
		{
			entity.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
			entity.GetComponent<Rigidbody>().angularVelocity = UnityEngine.Random.onUnitSphere * 10;
		}*/
	}
	
	public override void update(float time_delta_fraction)
	{
		life -= time_delta_fraction;

		// Setting 'finished' to true ends the Element.
		if(life <= 0)
			finished = true;
	}
	
	public override void onRemove()
	{
		entity.currentState = EntityState.NORMAL;

		// Return the entity's color to normal.
		entity.GetComponent<Renderer>().material.color = Color.white;

		// Reconfigure the entity's physics constraints.
		/*if(shouldSpin)
		{
			entity.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
			entity.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			
			Quaternion q = new Quaternion();
			q.eulerAngles = Vector3.zero;
			entity.transform.rotation = q;
		}*/
	}
}

/*
 * This element handles the 'chasing' behavior witnessed in enemies.
 */
public class ElementChase : Element
{
	Enemy enemy;
	float acceleration;
	
	Entity pursuer;
	Entity target;
	
	public ElementChase(Entity pursuer, Entity target, float acceleration)
	{
		this.pursuer = pursuer;
		this.target = target;
		this.acceleration = acceleration;
	}
	
	public override void update(float time_delta_fraction)
	{
		// Don't chase a stunned target.
		if(target.currentState == EntityState.STUNNED)
		{
			return;
		}
		
		Vector3 direction = (target.transform.position - pursuer.transform.position).normalized;
		pursuer.GetComponent<Rigidbody>().velocity += direction * acceleration;
	}
}

/*
 * This Element handles the Spin Attack behavior witnessed in enemies.
 */
public class ElementSpinAttack : Element
{
	Enemy enemy;
	float acceleration;
	
	Entity pursuer;
	Entity target;

	float timer = 360;
	
	public ElementSpinAttack(Entity pursuer, Entity target, float acceleration)
	{
		this.pursuer = pursuer;
		this.target = target;
		this.acceleration = acceleration;
	}

	public override void onActive()
	{
		pursuer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

		// An entity performing a spin attack, may not be damaged.
		pursuer.currentState = EntityState.INVINCIBLE;
		pursuer.GetComponent<Rigidbody>().mass = 10;
		pursuer.GetComponent<Renderer>().material.color = Color.blue;
		pursuer.transform.localScale = Vector3.one * 1.25f;
	}

	public override void update(float time_delta_fraction)
	{
		timer -= time_delta_fraction;
		if(timer <= 0)
			finished = true;

		// Spin Like Crazy.
		pursuer.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 10000);

		// Don't chase a stunned target.
		if(target.currentState == EntityState.STUNNED)
		{
			return;
		}

		Vector3 direction = (target.transform.position - pursuer.transform.position).normalized;
		pursuer.GetComponent<Rigidbody>().velocity += direction * acceleration;
	}

	public override void onRemove()
	{
		pursuer.GetComponent<Rigidbody>().mass = 1;
		pursuer.currentState = EntityState.NORMAL;
		pursuer.GetComponent<Renderer>().material.color = Color.white;
		pursuer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
		pursuer.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		
		Quaternion q = new Quaternion();
		q.eulerAngles = Vector3.zero;
		pursuer.transform.rotation = q;
		pursuer.transform.localScale = Vector3.one;
	}
}

