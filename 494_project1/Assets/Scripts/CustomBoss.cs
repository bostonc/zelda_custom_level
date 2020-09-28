using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBoss : Boss {
    public AudioClip tinkSound;
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject == null) return;
        switch (coll.gameObject.tag)
        {
            case "Block_runner":
                Destroy(coll.gameObject);
                takeDamage(2f);
                break;
            default:
                break;
        }

        //add anything here that is solid!! 
        if (coll.gameObject.tag == "LockedDoor" || coll.gameObject.tag == "Solid" ||
            coll.gameObject.tag == "Stalfos" || coll.gameObject.tag == "Wall")
        {

            //print("bump");
            float dot = Vector3.Dot(target, coll.gameObject.transform.position - transform.position);
            if (dot > 0)
            { //If enemy is walking twoards the object it hit
                target *= -1; //reverse direction
                transform.position = roundGrid(transform.position, "V");
                transform.position = roundGrid(transform.position, "H");
            }

        }

        if (coll.gameObject.tag == "Explosion")
        {

            if (audioSource != null) audioSource.PlayOneShot(tinkSound);
        }


        if(coll.gameObject.tag == "SwordBeam") {

            Destroy(coll.gameObject);
            if (audioSource != null) audioSource.PlayOneShot(tinkSound);
        }

        if (coll.gameObject.tag == "Sword") {

            if (audioSource != null) audioSource.PlayOneShot(tinkSound);
        }

        if (coll.gameObject.tag == "PIckaxe") {

            if (audioSource != null) audioSource.PlayOneShot(tinkSound);
        }

    }
}
