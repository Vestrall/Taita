using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPuzzleSlave : Interaction {

	private SpriteRenderer spriteRenderer;
	private ColorPuzzleMaster master;
	private Sprite idleSprite;
	private Sprite targetSprite;
	private Sprite disabledSprite;
	private bool colorActive;

	protected override void Awake() {
		base.Awake();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public override bool Activate() {
		if (colorActive)
			return false;

		master.OnSlaveInteraction();
		idleSprite = master.NextSprite(idleSprite);
		spriteRenderer.sprite = idleSprite;
		return true;
	}

	public void SetMaster(ColorPuzzleMaster master) {
		this.master = master;
	}

	public void SetIdleSprite(Sprite idleSprite) {
		this.idleSprite = idleSprite;
		spriteRenderer.sprite = idleSprite;
	}

	public void SetTargetSprite(Sprite targetSprite) {
		this.targetSprite = targetSprite;
	}

	public void SetDisabledSprite(Sprite disabledSprite) {
		this.disabledSprite = disabledSprite;
	}

	public bool ActivateColor() {
		SetColorActive(true);
		Sprite activeSprite = master.GetActiveSprite(idleSprite);
		spriteRenderer.sprite = activeSprite;
		return activeSprite == targetSprite;
	}

	public void OnPuzzleFail(bool reset) {
		SetColorActive(false);
		if (reset) {
			SetIdleSprite(master.GetInitialIdleSprite());
		} else {
			spriteRenderer.sprite = idleSprite;
		}
	}

	private void SetColorActive(bool active) {
		colorActive = active;
		UpdateXAnimationVisibility();
	}

	protected override bool ShouldDisplayXAnimation() {
		return base.ShouldDisplayXAnimation() && !colorActive;
	}

	public void SetDisabled(bool disabled) {
		if (disabled) {
			SetIdleSprite(disabledSprite);
			SetColorActive(true);
		} else {
			SetIdleSprite(master.GetInitialIdleSprite());
			SetColorActive(false);
		}
	}
}
