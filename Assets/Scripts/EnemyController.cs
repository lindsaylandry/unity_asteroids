using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	public GameObject enemyShot;
	
	public float move;
	public float tumble;
	public int scoreValue;
	public bool aim;

	private int startHazard;

	void Awake() {
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
		move *= (transform.position.z > 0) ? -1 : 1;
		GetComponent<Rigidbody>().velocity = new Vector3 (0, 0, move);
	}

	void Start() {
		InvokeRepeating ("Shoot", 1.5F, 1.5F);
		InvokeRepeating ("randomMove", 2, 2);

		startHazard = GameController.instance.hazardCount;
	}

	void Shoot() {
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		if ((aim) && (player != null)) {
			Vector3 direction = (player.transform.position - transform.position).normalized;
			Instantiate (enemyShot, transform.position, Quaternion.LookRotation (direction));
		} else {
			Quaternion randomRotation = Quaternion.Euler (0, Random.Range (0.0F, 360.0F), 0);
			Instantiate (enemyShot, transform.position, randomRotation);
		}
	}

	void randomMove() {
		float xMove = Random.Range (-1.0F, 1.0F) * move;
		GetComponent<Rigidbody> ().velocity += new Vector3 
			(-GetComponent<Rigidbody> ().velocity[0] + xMove, 0, 0);
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Shot" || other.tag == "Player") {
			GameController.instance.AddScore (scoreValue);
			Destroy (other.gameObject);
			Destroy (gameObject);
		}
	}

	void OnDestroy() {
		if (startHazard == GameController.instance.hazardCount && !GameController.gameOver)
			GameController.instance.Invoke ("SpawnEnemy", Random.Range (3.0F, 10.0F));
	}
}
