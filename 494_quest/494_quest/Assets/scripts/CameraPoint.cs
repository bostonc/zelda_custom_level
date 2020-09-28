/*
 * Camera Points tell the Camera where to move to!
 * 
 * As the player navigates the environment, Camera Points help the Camera Manager decide what room to look at.
 * The Camera Point that is nearest the player is the one that the Camera Manager will focus on.
 * Camera Points may be placed in the environment to control the camera.
 */

using UnityEngine;
using System.Collections.Generic;
using System;

public class CameraPoint : MonoBehaviour {

	// A static list for keeping track of all existent Camera Points.
	public static List<GameObject> cameraPoints = new List<GameObject>();

	// Use this for initialization
	void Awake () {
		// A Camera Point registers itself when it instantiates.
		cameraPoints.Add(gameObject);
	}

	/*
	 * This publicly-available static function may be utilized to acquire the Camera Point nearest the Player.
	 */
	public static GameObject GetCurrentCameraPoint()
	{
		// Error condition
		if(cameraPoints.Count <= 0)
		{
			throw new Exception("No Camera Points exist.");
		}

		// Search for the closest Camera Point.
		GameObject closestCameraPoint = cameraPoints[0];
		float closestDistance = 9999999;
		foreach(GameObject c in cameraPoints)
		{
			float dist = Vector3.Distance(c.transform.position, Player.instance.transform.position);
			if(dist < closestDistance)
			{
				closestDistance = dist;
				closestCameraPoint = c;
			}
		}

		// Return the search result.
		return closestCameraPoint;
	}
}
