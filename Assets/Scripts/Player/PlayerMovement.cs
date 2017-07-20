using UnityEngine;
using System.Collections;
using Spine.Unity;

public class PlayerMovement : MonoBehaviour {

	public float speed = 4;
	public float jumpSpeed = 6;
	public float gravity = 20;

	private CharacterController controller;

	private Vector3 move = Vector3.zero;
	private Vector3 inputVector = Vector3.zero;
	private float inputMagnitude;
	private Quaternion moveRotate;
	private float accelerationPerSecond = float.MaxValue;
	private float slideStoppingSpeed;	// Fraction relative to directing the character in the opposite direction
	private Vector3 pushVector;
	private float pushTimeRemaining;
	private Vector3 pushReductionPerSecond;

	// Slope handling
	public float slidingSpeed = 15;
	private Vector3 groundNormal = Vector3.zero;
	private Vector3 lastGroundNormal = Vector3.zero;
	private Vector3 lastHitPoint = new Vector3(Mathf.Infinity, 0, 0);

	void Awake() {
		controller = GetComponent<CharacterController>();
		moveRotate = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
	}

	void FixedUpdate() {
		if (!GameManager.instance.playerController.controllable)
			return;

		float deltaTime = Time.fixedDeltaTime;
		if (!controller.enabled) {
			Vector3 position = transform.position;
			move.y -= gravity * deltaTime * deltaTime;
			transform.position += move;
			return;
		}

		float dx = Input.GetAxisRaw("Horizontal");
		float dz = Input.GetAxisRaw("Vertical");
		inputVector = new Vector3(dx, 0, dz);
		inputMagnitude = inputVector.magnitude;

		if (pushTimeRemaining > 0) {
			pushTimeRemaining -= deltaTime;
			if (!controller.isGrounded) {
				float y = move.y;
				move = pushVector;
				move.y = y - (gravity * deltaTime);		// TODO: Make gravity work for pushes that include y components
			} else {
				move = pushVector;
			}
			if (pushReductionPerSecond != Vector3.zero) {
				pushVector -= pushReductionPerSecond * deltaTime;
			}
		} else if (controller.isGrounded) {
			if (accelerationPerSecond < float.MaxValue) {
				// Player is sliding
				Vector3 curVelocity = controller.velocity;
				if (dx != 0 || dz != 0) {
					// Player is directing character
					move = moveRotate * move;
					Vector3 thisMove = inputVector;
					thisMove *= accelerationPerSecond * speed * deltaTime;
					move = Vector3.ClampMagnitude(controller.velocity + thisMove, speed);
				} else {
					// User is not moving character. Slide to a stop.
					Vector3 thisMove = -curVelocity.normalized;
					thisMove *= accelerationPerSecond * speed * deltaTime * slideStoppingSpeed;
					move = Vector3.ClampMagnitude(controller.velocity + thisMove, speed);
					if (Mathf.Abs(move.x) < Utils.EPSILON)
						move.x = 0;
					if (Mathf.Abs(move.z) < Utils.EPSILON)
						move.z = 0;
				}
				if (Input.GetButtonDown("Jump"))
					move.y = jumpSpeed;
			} else if (groundNormal.y <= Mathf.Cos(controller.slopeLimit * Mathf.Deg2Rad)) {
				// Sliding down steep slope
				// The direction we're sliding in
				move = new Vector3(groundNormal.x, 0, groundNormal.z).normalized;
				move *= slidingSpeed;

				Vector3 sideways = Vector3.Cross(Vector3.up, move);
				move = Vector3.Cross(sideways, groundNormal).normalized * move.magnitude;
			} else {
				move.Set(dx, 0, dz);
				move = moveRotate * move;
				move = transform.TransformDirection(move);
				move *= speed;
				if (Input.GetButton("Jump"))
					move.y = jumpSpeed;
			}
		} else {
			move.y -= gravity * deltaTime;
		}

		// Will be updated by controller.Move()
		groundNormal = Vector3.zero;

		controller.Move(move * deltaTime);

		lastGroundNormal = groundNormal;
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		if(hit.normal.y > 0 && hit.normal.y > groundNormal.y && hit.moveDirection.y < 0) {
			groundNormal = (hit.point - lastHitPoint).sqrMagnitude > 0.001 || lastGroundNormal == Vector3.zero ? hit.normal : lastGroundNormal;
			lastHitPoint = hit.point;
		}
	}

	public void StartSliding(float accelerationPerSecond, float slideStoppingSpeed) {
		this.accelerationPerSecond = accelerationPerSecond;
		this.slideStoppingSpeed = slideStoppingSpeed;
	}

	public void StopSliding() {
		accelerationPerSecond = float.MaxValue;
	}

	public void Push(Vector3 pushVector, float duration, float finishRatio) {
		this.pushVector = pushVector;
		pushTimeRemaining = duration;

		if (finishRatio != 1) {
			float ratioDiffPerSecond = (1 - finishRatio) / duration;
			pushReductionPerSecond = pushVector * ratioDiffPerSecond;
		} else {
			pushReductionPerSecond = Vector3.zero;
		}
	}

	public Vector3 GetInputVector() {
		return inputVector;
	}

	public float GetInputMagnitude() {
		return inputMagnitude;
	}

	public void SetPhysicalCollidersEnabled(bool enabled) {
		controller.enabled = false;
		move = Vector3.zero;
	}

//	public void SetMovementControl(bool movementControl) {
//		this.movementControl = movementControl;
//	}
//
//	public void SetVelocityMin(float moveSpeedPercent, bool excludeYAxis) {
//		float minMagnitude = moveSpeedPercent * speed;
//		Vector3 curMove = move;
//		if (excludeYAxis)
//			curMove.y = 0;
//		if (curMove.sqrMagnitude >= minMagnitude * minMagnitude)
//			return;
//
//		Debug.Log("Previous speed: " + curMove.magnitude);
//
//		curMove = curMove.normalized * minMagnitude;
//		move.x = curMove.x;
//		if (!excludeYAxis)
//			move.y = curMove.y;
//		move.z = curMove.z;
//	}
}
