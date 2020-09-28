/*
 * Limited Lifetime component destroys an object after a number of frames.
 * 
 * When the number of remaining frames falls below 30, the object will be shrunk
 * until it disappears-- a pleasing, reusable animation.
 */

using UnityEngine;
using System.Collections;

public class LimitedLifetime : MonoBehaviour {

	public int framesToLive = 180;

	// Remember the starting scale of our object to help with Lerping.
	Vector3 scaleAtVanishStart = Vector3.one;

	// Update is called once per frame
	void FixedUpdate () {
		framesToLive --;

		if(framesToLive <= 0)
			Destroy(gameObject);

		if(framesToLive <= 30)
		{
			if(framesToLive == 30)
			{
				scaleAtVanishStart = transform.localScale;
			}

			// Shrink the object.
			Vector3 newScale = Vector3.Lerp(Vector3.zero, scaleAtVanishStart, framesToLive / 30.0f);
			transform.localScale = newScale;
		}
	}
}
