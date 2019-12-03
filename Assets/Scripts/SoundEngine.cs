using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Zos
{
  // this class serves to play the sounds from simonEngine
  public class SoundEngine : MonoBehaviour
  {

    public void PlayTransitionSound(Transition transition)
    {
      switch (transition)
      {
        case Transition.SimonStart:
          // todo
          break;

        case Transition.BeginAction:
          // todo
          break;

        case Transition.SmallVictory:
          // todo
          break;

        case Transition.Fail:
          // todo
          break;

        case Transition.BigVictory:
          // todo
          break;
        default:
          Debug.LogError("Trying to play a sound for an unknown transition");
          break;
      }
      // todo
    }



  }
}