using UnityEngine;
using System.Collections;

public abstract class Interaction : MonoBehaviour, Utils.TriggerEventListener {

	public SpriteRenderer xAnimationRenderer;
	public AudioClip[] audioClips;

	protected bool inRange;

	private InteractionAbility interactionAbility;
	private bool closest;
	private AudioSource audioSource;

	protected virtual void Awake() {}

	protected virtual void Start() {
		interactionAbility = GameManager.instance.player.GetComponent<InteractionAbility>();
		audioSource = GetComponent<AudioSource>();
		UpdateXAnimationVisibility();
	}

	void OnDestroy() {
		if (interactionAbility != null)
			interactionAbility.RemoveAvailableInteraction(this);
	}

	public abstract bool Activate();

	protected bool interactionEnabled = true;

	protected virtual void OnRangeChange(bool inRange) {
		this.inRange = inRange;
		UpdateXAnimationVisibility();
	}

	/**
	 * Fires when this interaction moves into or out of being the closest available (and in range) interaction to the player
	 */
	public virtual void OnClosestInteractionChange(bool closest) {
		this.closest = closest;
		UpdateXAnimationVisibility();
	}

	public bool IsEnabled() {
		return interactionEnabled;
	}

	public void OnTriggerRangeChange(bool inRange, Collider other) {
		if (inRange) {
			OnTriggerEnter(other);
		} else {
			OnTriggerExit(other);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (!other.CompareTag("Player") || !(other is CharacterController))
			return;

		interactionAbility.AddAvailableInteraction(this);
		OnRangeChange(true);
	}

	void OnTriggerExit(Collider other) {
		if (!other.CompareTag("Player") || !(other is CharacterController))
			return;

		interactionAbility.RemoveAvailableInteraction(this);
		OnRangeChange(false);
	}

	protected void UpdateXAnimationVisibility() {
		if (xAnimationRenderer == null)
			return;
		
		xAnimationRenderer.enabled = ShouldDisplayXAnimation();
	}

	protected virtual bool ShouldDisplayXAnimation() {
		return inRange && interactionEnabled && closest;
	}

	public void PlayAudio() {
		if (audioSource == null || audioClips == null || audioClips.Length <= 0)
			return;

		AudioClip randomClip = audioClips[Random.Range(0, audioClips.Length)];
		audioSource.clip = randomClip;
		audioSource.Play();
	}
}
