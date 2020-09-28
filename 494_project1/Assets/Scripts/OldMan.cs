using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldMan : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip switchSound;

    public Text oldman_text;
    // Use this for initialization
    void Start ()
    {
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnTriggerEnter(Collider other) {
        
        if(other.gameObject.tag == "Pushblock") {

            print("switch hit");
            

            print(ShowMapOnCamera.MAP[(int)17, (int)38]);
            if (ShowMapOnCamera.MAP[(int)17, (int)38] == 100) {
                print("tile switch");
                ShowMapOnCamera.MAP[(int)17, (int)38] = 051;
                GameObject wallpoint = GameObject.FindWithTag("finalDoor");
                Destroy(wallpoint);
                if (audioSource != null) audioSource.PlayOneShot(switchSound);
            }

        }

    }
}
