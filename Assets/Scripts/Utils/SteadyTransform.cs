using UnityEngine;
using System.Collections;

public class SteadyTransform : MonoBehaviour {

	private Vector3 offset;

	void Awake() {
		offset = transform.localPosition;
	}

	void Update() {
		transform.position = transform.parent.position + offset;
		transform.rotation = Quaternion.Euler(Vector3.zero);
	}
}
