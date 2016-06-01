using UnityEngine;
using System.Collections;

public class MoveByBoundary : MonoBehaviour {

	void OnTriggerExit(Collider other) {
		float diffx = Mathf.Abs (other.transform.position.x) - transform.localScale.x / 2;
		float diffz = Mathf.Abs (other.transform.position.z) - transform.localScale.z / 2;

		Vector3 newPos = new Vector3 (0, 0, 0);
		if (diffx > 0) {
			newPos [0] = -other.gameObject.transform.position.x * 2.0f;
			newPos [0] += (newPos [0] > 0) ? -(diffx - other.bounds.extents.x) : 
											   diffx - other.bounds.extents.x;
		} 
		if (diffz > 0) {
			if ((other.tag == "Enemy") && 
			    (Mathf.Abs(other.attachedRigidbody.velocity.z + other.transform.position.z) >
			 	 Mathf.Abs(other.transform.position.z))) {
				Destroy(other.gameObject);
				return;
			} else {
				newPos [2] = -other.gameObject.transform.position.z * 2.0f;
				newPos [2] += (newPos [2] > 0) ? -(diffz - other.bounds.extents.z) : 
											   diffz - other.bounds.extents.z;
			}
		}
		other.gameObject.transform.position += newPos;
	}
}
