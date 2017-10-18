using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour {

    int direction = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Physics.Raycast(transform.position, direction * Vector3.right, 0.5f))
        {
            direction *= -1;
        }
        else if (!Physics.Raycast(transform.position, direction * Vector3.right - Vector3.up, 0.5f))
        {
            direction *= -1;
        }
    }
}
