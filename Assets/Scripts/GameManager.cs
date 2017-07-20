using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	// TODO: Get player, inventory with Find()?
	public GameObject player;
//	public InventoryManager inventory;

	[HideInInspector] public PlayerController playerController;
	[HideInInspector] public PlayerMovement playerMovement;
	[HideInInspector] public PlayerAnimator playerAnimator;

	void Awake() {
		if (instance != null && instance != this) {
			Destroy(instance.gameObject);
		}
		instance = this;

		// Don't be destroyed when reloading scene
//		DontDestroyOnLoad(gameObject);

		playerController = player.GetComponent<PlayerController>();
		playerMovement = player.GetComponent<PlayerMovement>();
		playerAnimator = player.GetComponentInChildren<PlayerAnimator>();
	}
}
