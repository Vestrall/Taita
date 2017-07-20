using UnityEngine;
using System.Collections;

public class Item {

	public readonly string name;
	public readonly Sprite icon;

	public Item(string name, Sprite icon) {
		this.name = name;
		this.icon = icon;
	}
}
