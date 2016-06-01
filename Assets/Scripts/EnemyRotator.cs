using UnityEngine;
using System.Collections;

public class EnemyRotator : MonoBehaviour {
	public float tumble;

	void Start () {
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
	}
}
