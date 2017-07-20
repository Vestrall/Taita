using UnityEngine;
using System.Collections;

public class BillboardSprite : MonoBehaviour {

	void LateUpdate() {
		transform.forward = Camera.main.transform.forward;
	}
}
