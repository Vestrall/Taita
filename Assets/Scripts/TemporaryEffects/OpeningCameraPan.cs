using UnityEngine;
using System.Collections;

public class OpeningCameraPan : MonoBehaviour {

	public Utils.Direction direction;
	public bool pointCalculation;
	public float pointBasePercent;
	public float pointDistanceMax;

	private Camera cam;
	private float finalSize;
	private float targetSize;

	private Transform playerTransform;
	private float maxSizeReached;
	private float minCoord;
	private float triggerSize;

	private Vector3 centerWithoutY;
	private float pointInitialSize;

	void Start() {
		if (pointCalculation && (pointBasePercent < 0 || pointBasePercent > 1)) {
			Debug.Log("Warning: invalid point values (OpeningCameraPan)");
		}

		cam = Camera.main;
		finalSize = cam.fieldOfView;
		playerTransform = GameManager.instance.player.transform;

		if (pointCalculation) {
			pointInitialSize = finalSize * pointBasePercent;
			centerWithoutY = transform.position;
			centerWithoutY.y = 0;
		} else {
			Bounds bounds = GetComponent<BoxCollider>().bounds;
			Vector3 extents = bounds.extents;
			Vector3 center = bounds.center;
			switch (direction) {
			case Utils.Direction.Left:
				minCoord = center.x + extents.x;
				triggerSize = 2 * extents.x;
				break;
			case Utils.Direction.Up:
				minCoord = center.z - extents.z;
				triggerSize = 2 * extents.z;
				break;
			case Utils.Direction.Right:
				minCoord = center.x - extents.x;
				triggerSize = 2 * extents.x;
				break;
			case Utils.Direction.Down:
				minCoord = center.z + extents.z;
				triggerSize = 2 * extents.z;
				break;
			}
		}

		UpdateSizeFromPosition(false);
	}
	
	void LateUpdate() {
		if (finalSize - cam.fieldOfView < 0.05f) {
			cam.fieldOfView = finalSize;
			Destroy(gameObject);
		} else {
			UpdateSizeFromPosition(true);
		}
	}

	private void UpdateSizeFromPosition(bool lerp) {
		float curSize;
		if (pointCalculation) {
			Vector3 playerPosition = playerTransform.position;
			playerPosition.y = 0;
			float curDistance = Vector3.Distance(playerPosition, centerWithoutY);
			curSize = pointInitialSize + ((finalSize - pointInitialSize) * (curDistance / pointDistanceMax));
		} else {
			float playerPosition = direction == Utils.Direction.Left || direction == Utils.Direction.Right
				? playerTransform.position.x : playerTransform.position.z;
			curSize = finalSize * (Mathf.Abs(playerPosition - minCoord) / triggerSize);
		}

		if (curSize >= finalSize)
			targetSize = finalSize;
		else if (curSize > targetSize)
			targetSize = curSize;

		cam.fieldOfView = lerp ? Mathf.Lerp(0, finalSize, targetSize / finalSize) : targetSize;
	}
}
