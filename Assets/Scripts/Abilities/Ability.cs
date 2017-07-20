using UnityEngine;
using System.Collections;

public abstract class Ability : MonoBehaviour {

	public string key;
	public string altKey;
	public float maxCooldown;

//	private GameObject owner;
	private float cooldown;
	private bool cooldownReady;
	private bool usesPressed;
	private bool activationRegistered;	// True from when Activate() is called until key/altKey is released (prevents multiple activations)

	public Ability() {
		cooldownReady = true;
//		owner = gameObject;
	}

	protected virtual void Awake() {}

	protected virtual void Start() {}

//	public virtual void SetOwner(GameObject owner) {
//		this.owner = owner;
//	}

	/**
	 * returns true if ability was activated
	 */
	protected abstract bool Activate();

	protected void SetActiveOnPress() {
		usesPressed = true;
	}

	public virtual void TimeStep(float deltaTime) {
		// Update cooldown
		if (cooldown > 0) {
			cooldown -= deltaTime;
			if (cooldown <= 0) {
				cooldownReady = true;
			}
		}

		// Ability activation
		if (KeyActive()) {
			if (!activationRegistered && cooldownReady && Activate()) {
				activationRegistered = true;
				TriggerCooldown();
			}
		} else if (activationRegistered)
			activationRegistered = false;
	}

	private bool KeyActive() {
		return usesPressed ? Input.GetButton(key) || Input.GetButton(altKey) : Input.GetButtonDown(key) || Input.GetButtonDown(altKey);
	}

	public void TriggerCooldown() {
		if (maxCooldown > 0) {
			cooldown = maxCooldown;
			cooldownReady = false;
		} else {
			cooldownReady = true;
		}
	}
}
