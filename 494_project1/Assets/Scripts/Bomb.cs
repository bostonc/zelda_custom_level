using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour {


    public GameObject explosionPrefab;
    
    public float bombTime = 2f;
    public float bombTimer = 0f; 

	// Use this for initialization
	void Start () {
        bombTimer = Time.time + bombTime;
        
	}
	
	// Update is called once per frame
	void Update () {
		if( bombTimer < Time.time) {
            ExplodeBomb();
            Destroy(this.gameObject);
        }
	}


    void ExplodeBomb() {
        print("explosion");
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
