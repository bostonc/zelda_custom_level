using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy {

    public float fireballInterval = 2;
    public float fireballTimer = 0f;
    public GameObject projectilePrefab;
    public float fireballSpeed;
    public int remainingChargeFrames;
    public int ChargeForFrames = 5; //# of frames to show charge
    private float waitTime = 0f; 
    // Use this for initialization
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        Move();
        damageFlash();

        if (remainingChargeFrames > 0) {
            remainingChargeFrames--;
            if (remainingChargeFrames == 0) {
                UnShowDamage();
            }
        }
        ///ANIMATION CODE HERE STATIC FOR NOW 
        BossAttack();
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
                int rand = Random.Range(0, 100);
                if (rand % 2 == 0) {
                    target = Vector3.left;
                    transform.position = roundGrid(transform.position, "V");
                }

                if (rand % 2 == 1) {
                    target = Vector3.right;
                    transform.position = roundGrid(transform.position, "V");
                }
                navigation = false;

            }

            GetComponent<Rigidbody>().velocity = target * speed;

            int reset_rand = Random.Range(0, 100);

            ///randomly reset the nav bool to reset 2D movement, should happen 1% of frames 
            if (reset_rand > 98) {
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
            print(GetComponent<Rigidbody>().velocity);
            if (Time.time > knockbackTimer) {
                currentState = EntityState.NORMAL;

            }
        }
    }

    void BossAttack() {
        //print(fireballTimer);
       // print(waitTime);
        //print(Time.time);
        if (currentState == EntityState.NORMAL) {

            currentState = EntityState.ATTACKING;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            //print(fireballInterval);
            GameObject go1 = Instantiate(projectilePrefab) as GameObject;
            go1.tag = "ProjectileBoss";
            go1.layer = LayerMask.NameToLayer("ProjectileBoss");
            go1.transform.position = transform.position;
            Vector3 dir_vector = Vector3.left;
            Vector3 projectilePosition = transform.position + dir_vector * .4f;
            go1.transform.parent = gameObject.transform; //sets as parent of this object
            go1.transform.position = projectilePosition;

            GameObject go2 = Instantiate(projectilePrefab) as GameObject;
            go2.tag = "ProjectileBoss";
            go2.layer = LayerMask.NameToLayer("ProjectileBoss");
            go2.transform.position = transform.position;
            
            Vector3 projectilePosition1 = transform.position + dir_vector * .4f;
            go2.transform.parent = gameObject.transform; //sets as parent of this object
            go2.transform.position = projectilePosition;

            GameObject go3 = Instantiate(projectilePrefab) as GameObject;
            go3.tag = "ProjectileBoss";
            go3.layer = LayerMask.NameToLayer("ProjectileBoss");
            go3.transform.position = transform.position;
            
            Vector3 projectilePosition2 = transform.position + dir_vector * .4f;
            go3.transform.parent = gameObject.transform; //sets as parent of this object
            go3.transform.position = projectilePosition;

            GameObject target = GameObject.FindGameObjectWithTag("Player");

            //go1.transform.LookAt(target.transform);
           // go2.transform.LookAt(target.transform);
            //go3.transform.LookAt(target.transform);

            Vector3 go1_vector = PlayerController.S.transform.position - go1.transform.position;
            go1_vector.Normalize();

            Vector3 go2_vector = PlayerController.S.transform.position - go2.transform.position;
            go2_vector.Normalize();
            Vector3 go3_vector = PlayerController.S.transform.position - go3.transform.position;
            go3_vector.Normalize();

            go3.GetComponent<Rigidbody>().velocity = go3_vector * fireballSpeed;

            go1.GetComponent<Rigidbody>().velocity = (new Vector3(0, 0.8f, 0) + go3_vector) * fireballSpeed;
            
            go2.GetComponent<Rigidbody>().velocity = (new Vector3(0, -.8f, 0) + go2_vector) * fireballSpeed;
            ///timer should happen on boomerang return
            //fireballTimer = Time.time + fireballInterval;

            if (Time.time > waitTime) {
                waitTime = Time.time + fireballInterval + 2f;
                fireballTimer = Time.time + fireballInterval;
            }

            
        }

        if (Time.time < waitTime) {
            if (Time.time > fireballTimer) {
                //show charge animation
                //print("charge");
                ShowCharge();
            }
        }

        if (Time.time > fireballTimer && Time.time > waitTime) { 
            currentState = EntityState.NORMAL;
        }
    }


    void ShowCharge() {

        //if (materials[0].color != Color.red) {
            foreach (Material m in materials) {
                m.color = Color.cyan;
            }
           remainingChargeFrames = ChargeForFrames;
        //}
    }

    void UnShowDamage() {
        for (int i = 0; i < materials.Length; i++) {
            materials[i].color = originalColors[i];
        }
    }
}
