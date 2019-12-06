using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOOD : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Rigidbody2D foodrigid;
    public Transform origine;





    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Rigidbody2D instance;
            instance = Instantiate (foodrigid, transform.position = Random.insideUnitCircle * 20, origine.rotation) as Rigidbody2D;


        }
    }
}
