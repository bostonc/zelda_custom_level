using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider coll) {
        //print("POOP");
        if (coll.gameObject.tag == "Wall")
            Destroy(this.gameObject);

        PlayerController.S.bowOut = false;
    }
}
