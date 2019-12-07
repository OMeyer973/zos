using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Specie : Flock
{
    [Header("Specie Settings")]

    [Tooltip("dmg to cause to specimens on Simon lost")]
    public float HurtDamage = 30f;

    public float freakOutTime = 5f;
    [Tooltip("see behavior template for details")]
    public List<float> freakOutBehaviorsParam;
    public float freakOutMaxSpeed;
    public float freakOutNeigbourgRadius;
    public float freakOutAvoidanceRadius;

    new void Start()
    {
        base.Start();
    }

    // spawn a new specimen at a given transform
    public void SpawnSpecimen(Transform tr)
    {
        GameObject newSpecimen = Instantiate(agentPrefab, tr.position, tr.rotation, transform);
        addAgent(newSpecimen);
    }

    // hurt all the specimen of the specie by a given amount
    public void HurtSpecimens()
    {
        FreakOut();
        foreach(GameObject specimenObject in agents)
        {
            Specimen specimen = specimenObject.GetComponent<Specimen>();
            specimen.TakeDamage(HurtDamage);
        }
    }

    // freak out the specimens for a brief moment
    public void FreakOut()
    {
        StartCoroutine(FreakOutAnimation());
    }

    IEnumerator FreakOutAnimation()
    {
        List<float> origBehaviorParameters = new List<float>();
        float origMaxSpeed = maxSpeed;
        float origNeighborRadius = neigbourgRadius;
        float origAvoidanceRadiusMultiplier = avoidanceRadiusMultiplier;

        maxSpeed = freakOutMaxSpeed;
        neigbourgRadius = freakOutNeigbourgRadius;
        avoidanceRadiusMultiplier = freakOutAvoidanceRadius;

        for (int i = 0; i < freakOutBehaviorsParam.Count; i++)
        {
            origBehaviorParameters.Add(behavior.weights[i]);
            behavior.weights[i] = freakOutBehaviorsParam[i];
        }

        yield return new WaitForSeconds(freakOutTime);

        maxSpeed = origMaxSpeed;
        neigbourgRadius = origNeighborRadius;
        avoidanceRadiusMultiplier = origAvoidanceRadiusMultiplier;

        for (int i = 0; i < freakOutBehaviorsParam.Count; i++)
        {
            behavior.weights[i] = origBehaviorParameters[i];
        }
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

    // hurt all the specimen in the specie
    public void HurtAll()
    {
        // todo : freak out
        foreach (GameObject agentObject in agents)
        {
            Debug.Log("hurting specimen");
            agentObject.GetComponent<Specimen>().TakeDamage(HurtDamage);
        }
        
    }
}

