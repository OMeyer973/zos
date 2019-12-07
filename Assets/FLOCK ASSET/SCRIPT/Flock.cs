using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{

    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehavior behavior;


    [Range(5, 500)]
    public int startingCount = 250;
    public float AgentDensity = 0.08f;

    [Range(1f, 100f)]
    public float drivefactor = 10;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;

    [Range(1f, 10f)]
    public float neigbourgRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    public float DommageFlock;

   


    float squareMaxspeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;

    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }


    // Start is called before the first frame update
    void Start()
    {
        squareMaxspeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neigbourgRadius * neigbourgRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;


        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitCircle * startingCount * AgentDensity,
                
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),

                transform

                ) ;
            newAgent.name = "Agent" + i;

           
            newAgent.Initialize(this);
           
            // newAgent.Initialize(this);
            agents.Add(newAgent);
        }

    }

    public void addAgent(FlockAgent agent) 
    {
        agents.Add(agent);
    }


   
   


    // Update is called once per frame

    void Update()
    {

        agents.RemoveAll(list_item => list_item == null);

        foreach (FlockAgent agent in agents)
        {

           /* OnCollisionEnter(Collision collision);*/  

            List<Transform> context = GetNearbyObjects(agent);

            // pour demo/debug 
           // agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

             Vector2 move = behavior.CalculateMove(agent, context, this);
             move *= drivefactor;
             if (move.sqrMagnitude > squareMaxspeed)
             {
                 move = move.normalized * maxSpeed;
             }
             agent.Move(move); 
        }
    }
    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neigbourgRadius);
        //pour la 3d virrer les 2d
        foreach (Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }

    public void HurtAll()

    {

       foreach (FlockAgent agent in agents)
        {
            agent.gameObject.GetComponent<aaa>().Hurt(DommageFlock);

        }
        
    }
}

