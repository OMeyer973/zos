using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcosystemManager : MonoBehaviour
{
    public float EcosystemRadius = 10f;

    public List<GameObject> species;

    public int FoodToSpawnPerBatch = 10;
    public GameObject foodPrefab;


    // Start is called before the first frame update
    void Start()
    {
    }

    // drop one food bit in the ecosystem
    public void DropFood()
    {
        Debug.Log("dropping food");
        Vector3 spawnPosition = Random.insideUnitSphere * EcosystemRadius;
        spawnPosition.z = 0;
        Instantiate(foodPrefab, spawnPosition, Quaternion.Euler(spawnPosition));
    }

    // drop a batch of food in the ecosystem
    IEnumerator DropFoodBatch()
    {
        for (int i=0; i<FoodToSpawnPerBatch; i++)
        {
            DropFood();
            yield return new WaitForSeconds(Time.deltaTime*.3f);
        }
    }

    void Update()
    {
        //food
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(DropFoodBatch());
        }
         //hurt
        if (Input.GetButtonDown("Fire2"))
        {
            HurtEcosystem();
            foreach (GameObject specieObject in species)
            {
                specieObject.GetComponent<Specie>().HurtAll();
            }
        }
    }
    //dropfood
    //boost
    //hurt

    // hurt and freak out all the specimen in the ecosystem
    public void HurtEcosystem()
    {
        foreach(GameObject specieObject in species)
        {
            Specie specie = specieObject.GetComponent<Specie>();
            specie.HurtSpecimens();
        }
    }

    //heal
    //addspecies

}
