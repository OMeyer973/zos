using System.Collections.Generic;
using UnityEngine;

// defines the serie of sequences of lianas in the simon game.
// a sequence is a series of lianas ids.

// this class is used to 
// - get the current liana in the current sequence,       - called on Demo & Action
// - advance to the next liana in the current sequence,   - called on Demo & Action
// - know if the current sequence is over,                - called on Demo & Action
// - restart the current sequence without reseting it,    - called on Demo->Action
// - get the next sequence in the serie,                  - called on small victory
// - know if the serie is over                            - called on action victory
// - restart the serie                                    - called on big victory

namespace Zos
{
  public class SimonSequence
  {
    static int initialSeqSize = 3;
    static int finalSeqSize = 5;

    List<int> Sequence = new List<int>();
    int CurrentLianaId = 0;
    
    public override string ToString()
    {
      string str = "SimonSequence: ";
      foreach (int seq in Sequence)
      {
        str += seq + ", ";
      }
      return str + " currLiana:" + CurrentLianaId + ", isFinished:" + CurrentLianaId;
    }

    // initialise the first sequence (called on simonstart, when coming from Idle scene)
    public void Init()
    {
      InitSequence(initialSeqSize);
      CurrentLianaId = 0;
    }

    // initialise the ids of the lianas in the sequence at random
    void InitSequence(int size)
    {
      Sequence.Clear();
      for (int i = 0; i < size; i++)
      {
        Sequence.Add((int)Random.Range(0, 2));
        // prevent 2 following lianas from being the same
        while (i > 0 && Sequence[i] == Sequence[i - 1])
          Sequence[i] = (int)Random.Range(0, 4);
      }
    }

    // goes back to the begining of the sequence without reseting it
    public void Restart()
    {
      CurrentLianaId = 0;
    }

    // return the id of the current liana in the sequence
    public int GetCurrentLiana()
    {
      return Sequence[CurrentLianaId];
    }

    // size of the current sequence of lianas
    public int SequenceSize()
    {
      return Sequence.Count;
    }

    // If sequence not over, advance one liana in the sequence
    // sets a flag when end of sequence is reached
    public void NextLiana()
    {
      if (CurrentLianaId < SequenceSize())
      {
        CurrentLianaId++;
      }
    }

    // is the current sequence finished ? 
    public bool IsFinished()
    {
      return CurrentLianaId >= SequenceSize();
    }

    // If serie not over, advance one sequence in the serie
    // sets a flag when end of serie is reached
    public void NextSequence()
    {
      if (SequenceSize() + 1 < finalSeqSize)
      {
        InitSequence(SequenceSize() + 1);
      }
    }

    // is the current sequence the last one of the serie ? 
    public bool IsLastSequence()
    {
      return SequenceSize() + 1 >= finalSeqSize;
    }

    private void Start()
    {
      Init();
    }



  }
}