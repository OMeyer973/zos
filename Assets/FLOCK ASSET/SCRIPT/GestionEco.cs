using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionEco : MonoBehaviour
{

    float speedBoostValue = 2f;

    public float timer = 0.0f;
    public string flockListName;
    public Rigidbody2D foodrigid;
    public Transform origine;
    public float NombreDeBouff;
   
    public List<GameObject> flockObjects;
    public List<GameObject> AgentsObjects;
    public string TagAgents;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in GameObject.Find(flockListName).transform)
        flockObjects.Add(child.gameObject);

       // AgentsObjects.Add(gameObject.CompareTag(TagAgents));

    }

   
    public void DropFood()
    {

        Rigidbody2D instance;
        instance = Instantiate(foodrigid, transform.position = Random.insideUnitCircle * 20, origine.rotation) as Rigidbody2D;


    }

    // Update is called once per frame
    void Update()
    {


        //food
        if (Input.GetButtonDown("Fire1"))
        {

            for (int i = 0; i < NombreDeBouff; i++)
            {
                Rigidbody2D instance;
                instance = Instantiate(foodrigid, transform.position = Random.insideUnitCircle * 20, origine.rotation) as Rigidbody2D;

            }


        }
         //hurt
        if (Input.GetButtonDown("Fire2"))
        {


            

            StartCoroutine(Hurt());
            foreach (GameObject flockObject in flockObjects)
            {
                flockObject.GetComponent<Flock>().HurtAll();
            }



        }



    }
    //dropfood
    //boost
    //hurt


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
    //heal
    //addspecies


}
