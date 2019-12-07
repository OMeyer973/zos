using UnityEngine;
using System.Collections;
using System;

namespace Zos
{

  public enum Command
  {
    ChangeScene  = 'S',
    TriggerLiana = 'L',
    LianaOn      = 'I',
    LianaOff     = 'O'
  }
  public enum Scene
  {
    Idle   = 'I',
    Demo   = 'D',
    Action = 'A'
  }

  public enum Transition
  {
    SimonStart   = 'S',
    BeginAction  = 'A',
    SmallVictory = 'V',
    BigVictory   = 'W',
    Fail         = 'F'
  }

  public class ArduinoWhisperer : MonoBehaviour
  {
    #region PUBLIC_MEMBERS
    public SerialController serialController;
    public SimonEngine simonEngine;
    #endregion

    #region PUBLIC_METHODS
    // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - 
    // sends a 'change scene with a given transition' command to the arduino board
    public void ChangeScene(Scene scene, Transition transition)
    {
      SendToArduino("" + (char)Command.ChangeScene + (char)scene + (char)transition);
    }

    // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - 
    // sends a 'trigger liana of given id' command to the arduino board
    // (command only executed when Arduino is in Demo scene)
    public void TriggerLiana(int i)
    {
      //Debug.Log("Unity triggers liana " + i + " on Arduino side");
      SendToArduino("" + (char)Command.TriggerLiana + i);
    }
    #endregion

    #region PRIVATE_METHODS
    // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - 
    // Initialization
    void Start()
    {
      serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
    }


    // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - 
    // Executed each frame
    void Update()
    {
      // todo : remove this line ! debug only
      testWithDebugCommandsFromKeyboard();

      
      // Receive data from arduino
      string message = serialController.ReadSerialMessage();

      if (message == null)
        return;

      // Check if the message is plain data or a connect/disconnect event.
      if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
      {
        Debug.Log("Connection established");
      }
      else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
      {
        Debug.Log("Connection attempt failed or disconnection detected");
      }
      else
      {
        Debug.Log("Arduino: " + message);
        DecodeMessage(message);
      }
    }

    // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - 
    // Decode a message received from the arduino on the serial port 
    // and execute the given command
    // 
    void DecodeMessage(string msg)
    {
      if (msg.Length == 3 && msg[0] == '>')
      {
        if (msg[1] == (char)Command.TriggerLiana)
        {
          Debug.Assert('0' <= msg[2] && msg[2] <= '9');
          //Debug.Log("Arduino triggers liana " + (msg[2] - '0') + " on unity side");
          simonEngine.TriggerLiana(msg[2] - '0');
        }

        if (msg[1] == (char)Command.LianaOn)
        {
          Debug.Assert('0' <= msg[2] && msg[2] <= '9');
          //Debug.Log("Arduino sets liana " + (msg[2] - '0') + " up on unity side");
          simonEngine.SetLianaState(msg[2] - '0', true);
        }

        if (msg[1] == (char)Command.LianaOff)
        {
          Debug.Assert('0' <= msg[2] && msg[2] <= '9');
          //Debug.Log("Arduino sets liana " + (msg[2] - '0') + " down on unity side");
          simonEngine.SetLianaState(msg[2] - '0', false);
        }

      }
    }


    // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - 
    // send a string message to the arduino board
    // message is sent to arduino character by character for more robustness
    // a command executed by the arduino program must end with ">"
    void SendToArduino(string msg)
    {
      // optional : flushing Arduino command buffer in case bad characters are still in it
      // serialController.SendSerialMessage(">");
      foreach (char c in msg)
      {
        serialController.SendSerialMessage(c.ToString());
      }
      // Arduinos commands are executed when a ">" is received
      serialController.SendSerialMessage(">");

      Debug.Log("ArduinoWhisperer: Sending " + msg + "> to arduino");
    }
    #endregion




    //---------------------------------------------------------------------
    // Receive and send fake commands to arduino
    void testWithDebugCommandsFromKeyboard()
    {

      // Send scene changes to arduino
      if (Input.GetKeyDown(KeyCode.I))
      {
        ChangeScene(Scene.Idle, Transition.BigVictory);
      }
      if (Input.GetKeyDown(KeyCode.O))
      {
        ChangeScene(Scene.Demo, Transition.BigVictory);
      }
      if (Input.GetKeyDown(KeyCode.P))
      {
        ChangeScene(Scene.Action, Transition.BigVictory);
      }

      // Send lianas triggers to arduino (normally only in demo mode)
      if (Input.GetKeyDown(KeyCode.J))
      {
        TriggerLiana(0);
      }
      if (Input.GetKeyDown(KeyCode.K))
      {
        TriggerLiana(1);
      }
      if (Input.GetKeyDown(KeyCode.L))
      {
        TriggerLiana(2);
      }
      if (Input.GetKeyDown(KeyCode.M))
      {
        TriggerLiana(3);
      }

      // Receive setlianas Up from Arduino (normally only in Idle mode)
      if (Input.GetKeyDown(KeyCode.A))
      {
        simonEngine.SetLianaState(0, true);
      }
      if (Input.GetKeyDown(KeyCode.Z))
      {
        simonEngine.SetLianaState(1, true);
      }
      if (Input.GetKeyDown(KeyCode.E))
      {
        simonEngine.SetLianaState(2, true);
      }
      if (Input.GetKeyDown(KeyCode.R))
      {
        simonEngine.SetLianaState(3, true);
      }

      // Receive setlianas Down from Arduino (normally only in Idle mode)
      if (Input.GetKeyDown(KeyCode.Q))
      {
        simonEngine.SetLianaState(0, false);
      }
      if (Input.GetKeyDown(KeyCode.S))
      {
        simonEngine.SetLianaState(1, false);
      }
      if (Input.GetKeyDown(KeyCode.D))
      {
        simonEngine.SetLianaState(2, false);
      }
      if (Input.GetKeyDown(KeyCode.F))
      {
        simonEngine.SetLianaState(3, false);
      }

      // Receive lianas triggers from Arduino (normally only in Action mode)
      if (Input.GetKeyDown(KeyCode.W))
      {
        simonEngine.TriggerLiana(0);
      }
      if (Input.GetKeyDown(KeyCode.X))
      {
        simonEngine.TriggerLiana(1);
      }
      if (Input.GetKeyDown(KeyCode.C))
      {
        simonEngine.TriggerLiana(2);
      }
      if (Input.GetKeyDown(KeyCode.V))
      {
        simonEngine.TriggerLiana(3);
      }
      // End tests
      //---------------------------------------------------------------------
    }
  }
}
