using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    static public PlayerController S;
    public PlayerAestheticView aestheticView;
    public Tile tilePrefab;
    public float health = 3f;
    public int keys = 0;
    public int rupees = 0;
    public int bombs = 5;
    public float gameRestartDelay = 2f;
    public float damageDelay = 1f; //period of invincibility after taking damage
    public Weapon[] weapons;
    public WeaponType equippedWeapon;

    public AudioClip attackSound;
    public AudioClip hitSound;
    public AudioClip dieSound;
    public AudioClip openDoorSound;
    public AudioClip itemPickupSound;
    public AudioClip pickaxePickupSound;
    public AudioClip lowHealthSound;
    public AudioClip winSound;
    public AudioSource audioSource;

    public float swordRate = .5f; //UPDATE THIS WITH PLAYERCONTORL
    public float PlayerMovementVelocity;
    //cheats
    public bool godMode = false;
    
    public bool _____________________________;
    public float nextAttack = 0f;
    public float lastDamaged; //last time damage was taken
    public bool reportingDamage = false;
    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;
    int levelbrickcounter = 0; //counter for levelbrick triggers 
    public GameObject shield;
    /* Inspector Tunables */
   

    private float knockbackTimer = 0;
    public float knockbackInterval = 1f;
    private bool knockback = false;

    //player facing direction
    public static Vector3 playerDirection = Vector3.right;
    /* Private Data */
    Rigidbody rb;

    //bools for turning off and on weapons for the player, controlled in player inspector
    public bool BoomActive = false;
    public bool BombActive = true;
    public bool BowActive = true;
    public bool PickaxeActive = true;

    public bool boomerangOut = false;
    public bool bombOut = false;
    public bool bowOut = false;

    void Awake()
    {
        S = this;
        shield = transform.Find("Shield").gameObject;
        //DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start ()
    {        
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    
    // Update is called once per frame
    void Update ()
    {
        ProcessMovement();
        ProcessAttacks();
        ShieldSet();

        if (Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.F3))
        {
            if (godMode)
            {
                print("Godmode off.");
                godMode = false;
            }
            else
            {
                print("Godmode on.");
                godMode = true;
            }            
        }

    }
    /* TODO: Deal with user-invoked movement of the player character */
    void ProcessMovement ()
    {
        if (!knockback) {
            Vector3 desired_velocity = Vector3.zero;

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");


            if (Mathf.Abs(horizontalInput) > 0) verticalInput = 0;

            if (horizontalInput != 0 || verticalInput != 0) {
                // "Clamp" rounds a value.
                PlayerController.playerDirection = new Vector3(Mathf.Clamp(horizontalInput, -1, 1),
                                                     Mathf.Clamp(verticalInput, -1, 1));

                ///Rounds link to the other movements position from the "grid" as it does in the NES Version
                ///This prevents link from getting caught on objects and allows the game to feel very smooth

                if (horizontalInput == 0) {
                    ///Rounds the position to the grid so that link is always in place
                    ///multiplying by 2 and then rounding sets link to the closest .5
                    float gridmovementx = transform.position.x * 2;
                    float xRounded = Mathf.Round(gridmovementx);
                    xRounded = xRounded / 2;

                    transform.position = new Vector3(xRounded, transform.position.y, 0);
                    //print("U");

                }

                if (verticalInput == 0) {
                    ///Rounds the position to the grid so that link is always in place
                    float gridmovementy = transform.position.y * 2;
                    float yRounded = Mathf.Round(gridmovementy);
                    yRounded = yRounded / 2;

                    transform.position = new Vector3(transform.position.x, yRounded, 0);
                    //print(transform.position.x);
                    //print("L");
                }
            }

            // Apply the velocity.
            if (Time.time > nextAttack) {
                GetComponent<Rigidbody>().velocity = new Vector3(horizontalInput, verticalInput, 0) * PlayerMovementVelocity;
            }

            if (Input.GetKeyDown(KeyCode.A)) {
                //print("stop");
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                nextAttack = Time.time + swordRate;
            }

        }
     }
        /* NOTE:
         * A reminder to study and implement the grid-movement mechanic.
         * Also, consider using Rigidbodies (GetComponent<Rigidbody>().velocity)
         * to attain movement automatic collision-detection.
         * https://docs.unity3d.com/ScriptReference/Rigidbody.html
         * Also also, remember to attain framerate-independence via Time.deltaTime
         * https://docs.unity3d.com/ScriptReference/Time-deltaTime.html 
         */
    

    private void OnTriggerEnter(Collider other)
    {
        
        //GameObject go = Utils.FindTaggedParent(other.gameObject);
        if (other.gameObject != null)
        {
            switch(other.gameObject.tag)
            {
                case "Enemy": //might not use this tag 
                    takeDamage(.5f);
                    break;
                case "Stalfos":
                    //GetComponent<Rigidbody>().velocity = ((other.transform.position - transform.position) * 4);
                    //PlayerControl.S.current_state = EntityState.KNOCKBACK;
                    //knockbackTimer = knockbackInterval + Time.time;
                    //print("player knockback");
                    takeDamage(.5f);
                    break;
                case "Keese":                    
                case "Goriya":                   
                case "Bladetrap":                    
                case "Gel":                    
                case "Wallmaster":
                    break;                    
                case "ProjectileBoss":
                    takeDamage(.5f);
                    //update UI?
                    break;
                case "Bomb":
                    bombs++;
                    Destroy(other.gameObject);
                    if (audioSource != null) audioSource.PlayOneShot(itemPickupSound);
                    break;
                case "Rupee":
                    print("Rupee");
                    Destroy(other.gameObject);
                    rupees++;
                    if (audioSource != null) audioSource.PlayOneShot(itemPickupSound);
                    break;
                case "Heart":
                    print("Heart");
                    Destroy(other.gameObject);
                    if(health == 3) {
                        
                    }else if(health == 2.5) {  
                        health += .5f;
                    }else{
                        health++;
                    }
                    if (audioSource != null) audioSource.PlayOneShot(itemPickupSound);
                    break;
                case "Key":
                    print("Key");
                    Destroy(other.gameObject);
                    keys++;
                    if (audioSource != null) audioSource.PlayOneShot(itemPickupSound);
                    break;
                case "BowPickup":
                    print("Bow");
                    BowActive = true;
                    Destroy(other.gameObject);
                    if (audioSource != null) audioSource.PlayOneShot(pickaxePickupSound);
                    break;

                case "BoomPickup":
                    print("Boom");
                    BoomActive = true;
                    Destroy(other.gameObject);
                    if (audioSource != null) audioSource.PlayOneShot(pickaxePickupSound);
                    break;

                case "WarpThere":
                    transform.position = new Vector3(20.28f, 77.58f, 0);
                    if (audioSource != null) audioSource.PlayOneShot(itemPickupSound);
                    break;
                case "WarpBack":
                    transform.position = new Vector3(20.28f, 59.67f, 0);
                    if (audioSource != null) audioSource.PlayOneShot(itemPickupSound);
                    break;
                case "LockedDoor":
                    print("...found a door...");
                    if (keys <= 0) return; //can't unlock a door without keys.
                    //find which door type we hit (west, east, north)
                    Vector3 linkPos = gameObject.transform.position;
                    Vector3 doorPos = other.gameObject.transform.position;
                    //West?
                    if (linkPos.x > doorPos.x &&
                        linkPos.x - doorPos.x > .35 &&
                        Mathf.Abs(linkPos.y - doorPos.y) < .3)
                    {
                        print("this is a WESTERN door!");
                        //idiot check
                        if (ShowMapOnCamera.MAP[(int)doorPos.x, (int)doorPos.y] == 106)
                        {
                            ShowMapOnCamera.MAP[(int)doorPos.x, (int)doorPos.y] = 051;
                            Destroy(other.gameObject);
                            keys--;
                            if (audioSource != null) audioSource.PlayOneShot(openDoorSound);
                        }
                        break;
                    }

                    //East?
                    if (linkPos.x < doorPos.x &&
                        doorPos.x - linkPos.x > .35 &&
                        Mathf.Abs(linkPos.y - doorPos.y) < .3)
                    {
                        print("this is a EASTERN door!");
                        //idiot check
                        if (ShowMapOnCamera.MAP[(int)doorPos.x, (int)doorPos.y] == 101)
                        {
                            ShowMapOnCamera.MAP[(int)doorPos.x, (int)doorPos.y] = 048;
                            Destroy(other.gameObject);
                            keys--;
                            if (audioSource != null) audioSource.PlayOneShot(openDoorSound);
                        }                        
                        break;
                    }

                    //North?
                    if (linkPos.y < doorPos.y &&
                        doorPos.y - linkPos.y > .35 &&
                        Mathf.Abs(linkPos.x - doorPos.x) <= .5)
                    {
                        print("this is a NORTHERN door!");
                        //did we hit the right side?
                        if (ShowMapOnCamera.MAP[(int)doorPos.x - 1, (int)doorPos.y] == 080)
                        {
                            if (audioSource != null) audioSource.PlayOneShot(openDoorSound);
                            keys--;
                            ShowMapOnCamera.MAP[(int)doorPos.x - 1, (int)doorPos.y] = 092;
                            ShowMapOnCamera.MAP[(int)doorPos.x, (int)doorPos.y] = 093;
                            Destroy(other.gameObject);
                            GameObject[] candidates = GameObject.FindGameObjectsWithTag("LockedDoor");
                            foreach (var door in candidates)
                            {
                                if (door.transform.position == new Vector3((int)doorPos.x - 1, (int)doorPos.y, 0))
                                {
                                    Destroy(door.gameObject);
                                    break;
                                }
                            }
                        }//end right BEWARE, THIS CODE INTRODUCES A STRANGE BUG
                        //else //must have hit the left side
                        //{
                        //    ShowMapOnCamera.MAP[(int)doorPos.x, (int)doorPos.y] = 092;
                        //    ShowMapOnCamera.MAP[(int)doorPos.x + 1, (int)doorPos.y] = 093;
                        //    Destroy(other.gameObject);
                        //    GameObject[] candidates = GameObject.FindGameObjectsWithTag("LockedDoor");
                        //    foreach (var door in candidates)
                        //    {
                        //        if (door.transform.position == new Vector3((int)doorPos.x + 1, (int)doorPos.y, 0))
                        //        {
                        //            Destroy(door.gameObject);
                        //            break;
                        //        }
                        //    }
                        //}//end left                    
                    }//end north
                    break;
                case "ProjectileEnemy":                   
                    takeDamage(.5f);                    
                    break;
                case "Block_follower":
                    takeDamage(.5f);
                    break;
                case "LinkProjectile":
                    break;
                case "Triforce":
                    Main.S.winning = true;
                    if (audioSource != null) audioSource.PlayOneShot(winSound);
                    break;
                case "Pickaxe_drop":
                    if (audioSource != null) audioSource.PlayOneShot(pickaxePickupSound);
                    PickaxeActive = true;
                    Destroy(other.gameObject);
                    break;
                case "BlockTrigger":
                    print("block Activation");
                    Main.S.block1.SetActive(true);
                    Main.S.block2.SetActive(true);
                    break;
                case "LevelBrickL":
                    print("LevelBrick");
                    transform.position = new Vector3(transform.position.x, 5.25f, 0);
                    break;
                case "LevelBrick":
                    print("LevelBrick");
                    print(levelbrickcounter);
                    if (levelbrickcounter == 1) {

                        float gridmovementy = transform.position.y * 4;
                        float yRounded = Mathf.Round(gridmovementy);
                        yRounded = yRounded / 4;
                        transform.position = new Vector3(transform.position.x,yRounded, 0);
                        levelbrickcounter = 0;
                    }else {
                        levelbrickcounter++;
                    }
                    break;
                default:
                    //print("Triggered " + other.gameObject.name);
                    break;
            }
        }
        else
        {
            print("Triggered: " + other.gameObject.name);
        }
    }

    //use this to make player take damage
    public void takeDamage(float n)
    {
        if (Time.time - lastDamaged < damageDelay) return;
        
        if (godMode) return;
        health -= n;
        lastDamaged = Time.time;
        reportDamage();
        print("add force");

        ///print(playerDirection * 7f);
        GetComponent<Rigidbody>().velocity = (-playerDirection * 4f);
        knockback = true;
        Invoke("SetKnockback", .75f);
        if (health <= 0)
        {
           ///Game Over
            transform.position = new Vector3(39.5f, 6f, 0);
            health = 3;
            if (audioSource != null) audioSource.PlayOneShot(dieSound);
            //Main.S.DelayedRestart(gameRestartDelay);
        }
        if (audioSource != null) audioSource.PlayOneShot(hitSound);
    }

    /* TODO: Deal with user-invoked usage of weapons and items */
    void ProcessAttacks()
    {
        if (Input.GetKeyDown(KeyCode.A) && fireDelegate != null) {
            fireDelegate();
            //Debug.Log("sword fire");
        }
        if (Input.GetKeyDown(KeyCode.S) && fireDelegate != null) {
            fireDelegate();
            //Debug.Log("boomerfire");
        }


    }

    //makes sprite flash as damage indication
    void reportDamage()
    {
        reportingDamage = true;
    }

    void ShieldSet() { 
        
        Vector3 projectilePosition = PlayerController.S.transform.position + PlayerController.playerDirection * .5f;
        shield.transform.position = projectilePosition;
        shield.GetComponent<BoxCollider>().size = new Vector3(.1f, .85f, 1);
        if (PlayerController.playerDirection == Vector3.up || PlayerController.playerDirection == Vector3.down) {
            ///transforms sword to shoot from the top and bottom 
            shield.GetComponent<BoxCollider>().size = new Vector3(.85f, .1f, 1); ///edit for shield
        }
       
    }

    void SetKnockback() {
        knockback = false;
    }
    
}
