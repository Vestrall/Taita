using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPuzzleHint : MonoBehaviour {

	public enum MovementAnimation {
		AnimOne, AnimTwo
	}
	private const string ANIM_ONE = "Movement 1";
	private const string ANIM_TWO = "Movement 2";

	public MovementAnimation movementAnimation;
	public float flapSpeedErrorMultiplier;
	public float flyAwaySpeedY;
	public float flyAwaySpeedXZMax;

	private Animator movementAnimator;
	private Animator errorColorAnimator;
	private Vector3 flyAwayVector = Vector3.zero;

	void Awake() {
		movementAnimator = GetComponent<Animator>();
		Animator[] animators = GetComponentsInChildren<Animator>();
		foreach (Animator animator in animators) {
			if (animator != movementAnimator) {
				errorColorAnimator = animator;
				break;
			}
		}

		string movementTrigger;
		switch (movementAnimation) {
		case MovementAnimation.AnimOne:
			movementTrigger = ANIM_ONE;
			break;
		case MovementAnimation.AnimTwo:
			movementTrigger = ANIM_TWO;
			break;
		default:
			movementTrigger = null;
			Debug.Log("Color puzzle hint movement animation not set");
			break;
		}
		movementAnimator.SetTrigger(movementTrigger);
	}

	void Update() {
		if (flyAwayVector == Vector3.zero)
			return;

		transform.position += flyAwayVector * Time.deltaTime;
	}

	public void ColorTrigger(string trigger) {
		errorColorAnimator.SetTrigger(trigger);
	}

	public void OnPuzzleFail() {
		errorColorAnimator.SetFloat("Flap Speed", flapSpeedErrorMultiplier);
		errorColorAnimator.SetTrigger("Error");
		StartCoroutine(OnPuzzleFailFinish());
	}

	IEnumerator OnPuzzleFailFinish() {
		yield return new WaitForEndOfFrame();	// Animation duration isn't available until the next frame
		yield return new WaitForSeconds(errorColorAnimator.GetCurrentAnimatorStateInfo(1).length);
		errorColorAnimator.SetFloat("Flap Speed", 1);
	}

	public void OnPuzzleSuccess() {
		movementAnimator.enabled = false;
		float dx = Random.Range(-flyAwaySpeedXZMax, flyAwaySpeedXZMax);
		float dz = Random.Range(-flyAwaySpeedXZMax, flyAwaySpeedXZMax);
		flyAwayVector = new Vector3(dx, flyAwaySpeedY, dz);
		Invoke("Die", 20);	// Arbitrary long delay
	}

	private void Die() {
		gameObject.SetActive(false);
	}
}
