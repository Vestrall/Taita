using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellPuppyCatcher : MonoBehaviour, Utils.TriggerEventListener {

	public Transform location;

	private PlayerController playerController;
	private WellPuppy carriedPuppyInNet;

	void Start() {
		playerController = GameManager.instance.playerController;
	}

	void Update() {
		if (carriedPuppyInNet != null && !playerController.IsCarryingObject(carriedPuppyInNet)) {
			teleportPuppy(carriedPuppyInNet);
			carriedPuppyInNet = null;
		}
	}

	public void OnTriggerRangeChange(bool inRange, Collider other) {
		if (inRange)
			OnTriggerEnter(other);
		else
			OnTriggerExit(other);
	}

	private void teleportPuppy(WellPuppy puppy) {
		Rigidbody puppyBody = puppy.GetComponent<Rigidbody>();
		puppyBody.MovePosition(location.position);
		puppyBody.velocity = Vector3.zero;
	}

	void OnTriggerEnter(Collider other) {
		if (!other.CompareTag("WellPuppy"))
			return;
		WellPuppy puppy = other.GetComponentInParent<WellPuppy>();
		if (puppy != null) {
			if (playerController.IsCarryingObject(puppy)) {
				carriedPuppyInNet = puppy;
			} else {
				teleportPuppy(puppy);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (!other.CompareTag("WellPuppy"))
			return;
		
		WellPuppy puppy = other.GetComponentInParent<WellPuppy>();
		if (puppy == carriedPuppyInNet)
			carriedPuppyInNet = null;
	}
}
