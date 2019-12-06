using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionEco : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Rigidbody2D foodrigid;
    public Transform origine;
    public float NombreDeBouff;


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
            Rigidbody2D instance;
            instance = Instantiate (foodrigid, transform.position = Random.insideUnitCircle * 20, origine.rotation) as Rigidbody2D;


        }
         //hurt
        if (Input.GetButtonDown("Fire2"))
        { 
        



        }

        

    }
    //dropfood
    //boost
    //hurt
    //heal
    //addspecies


}
