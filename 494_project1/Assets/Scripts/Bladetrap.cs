using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bladetrap : MonoBehaviour { 
    private Vector3 startingPosition;
    private Vector3 starting1Position;
    private Vector3 starting0Position;
    private bool trapActivated = false;
    private bool trapDeactivated = false;
    public float speed = 1f;

    public bool leftright = false;
	// Use this for initialization
	void Start () {
        startingPosition = transform.position;

        starting0Position = transform.GetChild(0).position;
        starting1Position = transform.GetChild(1).position;

        //print(starting0Position);
        //print(starting1Position);
    }
	
	// Update is called once per frame
	void Update () {
        if (trapActivated) {

            //print("POOP");
            //float step = speed * Time.deltaTime;
            if (leftright) {
                transform.GetChild(0).gameObject.GetComponent<Rigidbody>().velocity = Vector3.right * speed;
                transform.GetChild(1).gameObject.GetComponent<Rigidbody>().velocity = Vector3.left * speed;
            } else {
                transform.GetChild(0).gameObject.GetComponent<Rigidbody>().velocity = Vector3.down * speed;
                transform.GetChild(1).gameObject.GetComponent<Rigidbody>().velocity = Vector3.up * speed;
            }
            


            //transform.GetChild(0).position = Vector3.MoveTowards(transform.GetChild(1).position, 
               // starting0Position, step);

            if(Utils.vectorIsSimilar(transform.GetChild(0).position, transform.GetChild(1).position, 1f)) {
                trapDeactivated = true;
                trapActivated = false;
                
            }
        }

        if (trapDeactivated) {

             float trapspeed = speed / 3f;
            if (leftright){
                transform.GetChild(0).gameObject.GetComponent<Rigidbody>().velocity = Vector3.left* trapspeed;
                transform.GetChild(1).gameObject.GetComponent<Rigidbody>().velocity = Vector3.right * trapspeed;
            }else {
                transform.GetChild(0).gameObject.GetComponent<Rigidbody>().velocity = Vector3.up * trapspeed;
                transform.GetChild(1).gameObject.GetComponent<Rigidbody>().velocity = Vector3.down * trapspeed;
            } 
            

            if (Utils.vectorIsSimilar(transform.GetChild(0).position, starting0Position, .5f)) {
                print("stop");
                transform.GetChild(0).gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.GetChild(1).gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.GetChild(0).position = starting0Position;
                transform.GetChild(1).position = starting1Position;
                trapDeactivated = false;
                
            }
        }
	}

    private void OnTriggerEnter(Collider coll) {
        if(coll.gameObject.tag == "Player") {
            ActivateTrap();
        }
    }

    void ActivateTrap() {
        trapActivated = true;
    }



}
