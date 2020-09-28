using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    public float speed = 5f;
    public float health = 3;
    public int showDamageForFrames = 2;
    public float damageDelay = .25f; //period of invincibility after taking damage


    public float shiftTargetDelay = 5f; //seconds before AI picks new target location    
    public float moveDifference = .2f; //how similar a positions must be for skeleton to realize it has hit a wall
    public float knockbackDelay = 1f;

    public bool dropKey = false;
    public bool dropRupee = false;
    public bool dropBomb = false;
    public bool dropHeart = false;

    public AudioClip hitSound;
    public AudioClip dieSound;

    //sword knockback
    public float force = 10f;

    public bool _____________________________;
    public AudioSource audioSource;

    public float lastDamaged; //last time damage was taken
    public EntityState currentState = EntityState.NORMAL; //State of enemy 
    //movement
    public Vector3 lastPos; //position at previous frame
    public Vector3 newPos; //temp variable
    public float lastTargetShift; //last time target was shifted
    public Vector3 target; //direction to which the AI is trying to move

    public static Vector3 enemyDirection = Vector3.right;

    /// <summary>
    /// movement variables 
    /// </summary>
    public bool navigation = true;
    public float knockbackTimer = 0f;
    public Vector3 knockbackDir;
    public float stunTime;

    //damage flash
    public Color[] originalColors;
    public Material[] materials;
    public int remainingDamageFrames = 5;

    private Vector3 startingPosition;
    private float roamRadius = 3;

    void Awake() {
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++) {
            originalColors[i] = materials[i].color;
        }
        enabled = false;
    }

    // Use this for initialization
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        //target = acquireTarget();
        //currDirection = randomDirection(); //pick random starting direction
    }

    // Update is called once per frame
    void FixedUpdate() {
        Move();
        damageFlash();

        
    }

    public virtual void Move() {
        ///Implements random movement
        ///


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
           // print(GetComponent<Rigidbody>().velocity);
            if (Time.time > knockbackTimer) {
                currentState = EntityState.NORMAL;

            }
        }

    }

    public Vector3 roundGrid(Vector3 pos, string dir) {

        if (dir == "H") {
            float gridmovementy = pos.y * 4;
            float yRounded = Mathf.Round(gridmovementy);
            yRounded = yRounded / 4;

            return new Vector3(pos.x, yRounded, 0);
        } else {
            float gridmovementx = pos.x * 4;
            float xRounded = Mathf.Round(gridmovementx);
            xRounded = xRounded / 4;
            return new Vector3(xRounded, pos.y, 0);
        }
    }

    public void takeDamage(float n)
    {
        if (Time.time - lastDamaged < damageDelay) return;
        health -= n;
        lastDamaged = Time.time;
        ShowDamage();
        if (health <= 0) {
            //Destroy(this.gameObject);
            Main.S.EnemyDestroyed(this);
            LimitedLifetime l = gameObject.AddComponent<LimitedLifetime>();
            l.framesToLive = 30;

            if (audioSource != null) audioSource.PlayOneShot(dieSound);
        }
        if (audioSource != null) audioSource.PlayOneShot(hitSound);
    }

    public void damageFlash() {
        if (remainingDamageFrames > 0) {
            remainingDamageFrames--;
            if (remainingDamageFrames == 0) {
                UnShowDamage();
            }
        }
    }

    void ShowDamage() {
        foreach (Material m in materials) {
            m.color = Color.red;
        }
        remainingDamageFrames = showDamageForFrames;
    }

    void UnShowDamage() {
        for (int i = 0; i < materials.Length; i++) {
            materials[i].color = originalColors[i];
        }
    }

    void OnCollisionEnter(Collision coll) {

        // We only care about collisions with bubble projectiles.
        // Tags may be set for gameobjects near the top of the inspector.
        if (coll.gameObject.tag == "Sword" || coll.gameObject.tag == "Pickaxe") {

          // print("hit");
            // If this enemy is invincible no damage 
            takeDamage(1f);//sword does one damage 
           
            if (currentState == EntityState.NORMAL || currentState == EntityState.ATTACKING) {
                if (gameObject.tag != "Boss") {
                   // print("knockback");
                    // Calculate Angle Between the collision point and the player
                    Vector3 dir = (coll.transform.position - transform.position).normalized;
                    // We then get the opposite (-Vector3) and normalize it
                    // print(dir);
                    ///have the calcuation only go in most direction
                    if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) {
                        dir.y = 0;
                    } else if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x)) {
                        dir.x = 0;
                    }
                    dir = -dir;
                    knockbackDir = new Vector3(Mathf.Clamp(dir.x, -1, 1), Mathf.Clamp(dir.y, -1, 1));
                    // And finally we add force in the direction of dir and multiply it by force. 
                    // This will push back the enemy
                    // GetComponent<Rigidbody>().AddForce(dir * force);
                    GetComponent<Rigidbody>().velocity = knockbackDir * force;

                    currentState = EntityState.KNOCKBACK;
                    knockbackTimer = Time.time + knockbackDelay;
                }
            }

        }

        //add anything here that is solid!! 
        if (coll.gameObject.tag == "LockedDoor" || coll.gameObject.tag == "Solid" || 
            coll.gameObject.tag == "Stalfos" || coll.gameObject.tag == "Wall") { 

            //print("bump");
            float dot = Vector3.Dot(target, coll.gameObject.transform.position - transform.position);
            if (dot > 0) { //If enemy is walking twoards the object it hit
                target *= -1; //reverse direction
                transform.position = roundGrid(transform.position, "V");
                transform.position = roundGrid(transform.position, "H");
            }

        }

        if (coll.gameObject.tag == "Boomerang") {

            if (health != 1) {
                //print("stun");
                currentState = EntityState.STUNNED;
                stunTime = Time.time + 2.5f;
            }else {
                takeDamage(1f);
            }

            
       }

       if(coll.gameObject.tag == "Explosion") {
 
            takeDamage(5f);
        }

        if (coll.gameObject.tag == "SwordBeam") {
            takeDamage(1f);
            if (coll.gameObject.name == "Bow(Clone)"){
                print("bow delete");
                PlayerController.S.bowOut = false;
            }
            Destroy(coll.gameObject);
        }


    }

    void OnBecameVisible() {

        print("VISABLE");
        enabled = true;
    }

}
