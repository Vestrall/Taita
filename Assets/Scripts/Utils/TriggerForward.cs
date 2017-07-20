using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerForward : MonoBehaviour {

	List<Utils.TriggerEventListener> parentListeners;

	void Awake() {
		parentListeners = new List<Utils.TriggerEventListener>();
		MonoBehaviour[] scripts = GetComponentsInParent<MonoBehaviour>();
		if (scripts != null) {
			foreach (MonoBehaviour monoBehaviour in scripts) {
				Utils.TriggerEventListener listener = monoBehaviour as Utils.TriggerEventListener;
				if (listener != null)
					parentListeners.Add(listener);
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		foreach (Utils.TriggerEventListener listener in parentListeners)
			listener.OnTriggerRangeChange(true, other);
	}

	void OnTriggerExit(Collider other) {
		foreach (Utils.TriggerEventListener listener in parentListeners)
			listener.OnTriggerRangeChange(false, other);
	}
}
