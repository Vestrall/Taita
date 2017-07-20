using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenWaterDrops : MonoBehaviour {

	public AudioSource audioSource;
	public AudioClip[] audioClips;

	private int count;
	private int lastIndex = -1;

	void Awake() {
		count = audioClips.Length;
	}

	void OnParticleCollision(GameObject other) {
		if (!audioSource.isActiveAndEnabled)
			return;

		int randomIndex = Random.Range(0, count - 1);
		if (randomIndex >= lastIndex)
			randomIndex++;
		audioSource.clip = audioClips[randomIndex];
		audioSource.Play();
		lastIndex = randomIndex;
	}
}
