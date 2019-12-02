using UnityEngine;
using System.Collections;
using System;

namespace Zos
{

  public enum Command
  {
    ChangeScene  = 'S',
    TriggerLiana = 'L'
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
    SmallVictory = 'V',
    BigVictory   = 'W',
    Fail         = 'F'
  }

  public class ArduinoWhisperer : MonoBehaviour
  {
    #region PUBLIC_MEMBERS
    public SerialController serialController;
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
    public void TriggerLiana(int i)
    {
      SendToArduino("" + (char)Command.TriggerLiana + i);
    }
    #endregion

    #region PRIVATE_METHODS
    // Initialization
    void Start()
    {
      serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
    }


    // Executed each frame
    void Update()
    {
      //---------------------------------------------------------------------
      // Sending data
      //---------------------------------------------------------------------

      // changing scenes
      if (Input.GetKeyDown(KeyCode.A))
      {
        SendToArduino("SI");
      }

      if (Input.GetKeyDown(KeyCode.Z))
      {
        SendToArduino("SD");
      }
      if (Input.GetKeyDown(KeyCode.E))
      {
        SendToArduino("SA");
      }

      // triggering lianas
      if (Input.GetKeyDown(KeyCode.Q))
      {
        SendToArduino("L0");
      }
      if (Input.GetKeyDown(KeyCode.S))
      {
        SendToArduino("L1");
      }
      if (Input.GetKeyDown(KeyCode.D))
      {
        SendToArduino("L2");
      }
      if (Input.GetKeyDown(KeyCode.F))
      {
        SendToArduino("L3");
      }


      if (Input.GetKeyDown(KeyCode.G))
      {
        ChangeScene(Scene.Idle, Transition.BigVictory);
      }

      //---------------------------------------------------------------------
      // Receive data
      //---------------------------------------------------------------------

      string message = serialController.ReadSerialMessage();

      if (message == null)
        return;

      // Check if the message is plain data or a connect/disconnect event.
      if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
        Debug.Log("Connection established");
      else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
        Debug.Log("Connection attempt failed or disconnection detected");
      else
        Debug.Log("Message arrived: " + message);
    }


    // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - 
    // send a string message to the arduino board
    // message is sent to arduino character by character for more robustness
    // a command executed by the arduino program must end with ">"
    private void SendToArduino(string msg)
    {
      // optional : flushing Arduino command buffer in case bad characters are still in it
      // serialController.SendSerialMessage(">");
      foreach (char c in msg)
      {
        serialController.SendSerialMessage(c.ToString());
      }
      // Arduinos commands are executed when a ">" is received
      serialController.SendSerialMessage(">");

      Debug.Log("Sending " + msg + ">");
    }
    #endregion
  }
}
