using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

	public float speed;
	public float rotateSpeed;
	public float warpSeconds;
	public float blinkSpeed;

	public GameObject shot;
	public Transform shotSpawn;

	private bool isInteracting;
	private Collider collider;
	private Renderer rend;
	private Renderer engineRend;

	void Start() {
		collider = GetComponent<Collider> ();
		rend = GetComponent<Renderer> ();
		engineRend = GameObject.FindGameObjectWithTag ("Engine").GetComponent<Renderer> ();
		engineRend.enabled = false;

		toggleCollider ();
		Invoke ("toggleCollider", 2.0F);
	}

	void OnDestroy() {
		GameController.instance.TakeLife();
	}

	void Update () {
		if (Input.GetKeyDown ("space")) {
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
		}
	}

	void FixedUpdate () {
		float rotate = Input.GetAxis ("Horizontal") * rotateSpeed * Time.deltaTime;
		transform.Rotate (0, rotate, 0);

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

		if (Input.GetKeyDown ("up")) {
			InvokeRepeating ("BlinkEngine", blinkSpeed, blinkSpeed);
		} else if (Input.GetKey ("up")) {
			float moveForward = speed * Time.deltaTime;
		
			float xang = Mathf.Sin (transform.rotation.eulerAngles.y * Mathf.PI / 180);
			float zang = Mathf.Cos (transform.rotation.eulerAngles.y * Mathf.PI / 180);
		
			float xvel = moveForward * xang;
			float zvel = moveForward * zang;
			GetComponent<Rigidbody> ().velocity += new Vector3 (xvel, 0, zvel);
			engineRend.enabled = true;
		} else if (Input.GetKeyUp ("up")) {
			CancelInvoke("BlinkEngine");
			engineRend.enabled = false;
		}

		if (Input.GetKey (KeyCode.LeftShift)) {
			StartCoroutine (WarpShip());
		}
	}

	IEnumerator WarpShip() {
		Vector2 cameraBounds = new Vector2 (
			Camera.main.orthographicSize,
			Camera.main.aspect*Camera.main.orthographicSize);
		Vector3 newPosition = new Vector3 (
			Random.Range (-cameraBounds [0], cameraBounds [0]),
			0,
			Random.Range (-cameraBounds [1], cameraBounds [1])
			);
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().position = newPosition;
		
		GetComponent<Renderer>().enabled = false;
		yield return new WaitForSeconds(warpSeconds);
		GetComponent<Renderer>().enabled = true;
	}

	void toggleCollider() {
		collider.enabled = !collider.enabled;

		if (!collider.enabled) {
			InvokeRepeating ("BlinkSelf", blinkSpeed, blinkSpeed);
		} else {
			CancelInvoke ("BlinkSelf");
			rend.enabled = true;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Enemy Shot") {
			Destroy (other.gameObject);
			Destroy (gameObject);
		}
	}

	void BlinkSelf() {
		rend.enabled = !rend.enabled;
	}

	void BlinkEngine() {
		engineRend.enabled = !engineRend.enabled;
	}
}
