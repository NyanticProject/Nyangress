using System;
using System.Text;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Utils;

public class Mover : MonoBehaviour
{
    public ScreenTransformGesture transformGesture;
    Vector2d pointer;
    AbstractMap maps;
    public Vector2d changed;
    public bool isTouched = false;
    private void Start()
    {
        maps = GameObject.Find("LocationBasedGame/Map").GetComponent<AbstractMap>();
    }
    private void OnEnable()
    {
        transformGesture.TransformStarted += OnTransformStarted;
        transformGesture.Transformed += OnTransformed;
        transformGesture.TransformCompleted += OnTransformCompleted;
    }

    private void OnDisable()
    {
        transformGesture.TransformStarted -= OnTransformStarted;
        transformGesture.Transformed -= OnTransformed;
        transformGesture.TransformCompleted -= OnTransformCompleted;
    }

    private void OnTransformStarted(object sender, EventArgs e)
    {
        isTouched = true;
        Debug.Log("変形を開始した");
        pointer = maps.CenterLatitudeLongitude;
    }

    private void OnTransformed(object sender, EventArgs e)
    {
        var g = transformGesture;
        var sb = new StringBuilder();
        sb.AppendLine("変形中");
        sb.AppendLine("DeltaPosition : " + g.DeltaPosition);
        sb.AppendLine("DeltaScale: " + g.DeltaScale);
        Vector2d changed = new Vector2d(pointer.x+(g.DeltaPosition.x*0.5), pointer.y + (g.DeltaPosition.y*0.5));
        maps.UpdateMap();
        Debug.Log(sb);
    }

    private void OnTransformCompleted(object sender, EventArgs e)
    {
        Debug.Log("変形を完了した");
        isTouched = false;
    }
}