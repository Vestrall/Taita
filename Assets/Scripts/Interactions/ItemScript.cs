using UnityEngine;
using System.Collections;

public class ItemScript : Interaction {

	public string inventoryName;
	public Sprite inventoryIcon;

	public override bool Activate() {
		if (!IsAvailable())
			return false;

//		GameManager.instance.inventory.AddItem(new Item(inventoryName, inventoryIcon));
		Destroy(gameObject);
		return true;
	}

	protected virtual bool IsAvailable() {
		return true;
	}
}
