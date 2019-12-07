using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Zos
{
  // this class serves to play the sounds from simonEngine
  public class SoundEngine : MonoBehaviour
  {
    public List<AudioSource> Lianas = new List<AudioSource>(4);

    public AudioSource SimonStart;
    public AudioSource BeginAction;
    public AudioSource SmallVictory;
    public AudioSource Fail;
    public AudioSource BigVictory;

    public void PlayLianaSound(int liana)
    {
      Lianas[liana].GetComponent<AudioSource>().Play();
    }

    public void PlayTransitionSound(Transition transition)
    {
      switch (transition)
      {
        case Transition.SimonStart:
          SimonStart.GetComponent<AudioSource>().Play();
          break;

        case Transition.BeginAction:
          BeginAction.GetComponent<AudioSource>().Play();
          break;

        case Transition.SmallVictory:
          SmallVictory.GetComponent<AudioSource>().Play();
          break;

        case Transition.Fail:
          Fail.GetComponent<AudioSource>().Play();
          break;

        case Transition.BigVictory:
          BigVictory.GetComponent<AudioSource>().Play();
          break;
        default:
          Debug.LogError("Trying to play a sound for an unknown transition");
          break;
      }
      // todo
    }



  }
}