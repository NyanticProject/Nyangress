using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;

public class initializer : MonoBehaviour {
    public LocationUpdater updater;
    // Use this for initialization
    void Start () {
        GetComponent<AbstractMap>().Initialize(new Mapbox.Utils.Vector2d(updater.Latitude, updater.Longitude),5);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
