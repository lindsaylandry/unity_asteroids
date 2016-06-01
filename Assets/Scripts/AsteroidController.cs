using UnityEngine;
using System.Collections;

public class AsteroidController : MonoBehaviour {
	public GameObject spawnAsteroid;
	public int scoreValue;

	private Collider collider;

	void Awake() {
		collider = GetComponent <Collider> ();
	}

	void FixedUpdate() {
		if (Mathf.Abs (gameObject.transform.position.x) > (13.5f + collider.bounds.extents.x)) { 	
			Vector3 newPos = new Vector3(-2.0f*gameObject.transform.position.x, 0, 0);
			newPos[0] += (newPos[0] > 0) ? -0.5f : 0.5f;
			gameObject.transform.position += newPos;
		} 
		if (Mathf.Abs (gameObject.transform.position.z) > (36.0f/2.0f + collider.bounds.extents.z)) {
			Vector3 newPos = new Vector3(0, 0, -2.0f*gameObject.transform.position.z);
			newPos[2] += (newPos[2] > 0) ? -0.5f : 0.5f;
			gameObject.transform.position += newPos;
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Shot" || other.tag == "Enemy Shot" || other.tag == "Player" || other.tag == "Enemy") {
			if (other.tag != "Enemy" && other.tag != "Enemy Shot")
				GameController.instance.AddScore(scoreValue);
			Destroy (other.gameObject);
			if (spawnAsteroid) {
				Instantiate (spawnAsteroid, transform.position, transform.rotation);
				Instantiate (spawnAsteroid, transform.position, transform.rotation);
			} 
			Destroy (gameObject);
		}
	}

	void OnDestroy() {
		GameController.instance.SubtractAsteroidCount ();
	}
}