using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This class controls the movement behavior of a flock 
// !! only the movement !!

// for prey / predator damage and reproduction behaviors,
// see Specie and Specimen classes

public class Flock : MonoBehaviour
{
    [Header("Flock Settings")]
    public GameObject agentPrefab;
    public CompositeBehavior behaviorTemplate;
    protected CompositeBehavior behavior;

    protected List<GameObject> agents = new List<GameObject>();


    public int maxAgentCount = 30;
    public int startingCount = 10;
    public float AgentDensity = 0.08f;

    public float drivefactor = 10;
    public float maxSpeed = 5f;

    public float neigbourgRadius = 1.5f;
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxspeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;

    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    // Start is called before the first frame update
    protected void Start()
    {
        behavior = Object.Instantiate(behaviorTemplate);
    
        squareMaxspeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neigbourgRadius * neigbourgRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        //SpawnInitialAgentCount();
    }

    public int AgentCount()
    {
        return agents.Count;
    }

    // spawn a new specimen at a given transform
    public void SpawnAgent(Vector3 pos, Quaternion rot)
    {
        if (agents.Count < maxAgentCount)
        {
            GameObject newSpecimen = Instantiate(agentPrefab, pos, rot, transform);
            addAgentToList(newSpecimen);
        }
    }

    // spawn a new specimen at a given transform
    public void SpawnInitialAgentCount()
    {
        for (int i = 0; i < startingCount; i++)
        {
            SpawnAgent(
                Random.insideUnitCircle * startingCount * AgentDensity,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f))
            );
        }
    }

    public void addAgentToList(GameObject agent) 
    {
        agents.Add(agent.gameObject);
    }

    // Update is called once per frame
    protected void Update()
    {
        // todo: remove
        //behavior = Object.Instantiate(behaviorTemplate);

        agents.RemoveAll(list_item => list_item == null);

        foreach (GameObject agent in agents)
        {
            FlockAgent flockAgent = agent.GetComponent<FlockAgent>();
           /* OnCollisionEnter(Collision collision);*/  

            List<Transform> context = GetNearbyObjects(flockAgent);

            // pour demo/debug 
           // agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

             Vector2 move = behavior.CalculateMove(flockAgent, context, this);
             move *= drivefactor;
             if (move.sqrMagnitude > squareMaxspeed)
             {
                 move = move.normalized * maxSpeed;
             }
             flockAgent.Move(move); 
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
}

