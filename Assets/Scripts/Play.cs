using UnityEngine;
using System.Collections;
using GDGeek;
using System;
public class Play : MonoBehaviour {
    public Square phototype;
    public GameObject _root = null;
    private Square[] list = null;
    // Use this for initialization
    void Awake()
    {
        list = this._root.GetComponentsInChildren<Square>();
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

    public Task moveTask(int number, Vector2 begin, Vector2 end)
    {
        Square s = (Square)GameObject.Instantiate(phototype);
        Square b = this.getSquare((int)begin.x, (int)begin.y);
        Square e = this.getSquare((int)end.x, (int)end.y);
        s.transform.parent = b.transform.parent;
        s.transform.localScale = b.transform.localScale;
        s.transform.localPosition = b.transform.localPosition;
        s.show();
        s.number = number;
        b.hide();
        TweenTask tt = new TweenTask(delegate () { return TweenLocalPosition.Begin(s.gameObject, 0.8f, e.transform.localPosition); });
        TaskManager.PushBack(tt, delegate
        {
          GameObject.DestroyObject(s.gameObject);
           
        });
        return tt;
    }
}
