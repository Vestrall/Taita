using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroMenu : Menu {

	protected override void Start() {
		base.Start();
		GameManager.instance.playerController.SetControllable(false);
		GetComponent<PauseMenu>().SetSuspended(true);
		SetMenuVisible(true);
	}

	public void Play() {
		// Hide menu buttons
		foreach (Button button in buttons) {
			GameObject.Destroy(button.gameObject);
		}
		foreach (GameObject gameo in otherUiElements) {
			GameObject.Destroy(gameo);
		}

		// Start camera pan
		Camera.main.GetComponent<MainMenuPan>().StartPan();

		GetComponent<PauseMenu>().SetSuspended(false);

		GameObject.Destroy(this);
	}

	public void About() {
		Debug.Log("TODO: About from Intro Menu");
	}

	public void Quit() {
		Application.Quit();
	}
}
