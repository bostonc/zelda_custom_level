using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goriya : Enemy {

    bool boomerangisActive = false;
    public Sprite[] goriya_run_down;
    public Sprite[] goriya_run_up;
    public Sprite[] goriya_run_right;
    public Sprite[] goriya_run_left;

    public GameObject projectilePrefab; 

    private float boomerangInterval = 0f;
    private float boomerangTimer = 0;
    //StateMachine animation_state_machine;
   
    carDirection current_direction = carDirection.EAST;
	// Use this for initialization
	void Start ()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        boomerangInterval = Random.Range(3f, 5f);
	}
	
	// Update is called once per frame
	void Update () {
        Move();
        damageFlash();
        ///ANIMATION CODE HERE STATIC FOR NOW 
        Attack();
        //print(boomerangInterval);
        //print(currentState);
	}

    override public void Move() {
        ///Implements random movement for goriya

        if (currentState != EntityState.KNOCKBACK && currentState != EntityState.STUNNED) {
            ////Implement speed based movement
            if (target == Vector3.down) {
                current_direction = carDirection.SOUTH;
                GetComponent<SpriteRenderer>().sprite = goriya_run_down[0];
            } else if (target == Vector3.up) {
                current_direction = carDirection.NORTH;
                GetComponent<SpriteRenderer>().sprite = goriya_run_up[0];
            } else if (target == Vector3.right) {
                current_direction = carDirection.EAST;
                GetComponent<SpriteRenderer>().sprite = goriya_run_right[0];
            } else if (target == Vector3.left) {
                current_direction = carDirection.WEST;
                GetComponent<SpriteRenderer>().sprite = goriya_run_left[0];
            }
    


            if (navigation) {
                //Rounds the position to the grid on change so that enemy is always in place
                ///multiplying by 2 and then rounding sets enemy to the closest .5
               // print("nav");
                int rand = Random.Range(0, 3);
                if (rand == 0) {
                    
                    target = Vector3.down;
                    transform.position = roundGrid(transform.position, "V");
                    current_direction = carDirection.SOUTH;
                }

                if (rand == 1) {
                    target = Vector3.up;
                    transform.position = roundGrid(transform.position, "V");
                    current_direction = carDirection.NORTH;
                }

                if (rand == 2) {
                    target = Vector3.right;
                    transform.position = roundGrid(transform.position, "H");
                    current_direction = carDirection.EAST;
                }
                if (rand == 3) {
                    target = Vector3.left;
                    transform.position = roundGrid(transform.position, "H");
                    current_direction = carDirection.WEST;
                }
                navigation = false;

            }

            GetComponent<Rigidbody>().velocity = target * speed;

            int reset_rand = Random.Range(0, 1000);

            ///randomly reset the nav bool to reset 2D movement, should happen 1% of frames 
            if (reset_rand > 990) {
                //print("update");
                navigation = true;
            }

        } else if (currentState == EntityState.STUNNED) {
            //print("stunned");
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (Time.time > stunTime) {
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

    void Attack() {
        
        if(currentState == EntityState.NORMAL) {
        
            currentState = EntityState.ATTACKING;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            //print(boomerangInterval);
            GameObject go = Instantiate(projectilePrefab) as GameObject;
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
            go.transform.position = transform.position;
            Vector3 dir_vector = Vector3.zero;

            if(current_direction== carDirection.SOUTH) {
                dir_vector = Vector3.down;
            }else if(current_direction == carDirection.NORTH) {
                dir_vector = Vector3.up;
            } else if (current_direction == carDirection.WEST) {
                dir_vector = Vector3.left;
            } else if (current_direction == carDirection.EAST) {
                dir_vector = Vector3.right;
            }

            Vector3 projectilePosition = transform.position + dir_vector * .4f;
            go.transform.parent = this.gameObject.transform; //sets as parent of this object
            go.transform.position = projectilePosition;
            
            go.GetComponent<Rigidbody>().velocity = dir_vector * 4.5f;
            
            ///timer should happen on boomerang return
            boomerangTimer = Time.time + boomerangInterval; 
        }

        if (Time.time > boomerangTimer && 
           (currentState != EntityState.STUNNED && currentState !=EntityState.KNOCKBACK ) ) { 
            //int("goriya to normal");
                //if the boomerang is back to the goriya
            currentState = EntityState.NORMAL;
        }
    }
}
