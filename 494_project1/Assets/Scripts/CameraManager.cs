using UnityEngine;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour {

    List<Element> elementQueue = new List<Element>();

    public static CameraManager instance;

    // Use this for initialization
    void Start() {
        // Establish the singleton.
        instance = this;

        // Then begin following Camera Points for the remainder of the game.
        Element.addElement(elementQueue, new ElementFollowCameraPoints(gameObject));
    }

    // Update is called once per frame
    void Update() {
        Element.updateElements(elementQueue);
    }
}


 /// This element compels the camera to move to the Camera Point nearest the player.
 // It is the true "Gameplay Mode" of the Camera.

public class ElementFollowCameraPoints : Element {
    GameObject cam;

    public ElementFollowCameraPoints(GameObject cam) {
        this.cam = cam;
    }

    public override void update(float time_delta_fraction) {
        // Acquire the Camera Point nearest the player.
        GameObject targetPoint = CameraPoint.GetCurrentCameraPoint();

        // Calculate the displacement the camera should make.
        Vector3 dp = (targetPoint.transform.position - cam.transform.position) * 0.02f;
        dp = new Vector3(dp.x, dp.y, 0);

        // Apply the calculated displacement.
        cam.transform.position += dp * time_delta_fraction * 1.5f ;
    }
} 