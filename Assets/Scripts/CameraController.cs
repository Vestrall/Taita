using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float zoomMin = 25f;
	public float zoomMax = 45f;
	public float zoomSpeed = 5f;
	public float followSmoothing = 5f;
	public float alternateYOffset = 20f;
	public float maxZoomBeforeAlternate = 2f;
	public float xRotationDuringAlternate = 30f;

//	private Camera cam;
	private Vector3 offset;
	private Transform followTarget;
//	private int layerMask;
//	private Vector3 alternateOffset;
//	private float maxZoomSqr;
	private Vector3 standardRotation;
	private Vector3 alternateRotation;

	void Start() {
//		cam = GetComponent<Camera>();
		followTarget = GameManager.instance.player.transform;
		offset = transform.position - followTarget.position;
//		layerMask = LayerMask.GetMask("Ground");
//		alternateOffset = new Vector3(0, alternateYOffset, 0);
//		maxZoomSqr = maxZoomBeforeAlternate * maxZoomBeforeAlternate;
		standardRotation = transform.eulerAngles;
		alternateRotation = standardRotation;
		alternateRotation.x += xRotationDuringAlternate;
	}

	// TODO: Enable manual zoom for orthographic camera?
//	void Update() {
//		// Camera zoom
//		float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
//		if (mouseWheel != 0) {
//			// Perspective zoom
//			float newValue = cam.fieldOfView - (mouseWheel * zoomSpeed);
//			cam.fieldOfView = newValue < zoomMin ? zoomMin : newValue > zoomMax ? zoomMax : newValue;
//
//			// Orthographic zoom
////			float newValue = cam.orthographicSize - (mouseWheel * zoomSpeed);
////			cam.orthographicSize = newValue < zoomMin ? zoomMin : newValue > zoomMax ? zoomMax : newValue;
//		}
//	}

	void FixedUpdate() {
		Vector3 followTargetPosition = followTarget.position;
		Vector3 standardCameraPosition = followTargetPosition + offset;

		transform.position = Vector3.Lerp(transform.position, standardCameraPosition, Time.fixedDeltaTime * followSmoothing);
		transform.eulerAngles = standardRotation;

		// TODO: Create auto-zoom for orthographic camera
//		Ray ray = new Ray(followTargetPosition, standardCameraPosition - followTargetPosition);
//		RaycastHit hit;
//		if (Physics.Raycast(ray, out hit, Utils.BIG_DISTANCE, layerMask)) {
////			Vector3 distance = standardCameraPosition - hit.point;
////			if (distance.sqrMagnitude > maxZoomSqr) {
////				Vector3 alternateCameraPosition = standardCameraPosition + alternateOffset;
////				if (!Physics.Raycast(followTargetPosition, alternateCameraPosition - followTargetPosition, Utils.BIG_DISTANCE, layerMask)) {
////					// Alternate camera position
////					transform.position = Vector3.Lerp(transform.position, alternateCameraPosition, Time.deltaTime * followSmoothing);	// TODO: Different smoothing value?
////					transform.eulerAngles = alternateRotation;
////				} else {
//					// ALternate failed, zoom in as much as ne
//					transform.position = Vector3.Lerp(transform.position, hit.point, Time.deltaTime * followSmoothing);	// TODO: Different smoothing value?
//					transform.eulerAngles = standardRotation;
////				}
////			}  // TODO: else?!
//		} else {
//			// No obstructions, use standard camera position
//			transform.position = Vector3.Lerp(transform.position, standardCameraPosition, Time.deltaTime * followSmoothing);
//			transform.eulerAngles = standardRotation;
//		}
	}
}
