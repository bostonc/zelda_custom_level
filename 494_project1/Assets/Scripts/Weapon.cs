using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this is a enum of the various possible weapontypes 
// It also includes a "shield" 

public enum WeaponType {
    none,
    sword,
    boomerang,
    bomb,
    shield,
    swordbeam,//raise shieldLevel
    pickaxe,
    bow
}
//The weaponDeifnition class allows you to set the properies of a specific weapon 
// in the inspector, Main has an array of WeaponDefinitions that makes this possible

[System.Serializable]
public class WeaponDefinition {
    public WeaponType type = WeaponType.none;
    public GameObject projectilePrefab;
    public float delayBetweenShots = 0;
    public float velocity = 20;
}

public class Weapon : MonoBehaviour {
    static public Transform PROJECTILE_ANCHOR;
    public bool ________________;
    [SerializeField]
    private WeaponType _type = WeaponType.sword;
    public WeaponDefinition def;
    public GameObject collar; //Link's point of spawning for weapons
    public float lastShot; //time last shot was fired 

    public Sprite link_pickaxe_down;
    public Sprite link_pickaxe_up;
    public Sprite link_pickaxe_right;
    public Sprite link_pickaxe_left;

    void Awake() {
        collar = transform.Find("Collar").gameObject;
    }
    void Start() {

        //Call SetType property for the default dtype 
        SetType(_type);

        if (PROJECTILE_ANCHOR == null) {
            GameObject go = new GameObject("_Projectile_Anchor");
            PROJECTILE_ANCHOR = go.transform;
        }
        //find fireDelegate of the parent 
        GameObject parentGO = transform.parent.gameObject;
        if (parentGO.tag == "Player") {
            PlayerController.S.fireDelegate += Fire;
        }
        if (_type == WeaponType.sword) {

            print("sword is equipped");
        }

    }

    public WeaponType type {
        get {
            return _type;
        }
        set {
            SetType(value);
        }
    }
    public void SetType(WeaponType wt) {
        _type = wt;
        if (type == WeaponType.none) {
            this.gameObject.SetActive(false);
            return;
        } else {
            this.gameObject.SetActive(true);
        }
        def = Main.GetWeaponDefinition(_type);

        ///collar.GetComponent<Renderer>().material.color = def.color;
        lastShot = 0;

    }

