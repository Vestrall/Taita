using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : Menu {

	[HideInInspector] public static PauseMenu latestMenu;

	public static bool paused;

	private bool suspended;
	private AboutMenu aboutMenu;

	protected override void Awake() {
		base.Awake();
		blurBackground = true;
		aboutMenu = GetComponent<AboutMenu>();
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		foreach (Button button in buttons) {
			if (button.name == "Scene Change 01") {
				button.GetComponentInChildren<Text>().text = currentSceneIndex == 0 ? "Load Cave Scene" : "Load Cliffs Scene";
			} else if (button.name == "Scene Change 02") {
				button.GetComponentInChildren<Text>().text = currentSceneIndex == 2 ? "Load Cave Scene" : "Load Snow Scene";
			}
		}
		Paused = false;
	}

	protected override void Start() {
		latestMenu = this;
	}

	protected override void LateUpdate() {
		if (suspended || subMenuVisible)
			return;

		base.LateUpdate();

		if (!clickHandled && !subMenuVisible) {
			if (Input.GetKeyUp(KeyCode.Escape) || Input.GetButtonUp("ControllerStart"))
				Paused = !Paused;
		}
	}

	protected override bool OnCancel() {
		Resume();
		return true;
	}

	private bool Paused {
		get {
			return paused;
		}
		set {
			paused = value;
			Time.timeScale = value ? 0 : 1;
			SetMenuVisible(value);
		}
	}
	
	public void RestartScene() {
		if (subMenuVisible)
			return;
		
		Paused = false;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
	}

	public void ChangeScene1() {
		if (subMenuVisible)
			return;

		Paused = false;
		int sceneIndex = SceneManager.GetActiveScene().buildIndex == 0 ? 1 : 0;
		SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
	}

	public void ChangeScene2() {
		if (subMenuVisible)
			return;

		Paused = false;
		int sceneIndex = SceneManager.GetActiveScene().buildIndex == 2 ? 1 : 2;
		SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
	}

	public void About() {
		if (subMenuVisible)
			return;

		OpenSubMenu(aboutMenu);
	}

	public void Resume() {
		if (subMenuVisible)
			return;

		Paused = false;
	}

	void OnApplicationFocus(bool hasFocus) {
//		if (!suspended && !hasFocus)
//			Paused = true;
	}

	void OnApplicationPause(bool paused) {
//		if (!suspended && paused)
//			Paused = true;
	}

	public void SetSuspended(bool suspended) {
		this.suspended = suspended;
		if (suspended)
			SetMenuVisible(false);
		else if (paused)
			SetMenuVisible(true);
	}
}
