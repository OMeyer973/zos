using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcosystemManager : MonoBehaviour
{

    public float EcosystemRadius = 10f;


    public List<Specie> species;

    public GameObject foodPrefab;
    public int AmountOfFoodToSpawn = 10;

    public List<GameObject> flockObjects;
    public List<GameObject> AgentsObjects;
    public string TagAgents;

    float speedBoostValue = 2f;

    public float timer = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        /*
        foreach (Transform child in GameObject.Find(flockListName).transform)
        flockObjects.Add(child.gameObject);
        */
        // AgentsObjects.Add(gameObject.CompareTag(TagAgents));

    }

    public void DropFood()
    {
        Debug.Log("dropping food");
        Vector3 spawnPosition = Random.insideUnitSphere * EcosystemRadius;
        spawnPosition.z = 0;
        Instantiate(foodPrefab, spawnPosition, Quaternion.Euler(spawnPosition));
    }

    // Update is called once per frame
    void Update()
    {
        //food
        if (Input.GetButtonDown("Fire1"))
        {
            for (int i = 0; i < AmountOfFoodToSpawn; i++)
            {
                DropFood();
            }


        }
         //hurt
        if (Input.GetButtonDown("Fire2"))
        {
            HurtEcosystem();
            foreach (GameObject flockObject in flockObjects)
            {
                flockObject.GetComponent<Specie>().HurtAll();
            }



        }



    }
    //dropfood
    //boost
    //hurt

    public void HurtEcosystem()
    {
        foreach(Specie specie in species)
        {
            specie.HurtSpecimens();
        }
    }

    /*
    IEnumerator Hurt ()
    {
        foreach (GameObject flockObject in flockObjects)
        {
            flockObject.GetComponent<Flock>().maxSpeed *= speedBoostValue;
        }

        yield return new WaitForSeconds(timer);

        foreach (GameObject flockObject in flockObjects)
        {
            flockObject.GetComponent<Flock>().maxSpeed /= speedBoostValue;
        }
    }
    */
    //heal
    //addspecies


}
