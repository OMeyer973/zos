using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aaa : MonoBehaviour
{
    public string TagToEat;
    public float atk;
    public float health;

    //Detect collisions between the GameObjects with Colliders attached
    void OnTriggerEnter2D(Collider2D other)
    {
       // Debug.Log("Trigger entered |||||||||||||||||||||||||||");

        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (other.gameObject.CompareTag(TagToEat))
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            other.gameObject.GetComponent<aaa>().hurt(atk);
        }

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
       /* if (other.gameObject.tag == "MyGameObjectTag")
        {
            //If the GameObject has the same tag as specified, output this message in the console
            Debug.Log("Do something else here");
        }*/
        // Debug.Log("TestaaaaTest");
    }

    public void hurt(float dmg)
    {
        health -= atk;
        if ( health  <= 0)
        {
            die();
        }
    }
    public void die()
    {
        // spawn food



        Destroy(this.gameObject);
    }
}





    