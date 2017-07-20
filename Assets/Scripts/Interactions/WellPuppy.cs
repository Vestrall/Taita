using UnityEngine;
using System.Collections;

public class WellPuppy : Pickupable {

	protected override void Start() {
		base.Start();
	}

	public override bool Activate() {
		if (base.Activate()) {
			UpdateXAnimationVisibility();
			return true;
		}
		return false;
	}

	protected override bool ShouldDisplayXAnimation() {
		return base.ShouldDisplayXAnimation() && !playerController.IsCarryingObject();
	}
}
