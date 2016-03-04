using UnityEngine;
using System.Collections;
using GDGeek;
using System;
using UnityEngine.UI;

public class Square : MonoBehaviour
{
    public Text _number = null;

    public int number { set {
            _number.text = value.ToString();
        } }

    public void hide()
    {
        this.gameObject.SetActive(false);
    }

    public void show()
    {
        this.gameObject.SetActive(true);
    }
}
