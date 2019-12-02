using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Zos
{
  public class SimonEngine : MonoBehaviour
  {
    #region PUBLIC_MEMBERS
    public Scene CurrScene { get; private set; }
    public List<int> Sequence { get; private set; }
    #endregion


    #region PUBLIC_METHODS
    #endregion

    #region PRIVATE_MEMBERS
    int state;
    #endregion

    #region PRIVATE_METHODS
    // Initialization
    void Start()
    {
      CurrScene = Scene.Idle;
      Sequence = new List<int>();
    }


    // Executed each frame
    void Update()
    {
      
    }
    #endregion
  }
}
