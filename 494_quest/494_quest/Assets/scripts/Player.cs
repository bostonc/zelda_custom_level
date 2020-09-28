/*
 * The Player component manages the data and behavior of the Player.
 * 
 * It does this by maintaining and processing an Element Queue for the Player.
 * This implementation relies on their being only one player at a time.
 */

using UnityEngine;
using System.Collections.Generic;

public class Player : Entity {

	// How fast the player moves.
	public float movementVelocity = 2.0f;

	// Globally accessible count of the player's rupees.
	public static int rupeeCount = 0;

	// Globally accessible vector representing the player's facing direction.
	public static Vector3 playerDirection = Vector3.right;

	// A prefab of the bubble projectile. This is needed to instantiate bubbles.
	public GameObject bubblePrefab;

	// Determines how fast the bubble projectiles fly when they are launched.
	public float projectileBaseVelocity;

	// A global singleton allowing more intricate access to the Player's data from external code.
	public static Player instance;

	// The element queue for the Player. It is filled with elements to provide
	// behavior to the player.
	List<Element> elementQueue = new List<Element>();

	// This function is executed after Awake() is called in the parent (Entity) class.
	protected override void PostAwake()
	{
		instance = this;
	}

	void Start()
	{
		health = initialHealth;

		// Refresh the HUD to show the altered health status.
		Hud.RefreshDisplay();
	}
	
	void Update()
	{
		// If no elements are pending, allow the player their standard movement.
		if(elementQueue.Count == 0)
			Element.addElement(elementQueue, new ElementStandardMovement(this));

		// Update the elements to provide behavior.
		Element.updateElements(elementQueue);
	}

	void FixedUpdate()
	{
		// Update the elements that require a fixed-interval.
		Element.fixedUpdateElements(elementQueue);
	}
	
	void OnCollisionEnter(Collision coll)
	{
		// We only care if we collide with enemies.
		// Tags may be set for gameobjects at the top of the inspector.
		if(coll.gameObject.tag == "Enemy")
		{
			// An entity may be in a number of different states.
			// We only care if we are in a normal state.
			if(currentState == EntityState.NORMAL)
			{
				// Disrupt whatever element is currently running, and put a Stunned element
				// in its place.
				Element.disruptElement(elementQueue, new ElementStunned(this, 120, true));

				health --;

				// If the player is out of health, change the scene.
				if(health <= 0)
					Application.LoadLevel("GameOver");

				// Refresh the HUD to display these Status changes.
				Hud.RefreshDisplay();
			}
		}
	}
}

/*
 * This element is responsible for Standard, non-stunned player control.
 */
public class ElementStandardMovement : Element
{
	Player player;
	public ElementStandardMovement (Player player)
	{
		this.player = player;
	}
	
	public override void update(float time_delta_fraction)
	{
		// Attack
		if(Input.GetAxis("PrimaryAttack") > 0 && UnityEngine.Random.Range(0.0f, 1.0f) > 0.75f)
		{
			// Calculate a random offset in the direction perpendicular to the direction of the bubbles final velocity.
			Vector3 randomOffset = UnityEngine.Random.onUnitSphere * 0.2f;
			randomOffset = new Vector3(randomOffset.x, randomOffset.y, 0);

			// Instantiate the actual bubble object.
			Vector3 projectilePosition = player.transform.position + Player.playerDirection * 0.4f + randomOffset;
			GameObject newBubble = MonoBehaviour.Instantiate(player.bubblePrefab, projectilePosition, Quaternion.identity) as GameObject;
			newBubble.GetComponent<Bubble>().initialVelocity = Player.playerDirection * player.projectileBaseVelocity + player.GetComponent<Rigidbody>().velocity;
		}

		// Handle the graphical representation of the Player.
		Animate();
	}

	public override void fixedUpdate()
	{
		// Input for your game is managed in the Input Project settings.
		// Find them at... Edit -> Project Settings -> Input.
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		
		// Restrict movement to one simoultaneous axis.
		if(Mathf.Abs(horizontalInput) > 0)
			verticalInput = 0;
		
		if(horizontalInput != 0 || verticalInput != 0)
		{
			// "Clamp" rounds a value.
			Player.playerDirection = new Vector3(Mathf.Clamp(horizontalInput, -1, 1), 
			                                     Mathf.Clamp(verticalInput, -1, 1));
		}

		// Apply the velocity.
		player.GetComponent<Rigidbody>().velocity = new Vector3(horizontalInput, verticalInput, 0) * player.movementVelocity;
	}

	void Animate()
	{
		// Make the Player sprite look left or right.
		if(Player.playerDirection.x > 0)
			player.transform.localScale = new Vector3(-1, 1, 1);
		else if(Player.playerDirection.x < 0)
			player.transform.localScale = new Vector3(1, 1, 1);
	}
}



