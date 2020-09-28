
 /* Camera Points tell the Camera where to move to!
 * 
 * As the player navigates the environment, Camera Points help the Camera Manager decide what room to look at.
 * The Camera Point that is nearest the player is the one that the Camera Manager will focus on.
 * Camera Points may be placed in the environment to control the camera. */


using UnityEngine;
using System.Collections.Generic;
using System;

using UnityEngine.Assertions;

public class CameraPoint : MonoBehaviour {

    // A static list for keeping track of all existent Camera Points.
    public static List<GameObject> cameraPoints = new List<GameObject>();
    // Use this for initialization
    void Awake() {
        // A Camera Point registers itself when it instantiates.
        cameraPoints.Add(gameObject);
    }

    
	 // This publicly-available static function may be utilized to acquire the Camera Point nearest the Player. 
	 
    public static GameObject GetCurrentCameraPoint() {
        // Error condition

        GameObject closestCameraPoint = cameraPoints[0];
        if (cameraPoints.Count <= 0) {

            //return closestCameraPoint;
            throw new Exception("No Camera Points exist.");
        }

        // Search for the closest Camera Point.
        //GameObject closestCameraPoint = cameraPoints[0];
        float closestDistance = 9999999;
        foreach (GameObject c in cameraPoints) {
            //sAssert.IsNotNull(PlayerController.S);
            Assert.IsNotNull(cameraPoints);
            //print(cameraPoints.Count);
            Assert.IsNotNull(c);
            float dist = Vector3.Distance(c.transform.position, PlayerController.S.transform.position);
            if (dist < closestDistance) {
                
                closestDistance = dist;
                closestCameraPoint = c;
                //print(closestCameraPoint.name);
            }
        }

        // Return the search result.
        return closestCameraPoint;
    }
} 

