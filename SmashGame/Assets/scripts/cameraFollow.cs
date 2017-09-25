using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour {

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private float distance;
    private float middle;
    private float height;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        UpdateBounds();
        UpdatePos();
	}

    void UpdateBounds()
    {
        minX = Mathf.Infinity;
        maxX = -Mathf.Infinity;
        minY = Mathf.Infinity;
        maxY = -Mathf.Infinity;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("player"))
        {
            if (player.transform.position.x < minX)
            {
                minX = player.transform.position.x;
            }
            if (player.transform.position.x > maxX)
            {
                maxX = player.transform.position.x;
            }
            if (player.transform.position.y < minY)
            {
                minY = player.transform.position.y;
            }
            if (player.transform.position.y > maxY)
            {
                maxY = player.transform.position.y;
            }
        }
        distance = Mathf.Abs(maxX - minX);
        distance = Mathf.Clamp(distance, 3, 10);
        middle = (maxX + minX) / 2;
        height = (maxY + minY) / 2;
    }

    void UpdatePos()
    {
        transform.position = new Vector3(middle, height + 1, -4 - (distance * 0.25f));
        //Debug.Log(distance);
    }
}
