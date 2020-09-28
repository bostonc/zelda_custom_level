using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallmaster : Enemy {

    public Transform[] wayPointList;
    public int currentWayPoint = 0;
    Transform targetWayPoint;
    public float activationDistance = .002f;
    bool activated = false;
    Vector3 startposition;
    private void Awake() {
        startposition = transform.position;
        enabled = true;
    }

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Walk() {
        // rotate towards the target
        //.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.position - transform.position, speed * Time.deltaTime, 0.0f);

        // move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, speed * Time.deltaTime);

        if (transform.position == targetWayPoint.position) {
            currentWayPoint++;
            if(currentWayPoint == this.wayPointList.Length) {
                currentWayPoint = 0;
            }
            targetWayPoint = wayPointList[currentWayPoint];
        }
    }
    public override void Move() {

        if (!activated) {
            if (distanceFromPlayer() > activationDistance) {
                //don't move
                activated = false;
                enabled = true;
                return;
            }
        }

        activated = true;
        if(Utils.vectorIsSimilar(PlayerController.S.transform.position,transform.position, .5f)) {
            print("stopping link");
            PlayerController.S.transform.position = transform.position;
            Invoke("MovePlayer", 2f);
        }
        //check if there is a waypoint to walk to
        if (currentWayPoint < this.wayPointList.Length) {
            if (targetWayPoint == null)
                targetWayPoint = wayPointList[currentWayPoint];
            Walk();
        }
    }

    float distanceFromPlayer() {
        return Vector3.Distance(PlayerController.S.transform.position, gameObject.transform.position);
    }

   void MovePlayer() {
        PlayerController.S.transform.position = transform.position = new Vector3(39.5f, 6f, 0);
        transform.position = startposition;
        enabled = false;
        Destroy(this.gameObject);
    }


}
