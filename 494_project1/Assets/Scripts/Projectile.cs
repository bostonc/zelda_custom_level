using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField]
    private WeaponType _type;
    private int first = 0;
    public float swordTime = .3f;
    //this public property masks the field _type and takes action when it is set 
    public WeaponType type {
        get {
            return _type;
        }
        set {
            SetType(value);
        }
    }

    void Awake() {
        //test to see whether this has passed of screen every two seconds 
        ///InvokeRepeating("SwordCheck", 1f, 1f);

    }

    private void Start() {
        if(_type == WeaponType.sword || _type == WeaponType.pickaxe) {
            Destroy(this.gameObject, .3f);
        }
    }

    public void SetType(WeaponType eType) {
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        
        ///This instead of color should set the sprite
        ///GetComponent<Renderer>().material.color = def.projectileColor;
    }

  
    // Use this for initialization

}