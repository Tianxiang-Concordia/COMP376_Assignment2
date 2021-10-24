using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointBoard : MonoBehaviour
{
    private int point;

    public PointBoard()
    {
        point = 0;
    }

    public void Increment(int increment)
    {
        point += increment;
    }

    public void Decrement(int decrement)
    {
        point -= decrement;
    }

    private void Update()
    {
        GetComponent<Text>().text = point.ToString();
    }

    public int GetPoint()
    {
        return point;
    }
}