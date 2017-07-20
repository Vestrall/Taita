using UnityEngine;
using System.Collections;
using UnityEditor;

public class TagUsage : MonoBehaviour {

	private static string SelectedTag = "ColorBox";

	[MenuItem("Custom/Select By Tag")]
	public static void SelectObjectsWithTag()
	{
		GameObject[] objects = GameObject.FindGameObjectsWithTag(SelectedTag);
		Selection.objects = objects;
	}
}
