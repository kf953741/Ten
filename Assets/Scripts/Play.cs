using UnityEngine;
using System.Collections;
using GDGeek;
using System;
public class Play : MonoBehaviour {
    private Square[] list = null;
    // Use this for initialization
    void Awake()
    {
        list = this.GetComponentsInChildren<Square>();
        Debug.Log(list.Length);
    }
	void Start () {
        //list[0].gameObject.SetActive(false);
        foreach (Square square in list)
        {
            square.gameObject.SetActive(false);
        }
	}

    // Update is called once per frame
    public Square getSquare(int x, int y)
    {
        int n = x + y * 4;
        return list[n];
    }
}
