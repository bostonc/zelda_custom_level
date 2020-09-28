using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Block_follower : Enemy
{
    public Sprite blueTile;
    public Sprite redTile;
    public float activationDistance = 10f;

    public AudioClip tinkSound;
    //
    public bool ____________________________;
    public GameObject player;
    bool activated = false; //used elsewhere in code, do not delete

    // Use this for initialization
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Move()
    {
        //don't move if player is far away.
        if (distanceFromPlayer() > activationDistance)
        {
            //change back to blue
            gameObject.GetComponent<SpriteRenderer>().sprite = blueTile;
            return;
        }
        //make sure nav is enabled
        if (!navigation) return;

        //change to red
        gameObject.GetComponent<SpriteRenderer>().sprite = redTile;

        Vector3 moveDir = player.transform.position - gameObject.transform.position;
        gameObject.transform.Translate(moveDir.normalized * speed * Time.deltaTime);
    }

    float distanceFromPlayer()
    {
        return Vector3.Distance(player.transform.position, gameObject.transform.position);
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject == null) return;
        if (coll.gameObject.tag == "Pickaxe")
        {

            print("pickaxehit");
            takeDamage(1f);
            PlayerController.S.takeDamage(.5f);
            return;
        }

        if (coll.gameObject.tag == "SwordBeam")
        {

            Destroy(coll.gameObject);
            if (audioSource != null) audioSource.PlayOneShot(tinkSound);
            return;
        }
        if (coll.gameObject.tag == "Sword")
        {
            if (audioSource != null) audioSource.PlayOneShot(tinkSound);
        }
        if (coll.gameObject.tag == "Boomerang")
        {
            if (audioSource != null) audioSource.PlayOneShot(tinkSound);
        }


    }
    
}
