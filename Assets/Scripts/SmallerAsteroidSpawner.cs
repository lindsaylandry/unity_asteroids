using UnityEngine;
using System.Collections;

using AsteroidsLibrary;

public class SmallerAsteroidSpawner : MonoBehaviour {

	public float move;
	public float tumble;
	
	private Rigidbody rigid;
	private RandomSpawner randomSpawner;
	
	void Awake() {
		rigid = GetComponent<Rigidbody> ();
		randomSpawner = new RandomSpawner ();
	}
	
	void Start() {
		rigid.angularVelocity = randomSpawner.randomAngularVelocity (tumble);
		rigid.velocity = randomSpawner.randomVelocity (move);
	}
}
