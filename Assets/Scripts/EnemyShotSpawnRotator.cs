using UnityEngine;
using System.Collections;

public class EnemyShotSpawnRotator : MonoBehaviour {
	
	void Start () {
		Vector3 parentRotator = GetComponent<Transform>().parent.gameObject.
								GetComponent<Rigidbody> ().angularVelocity;
		Debug.Log (parentRotator);
		GetComponent<Rigidbody> ().angularVelocity = -parentRotator;
	}
}
