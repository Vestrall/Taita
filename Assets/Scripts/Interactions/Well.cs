using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;

public class Well : MonoBehaviour {

	public Material colorDeactivated;

	public Material colorOneMaterial;
	public Color colorOneFanfareColor;
	public string colorOnePrefix;
	public int colorOneCount;

	public Material colorTwoMaterial;
	public Color colorTwoFanfareColor;
	public string colorTwoPrefix;
	public int colorTwoCount;

	public int fanfareParticleCount = 15;
	public float rejectForce;
	public float minZAngle;
	public float maxZAngle;
	public float minYAngle = 0;
	public float maxYAngle = 360;

	public DogGod dogGod;
	public OnEvent onSolveEvent;

	private MeshRenderer wellCenterRenderer;
	private WellPuppy wellPuppyInTrigger;
	private GameObject accepting;			// GameObject that has entered the well and is waiting to be accepted
	private List<GameObject> rejecting;		// GameObjects that have entered the well and are waiting to be rejected
	private Vector3 rejectSpawn;
	private PlayerController playerController;
	private ParticleSystem fanfareParticles;
	private int layerDefault;
	private int layerSelfOnly;
	private int layerNoCollisions;
	private bool puzzleSolved;
	private AudioSource audioSource;

	void Awake() {
		rejecting = new List<GameObject>();
		rejectSpawn = transform.Find("Reject Spawn Location").transform.position;
		wellCenterRenderer = transform.Find("Well Center").GetComponent<MeshRenderer>();
		NextMaterial();
		fanfareParticles = GetComponentInChildren<ParticleSystem>();
		layerDefault = LayerMask.NameToLayer("Default");
		layerSelfOnly = LayerMask.NameToLayer("Self Only");
		layerNoCollisions = LayerMask.NameToLayer("No Collisions");
		audioSource = GetComponent<AudioSource>();
//		StartCoroutine(testSpawner());
	}

	void Start() {
		playerController = GameManager.instance.playerController;
	}

	void Update() {
		if (wellPuppyInTrigger != null && !playerController.IsCarryingObject(wellPuppyInTrigger)) {
			foreach (Collider collider in wellPuppyInTrigger.GetComponentsInChildren<Collider>())
				OnTriggerEnter(collider);
		}
	}

	private bool IsCurrentMaterial(Material material) {
		return wellCenterRenderer.material.name.StartsWith(material.name);
	}

	//test
//	public Transform testBox;
//	IEnumerator testSpawner() {
//		yield return new WaitForSeconds(1);
//		for (int i = 0; i < 1000; i++) {
//			Transform transform = Instantiate(testBox);
//			BoxCollider[] boxColliders = transform.GetComponentsInChildren<BoxCollider>();
//			BoxCollider boxy = null;
//			foreach (BoxCollider bc in boxColliders) {
//				if (!bc.isTrigger) {
//					boxy = bc;
//					bc.enabled = false;
//				}
//			}
//			transform.GetComponent<Pickupable>().SetInteractionEnabled(false);
//			StartCoroutine(Reject(boxy));
//			yield return new WaitForSeconds(0.5F);
//		}
//	}

	private void NextMaterial() {
		if (colorOneCount <= 0 && colorTwoCount <= 0) {
			puzzleSolved = true;
			wellCenterRenderer.material = colorDeactivated;
			if (onSolveEvent != null) {
				onSolveEvent.Fire();
			}
		} else if (colorOneCount <= 0) {
			wellCenterRenderer.material = colorTwoMaterial;
			colorTwoCount--;
		} else if (colorTwoCount <= 0) {
			wellCenterRenderer.material = colorOneMaterial;
			colorOneCount--;
		} else if (Random.value > 0.5f) {
			wellCenterRenderer.material = colorTwoMaterial;
			colorTwoCount--;
		} else {
			wellCenterRenderer.material = colorOneMaterial;
			colorOneCount--;
		}
	}

	void OnTriggerEnter(Collider other) {
		GameObject otherGameObject = other.gameObject;
		if (!other.CompareTag("WellPuppy") || other.isTrigger || accepting == otherGameObject || rejecting.Contains(otherGameObject))
			return;

		WellPuppy wellPuppy = other.GetComponentInParent<WellPuppy>();
		if (playerController.IsCarryingObject(wellPuppy)) {
			wellPuppyInTrigger = wellPuppy;
			return;
		}

		other.gameObject.layer = layerSelfOnly;
		wellPuppy.SetInteractionEnabled(false);

		string otherName = otherGameObject.transform.parent.name;
		if (IsCurrentMaterial(colorDeactivated)
				|| (accepting == null && ((IsCurrentMaterial(colorOneMaterial) && otherName.StartsWith(colorOnePrefix))
				|| (IsCurrentMaterial(colorTwoMaterial) && otherName.StartsWith(colorTwoPrefix)))))
			StartCoroutine(Accept(otherGameObject));
		else
			StartCoroutine(Reject(other));
	}

	void OnTriggerExit(Collider other) {
		if (wellPuppyInTrigger == null)
			return;

		WellPuppy wellPuppy = other.GetComponentInParent<WellPuppy>();
		if (wellPuppy == wellPuppyInTrigger)
			wellPuppyInTrigger = null;
	}

	IEnumerator Reject(Collider other) {
		rejecting.Add(other.gameObject);

		// Move box to reject spawn location
		yield return new WaitForSeconds(1.5F);
		other.gameObject.layer = layerNoCollisions;
		Rigidbody body = other.GetComponentInParent<Rigidbody>();
		body.transform.position = rejectSpawn;
		body.velocity = Vector3.zero;
		body.angularVelocity = Vector3.zero;

		// Launch box back out of well
		float zAxisAngle = Random.Range(minZAngle, maxZAngle);
		float yAxisAngle = Random.Range(minYAngle, maxYAngle);
		body.velocity = Quaternion.Euler(0, yAxisAngle, zAxisAngle) * new Vector3(0, rejectForce, 0);

		// Re-enable normal box functionality
		yield return new WaitForSeconds(1.5F);
		other.gameObject.layer = layerDefault;
		other.GetComponentInParent<WellPuppy>().SetInteractionEnabled(true);
		rejecting.Remove(other.gameObject);
	}

	IEnumerator Accept(GameObject gameObject) {
		accepting = gameObject;
		yield return new WaitForSeconds(1);
		if (!puzzleSolved) {
			ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
			emitParams.startColor = gameObject.transform.parent.name.StartsWith(colorOnePrefix) ? colorOneFanfareColor : colorTwoFanfareColor;
			fanfareParticles.Emit(emitParams, fanfareParticleCount);
			audioSource.Play();
		}
		Destroy(gameObject);
		yield return new WaitForSeconds(0.5F);
		accepting = null;
		if (!puzzleSolved) {
			dogGod.IncrementSolveCount();
			NextMaterial();
		}
	}
}
