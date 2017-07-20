using UnityEngine;
using System.Collections;

public class IcePatch : MonoBehaviour, Utils.TriggerEventListener {

	public float accelerationPerSecond = 1;
	public float slideStoppingSpeed = 0.5F;

	private PlayerMovement playerMovement;

	void Start() {
		playerMovement = GameManager.instance.playerMovement;
	}

	public void OnTriggerRangeChange(bool inRange, Collider other) {
		if (inRange) {
			OnTriggerEnter(other);
		} else {
			OnTriggerExit(other);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (!other.CompareTag("Player"))
			return;

		playerMovement.StartSliding(accelerationPerSecond, slideStoppingSpeed);
	}

	void OnTriggerExit(Collider other) {
		if (!other.CompareTag("Player"))
			return;

		playerMovement.StopSliding();
	}
}
