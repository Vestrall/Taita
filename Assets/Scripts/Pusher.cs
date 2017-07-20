using UnityEngine;
using System.Collections;

public class Pusher : MonoBehaviour {

	public Vector3 pushVector;
	public float pushDuration = 1;
	public float pushRatioFinal = 1;
	public int[] pushIntervals;

	private PlayerMovement playerMovement;
	private Collider pushTrigger;
	private ParticleSystem particles;

	private bool disabled;
	private bool pushing;
	private float intervalDurationRemaining;
	private int intervalIndex;

	void Start() {
		playerMovement = GameManager.instance.playerMovement;
		foreach (BoxCollider box in GetComponents<BoxCollider>()) {
			if (box.isTrigger) {
				pushTrigger = box;
				break;
			}
		}
		particles = GetComponentInChildren<ParticleSystem>();
		pushing = true;
		intervalDurationRemaining = pushIntervals[0];
	}

	void Update() {
		if (disabled)
			return;

		intervalDurationRemaining -= Time.deltaTime;
		if (intervalDurationRemaining <= 0)
			ToggleState();
	}

	void OnTriggerEnter(Collider other) {
		if (!other.CompareTag("Player"))
			return;

		playerMovement.Push(pushVector, pushDuration, pushRatioFinal);
	}

	private void ToggleState() {
		pushing = !pushing;
		intervalIndex++;
		if (intervalIndex >= pushIntervals.Length)
			intervalIndex = 0;
		intervalDurationRemaining += pushIntervals[intervalIndex];
		pushTrigger.enabled = pushing;
		if (pushing) {
			particles.Play();
		} else {
			particles.Stop();
		}
	}

	public void Disable() {
		disabled = true;
		pushing = false;
		pushTrigger.enabled = false;
		particles.Stop();
	}
}
