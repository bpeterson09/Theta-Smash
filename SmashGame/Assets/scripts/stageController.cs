using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stageController : MonoBehaviour {

    public GameObject pos1;
    public GameObject pos2;
    public bool canGrab;

    // Use this for initialization
    void Start () {
        canGrab = true;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {

        bool direction = col.GetComponentInParent<playerController>().GetRightLooking();
        Debug.Log(direction);

            if (direction)
            {
                col.transform.parent.position = pos1.transform.position;
                col.GetComponentInParent<playerController>().setOnLedge(true);
            }
            else
            {
                col.transform.parent.position = pos2.transform.position;
                col.GetComponentInParent<playerController>().setOnLedge(true);
            }

    }
    
}
