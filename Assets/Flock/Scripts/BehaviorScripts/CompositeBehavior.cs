using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Composite")]
public class CompositeBehavior : FilteredFlockBehavior
{

    public FlockBehavior[] behaviors;
    public float[] weights;
    public float behaviorIntensity = 1f;
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {

        //handle data mismatch
       if(weights.Length != behaviors.Length)
        {
            Debug.LogError("Data mismatch in" + name, this);
            return Vector2.zero;
        }
        //set up move
        Vector2 move = Vector2.zero;

        float totalWeight = 0f;
        for (int i = 0; i < behaviors.Length; i++)
        {   
            if (behaviors[i] != null)
            {
                Vector2 partialMove = behaviors[i].CalculateMove(agent, context, flock) * weights[i];
                totalWeight += weights[i];

                if (partialMove != Vector2.zero)
                {
                    move += partialMove;
                }
            }
        }
        if (totalWeight != 0)
            move = move * behaviorIntensity / totalWeight;
        else move = Vector2.zero;

        //iterate through behavior
        /*
        for (int i = 0; i < behaviors.Length; i++)
        {
            Vector2 partialMove = behaviors[i].CalculateMove(agent, context, flock) * weights[i];

            if (partialMove != Vector2.zero)
            {
                if (partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];

                }

                move += partialMove;
            }
        }
        */
        return move;

    }
}
