using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy {

    private float slimeTime;
    private float waitTime;
	// Use this for initialization
	void Start ()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        slimeTime = Time.time;
        waitTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    override public void Move() {
        ///Implements random movement
        ///
        //print(currentState);
        //print(Time.time);
        //print(stunTime);
        if (currentState != EntityState.KNOCKBACK && currentState != EntityState.STUNNED) {
            ////Implement speed based movement
            if (navigation) {
                //Rounds the position to the grid on change so that enemy is always in place
                ///multiplying by 2 and then rounding sets enemy to the closest .5
               // print("nav");
                int rand = Random.Range(0, 3);
                if (rand == 0) {
                    target = Vector3.down;
                    transform.position = roundGrid(transform.position, "V");
                }

                if (rand == 1) {
                    target = Vector3.up;
                    transform.position = roundGrid(transform.position, "V");
                }

                if (rand == 2) {
                    target = Vector3.right;
                    transform.position = roundGrid(transform.position, "H");
                }
                if (rand == 3) {
                    target = Vector3.left;
                    transform.position = roundGrid(transform.position, "H");
                }
                navigation = false;

            }
            if (Time.time < slimeTime) {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target + transform.position, step);
            }
            int reset_rand = Random.Range(0, 100);

            ///randomly reset the nav bool to reset 2D movement, should happen 1% of frames 
            if (reset_rand > 98) {
                //print("update");
                navigation = true;
            }
           
            if (Time.time > waitTime) {
                waitTime = Time.time + 2f;
                slimeTime = Time.time + 1f; 
            }

        } else if (currentState == EntityState.STUNNED) {
            //print("stunned");
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (Time.time > stunTime) {
                //print("POOP");
                navigation = true;
                currentState = EntityState.NORMAL;
            }
        } else { //// need to add stuff here as elseif
                 ///else player is locked into knockback

            GetComponent<Rigidbody>().velocity = knockbackDir * force;
            //print("knockbackupdating");
            //print(GetComponent<Rigidbody>().velocity);
            if (Time.time > knockbackTimer) {
                currentState = EntityState.NORMAL;

            }
        }

    }


}
