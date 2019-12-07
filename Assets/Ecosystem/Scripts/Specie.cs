using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Specie : Flock
{

    [Header("Specie Settings")]
    [SerializeField]

    [Tooltip("dmg to cause to specimens on Simon lost")]
    public float HurtDamage;
    [Tooltip("speed multiplier to apply on Simon lost")]
    public float HurtSpeedMultiplier;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    public void HurtSpecimens()
    {
        FreakOut();
        foreach(GameObject specimenObject in agents)
        {
            Specimen specimen = specimenObject.GetComponent<Specimen>();
            specimen.Hurt(HurtDamage);
        }
    }

    public void FreakOut()
    {
        Debug.Log("yipikaye motherfucker");
        behavior.weights[2] = 500;
    }


    // Update is called once per frame

    new void Update()
    {
        base.Update();

        // Send scene changes to arduino
        if (Input.GetKeyDown(KeyCode.G))
        {
            FreakOut();
            maxSpeed = 100f;
        }
    }

    public void HurtAll()
    {
    /*
    foreach (FlockAgent agent in agents)
    {
        agent.gameObject.GetComponent<Specimen>().Hurt(DommageFlock);

    }
    */
        
    }
}

