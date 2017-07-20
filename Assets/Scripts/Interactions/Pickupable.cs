using UnityEngine;
using System.Collections;

public class Pickupable : Interaction {

	protected PlayerController playerController;

	private Rigidbody body;

	protected override void Start() {
		base.Start();
		body = GetComponent<Rigidbody>();
		playerController = GameManager.instance.playerController;
	}

	public override bool Activate() {
		if (playerController.CarryObject(this)) {
			body.useGravity = false;
			body.freezeRotation = true;
			interactionEnabled = false;
			return true;
		} else {
			return false;
		}
	}

	public void MoveTo(Vector3 position) {
		transform.position = position;
	}

	public virtual void Use(Interaction interaction) {
		// Override to implement
	}

	public virtual void Drop() {
		body.useGravity = true;
		body.freezeRotation = false;
		interactionEnabled = true;
	}

	public void SetInteractionEnabled(bool enabled) {
		interactionEnabled = enabled;
	}
}
