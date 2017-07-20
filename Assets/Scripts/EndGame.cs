using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGame : OnEvent {

	public Text thanks;
	public float initialDelay;
	public float fadeDuration;
	public float fullOpacityDuration;
	public Text[] credits;

	private int fading;
	private float fadeTime;
	private bool showingCredits;

	void Update() {
		if (showingCredits) {
			if (fading > 0) {
				fadeTime += Time.deltaTime;
				float alpha = Mathf.Lerp(0, 1, fadeTime / fadeDuration);
				foreach (Text text in credits) {
					SetTextAlpha(text, alpha);
				}
			}
			if (Input.GetButtonUp("Jump")) {
				SceneManager.LoadScene("Cliffs");
			}
		} else if (fading > 0) {
			fadeTime += Time.deltaTime;
			SetTextAlpha(thanks, Mathf.Lerp(0, 1, fadeTime / fadeDuration));
		} else if (fading < 0) {
			fadeTime += Time.deltaTime;
			SetTextAlpha(thanks, Mathf.Lerp(1, 0, fadeTime / fadeDuration));
		}
	}

	private void SetTextAlpha(Text text, float alpha) {
		Color color = text.color;
		color.a = alpha;
		text.color = color;
	}

	public override void Fire() {
		StartCoroutine(ThanksForPlaying());
	}

	IEnumerator ThanksForPlaying() {
		yield return new WaitForSeconds(initialDelay);
		fading = 1;
		thanks.gameObject.SetActive(true);
		yield return new WaitForSeconds(fadeDuration);
		SetTextAlpha(thanks, 1);
		yield return new WaitForSeconds(fullOpacityDuration);
		fadeTime = 0;
		fading = -1;
		yield return new WaitForSeconds(fadeDuration);
		thanks.gameObject.SetActive(false);
		foreach (Text text in credits) {
			text.gameObject.SetActive(true);
		}
		showingCredits = true;
		fadeTime = 0;
		fading = 1;
	}
}
