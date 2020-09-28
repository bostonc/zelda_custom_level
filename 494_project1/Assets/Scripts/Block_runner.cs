using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Block_runner : Enemy
{
    public Sprite blueTile;
    public Sprite greenTile;
    public float activationDistance = 10f;

    public AudioClip openDoorSound;
    public AudioClip tinkSound;
    //
    public bool ____________________________;
    public GameObject player;
    bool activated = false;

    

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

        //change to green
        gameObject.GetComponent<SpriteRenderer>().sprite = greenTile;

        Vector3 moveDir = gameObject.transform.position - player.transform.position;
        gameObject.transform.Translate(moveDir.normalized * speed * Time.deltaTime);
    }

    float distanceFromPlayer()
    {
        return Vector3.Distance(player.transform.position, gameObject.transform.position);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject == null) return;
        switch (coll.gameObject.tag)
        {
            case "TargetBlock":
                Vector3 otherPos = coll.transform.position;
                ShowMapOnCamera.MAP[(int)otherPos.x, (int)otherPos.y] = 044;
                Destroy(coll.gameObject);
                Main.S.open_green_door();
                if (audioSource != null) audioSource.PlayOneShot(openDoorSound);
                gameObject.SetActive(false);
                break;
            case "SwordBeam":
                Destroy(coll.gameObject);
                if (audioSource != null) audioSource.PlayOneShot(tinkSound);
                break;
            case "Sword":
                if (audioSource != null) audioSource.PlayOneShot(tinkSound);
                break;
            case "Boomerang":
                if (audioSource != null) audioSource.PlayOneShot(tinkSound);
                break;
            default:
                break;
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject == null) return;
        switch (coll.gameObject.tag)
        {
            case "TargetBlock":
                Vector3 otherPos = coll.transform.position;
                ShowMapOnCamera.MAP[(int)otherPos.x, (int)otherPos.y] = 044;
                Destroy(coll.gameObject);
                Main.S.open_green_door();
                if (audioSource != null) audioSource.PlayOneShot(openDoorSound);                
                gameObject.SetActive(false);
                break;
            case "SwordBeam":
                Destroy(coll.gameObject);
                if (audioSource != null) audioSource.PlayOneShot(tinkSound);
                break;
            case "Sword":
                if (audioSource != null) audioSource.PlayOneShot(tinkSound);
                break;
            case "Boomerang":
                if (audioSource != null) audioSource.PlayOneShot(tinkSound);
                break;
            default:
                break;
        }
    }

}
