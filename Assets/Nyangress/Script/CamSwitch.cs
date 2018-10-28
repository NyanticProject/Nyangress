using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitch : MonoBehaviour {
    public GameObject target;
    [SerializeField]
    bool isActive = false;
    // Use this for initialization
    public void camSwitch() {
        if (isActive)
        {
            target.SetActive(false);
            isActive = false;
        }
        else {
            target.SetActive(true);
            isActive = true;
        }

    }
}
