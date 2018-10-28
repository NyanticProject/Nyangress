using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Visualizer : MonoBehaviour {
    public GameObject target;
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="MainCamera")
        target.SetActive(true);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "MainCamera")
            target.SetActive(false);
    }
}
