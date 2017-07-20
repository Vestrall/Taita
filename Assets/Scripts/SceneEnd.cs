using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneEnd : MonoBehaviour, Utils.TriggerEventListener {

	public Image blackout;
	public float fadeDuration;
	public float transitionDelay;
	public string nextScene;
	public OnEvent onEvent;
	public bool fall;

	private bool transitioning;
	private float fadeTime;

	void Update() {
		if (!transitioning || fadeTime >= fadeDuration)
			return;

		fadeTime += Time.deltaTime;
		Color color = blackout.color;
		color.a = Mathf.Lerp(0, 1, fadeTime / fadeDuration);
		blackout.color = color;
	}

	public void OnTriggerRangeChange(bool inRange, Collider other) {
		if (inRange) {
			OnTriggerEnter(other);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (transitioning || other.isTrigger || !other.CompareTag("Player"))
			return;

		if (fall) {
			GameManager.instance.playerMovement.SetPhysicalCollidersEnabled(false);
		} else {
			GameManager.instance.playerController.SetControllable(false);
		}
		transitioning = true;
		PauseMenu.latestMenu.SetSuspended(true);
		blackout.gameObject.SetActive(true);
		Invoke("NextScene", transitionDelay);
	}

	private void NextScene() {
		if (onEvent != null) {
			onEvent.Fire();
		} else if (nextScene.Length > 0) {
			SceneManager.LoadScene(nextScene);
		} else {
			Debug.Log("Warning: SceneEnd executed with no OnEvent and no scene to transition to");
		}
	}
}
