using UnityEngine;
using System.Collections;

using AsteroidsLibrary;

public class HugeAsteroidSpawner : MonoBehaviour {

	public float move;
	public float tumble;
	
	private GameObject player;
	private Rigidbody rigid;
	private RandomSpawner randomSpawner;
	
	void Awake () {
		player = GameObject.FindGameObjectWithTag ("Player");
		rigid = GetComponent <Rigidbody> ();
		randomSpawner = new RandomSpawner ();
	}

	void Start() {
		rigid.angularVelocity = randomSpawner.randomAngularVelocity (tumble);

		transform.position = randomSpawner.randomPosition ();
		if (player != null) {
			while ((gameObject.transform.position - player.transform.position).magnitude < 4)
				transform.position = randomSpawner.randomPosition ();
		}
		rigid.velocity = randomSpawner.randomVelocity (move);
	}
}
