using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aaa : MonoBehaviour
{

    //nom du tableau de flock
    public string flockName;


    //flock qui spawn
    public FlockAgent agentPrefab;
    //
    public Rigidbody2D foodSpawn;

    public string TagToEat;
    public float atk;
    public float health;
    public float regain;
   
    [Range(1f, 100f)]
    public float delaisreprod;


    //Detect collisions between the GameObjects with Colliders attached
    void OnTriggerEnter2D(Collider2D other)
    {
       // Debug.Log("Trigger entered |||||||||||||||||||||||||||");

        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (other.gameObject.CompareTag(TagToEat))
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            other.gameObject.GetComponent<aaa>().Attaque(atk, other);
            GetComponent<aaa>().eat();
        }

        

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        /* if (other.gameObject.tag == "MyGameObjectTag")
         {
             //If the GameObject has the same tag as specified, output this message in the console
             Debug.Log("Do something else here");
         }*/
        // Debug.Log("TestaaaaTest");
    }

    public void Attaque(float dmg, Collider2D other)
    {
        //float speed = this.gameObject.GetComponent<Flock>.maxSpeed;

        //float speed =  this.gameObject.GetComponentInParent<Flock>.maxSpeed;

        other.gameObject.GetComponent<aaa>().Hurt(dmg);


       
    }
    public void die()
    {
        // spawn food
        Rigidbody2D instance;

        instance = Instantiate(foodrigid, this.transform.position ) as Rigidbody2D;

        // other.gameObject.GetComponent<aaa>().health += regain;
        //other.gameObject.GetComponent<aaa>().delaisreprod += regain ;

        Destroy(this.gameObject);
    }

    private void Update()
    {
       // eat();
    }

    public void eat()
    {

        Debug.Log("eat ");
        health += regain;
       delaisreprod += regain;


        if ( delaisreprod >= 100f)
        {
            
            Debug.Log(" ça reprod");
            delaisreprod = 1f;

            FlockAgent newAgent = Instantiate(
              agentPrefab,
              transform.position,

              Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f))
            );
            Flock flock = GameObject.Find(flockName).GetComponent<Flock>();
            flock.addAgent(newAgent);

            Debug.Log("delaisreprod post spawn " + delaisreprod);


        }


    }

   public void Hurt (float dmg)
    {
        health -= dmg;
        // gameObject.GetComponent<Flock>
        if (health <= 0)
        {
            die();

        }

    }
   


}





    