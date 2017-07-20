using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorOnly : MonoBehaviour {

	// Use this for initialization
	void Awake() {
		Destroy(gameObject);
	}
}
