using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public float explosionTime = .5f;
    private float explosionTimer = 0f;
	// Use this for initialization
	void Start () {
        this.tag = "Explosion";
        explosionTimer = Time.time + explosionTime;
	}
	
	// Update is called once per frame
	void Update () {

        if(Time.time > explosionTimer) {
            PlayerController.S.bombOut = false;
            Destroy(this.gameObject);
        }
		
	}
}
