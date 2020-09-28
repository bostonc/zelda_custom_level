/*
 * An Element is a unit of behavior.
 * 
 * It is an organizational concept that simplifies Cutscenes, Attack chains, Enemy AI, and Player State, among other game concepts. It allows one to predictable and cleanly queue game behaviors.
 * 
 * There are two key concepts to the system...
 * (1) The Element
 * An Element defines behavior by way of its update(), onActive(), and onRemove() functions. An Element is responsible only for the behavior it defines.
 * For example, an Element called "ElementMoveToPositionOverTime" would implement the mentioned functions to move an object to a position over time.
 * An Element called "ElementPerformDanceAnimation" would implement the mentioned functions to animate an object to dance.
 * Creating these elements brings behavior into your game.
 * 
 * A handful of elements are provided for examination and use below.
 * 
 * (2) The Element Queue
 * Every object that makes use of Elements needs a queue to store and process them. Only one Element runs (IE, has its update() function called) at a time.
 * To add an element to the queue, use Element.addElement(<your_ele_queue>, <a_new_element>);
 * To Process an element queue (give it life) call Element.updateElements(<your_ele_queue>); every frame.
 * To insert a new element AT the current running element, causing the new element to run immediately, use Element.disruptElement(<ele_queue>, <new_ele>);
 * 
 * To create a cutscene, for example one might put the following elements into the Player's Element Queue.
 * ElementWarp(<Just left of screen>) -> ElementMoveOverTime(<middle of screen>, 3 seconds) -> ElementDance() -> ElementMoveOverTime(<just right of screen>, 3 seconds) -> ElementLoadScene("Stage 1");
 * This Queue of Elements would spawn the player off screen, make him walk to the middle, do a dance, walk off screen, then load the "Stage 1" scene.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element
{
	// Setting finished to true causes an Element to end.
	public bool finished = false;

	// Called when an element is added to the queue.
	public virtual void onAddedToQueue() {}

	// Called when an element begins to run.
	public virtual void onActive() {}

	// Called every frame that an element is active.
	public virtual void update(float time_delta_fraction) {}

	// Called every frame (with consistent time-interval) that an element is active.
	public virtual void fixedUpdate() {}

	// Called when an element is removed from the queue (ends).
	public virtual void onRemove() {}

	// Disrupt the current running element, causing it to end, and insert a new element into the active slot in the queue.
	public static void disruptElement(List<Element> ele_queue, Element ele)
	{
		if(ele_queue.Count > 0)
		{
			ele_queue[0].onRemove();
			ele_queue[0] = ele;
		}
		else
			ele_queue.Add(ele);
		
		ele_queue[0].onAddedToQueue();
		ele_queue[0].onActive();
	}

	// Add an element to the end of the queue.
	public static void addElement(List<Element> ele_queue, Element ele)
	{
		ele_queue.Add(ele);
		if(ele_queue.Count == 1)
			ele.onActive();
	}

	// Update the elements in the element queue.
	public static void updateElements(List<Element> ele_queue)
	{
		Application.targetFrameRate = 60;
		if(ele_queue.Count > 0)
		{
			float time_delta_fraction = Time.deltaTime / (1.0f / Application.targetFrameRate);
			ele_queue[0].update(time_delta_fraction);
			
			if(ele_queue[0].finished)
			{
				ele_queue[0].onRemove();
				ele_queue.RemoveAt(0);
				if(ele_queue.Count > 0)
					ele_queue[0].onActive();
			}
		}
	}

	// Update the elements in the element queue (fixed time interval).
	public static void fixedUpdateElements(List<Element> ele_queue)
	{
		if(ele_queue.Count > 0)
		{
			ele_queue[0].fixedUpdate();
			
			if(ele_queue[0].finished)
			{
				ele_queue[0].onRemove();
				ele_queue.RemoveAt(0);
				if(ele_queue.Count > 0)
					ele_queue[0].onActive();
			}
		}
	}
}

// An element that does nothing but wait for 'life' frames to pass.
public class ElementNoop : Element
{
	float life = 0;
	
	public ElementNoop(float life)
	{ 
		this.life = life; 
	}
	
	public override void update(float time_delta_fraction)
	{
		life -= time_delta_fraction;
		if(life <= 0)
			finished = true;
	}
}

public class ElementMoveOverTime : Element
{
	float total_life = 0;
	float life = 0;
	GameObject my_object;
	Vector3 destination;
	Vector3 initial_position;

	// Local determines whether the dest and initial vectors specified are local positions or absolute positions.
	bool local;

	public ElementMoveOverTime(float total_life, Vector3 dest, Vector3 initial, GameObject my_object, bool local)
	{ 
		this.total_life = total_life;
		this.my_object = my_object;
		this.destination = dest;
		this.initial_position = initial;
		this.local = local;
	}
	
	public override void update(float time_delta_fraction)
	{
		life += time_delta_fraction;
		float ratio = (float)life / total_life;

		if(local)
			my_object.transform.localPosition = Vector3.Lerp(initial_position, destination, ratio);
		else
			my_object.transform.position = Vector3.Lerp(initial_position, destination, ratio);

		if(ratio >= 1.0f)
			finished = true;
	}
}

// An element that eases an object into a position.
public enum MovementType {ABSOLUTE, LOCAL, UIABSOLUTE, UILOCAL};
public class ElementMoveEase : Element
{
	float easeFactor = 0;
	GameObject my_object;
	Vector3 destination;
	Vector3 initial_position;
	MovementType movementType;

	public ElementMoveEase(Vector3 dest, float easeFactor, GameObject my_object, MovementType movementType)
	{ 
		this.my_object = my_object;
		this.destination = dest;
		this.easeFactor = easeFactor;
		this.movementType = movementType;
	}
	
	public override void update(float time_delta_fraction)
	{
		Vector3 diff = Vector3.zero;
		
		if(movementType == MovementType.LOCAL)
		{
			diff = (destination - my_object.transform.localPosition) * easeFactor;
			my_object.transform.localPosition += diff;
		}
		else if(movementType == MovementType.ABSOLUTE)
		{
			diff = (destination - my_object.transform.position) * easeFactor;
			my_object.transform.position += diff;
		} else if(movementType == MovementType.UIABSOLUTE)
		{
			Vector2 desiredPosition = new Vector2(destination.x, destination.y);
			Vector2 diff2 = (desiredPosition - my_object.GetComponent<RectTransform>().anchoredPosition) * easeFactor;
			my_object.GetComponent<RectTransform>().anchoredPosition += diff2;
			diff = new Vector3(diff2.x, diff2.y, 0);
		}

		if(diff.magnitude <= 0.01f)
			finished = true;
	}
}

// An Element that instantly moves an object to a destination location, and gives it a particular direction.
public class ElementWarpObject : Element
{
	GameObject my_object;
	Vector3 destination;
	Quaternion direction;
	MovementType movementType;

	public ElementWarpObject(Vector3 dest, Vector3 dir, GameObject my_object, MovementType movementType)
	{ 
		this.my_object = my_object;
		this.destination = dest;
		Quaternion q = new Quaternion();
		q.eulerAngles = dir;
		direction = q;
		this.movementType = movementType;
	}
	
	public override void onActive()
	{
		if(movementType == MovementType.ABSOLUTE)
			my_object.transform.localPosition = destination;
		else if(movementType == MovementType.LOCAL)
			my_object.transform.position = destination;
		else if(movementType == MovementType.UIABSOLUTE)
			my_object.GetComponent<RectTransform>().anchoredPosition = destination;
		my_object.transform.rotation = direction;
		finished = true;
	}
}

// An Element that Loads a scene when it becomes active.
public class ElementLoadScene : Element
{
	string scene_name;
	
	public ElementLoadScene(string scene_name)
	{ 
		this.scene_name = scene_name;
	}
	
	public override void onActive()
	{
		Application.LoadLevel(scene_name);
	}
}
