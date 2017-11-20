using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBody_Pickup : MonoBehaviour {

	public float radius;
	public float maxRadius;
	public float force;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) {
			if (radius < maxRadius) {
				radius += Time.deltaTime;
			}
			Collider2D[] objects = Physics2D.OverlapCircleAll (Camera.main.ScreenToWorldPoint(Input.mousePosition), radius);
			foreach (Collider2D objCol in objects) {
				GameObject obj = objCol.gameObject;
				if (obj.GetComponent<Rigidbody2D> ()) {
					if (Vector3.Distance (Camera.main.ScreenToWorldPoint (Input.mousePosition), obj.transform.position) > 0.5f) {
						obj.GetComponent<Rigidbody2D> ().AddForce ((Camera.main.ScreenToWorldPoint (Input.mousePosition) - obj.transform.position) * force * Vector3.Distance (Camera.main.ScreenToWorldPoint (Input.mousePosition), obj.transform.position));
					}
				}
			}
		}
		if (Input.GetMouseButtonUp (0)) {
			radius = 0;
		}
	}

	void OnDrawGizmos() {
		Gizmos.DrawSphere (Camera.main.ScreenToWorldPoint(Input.mousePosition), radius);
	}
}
