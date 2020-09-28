using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour {

    Vector3 startingposition;
    private float speed = 6;
    private bool callback = false;

    public AudioClip tinkSound;
    public AudioClip itemPickupSound;
    public bool ____________________________;
    public AudioSource audioSource;

    // Use this for initialization
    void Start ()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(transform.position, PlayerController.S.transform.position) > 4 || callback == true) {
            GoBackToPlayer();
        }

    }

    void GoBackToPlayer() {
        
       // Vector3 targetDir = PlayerController.S.transform.position - transform.position;
        //print(targetDir);
        float step =  speed * Time.deltaTime;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = Vector3.MoveTowards( transform.position, PlayerController.S.transform.position, step);
        callback = true;
        //Debug.DrawRay(transform.position, newDir, Color.red);
        ///transform.rotation = Quaternion.LookRotation(newDir);
        ///

        if(Utils.vectorIsSimilar(transform.position, PlayerController.S.transform.position, .65f)) {
            Destroy(this.gameObject);
            PlayerController.S.boomerangOut = false;
        }
    }

    private void OnTriggerEnter(Collider coll) {

        if (coll.gameObject.tag == "Solid" || coll.gameObject.tag == "Wall") {
            print("bump");
            if (audioSource != null) audioSource.PlayOneShot(tinkSound);
            GoBackToPlayer();
        }
        if (coll.gameObject.tag == "Stalfos") {
            print("hit");
            GoBackToPlayer();
        }

        if (coll.gameObject.tag == "Rupee") {
            print("Rupee");
            Destroy(coll.gameObject);
            PlayerController.S.rupees++;
            if (audioSource != null) audioSource.PlayOneShot(itemPickupSound);
            GoBackToPlayer();
        }
        if (coll.gameObject.tag == "Key") {
            print("key");
            Destroy(coll.gameObject);
            PlayerController.S.keys++;
            if (audioSource != null) audioSource.PlayOneShot(itemPickupSound);
            GoBackToPlayer();
        }
        if (coll.gameObject.tag == "Heart") {
            print("Heart");
            Destroy(coll.gameObject);
            if (audioSource != null) audioSource.PlayOneShot(itemPickupSound);

            if (PlayerController.S.health == 3) {

            }else if (PlayerController.S.health == 2.5) {
                PlayerController.S.health += .5f;
            } else {
                PlayerController.S.health += 1f;
            }
            
            GoBackToPlayer();
        }
        if(coll.gameObject.tag == "Bomb") {
            Destroy(coll.gameObject);
            PlayerController.S.bombs++;
            if (audioSource != null) audioSource.PlayOneShot(itemPickupSound);
            GoBackToPlayer();
        }

        if (coll.gameObject.tag == "Block_follower") {
            if (audioSource != null) audioSource.PlayOneShot(tinkSound);
            GoBackToPlayer();
        }
        if (coll.gameObject.tag == "Block_runner")
        {
            if (audioSource != null) audioSource.PlayOneShot(tinkSound);
            GoBackToPlayer();
        }



    }
}
