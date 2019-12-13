using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcosystemManager : MonoBehaviour
{
    public float EcosystemRadius = 10f;

    public List<GameObject> species;

    public int FoodToSpawnPerBatch = 10;
    public GameObject foodSpecie;

    public float continuousFoodDropRate = 1f;
    float continuousFoodDropTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        AddNewSpecie();
    }

    // drop one food bit in the ecosystem
    void DropFood()
    {
        // Debug.Log("dropping food");
        Vector3 spawnPosition = Random.insideUnitSphere * EcosystemRadius;
        spawnPosition.z = 0;
        foodSpecie.GetComponent<Specie>().SpawnAgent(spawnPosition, Quaternion.Euler(spawnPosition));
    }

    // drop a batch of food in the ecosystem
    public void DropFoodBatch()
    {
        StartCoroutine(DropFoodBatchAnim());
    }

    // coroutine called when dropping a batch of food into the ecosystem
    IEnumerator DropFoodBatchAnim()
    {
        for (int i=0; i<FoodToSpawnPerBatch; i++)
        {
            DropFood();
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void DropFoodContinuous(float amount)
    {
        continuousFoodDropTime += Time.deltaTime * amount;
        if (continuousFoodDropTime > continuousFoodDropRate)
        {
            continuousFoodDropTime = 0f;
            DropFood();
        }
    }

    //////////////////////
    // DEBUG STUFF
    void Update()
    {
        DropFoodContinuous(1f);
        //food
        if (Input.GetButtonDown("Fire1"))
        {
            AddNewSpecie();
            //DropFoodBatch();
            //FreakOutEcosystem();
            //HealEcosystem();
        }
        //hurt
        if (Input.GetButtonDown("Fire2"))
        {
            HurtEcosystem();
        }
    }
    // END DEBUG STUFF
    //////////////////////

    // makes the whole ecosystem go wild
    public void FreakOutEcosystem()
    {
        foreach (GameObject specieObject in species)
        {
            Specie specie = specieObject.GetComponent<Specie>();
            specie.FreakOut();
        }
    }


    // hurt and freak out all the specimen in the ecosystem
    public void HurtEcosystem()
    {
        foreach (GameObject specieObject in species)
        {
            Specie specie = specieObject.GetComponent<Specie>();
            specie.HurtSpecimens();
        }
    }

    // heal and calm down all the specimen in the ecosystem
    public void HealEcosystem()
    {
        foreach (GameObject specieObject in species)
        {
            Specie specie = specieObject.GetComponent<Specie>();
            specie.HealSpecimens();
        }
    }

    // add specimen and new specie in the ecosystem
    public void AddNewSpecie()
    {
        bool canAddMore = true;
        foreach (GameObject specieObject in species)
        {
            // Debug.Log("adding specie");
            if (canAddMore)
            {
                Specie specie = specieObject.GetComponent<Specie>();

                if (specie.AgentCount() <= 0)
                    canAddMore = false;

                specie.SpawnInitialAgentCount();
            }
        }
    }
}
