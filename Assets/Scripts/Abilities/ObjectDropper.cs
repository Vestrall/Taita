using UnityEngine;
using System.Collections;

public class ObjectDropper : Ability {

	private PlayerController playerController;

	protected override void Start() {
		base.Start();
		playerController = GameManager.instance.playerController;
	}

	protected override bool Activate() {
		if (playerController.IsCarryingObject()) {
			playerController.DropCarriedObject();
			return true;
		} else
			return false;
	}
}
