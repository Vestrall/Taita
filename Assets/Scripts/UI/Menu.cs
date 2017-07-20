using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets.ImageEffects;

public abstract class Menu : MonoBehaviour {

	private static Sprite subMenuSprite;

	private void initSubMenuSprite() {
		if (subMenuSprite != null)
			return;

		subMenuSprite = Resources.Load<Sprite>("Sub Menu Selected") as Sprite;
		if (subMenuSprite == null) {
			Debug.Log("Unable to find \"Sub Menu Selected\"");
		}
	}

	public Button[] buttons;
	public Button defaultButton;
	public GameObject[] otherUiElements;

	protected bool blurBackground;
	protected bool clickHandled;

	private bool visible;
	private int defaultIndex = -1;
	private int currentIndex = -1;
	private BlurOptimized blurScript;

	protected bool subMenuVisible;
	private Button selectedSubMenuButton;
	private Sprite selectedSubMenuButtonOriginalSprite;
	private Menu parentMenu;

	protected virtual void Awake() {
		initSubMenuSprite();

		if (defaultButton != null) {
			for (int i = 0; i < buttons.Length; i++) {
				if (buttons[i] == defaultButton) {
					defaultIndex = i;
					break;
				}
			}
		}
		visible = false;
		blurScript = Camera.main.GetComponent<BlurOptimized>();
	}

	protected virtual void Start() {}

	protected virtual void Update() {
		clickHandled = false;
		subMenuVisible = selectedSubMenuButton != null;
	}

	protected virtual void LateUpdate() {
		if (!visible || subMenuVisible)
			return;

		// Manually handle button clicks to avoid character also receiving the input
		if (Input.GetButtonUp("Submit")) {
			Button button = GetSelectedButton();
			if (button != null) {
				clickHandled = true;
				button.onClick.Invoke();
			}
		} else if ((parentMenu != null && Input.GetAxisRaw("DPadX") < 0)
				|| Input.GetButtonUp("Cancel") || Input.GetButtonUp("ControllerB")) {
			if (parentMenu != null) {
				// This menu is open as a sub-menu
				clickHandled = true;
				SetMenuVisible(false);
				parentMenu.OnSubMenuClosed();
				parentMenu = null;
			} else {
				clickHandled = OnCancel();
			}
		} else if (Input.GetAxisRaw("DPadX") < 0 && parentMenu != null) {
			// TODO: Close sub-menu?
		}
	}

	private Button GetSelectedButton() {
		GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
		if (selectedButton != null) {
			foreach (Button button in buttons) {
				if (button.name.Equals(selectedButton.name)) {
					return button;
				}
			}
		}
		return null;
	}

	protected void SetMenuVisible(bool visible) {
		if (this.visible == visible)
			return;

		this.visible = visible;
		if (blurBackground && blurScript != null) {
			blurScript.enabled = visible;
		}
		foreach (Button button in buttons)
			button.gameObject.SetActive(visible);
		foreach (GameObject gameo in otherUiElements)
			gameo.SetActive(visible);
		SetSelected(visible ? defaultIndex : -1);
	}

	private void SetSelected(Button button) {
		for (int i=0; i<buttons.Length; i++) {
			if (buttons[i] == button) {
				SetSelected(i);
				break;
			}
		}
	}

	private void SetSelected(int index) {
		if (currentIndex == index)
			return;

		currentIndex = index;
		EventSystem.current.SetSelectedGameObject(index < 0 ? null : buttons[index].gameObject);
	}

	protected virtual bool OnCancel() {
		return false;
	}

	protected virtual void OpenSubMenu(Menu subMenu) {
		subMenuVisible = true;
		if (selectedSubMenuButton != null) {
			Debug.Log("selectedSubMenuButton replaced (previous reference lost)");
		}
		Button subMenuButton = GetSelectedButton();
		if (subMenuButton == null) {
			Debug.Log("Unable to locate selectedSubMenuButton");
		}
		selectedSubMenuButton = subMenuButton;
		Image selectedSubMenuButtonImage = selectedSubMenuButton.GetComponent<Image>();
		selectedSubMenuButtonOriginalSprite = selectedSubMenuButtonImage.sprite;
		selectedSubMenuButtonImage.sprite = subMenuSprite;
		SetSelected(-1);
		subMenu.OpenAsSubMenu(this);
	}

	private void OpenAsSubMenu(Menu parentMenu) {
		this.parentMenu = parentMenu;
		SetMenuVisible(true);
	}

	private void OnSubMenuClosed() {
		selectedSubMenuButton.GetComponent<Image>().sprite = selectedSubMenuButtonOriginalSprite;
		SetSelected(selectedSubMenuButton);
		selectedSubMenuButton = null;
	}
}