    public void Fire() {
        
        //if this.gameObject is inactive return
        if (!gameObject.activeInHierarchy) return;
        //if it hasn't been enough time btwn shots return
        if (Time.time - lastShot < def.delayBetweenShots) {
            return;
        }
        Projectile p;
        ////A button Attacks 
        if (Input.GetKeyDown(KeyCode.A)) {

            SetType(WeaponType.sword);
            Debug.Log("sword trigger");
            p = MakeProjectile();
            print("Sword");

            Vector3 projectilePosition = PlayerController.S.transform.position + PlayerController.playerDirection * .4f;
            p.transform.position = projectilePosition;
            if (PlayerController.playerDirection == Vector3.up || PlayerController.playerDirection == Vector3.down) {
                ///transforms sword to shoot from the top and bottom 
                p.GetComponent<BoxCollider>().size = new Vector3(.18f, .84f, 1);
            }
            p.tag = "Sword";

            if(PlayerController.S.health == 3) {
                print("beam");
                Projectile ps;///swordbeam if health is full
                SetType(WeaponType.swordbeam);
                ps = MakeProjectile();
                Vector3 projectilePositionbeam = PlayerController.S.transform.position + PlayerController.playerDirection * .4f;
                ps.transform.position = projectilePositionbeam;
                if (PlayerController.playerDirection == Vector3.up || PlayerController.playerDirection == Vector3.down) {
                    ///transforms sword to shoot from the top and bottom 
                    ps.GetComponent<BoxCollider>().size = new Vector3(.18f, .84f, 1);
                    ps.transform.localScale = new Vector3(.18f, .84f, 1);
                }
                ps.GetComponent<Rigidbody>().velocity = PlayerController.playerDirection * 7;
                ps.tag = "SwordBeam";
            }

            if (PlayerController.S.PickaxeActive) {
                print("pickaxe");
                p.tag = "Pickaxe";
                if(PlayerController.playerDirection == Vector3.down) {
                    p.GetComponent<SpriteRenderer>().sprite = link_pickaxe_down;
                    print("pickaxedown");
                } else if (PlayerController.playerDirection == Vector3.up) {
                    p.GetComponent<SpriteRenderer>().sprite = link_pickaxe_up;
                    print("pickaxeup");
                } else if (PlayerController.playerDirection == Vector3.left) {
                    p.GetComponent<SpriteRenderer>().sprite = link_pickaxe_left;
                    print("pickaxeleft");
                } else if (PlayerController.playerDirection == Vector3.right) {
                    p.GetComponent<SpriteRenderer>().sprite = link_pickaxe_right;
                    print("pickaxeright");
                }

                p.GetComponent<SpriteRenderer>().sortingOrder = 4;
            }
        }

        /////B Button Attacks
        if (Input.GetKeyDown(KeyCode.S)) {
            Debug.Log("B button trigger");
            if (PlayerController.S.BoomActive) {
                if (PlayerController.S.equippedWeapon == WeaponType.boomerang) {
                    if (!PlayerController.S.boomerangOut) {
                        /// if Boomerang 
                        PlayerController.S.boomerangOut = true;
                        SetType(WeaponType.boomerang);
                        p = MakeProjectile();
                        print("Boomerang");

                        Vector3 projectilePosition = PlayerController.S.transform.position + PlayerController.playerDirection * .4f;
                        p.transform.position = projectilePosition;
                        p.GetComponent<Rigidbody>().velocity = PlayerController.playerDirection * def.velocity;
                        p.tag = "Boomerang";
                    }
                }
            }


            if (PlayerController.S.BombActive) {
                if (PlayerController.S.equippedWeapon == WeaponType.bomb) {
                    if (PlayerController.S.bombs > 0) {
                        if (!PlayerController.S.bombOut) {
                            /// if Boomerang 
                            PlayerController.S.bombOut = true;
                            PlayerController.S.bombs--;
                            SetType(WeaponType.bomb);
                            p = MakeProjectile();
                            print("Bomb");

                            Vector3 projectilePosition = PlayerController.S.transform.position + PlayerController.playerDirection * .4f;
                            p.transform.position = projectilePosition;
                            //p.GetComponent<Rigidbody>().velocity = PlayerController.playerDirection * def.velocity;
                            p.tag = "Bomb";
                        }
                    }
                }
            }

            if (PlayerController.S.BowActive) {
                if (PlayerController.S.equippedWeapon == WeaponType.bow) {
                    print("bow equip");
                    if (PlayerController.S.rupees > 0) {
                        if (!PlayerController.S.bowOut) {
                            /// if Boomerang 
                            PlayerController.S.bowOut = true;
                            PlayerController.S.rupees--;
                            SetType(WeaponType.bow);
                            p = MakeProjectile();
                            print("Bow");
                            Vector3 projectilePositionbeam = PlayerController.S.transform.position + PlayerController.playerDirection * .4f;
                            p.transform.position = projectilePositionbeam;
                            if (PlayerController.playerDirection == Vector3.up || PlayerController.playerDirection == Vector3.down) {
                                ///transforms sword to shoot from the top and bottom 
                                p.GetComponent<BoxCollider>().size = new Vector3(.18f, .84f, 1);
                                p.transform.localScale = new Vector3(.18f, .84f, 1);
                            }
                            p.GetComponent<Rigidbody>().velocity = PlayerController.playerDirection * 7;
                            p.tag = "SwordBeam";
                        }
                    }
                }
            }

        }
       
    }

    public Projectile MakeProjectile() {
        GameObject go = Instantiate(def.projectilePrefab) as GameObject;
        if (transform.parent.gameObject.tag == "Player") {
            go.tag = "ProjectilePlayer";
            go.layer = 12; //set this to the projectile layer 

        } else {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }

        go.transform.position = collar.transform.position;
        go.transform.parent = PROJECTILE_ANCHOR;
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShot = Time.time;
        return p;
    }


}
