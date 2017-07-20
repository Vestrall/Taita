using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayCameraZoom : MonoBehaviour {

	public Utils.Direction direction;
	public float finalSizePercent;

	private float initialSize;
	private float finalSize;
	private Camera cam;
	private Transform playerTransform;

	private bool inTrigger;
	private float triggerSize;
	private float minCoord;
	private float centerCoord;

	void Start() {
		cam = Camera.main;
		playerTransform = GameManager.instance.player.transform;

		Bounds bounds = GetComponent<BoxCollider>().bounds;
		Vector3 extents = bounds.extents;
		Vector3 center = bounds.center;
		switch (direction) {
		case Utils.Direction.Left:
			centerCoord = center.x;
			minCoord = centerCoord + extents.x;
			triggerSize = 2 * extents.x;
			break;
		case Utils.Direction.Up:
			centerCoord = center.z;
			minCoord = centerCoord - extents.z;
			triggerSize = 2 * extents.z;
			break;
		case Utils.Direction.Right:
			centerCoord = center.x;
			minCoord = centerCoord - extents.x;
			triggerSize = 2 * extents.x;
			break;
		case Utils.Direction.Down:
			centerCoord = center.z;
			minCoord = centerCoord + extents.z;
			triggerSize = 2 * extents.z;
			break;
		}
	}
	
	void LateUpdate() {
		if (!inTrigger)
			return;

		UpdateSizeFromPosition(true);
	}

	void OnTriggerEnter(Collider other) {
		if (!other.CompareTag("Player"))
			return;

		if (initialSize <= 0) {
			initialSize = cam.fieldOfView;
			finalSize = initialSize * finalSizePercent;
		}
		UpdateSizeFromPosition(false);
		inTrigger = true;
	}

	void OnTriggerExit(Collider other) {
		if (!other.CompareTag("Player"))
			return;

		inTrigger = false;
		UpdateSizeFromPosition(true);
	}

	private void UpdateSizeFromPosition(bool lerp) {
		float curSize;
		float playerPosition = direction == Utils.Direction.Left || direction == Utils.Direction.Right
			? playerTransform.position.x : playerTransform.position.z;
		curSize = initialSize + ((finalSize - initialSize) * (Mathf.Abs(playerPosition - minCoord) / triggerSize));

		if (curSize > finalSize)
			curSize = finalSize;
		else if (curSize < initialSize)
			curSize = initialSize;

		cam.fieldOfView = lerp ? Mathf.Lerp(0, finalSize, curSize / finalSize) : curSize;
	}
}
