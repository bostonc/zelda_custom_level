/*
 * A floaty projectile!
 * 
 * This component controls the movement of the bubble projectile Game Object.
 */

using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour {

	// These two members are used to supply the bubbles with their floaty, sin-wave-like animation.
	float sinWaveCounter = 0.0f;
	public float sinWaveCounterRate = 0.1f;

	// We need to track the initial movement of the bubble, so it doesn't get lost
	// when we add-in the sin-wave movement.
	public Vector3 initialVelocity = Vector3.zero;

	// Use this for initialization
	void Start () {
		// Randomizing the initial value of the counter randomizes the progress of the bubble
		// in its sin-wave arc.
		sinWaveCounter = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);

		// Random scaling is an easy way to add variety to objects in a game.
		transform.localScale = Vector3.one * UnityEngine.Random.Range(1.0f, 1.5f);
	}
	
	// Update is called once per frame
	void Update () {
		// Progress the counter. This progresses the sin-wave animation.
		sinWaveCounter += sinWaveCounterRate;

		// Acquire the perpendicular vector.
		Vector3 perpendicularVector = Vector3.Cross(initialVelocity, new Vector3(0, 0, 1)).normalized;

		// Calculate the perpendicular velocity.
		Vector3 perpendicularVelocity = perpendicularVector * Mathf.Sin(sinWaveCounter);

		// Net velocity is the initial Velocity added to the perpendicular velocity.
		Vector3 netVelocity = initialVelocity + perpendicularVelocity;
		GetComponent<Rigidbody>().velocity = netVelocity;
	}
}
