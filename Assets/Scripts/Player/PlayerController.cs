using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public float dropVelocityMultiplier = 0.7F;

	[HideInInspector] public bool controllable = true;

	private Transform carryPosition;
	private Transform playerCenter;
	private CharacterController characterController;

	private Vector3 characterDirection;
	private Pickupable carriedObject;
	private Rigidbody carriedBody;

	private Ability[] abilities;
	private InteractionAbility interactionAbility;
	private ObjectDropper objectDropper;
	private List<Item> inventory;

	void Awake() {
		Transform mainTransform = gameObject.transform;
		playerCenter = mainTransform;
		characterController = GetComponent<CharacterController>();
		foreach (Transform transform in mainTransform) {
			if (transform.name == "CarryPosition") {
				carryPosition = transform;
				break;
			}
		}
		characterDirection = carryPosition.position - playerCenter.position;
		characterDirection.y = 0;
		abilities = GetComponents<Ability>();
		interactionAbility = GetComponent<InteractionAbility>();
		objectDropper = GetComponent<ObjectDropper>();
		inventory = new List<Item>();
	}

	public void SetControllable(bool controllable) {
		this.controllable = controllable;
	}

	void Update() {
		if (!controllable)
			return;

		float deltaTime = Time.deltaTime;
		float dx = Input.GetAxisRaw("Horizontal");
		float dz = Input.GetAxisRaw("Vertical");

		foreach (Ability ability in abilities)
			ability.TimeStep(deltaTime);

		// Move carry position based on user input
		UpdateCarryPosition(dx, dz);
	}

	void FixedUpdate() {
		// Keep carried object in carry position
		if (carriedObject != null) {
			carriedObject.transform.localPosition = Vector3.zero;
			carriedBody.velocity = Vector3.zero;
		}
	}

	private void UpdateCarryPosition(float dx, float dz) {
		if (dx == 0 && dz == 0)
			return;

		Vector3 newDirection = new Vector3(dx, 0, dz);
		float dAngle = Utils.GetAngle(characterDirection, newDirection);
		carryPosition.RotateAround(playerCenter.position, Vector3.up, dAngle);
		characterDirection = newDirection;
	}

	public void AddItem(Item item) {
		inventory.Add(item);
	}

	public bool CarryObject(Pickupable carriedObject) {
		if (this.carriedObject != null)
			return false;

		objectDropper.TriggerCooldown();
		carriedObject.transform.parent = carryPosition.transform;
		carriedObject.transform.localPosition = Vector3.zero;	// Move to carryPosition
		carriedBody = carriedObject.GetComponent<Rigidbody>();
		carriedBody.velocity = Vector3.zero;
		this.carriedObject = carriedObject;
			
		return true;
	}

	public void UseCarriedObject(Interaction interaction) {
		if (carriedObject == null)
			return;

		carriedObject.Use(interaction);
	}

	public bool IsCarryingObject() {
		return carriedObject != null;
	}

	public bool IsCarryingObject(Pickupable pickupable) {
		return pickupable != null && carriedObject == pickupable;
	}

	public void DropCarriedObject() {
		if (carriedObject == null)
			return;

		carriedObject.transform.parent = null;
		carriedBody.velocity = characterController.velocity * dropVelocityMultiplier;
		carriedObject.Drop();
		carriedObject = null;
		carriedBody = null;
		if (interactionAbility != null)
			interactionAbility.OnCarriedObjectDropped();
	}
}
