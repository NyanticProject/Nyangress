using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class debugLogger : MonoBehaviour {


    public Text txt;
    public Mover mover;
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
        txt.text = "Mover : " + mover.isTouched;
        
	}

    private void LateUpdate()
    {
        if (mover.isTouched)
        {

            txt.text += "\r\nPosition : " + mover.changed.x.ToString()+" , "+ mover.changed.x.ToString();

        }
    }
}
