using UnityEngine;
using System.Collections;
using System;

public class Model : MonoBehaviour {
    public Cube[] list;
    public int Width = 4;
    public int height = 7;

    public Cube getCube(int x, int y)
    {
        if (x < 0 || x >= 4)
        {
            return null;
        }
        if (y < 0 || y >= 7)
        {
            Debug.Log(y);
            return null;
            
        }   
        int n = x + y * 4;

        return list[n];
    }

}
