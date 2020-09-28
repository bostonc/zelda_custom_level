using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoriyaBoomerang : MonoBehaviour {

    Vector3 startingposition;
    private float speed = 6;
    private bool callback = false;

    public AudioClip tinkSound;
    public bool ____________________________;
    public AudioSource audioSource;
    // Use this for initialization
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (Vector3.Distance(transform.position, transform.parent.position) > 4 || callback == true) {
            GoBackToPlayer();
        }

    }

    void GoBackToPlayer() {

        Vector3 targetDir = transform.parent.position - transform.position;
        //print(targetDir);
        float step = speed * Time.deltaTime;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = Vector3.MoveTowards(transform.position, transform.parent.position, step);
        callback = true;
        //Debug.DrawRay(transform.position, newDir, Color.red);
        ///transform.rotation = Quaternion.LookRotation(newDir);
        ///

        if (Utils.vectorIsSimilar(transform.position, transform.parent.position, .65f)) {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider coll) {

        if (coll.gameObject.tag == "Solid" || coll.gameObject.tag == "Wall") {
            print("bump");
            GoBackToPlayer();
        }
        if(coll.gameObject.tag == "Player") {
            GoBackToPlayer();
        }
        if (coll.gameObject.tag == "Shield") {
            this.gameObject.tag = "Collar";
            if (audioSource != null) audioSource.PlayOneShot(tinkSound);
            print("taps shield");
            GoBackToPlayer();
        }

    }
}