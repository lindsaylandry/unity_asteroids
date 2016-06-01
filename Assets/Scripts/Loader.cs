using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

	public GameObject gameController;

	void Awake () {
		if (GameController.instance == null) 
			Instantiate (gameController);
	}
}
