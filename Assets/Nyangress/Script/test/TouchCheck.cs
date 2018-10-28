using System;
using TouchScript.Gestures;
using UnityEngine;

public class TouchCheck : MonoBehaviour
{
    public LongPressGesture longPressGesture;

    private void OnEnable()
    {
        longPressGesture.LongPressed += OnLongPressed;
    }

    private void OnDisable()
    {
        longPressGesture.LongPressed -= OnLongPressed;
    }

    private void OnLongPressed(object sender, EventArgs e)
    {
        Debug.Log("長押しされた");
    }
}