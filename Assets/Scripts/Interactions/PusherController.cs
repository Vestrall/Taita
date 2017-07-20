using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PusherController : Interaction {

	public Pusher[] pushers;

	public override bool Activate() {
		foreach (Pusher pusher in pushers) {
			pusher.Disable();
		}
		return true;
	}
}
