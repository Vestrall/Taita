using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPuzzleMaster : Interaction {

	public ColorPuzzleSlave[] slaves;
	public Sprite[] idleSprites;
	public Sprite[] activeSprites;
	public Sprite disabledSprite;
	public ColorPuzzleHint[] hints;
	public string[] solutionStateTriggers;
	public OnEvent onSolveEvent;

	protected bool resetOnFail;
	protected bool solveInProgress;
	private int slaveCount;

	protected override void Start() {
		slaveCount = slaves.Length;
		if (slaveCount <= 1 || slaveCount != idleSprites.Length || slaveCount != activeSprites.Length
				|| slaveCount != hints.Length || slaveCount != solutionStateTriggers.Length) {
			Debug.Log("Error initializing color puzzle");
			return;
		}

		base.Start();

		// Init slaves
		foreach (ColorPuzzleSlave slave in slaves) {
			slave.SetMaster(this);
			slave.SetIdleSprite(idleSprites[0]);
			slave.SetDisabledSprite(disabledSprite);
		}

		// Randomly assign target active sprites
		int[] indices = new int[slaveCount];
		bool invalidPuzzle = true;
		while (invalidPuzzle) {
			for (int i=0; i<slaveCount; i++) {
				int randomIndex = Random.Range(0, slaveCount);
				indices[i] = randomIndex;
				if (randomIndex != 0)
					invalidPuzzle = false;
			}
		}
		for (int i=0; i<slaveCount; i++) {
			int solutionIndex = indices[i];
			slaves[i].SetTargetSprite(activeSprites[solutionIndex]);
			hints[i].ColorTrigger(solutionStateTriggers[solutionIndex]);
		}
	}

	public override bool Activate() {
		if (solveInProgress)
			return false;
		
		StartCoroutine(ActivateColor());
		return true;
	}

	protected virtual IEnumerator ActivateColor() {
		solveInProgress = true;
		UpdateXAnimationVisibility();
		bool success = true;
		for (int i=0; i<slaveCount; i++) {
			if (!slaves[i].ActivateColor()) {
				success = false;
				hints[i].OnPuzzleFail();
			}
		}

		yield return new WaitForSeconds(2);

		if (success) {
			foreach (ColorPuzzleHint hint in hints) {
				hint.OnPuzzleSuccess();
			}
			foreach (ColorPuzzleSlave slave in slaves) {
				slave.SetDisabled(true);
			}
			onSolveEvent.Fire();
			interactionEnabled = false;
		} else {
			foreach (ColorPuzzleSlave slave in slaves) {
				slave.OnPuzzleFail(resetOnFail);
			}
		}
		solveInProgress = false;
		UpdateXAnimationVisibility();
	}

	public Sprite NextSprite(Sprite idleSprite) {
		for (int i=0; i<slaveCount; i++) {
			if (idleSprites[i] == idleSprite) {
				return idleSprites[(i + 1) % slaveCount];
			}
		}

		Debug.Log("Unable to determine next color puzzle sprite after: " + idleSprite);
		return null;
	}

	public Sprite GetActiveSprite(Sprite idleSprite) {
		for (int i=0; i<slaveCount; i++) {
			if (idleSprites[i] == idleSprite) {
				return activeSprites[i];
			}
		}

		Debug.Log("Unable to determine active sprite that corresponds to: " + idleSprite.name);
		return null;
	}

	protected override bool ShouldDisplayXAnimation() {
		return base.ShouldDisplayXAnimation() && !solveInProgress;
	}

	public virtual void OnSlaveInteraction() {}

	public Sprite GetInitialIdleSprite() {
		return idleSprites[0];
	}
}
