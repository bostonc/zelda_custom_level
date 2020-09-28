using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keese : Enemy {
    private float slimeTime;
    private float waitTime;

    // Use this for initialization
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        slimeTime = Time.time;
        waitTime = Time.time;
    }

    // Update is called once per frame
    void Update() {

    }

    override public void Move() {
        ///Implements random movement
        ///

        if (currentState != EntityState.KNOCKBACK && currentState != EntityState.STUNNED) {
            ////Implement speed based movement
            if (navigation) {
                //Rounds the position to the grid on change so that enemy is always in place
                ///multiplying by 2 and then rounding sets enemy to the closest .5
                // print("nav");

                int rand = Random.Range(0, 7);
                if (rand == 0) {
                    target = new Vector3(1,1,0);
                    //transform.position = roundGrid(transform.position, "V");
                }

                if (rand == 1) {
                    target = new Vector3(1,-1,0);
                    //transform.position = roundGrid(transform.position, "V");
                }

                if (rand == 2) {
                    target = new Vector3(-1,-1,0);
                    // transform.position = roundGrid(transform.position, "H");
                }
                if (rand == 3) {
                    target = new Vector3(-1, 1, 0);
                    //transform.position = roundGrid(transform.position, "H");
                }
                if (rand == 4) {
                    target = new Vector3(1, .5f, 0);
                    //transform.position = roundGrid(transform.position, "V");
                }

                if (rand == 5) {
                    target = new Vector3(1, -.5f, 0);
                    //transform.position = roundGrid(transform.position, "V");
                }

                if (rand == 6) {
                    target = new Vector3(-1, -.5f, 0);
                    // transform.position = roundGrid(transform.position, "H");
                }
                if (rand == 7) {
                    target = new Vector3(-1, .5f, 0);
                    //transform.position = roundGrid(transform.position, "H");
                }
                navigation = false;

            }

            if (Time.time < slimeTime) {
                GetComponent<Rigidbody>().velocity = target * speed;
            }

            if(Time.time > slimeTime) {
                //print("rest");
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            int reset_rand = Random.Range(0, 100);

            ///randomly reset the nav bool to reset 2D movement, should happen 1% of frames 
            if (reset_rand > 98) {
                //print("update");
                navigation = true;
            }

            if (Time.time > waitTime) {
                waitTime = Time.time + Random.Range(2.5f,3f);
                slimeTime = Time.time + Random.Range(1.5f,2f);
            }

        } else if (currentState == EntityState.STUNNED) {
            //print("stunned");
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (Time.time > stunTime) {
               // print("POOP");
                navigation = true;
                currentState = EntityState.NORMAL;
            }
        } else { //// need to add stuff here as elseif
                 ///else player is locked into knockback

            GetComponent<Rigidbody>().velocity = knockbackDir * force;
            //print("knockbackupdating");
            print(GetComponent<Rigidbody>().velocity);
            if (Time.time > knockbackTimer) {
                currentState = EntityState.NORMAL;

            }
        }

    }
}