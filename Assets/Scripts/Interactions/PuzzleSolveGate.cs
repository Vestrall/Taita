using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class PuzzleSolveGate : OnEvent {

	public float duration;
	[SpineAnimation] public string rockIdleAnim;
	[SpineAnimation] public string rockDescendAnim;
	public float rockDescendSpeedMultiplier = 1;
	[SpineAnimation] public string rockFinishedAnim;
	public Transform gateTransform;
	public float audioFadeOutDuration = 0.5f;

	private bool animInProgress;
	private float deltaYPerSecond;
	private SkeletonAnimation rockAnim;
	private AudioSource audioSource;

	void Start() {
		rockAnim = GetComponent<SkeletonAnimation>();
		rockAnim.state.SetAnimation(0, rockIdleAnim, true);
		audioSource = GetComponent<AudioSource>();
	}
	
	void FixedUpdate() {
		if (!animInProgress)
			return;

		float deltaTime = Time.fixedDeltaTime;
		Vector3 position = gateTransform.position;
		position.y -= deltaYPerSecond * deltaTime;
		gateTransform.position = position;
	}

	public override void Fire() {
		animInProgress = true;
		AnimationStateData stateData = new AnimationStateData(rockAnim.skeletonDataAsset.GetSkeletonData(false));
		Spine.AnimationState newState = new Spine.AnimationState(stateData);
		newState.SetAnimation(0, rockDescendAnim, false);
		newState.AddAnimation(0, rockFinishedAnim, true, 0);
		rockAnim.state = newState;

		rockAnim.timeScale = rockDescendSpeedMultiplier;
		rockAnim.state.Complete += OnAnimEnd;
		deltaYPerSecond = gateTransform.lossyScale.y / duration;

		audioSource.Play();
		Invoke("AudioEnd", duration - 1);
	}

	private void OnAnimEnd(Spine.TrackEntry trackEntry) {
		rockAnim.state.Complete -= OnAnimEnd;
		rockAnim.timeScale = 1;
		animInProgress = false;
	}

	private void AudioEnd() {
		StartCoroutine(Utils.FadeOutSound(audioSource, audioFadeOutDuration));
	}
}
