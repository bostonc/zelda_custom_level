/*
 * As I see it, there are three known ways to perform animation in Unity.
 * 
 * (1) Use the built-in Unity Animator and Animation objects. Unity provides convenient editor UIs for these,
 * and they are the 'default' way of animating objects and their properties over time.
 * 
 * (2) Use physics for animation. By influencing an object's rigidbody at runtime, natural, dynamic, and interesting
 * animations may be created with minimal effort. The results achieved, however, may be somewhat unpredictable due to
 * the inherent complexity of the physics engine.
 * 
 * (3) Use a frame-based scripting approach (As seen here). For simple animations, it can be quick and convenient (though less scalable) to
 * simply write a script (like this one) that iterates a frame counter, and alters properties based on said counter.
 * Please see below for an example of this-- it performs a text-based victory dance.
 */

using UnityEngine;

public class VictoryTextAnimation : MonoBehaviour {

	int frames = 0;
	public int frameRate = 1;

	// Use this for initialization
	void Start () {
		GetComponent<TextMesh>().text = "A winner is you!\n<(^.^)>";
	}
	
	void FixedUpdate () {
		frames += frameRate;

		// Reset animation loop.
		if(frames >= 120)
			frames = 0;

		Animate();
	}

	void Animate()
	{
		if(frames < 30)
		{
			GetComponent<TextMesh>().text = "A winner is you!\n<(^.^)>";
			GetComponent<TextMesh>().alignment = TextAlignment.Center;
		}
		else if (frames < 60)
		{
			GetComponent<TextMesh>().text = "A winner is you!\n<(^o^<)";
			GetComponent<TextMesh>().alignment = TextAlignment.Left;
		}
		else if (frames < 90)
		{
			GetComponent<TextMesh>().text = "A winner is you!\n<(^.^)>";
			GetComponent<TextMesh>().alignment = TextAlignment.Center;
		}
		else if (frames < 120)
		{
			GetComponent<TextMesh>().text = "A winner is you!\n(>^o^)>";
			GetComponent<TextMesh>().alignment = TextAlignment.Right;
		}
	}
}
