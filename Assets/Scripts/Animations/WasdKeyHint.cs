using UnityEngine;
using System.Collections;

public class WasdKeyHint : MonoBehaviour {

	public float animDelay = 3;

	private SpriteRenderer spriteRenderer;

	void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.enabled = false;
	}

	public void StartAnimCountdown() {
		StartCoroutine(StartAnimDelayed());
	}

	void OnTriggerExit(Collider other) {
		if (!other.CompareTag("Player"))
			return;

		GameObject.Destroy(gameObject);
	}

	IEnumerator StartAnimDelayed() {
		yield return new WaitForSeconds(animDelay);
		spriteRenderer.enabled = true;
	}
}
