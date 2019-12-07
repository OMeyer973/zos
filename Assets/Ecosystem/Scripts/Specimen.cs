using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Specimen : MonoBehaviour
{

    //nom du tableau de flock
    public string specieName;

    public GameObject childPrefab;
    public GameObject corpsePrefab;

    [Tooltip("tag of the specie to eat")]
    public string SpecieToEat;
    public float atk;
    public float health;
    public float regain;
   
    [Range(1f, 100f)]
    public float delaisreprod;

    Specie specie;

    //Detect collisions between the GameObjects with Colliders attached
    void OnTriggerEnter2D(Collider2D other)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (other.gameObject.CompareTag(SpecieToEat))
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            other.gameObject.GetComponent<Specimen>().Attack(atk, other);
            GetComponent<Specimen>().eat();
        }

        

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        /* if (other.gameObject.tag == "MyGameObjectTag")
         {
             //If the GameObject has the same tag as specified, output this message in the console
             Debug.Log("Do something else here");
         }*/
        // Debug.Log("TestaaaaTest");
    }

    public void Attack(float dmg, Collider2D other)
    {

        other.gameObject.GetComponent<Specimen>().Hurt(dmg);


       
    }
    public void die()
    {
        if (childPrefab != null)
            Instantiate(childPrefab, this.transform);
        Destroy(this.gameObject);
    }

    void Start()
    {
        specie = GameObject.Find(specieName).GetComponent<Specie>();
        Debug.Log(specie);
        if (specie == null) Debug.Log("ERR : failed to initiate specie");
    }
    void Update()
    {
       // eat();
    }

    public void eat()
    {

        Debug.Log("eating");
        health += regain;
       delaisreprod += regain;


        if ( delaisreprod >= 100f)
        {
            //Debug.Log("Specimen " + name + " has reproduced!");
            delaisreprod = 1f;

            GameObject child = Instantiate(
              childPrefab,
              transform.position,
              Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
              transform.parent
            );
            child.name = name;
            Debug.Log(specie);
            if (specie == null) Debug.Log("ERR SPECIE NULL");
            specie.addAgent(child);

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





    