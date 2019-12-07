using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Flock/Behavior/Alignement")]


public class AlignementBehavior : FilteredFlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //if no neighbors, maintaint current alignement
        if (context.Count == 0)
            return agent.transform.up;

        //add all points together and average
        Vector2 alignementMove = Vector2.zero;

        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            alignementMove += (Vector2)item.transform.transform.up;
        }
        alignementMove /= context.Count;

      
        return alignementMove;
    }
}
