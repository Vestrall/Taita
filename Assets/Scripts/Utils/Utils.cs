using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utils {

	public const float EPSILON = 0.0001f;
	public const float BIG_DISTANCE = 100;

	public enum Direction {
		Left, Up, Right, Down
	}

	public interface TriggerEventListener {
		void OnTriggerRangeChange(bool inRange, Collider other);
	}

	public static Vector3 GetMousePosition() {
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit floorHit;
		if (Physics.Raycast(camRay, out floorHit, BIG_DISTANCE, LayerMask.GetMask("Ground")))
			return floorHit.point;
		return Vector3.zero;
	}

	public static bool IsEmpty(string str) {
		return str == null || str.Length <= 0;
	}

	public static Transform GetRoot(Transform transform) {
		while (transform.parent != null)
			transform = transform.parent;
		return transform;
	}

	public static float GetAngle(Vector3 from, Vector3 to) {
		float dAngle = Vector3.Angle(from, to);
		Vector3 cross = Vector3.Cross(from, to);
		return cross.y < 0 ? -dAngle : dAngle;
	}

	public static bool IsEmpty<T>(List<T> list) {
		return list == null || list.Count <= 0;
	}

	public static IEnumerator FadeOutSound(AudioSource source, float duration) {
		float timeRemaining = duration;
		float initialVolume = source.volume;
		float volumeChangePerSecond = initialVolume / duration;
		while (timeRemaining > 0) {
			float deltaTime = Time.deltaTime;
			timeRemaining -= deltaTime;
			source.volume = Mathf.Max(0, source.volume - (volumeChangePerSecond * deltaTime));
			yield return new WaitForEndOfFrame();
		}
		source.Stop();
		source.volume = initialVolume;	// Reset volume for next play
	}
}
