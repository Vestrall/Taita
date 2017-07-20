using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {

	private List<Item> items;
	private List<Image> images;
	private Image background;
	private bool visible;

	void Awake() {
		items = new List<Item>();
		images = new List<Image>();
		Image[] allImages = GetComponentsInChildren<Image>();
		foreach (Image image in allImages) {
			if (image.gameObject.GetInstanceID() == gameObject.GetInstanceID()) {
				background = image;
				background.enabled = false;
			} else {
				image.enabled = false;
				images.Add(image);
			}
		}
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Tab)) {
			SetVisible(!visible);
		}
	}

	private void SetVisible(bool visible) {
		this.visible = visible;
		background.enabled = visible;
		for (int i = 0; i < images.Count; i++) {
			images[i].enabled = visible && i < items.Count && items[i] != null && items[i].icon != null;
		}
	}

	public void AddItem(Item item) {
		if (items.Contains(item))
			return;

		items.Add(item);
		UpdateImage(items.Count - 1, item.icon);
		SetVisible(true);
	}

	public void RemoveItem(int index) {
		if (index < 0 || index >= items.Count)
			return;

		items.RemoveAt(index);
		for (int i = index; i < items.Count; i++)
			UpdateImage(i, items[i].icon);
		UpdateImage(items.Count, null);
	}

	private void UpdateImage(int index, Sprite sprite) {
		if (sprite == null) {
			images[index].sprite = null;
			images[index].enabled = false;
		} else {
			images[index].sprite = sprite;
			images[index].enabled = visible;
		}
	}

	public bool ConsumeItem(string itemName) {
		if (Utils.IsEmpty(itemName))
			return false;

		for (int i = 0; i < items.Count; i++) {
			if (items[i].name == itemName) {
				RemoveItem(i);
				return true;
			}
		}
		return false;
	}
}
