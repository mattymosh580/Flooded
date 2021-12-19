using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeOrientationManager : MonoBehaviour
{
    [SerializeField]
    List<FixPipeGame2.Directions> startingDirections = new List<FixPipeGame2.Directions>();
    [SerializeField]
    List<FixPipeGame2.Directions> directions;

    public void Start()
    {
        Reset();
    }

    public void RotateLeft()
    {
        for(int i = 0; i < directions.Count; i++)
        {
            directions[i]--;
            if(directions[i] < FixPipeGame2.Directions.Left)
            {
                directions[i] = FixPipeGame2.Directions.Down;
            }
        }
    }

    public void RotateRight()
    {
        for (int i = 0; i < directions.Count; i++)
        {
            directions[i]++;
            if (directions[i] > FixPipeGame2.Directions.Down)
            {
                directions[i] = FixPipeGame2.Directions.Left;
            }
        }
    }

    public void Reset()
    {
        directions.Clear();
        foreach(var direction in startingDirections)
        {
            directions.Add(direction);
        }
    }

    public List<FixPipeGame2.Directions> GetDirections()
    {
        return directions;
    }
}
