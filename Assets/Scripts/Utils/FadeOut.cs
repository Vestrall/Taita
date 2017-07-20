using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour {

	public float duration;

	private Image fadeImage; 
	private float fadeTime;

	void Start() {
		fadeImage = GetComponent<Image>();
	}
	
	void Update() {
		fadeTime = Mathf.Min(duration, fadeTime + Time.deltaTime);
		Color color = fadeImage.color;
		color.a = Mathf.Lerp(1, 0, fadeTime / duration);
		fadeImage.color = color;
		if (fadeTime >= duration) {
			gameObject.SetActive(false);
			Destroy(this);
		}
	}
}
