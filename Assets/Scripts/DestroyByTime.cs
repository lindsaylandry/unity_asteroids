using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {
	public float life;

	void Start() {
		Destroy (gameObject, life);
	}
}
