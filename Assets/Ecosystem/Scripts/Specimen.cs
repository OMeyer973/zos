using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Specimen : MonoBehaviour
{

    //nom du tableau de flock
    public string specieName;
    Specie specie;

    public GameObject childPrefab;

    public string corpseName;
    Specie corpseSpecie;

    public float maxHealth = 100f;
    float health;
    public float lifeTime = 60f; // in seconds
    float aliveTime = 0f;
    public float foodToEatBeforeReproducing = 60f;
    float foodEaten = 0;
    public float timeBetween2Meals = 3f;
    float eatingDelay;

    [Tooltip("tag of the specie to Eat")]
    public string SpecieToEat = "food";
    public float attackStrength = 30f;

    [Tooltip("how much does it regen the specimen who is eating it")]
    public float nutritiousValue;
   
    void Start()
    {
        specie = GameObject.Find(specieName).GetComponent<Specie>();
        if (corpseName != "") corpseSpecie = GameObject.Find(corpseName).GetComponent<Specie>();
        if (specie == null) Debug.Log("ERR : failed to initiate specie");
        health = maxHealth;
        aliveTime = 0f;
        foodEaten = 0f;
        eatingDelay = 0f;
    }

    void Update()
    {
        aliveTime += Time.deltaTime;
        if (eatingDelay > 0)
            eatingDelay -= Time.deltaTime;
        if (aliveTime >= lifeTime)
            Die();
    }

    //Detect collisions between the GameObjects with other collider
    void OnTriggerEnter2D(Collider2D other)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (other.gameObject.CompareTag(SpecieToEat))
        {
            Specimen otherSpecimen = other.gameObject.GetComponent<Specimen>();
            otherSpecimen.TakeDamage(attackStrength);
            GetComponent<Specimen>().Eat(otherSpecimen);
        }
    }

    // munch on another specimen
    public void Eat(Specimen other)
    {
        eatingDelay = timeBetween2Meals;
        //Debug.Log("eating");
        health += other.nutritiousValue;
        health = Math.Min(health, maxHealth);
        foodEaten += other.nutritiousValue;

        if ( foodEaten >= foodToEatBeforeReproducing)
        {
            ReproductSelf();
            foodEaten = 0f;
        }
    }

    void ReproductSelf()
    {
        //Debug.Log("Specimen " + name + " has reproduced!");

        if (specie == null) Debug.Log("error: can't find specie to add child");
        specie.SpawnAgent(transform.position, transform.rotation);
    }

    // inflict damage to specimen and eventually die
    public void TakeDamage (float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Die();
        }
    }

    // inflict damage to specimen and eventually die
    public void Heal()
    {
        health = maxHealth;
    }

    // make the specimen die (spawn a corpse and destroy gameobject)
    public void Die()
    {
        if (corpseSpecie != null)
        {
            corpseSpecie.SpawnAgent(transform.position, transform.rotation);
        }
        Destroy(this.gameObject);
    }
}





    