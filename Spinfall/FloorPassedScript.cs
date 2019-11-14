using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPassedScript : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            EventManager.RaiseBallPassedFloorEvent();
            Destroy(this.gameObject);
        }
    }
}
