using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AsteroidsLibrary {
public class RandomSpawner
{
	public RandomSpawner ()
	{
	}

	public Vector3 randomPosition() {
		Vector2 cameraBounds = new Vector2 (
			Camera.main.orthographicSize,
			Camera.main.aspect*Camera.main.orthographicSize);
		Vector3 spawnPosition = new Vector3 (
			Random.Range (-cameraBounds [0], cameraBounds [0]),
			0,
			Random.Range (-cameraBounds [1], cameraBounds [1])
			);
		return spawnPosition;
	}
	
	public Vector3 randomVelocity(float move) {
		Vector2 direction = Random.insideUnitCircle * move;
		return new Vector3 (direction.x, 0, direction.y);
	}
	
	public Vector3 randomAngularVelocity(float tumble) {
		return Random.insideUnitSphere * tumble;
	}
}
}

