﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aaa : MonoBehaviour
{

    public FlockAgent agentPrefab;
    public string TagToEat;
    public float atk;
    public float health;
    public float regain;
   
    [Range(1f, 100f)]
    public float delaisreprod = 50;


    //Detect collisions between the GameObjects with Colliders attached
    void OnTriggerEnter2D(Collider2D other)
    {
       // Debug.Log("Trigger entered |||||||||||||||||||||||||||");

        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (other.gameObject.CompareTag(TagToEat))
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            other.gameObject.GetComponent<aaa>().hurt(atk, other);
            other.gameObject.GetComponent<aaa>().eat();
        }

        

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        /* if (other.gameObject.tag == "MyGameObjectTag")
         {
             //If the GameObject has the same tag as specified, output this message in the console
             Debug.Log("Do something else here");
         }*/
        // Debug.Log("TestaaaaTest");
    }

    public void hurt(float dmg, Collider2D other)
    {
        float speed = this.gameObject.GetComponent<Flock>.maxSpeed;

         health -= dmg;
        gameObject.GetComponent<Flock>
        if ( health  <= 0)
        {
            die(other);

        }
    }
    public void die(Collider2D other)
    {
        // spawn food

        other.gameObject.GetComponent<aaa>().health += regain;
        other.gameObject.GetComponent<aaa>().delaisreprod += regain ;

        Destroy(this.gameObject);
    }

    public void eat()
    {

        Debug.Log(" ça mange");
        health += regain;
       delaisreprod += regain;

        if( delaisreprod > 99)
        {
            
            Debug.Log(" ça reprod");

            FlockAgent newAgent = Instantiate(
              agentPrefab,
              Random.insideUnitCircle * 1 * 1,

              Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),

              transform

              );
            //delaisreprod = 50;
            //Instantiate(this.gameObject);

        }


    }
}





    