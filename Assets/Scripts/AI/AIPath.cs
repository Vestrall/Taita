using UnityEngine;
using System.Collections;

[System.Serializable]
public class AIPath {

	public string name;
	public Transform[] navPoints;

	private int currentIndex;

	public Vector3 StartPosition() {
		if (navPoints.Length <= 0)
			return Vector3.zero;

		return navPoints[0].position;
	}

	public Vector3 NextPosition() {
		if (navPoints.Length <= 0 || currentIndex >= navPoints.Length - 1)
			return Vector3.zero;
		
		return navPoints[++currentIndex].position;
	}
}
