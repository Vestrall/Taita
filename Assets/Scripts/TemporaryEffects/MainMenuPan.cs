using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPan : MonoBehaviour {

	public float panDuration;

	private Vector3 startPos;
	private Vector3 destinationPos;
	private float timeElapsed;
	private bool panStarted;

	private WasdKeyHint wasdKeyHint;

	void Start() {
		startPos = transform.position;
		destinationPos = startPos;
		destinationPos.x = GameManager.instance.player.transform.position.x;

//		wasdKeyHint = GameObject.Find("WASD Key Hint").GetComponent<WasdKeyHint>();
	}
	
	void LateUpdate() {
		if (!panStarted)
			return;

		timeElapsed += Time.deltaTime;
		if (timeElapsed >= panDuration) {
			// End pan
			transform.position = destinationPos;
			InitializeGame();
			Destroy(this);
		} else {
			transform.position = Vector3.Lerp(startPos, destinationPos, timeElapsed / panDuration);
		}
	}

	public void StartPan() {
		panStarted = true;
	}

	// Initialize features that must wait for the game to start
	private void InitializeGame() {
		GameManager.instance.playerController.SetControllable(true);
		GetComponent<CameraController>().enabled = true;
//		wasdKeyHint.StartAnimCountdown();
	}
}
