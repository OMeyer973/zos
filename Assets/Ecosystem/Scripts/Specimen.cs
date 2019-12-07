﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Specimen : MonoBehaviour
{

    //nom du tableau de flock
    public string specieName;
    Specie specie;

    public GameObject childPrefab;
    public GameObject corpsePrefab;

    public float health = 100f;
    public float lifeTime = 60f; // in seconds
    float aliveTime = 0f;
    public float foodToEatBeforeReproducing = 60f;
    float foodEaten = 0;

    [Tooltip("tag of the specie to Eat")]
    public string SpecieToEat = "food";
    public float attackStrength = 30f;

    [Tooltip("how much does it regen the specimen who is eating it")]
    public float nutritiousValue;
   
    void Start()
    {
        specie = GameObject.Find(specieName).GetComponent<Specie>();
        if (specie == null) Debug.Log("ERR : failed to initiate specie");
        aliveTime = 0f;
        foodEaten = 0f;
    }

    void Update()
    {
        aliveTime += Time.deltaTime;
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
        Debug.Log("eating");
        health += other.nutritiousValue;
        foodEaten += other.nutritiousValue;

        if ( foodEaten >= foodToEatBeforeReproducing)
        {
            ReproductSelf();
        }
    }

    void ReproductSelf()
    {
        //Debug.Log("Specimen " + name + " has reproduced!");
        foodEaten = 0f;

        if (specie == null) Debug.Log("error: can't find specie to add child");
        specie.SpawnSpecimen(transform);
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

    // make the specimen die (spawn a corpse and destroy gameobject)
    public void Die()
    {
        if (corpsePrefab != null)
            Instantiate(corpsePrefab, this.transform);
        Destroy(this.gameObject);
    }


}





    