using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class VRC_Destination : MonoBehaviour {
	private float magnitude = 2f;
	
    void Update () {
		Debug.DrawLine(transform.position, transform.position + (transform.forward * magnitude), Color.red);
	}
}
