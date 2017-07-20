using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractionAbility : Ability {

	private PlayerController playerController;
	private List<Interaction> availableInteractions;
	private Interaction closestInteraction;

	protected override void Awake() {
		base.Awake();
		playerController = GetComponent<PlayerController>();
		availableInteractions = new List<Interaction>();
	}

	protected override bool Activate() {
		UpdateClosestInteraction();
		if (closestInteraction != null) {
			if (playerController.IsCarryingObject()) {
				playerController.UseCarriedObject(closestInteraction);
			} else if (closestInteraction.Activate()) {
				closestInteraction.PlayAudio();
			}
			return true;
		}

		return false;
	}

	public override void TimeStep(float deltaTime) {
		base.TimeStep(deltaTime);
		UpdateClosestInteraction();
	}

	private void UpdateClosestInteraction() {
		if (Utils.IsEmpty(availableInteractions)) {
			if (closestInteraction != null) {
				closestInteraction.OnClosestInteractionChange(false);
				closestInteraction = null;
			}
			return;
		}

		Interaction interaction = null;
		float dSquared = int.MaxValue;
		foreach (Interaction availableInteraction in availableInteractions) {
			if (!availableInteraction.IsEnabled())
				continue;

			Vector3 d = availableInteraction.gameObject.transform.position - transform.position;
			float testDSquared = d.sqrMagnitude;
			if (testDSquared < dSquared) {
				dSquared = testDSquared;
				interaction = availableInteraction;
			}
		}
		if (closestInteraction != interaction) {
			if (closestInteraction != null)
				closestInteraction.OnClosestInteractionChange(false);
			closestInteraction = interaction;
			if (closestInteraction != null)
				closestInteraction.OnClosestInteractionChange(true);
		}
	}

	public void OnCarriedObjectDropped() {
		// Re-trigger OnClosestInteractionChange(true) (but not OnClosestInteractionChange(false)) if any interactions in range
		closestInteraction = null;
		UpdateClosestInteraction();
	}

	public void AddAvailableInteraction(Interaction interaction) {
		if (availableInteractions.Contains(interaction))
			return;
		
		availableInteractions.Add(interaction);
	}

	public void RemoveAvailableInteraction(Interaction interaction) {
		availableInteractions.Remove(interaction);
	}
}
