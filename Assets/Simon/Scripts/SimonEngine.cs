using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Zos
{
  // This class manages the core logic of the installation.
  // It is based on a big state machine of scenes and liana interactions
  // see botom of file for details about the state machine
  public class SimonEngine : MonoBehaviour
  {
    public ArduinoWhisperer arduinoWhisperer; // component that is responsible for talking to the arduino board
    public SoundEngine soundEngine; // component that is responsible for playing the audio

    public Scene CurrScene { get; private set; } // Current scene : Idle, Demo, Action

    private SimonSequence simonSequence = new SimonSequence(); // sequence of lianas to be touched

    private bool[] LianaStates = { false, false, false, false }; // current interaction state of the lianas

    [SerializeField]
    private float MaxActionTime = 60; // in action scene : time before going back to idle scene (in s)
    private float ActionTime = 0; // variable to count time in action scene


    // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - 
    // Sets the state of the liana of id i
    public void SetLianaState(int i, bool state)
    {
      Debug.Assert(0 <= i && i < 4, "SetLianaState called with invalid id");
      Debug.Log("SimonEngine: Setting Liana " + i + " " + state);
      LianaStates[i] = state;
      if (state)
        soundEngine.PlayLianaSound(i);
    }

    // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - 
    void ResetLianaStates()
    {
      for (int i = 0; i < 4; i++)
      {
        LianaStates[i] = false;
      }
    }

    void ResetActionTime()
    {
      ActionTime = 0;
    }

    // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - 
    // Initialization
    void Start()
    {
      Debug.Log("SimonEngine: Initiating Simon Engine");

      arduinoWhisperer = GameObject.Find("ArduinoWhisperer").GetComponent<ArduinoWhisperer>();
      arduinoWhisperer.simonEngine = this;
      CurrScene = Scene.Idle;
      ResetLianaStates();
      arduinoWhisperer.ChangeScene(Scene.Idle, Transition.SimonStart);
    }

    //---------------------------------------------------------------------
    // Scene transition methods
    //---------------------------------------------------------------------

    // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - 
    // transition function called when going from Idle to Demo
    void SimonStart()
    {
      Debug.Log("SimonEngine: Starting Simon game");
      CurrScene = Scene.Demo;
      ResetLianaStates(); // useless
      arduinoWhisperer.ChangeScene(Scene.Demo, Transition.SimonStart);
      soundEngine.PlayTransitionSound(Transition.SimonStart);

      simonSequence.Init();
      StartCoroutine(PlayDemo());
    }

    // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - 
    // transition function called when going from Demo to Action
    void GoToActionScene()
    {
      Debug.Log("SimonEngine: Simon demo ended, now it's your turn");

      CurrScene = Scene.Action;
      ResetLianaStates(); // useless
      simonSequence.Restart();
      arduinoWhisperer.ChangeScene(Scene.Action, Transition.BeginAction);
      soundEngine.PlayTransitionSound(Transition.BeginAction);

      ResetActionTime();
    }

    // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - 
    // transition function called when going from Action to demo (small victory)
    void PlayNextSimonSequence()
    {
      Debug.Log("SimonEngine: Simon game complete, Starting next simon demo (small victory)");

      CurrScene = Scene.Demo;
      ResetLianaStates(); // useless
      simonSequence.Restart();
      arduinoWhisperer.ChangeScene(Scene.Demo, Transition.SmallVictory);
      soundEngine.PlayTransitionSound(Transition.SmallVictory);

      simonSequence.NextSequence();
      // todo : drop food in ecosystem
      // todo : boost ecosystem
      StartCoroutine(PlayDemo());
    }

    // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - 
    // transition function called when going from Action to Idle (Simon failed)
    void FailSimon()
    {
      Debug.Log("SimonEngine: Simon game failed");

      CurrScene = Scene.Idle;
      ResetLianaStates();
      arduinoWhisperer.ChangeScene(Scene.Idle, Transition.Fail);
      soundEngine.PlayTransitionSound(Transition.Fail);

      // todo : hurt ecosystem
    }

    // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - 
    // transition function called when going from Action to Idle (Simon passed)
    void WinSimon()
    {
      Debug.Log("SimonEngine: Simon game won ! (big victory)");

      CurrScene = Scene.Idle;
      ResetLianaStates();
      arduinoWhisperer.ChangeScene(Scene.Idle, Transition.BigVictory);
      soundEngine.PlayTransitionSound(Transition.BigVictory);

      // todo : add specie to ecosystem
      // todo : drop food in ecosystem
      // todo : heal ecosystem
      // todo : boost ecosystem
    }

    //---------------------------------------------------------------------
    // Core scene methods
    //---------------------------------------------------------------------

    // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - 
    // Core Action scene method : called by ArduinoWhisperer when a liana is touched
    public void TriggerLiana(int triggeredLiana)
    {
      Debug.Assert(CurrScene == Scene.Action, "tried to triggerLiana in a scene != Action scene");
      Debug.Assert(0 <= triggeredLiana && triggeredLiana < 4, "SetLianaState called with invalid id");
      soundEngine.PlayLianaSound(triggeredLiana);

      if (CurrScene == Scene.Action) // should be useless but We'll keep it as security
      {
        Debug.Log("SimonEngine: triggering Liana " + triggeredLiana);

        if (triggeredLiana == simonSequence.GetCurrentLiana())
        {
          simonSequence.NextLiana();
        }
        else
        {
          FailSimon();
        }

        if (simonSequence.IsFinished() && simonSequence.IsLastSequence())
        {
          WinSimon();
        }
        else if (simonSequence.IsFinished())
        {
          PlayNextSimonSequence();
        }
      }
    }

    // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - 
    // Core Demo scene coroutine : called on new simon sequence launch
    IEnumerator PlayDemo()
    {
      Debug.Assert(CurrScene == Scene.Demo, "tried to play Demo out of Demo Scene !");
      if (CurrScene == Scene.Demo) // should be useless but We'll keep it as security
      {
        Debug.Log(simonSequence);

        yield return new WaitForSeconds(8);

        // for (int i = 0; i < simonSequence.SequenceSize(); i++)
        while (!simonSequence.IsFinished())
        {
          arduinoWhisperer.TriggerLiana(simonSequence.GetCurrentLiana());
          soundEngine.PlayLianaSound(simonSequence.GetCurrentLiana());
          simonSequence.NextLiana();
          //yield on a new YieldInstruction that waits for 5 seconds.
          if (!simonSequence.IsFinished())
            yield return new WaitForSeconds(3);
          else
            yield return new WaitForSeconds(3);
        }

        GoToActionScene();
      }
    }

    // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - 
    // Core Idle scene method : called each frame
    void IdleUpdate()
    {
      bool allLianasLit = true;
      for (int i = 0; i < 4; i++)
      {
        if (LianaStates[i])
        {
          //todo attract ecosysteme creatures toward point i
        }
        else
        {
          allLianasLit = false;
        }
      }
      // all lianas are being interacted at the same time : Simon Game can start
      if (allLianasLit)
      {
        SimonStart();
      }
    }

    // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - 
    // Core Action scene method : called each frame
    void ActionUpdate()
    {
      ActionTime += Time.deltaTime;
      // if we wait more than allowed before doing an action : going back to Idle
      if (ActionTime > MaxActionTime)
      {
        Debug.Log("SimonEngine: Action time is up ! going back to Idle mode");
        Start();
      }
    }

    void Update()
    {
      if (CurrScene == Scene.Idle)
      {
        IdleUpdate();
      }
      if (CurrScene == Scene.Action)
      {
        ActionUpdate();
      }
    }


    /*  
     *  Summary of the methods constitution the state machine and their sequencing
     *  
     *  Legend:
     *  "A->B" - method B is directly called by method A
     *  "A..B" - method B will follow method A but not directly. (needs input from arduino first)
     *  
     *  
     *  Start()           .. IdleUpdate()           // IdleUpdate() is the Idle scene (called in continuous every frame)
     *  
     *  IdleUpdate()      -> SimonStart()           // SimonStart() is the transition Idle -> Demo scenes (when 4 lianas touched at the same time)
     *  
     *  SimonStart()      -> PlayDemo()             // PlayDemo() is the Demo Scene (autoplay with timer) -- going through all the lianas in the curent simonSequence
     *  
     *  PlayDemo()        -> GoToActionScene()      // GoToActionScene() is the transition Demo -> Action scenes
     *  
     *  GoToActionScene() .. ActionUpdate()         // ActionUpdate() is the Action scene (called in continuous every frame)
     *  
     *  ActionUpdate()    -> Start()                // if action timer goes to 0
     *  or
     *  ActionUpdate()    .. TriggerLiana()         // triggerLiana called on arduino message received -- going through all the lianas in the curent simonSequence
     *  
     *  TriggerLiana()   -> PlayNextSimonSequence() // if the sequence is correct
     *  or
     *  TriggerLiana()    -> FailSimon()            // if the sequence is incorrect
     *  or
     *  TriggerLiana()    -> WinSimon()             // if the sequence is correct and it was the last one
     *  
     *  PlayNextSimonSequence() -> PlayDemo()       // setting up next round of Simon
     *  
     *  FailSimon()      .. IdleUpdate()            // back to idle after simon fail
     *  
     *  WinSimon()       .. IdleUpdate()            //  back to idle after simon win
    */


  }
}
