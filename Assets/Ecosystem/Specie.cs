using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Specie : MonoBehaviour
{
  public Flock flock;

  List<Specimen> population;

  public float DommageFlock;

  // Start is called before the first frame update
  void Start()
  {

  }

  
   


  // Update is called once per frame

  void Update()
  {

    // Send scene changes to arduino
    if (Input.GetKeyDown(KeyCode.G))
    {
      flock.FreakOut();
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

