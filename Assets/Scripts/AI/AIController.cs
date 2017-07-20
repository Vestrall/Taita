using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour {

	public AIPath[] paths;

	private SpriteRenderer spriteRenderer;
	private UnityEngine.AI.NavMeshAgent nav;
	private Vector3 currentDestination;
	private int currentPath = -1;

	private const float MOVEMENT_EPSILON = 0.1f;

	void Awake() {
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

	void Update() {
		// Navigation
		if (currentDestination != Vector3.zero && !nav.pathPending) {
			if (nav.remainingDistance > MOVEMENT_EPSILON)
				nav.SetDestination(currentDestination);	// TODO: Necessary every frame?
			else {
				currentDestination = paths[currentPath].NextPosition();
				if (currentDestination == Vector3.zero)
					nav.isStopped = true;
				else
					nav.SetDestination(currentDestination);
			}

			spriteRenderer.flipX = currentDestination.x > transform.position.x;
		}
	}

	public void StartPath(string pathName) {
		for (int i = 0; i < paths.Length; i++) {
			AIPath path = paths[i];
			if (path.name.Equals(pathName, System.StringComparison.InvariantCultureIgnoreCase)) {
				StartPath(i);
				break;
			}
		}
	}

	public void StartPath(int pathIndex) {
		if (pathIndex >= paths.Length)
			return;

		currentPath = pathIndex;
		currentDestination = paths[currentPath].StartPosition();
		nav.isStopped = false;
		nav.SetDestination(currentDestination);
	}

	public void NextPath() {
		StartPath(currentPath + 1);
	}
}
