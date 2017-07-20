using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Spine.Unity;
using UnityEngine.UI;

public class DogGod : MonoBehaviour, Utils.TriggerEventListener {

	[SpineAnimation] public string firstEye = "Top Eye";
	[SpineAnimation] public string secondEye = "Middle_ Eye";
	[SpineAnimation] public string thirdEye = "Bottom Eye";
	[SpineAnimation] public string fourthEye = "Big Eye";
	[SpineAnimation] public string sceneEnd = "Endscene";
	public string nextSceneName;
	public Image whiteout;
	public float fadeDuration;

	private const int SOLVE_COUNT_MAX = 4;
	private SkeletonAnimation skeletonAnimation;
	private int solveCount;
	private float fadeTime;
	private bool fading;
	private AudioSource audioSource;

	void Start() {
		skeletonAnimation = GetComponent<SkeletonAnimation>();
		audioSource = GetComponent<AudioSource>();
	}

	void Update() {
		if (!fading)
			return;

		fadeTime += Time.deltaTime;
		Color color = whiteout.color;
		color.a = Mathf.Lerp(0, 1, fadeTime / fadeDuration);
		whiteout.color = color;

		if (fadeTime >= fadeDuration) {
			SceneManager.LoadScene(nextSceneName);
		}
	}

	public void IncrementSolveCount() {
		solveCount++;
		switch (solveCount) {
		case 1:
			skeletonAnimation.state.SetAnimation(0, firstEye, true);
			break;
		case 2:
			skeletonAnimation.state.SetAnimation(0, secondEye, true);
			break;
		case 3:
			skeletonAnimation.state.SetAnimation(0, thirdEye, true);
			break;
		case SOLVE_COUNT_MAX:
			skeletonAnimation.state.SetAnimation(0, fourthEye, true);
			break;
		default:
			Debug.Log("Warning: Dog god animation received invalid solve count");
			break;
		}
	}

	public void OnTriggerRangeChange(bool inRange, Collider other) {
		if (solveCount < SOLVE_COUNT_MAX || !other.CompareTag("Player") || !inRange)
			return;

		GameManager.instance.playerController.controllable = false;
		GameManager.instance.playerAnimator.Idle();
		audioSource.Play();
		skeletonAnimation.state.SetAnimation(0, sceneEnd, false);
		skeletonAnimation.state.Complete += OnAnimEnd;
	}

	private void OnAnimEnd(Spine.TrackEntry trackEntry) {
		skeletonAnimation.state.Complete -= OnAnimEnd;
		fading = true;
		PauseMenu.latestMenu.SetSuspended(true);
		whiteout.gameObject.SetActive(true);
	}
}
