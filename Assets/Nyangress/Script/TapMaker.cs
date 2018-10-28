using System;
using TouchScript.Gestures;
using UnityEngine;
using UnityEngine.UI;
public class TapMaker : MonoBehaviour
{
    public TapGesture tapGesture;
    private void Start()
    {
    }
    private void OnEnable()
    {
        tapGesture.Tapped += OnTapped;
    }

    private void OnDisable()
    {
        tapGesture.Tapped -= OnTapped;
    }

    private void OnTapped(object sender, EventArgs e)
    {
        Debug.Log("tapped");
    }

    
}