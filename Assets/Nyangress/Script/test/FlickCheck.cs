using System;
using TouchScript.Gestures;
using UnityEngine;

public class FlickCheck : MonoBehaviour
{
    public FlickGesture flickGesture;

    private void OnEnable()
    {
        flickGesture.Flicked += OnFlicked;
    }

    private void OnDisable()
    {
        flickGesture.Flicked -= OnFlicked;
    }

    private void OnFlicked(object sender, EventArgs e)
    {
        Debug.Log("フリックされた: " + flickGesture.ScreenFlickVector);
    }
}