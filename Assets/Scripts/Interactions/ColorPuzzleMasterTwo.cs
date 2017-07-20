using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPuzzleMasterTwo : ColorPuzzleMaster {

	public float puzzleDuration;
	public float disabledDuration;
	public AudioClip errorSound;
	public AudioClip restartSound;

	private float puzzleTimeRemaining;
	private float disabledTimeRemaining;
	private AudioSource timerAudioSource;
	private AudioClip clockClip;

	protected override void Start() {
		base.Start();

		if (puzzleDuration <= 0 || disabledDuration <= 0) {
			Debug.Log("Error: color puzzle two requires positive durations");
		}

		timerAudioSource = GetComponent<AudioSource>();
		clockClip = timerAudioSource.clip;
		resetOnFail = true;
	}

	void Update() {
		if (puzzleTimeRemaining > 0) {
			puzzleTimeRemaining -= Time.deltaTime;
			if (puzzleTimeRemaining <= 0) {
				solveInProgress = true;
				timerAudioSource.Stop();
				timerAudioSource.clip = errorSound;
				timerAudioSource.loop = false;
				timerAudioSource.Play();
				disabledTimeRemaining = disabledDuration;
				foreach (ColorPuzzleSlave slave in slaves) {
					slave.SetDisabled(true);
				}
			}
		} else if (disabledTimeRemaining > 0) {
			disabledTimeRemaining -= Time.deltaTime;
			if (disabledTimeRemaining <= 0) {
				timerAudioSource.clip = restartSound;
				timerAudioSource.loop = false;
				timerAudioSource.Play();
				solveInProgress = false;
				foreach (ColorPuzzleSlave slave in slaves) {
					slave.SetDisabled(false);
				}
			}
		}
	}

	protected override IEnumerator ActivateColor() {
		timerAudioSource.Stop();
		puzzleTimeRemaining = 0;

		return base.ActivateColor();
	}

	public override void OnSlaveInteraction() {
		if (puzzleTimeRemaining > 0 || disabledTimeRemaining > 0)
			return;

		puzzleTimeRemaining = puzzleDuration;
		timerAudioSource.clip = clockClip;
		timerAudioSource.loop = true;
		timerAudioSource.Play();
	}
}
