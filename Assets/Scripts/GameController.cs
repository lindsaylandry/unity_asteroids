using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour {
	public GameObject hugeAsteroid;
	public GameObject player;
	public GameObject enemy1;
	public GameObject enemy2;
	public GameObject boundary;
	public static GameController instance = null;
	public GameObject[] scoreNameObjects;

	public int hazardCount;
	public int lifeCount;
	public int newLife;

	public Text scoreText;
	public Text livesText;
	public Text highScoreText;
	public Text gameOverTitle;
	public GameObject gameOverInputName;
	public GameObject gameOverGeneral;
	public GameObject titleMenu;
	public GameObject highScoreMenu;
	public InputField newName;

	private int score = 0;
	public static bool gameOver;

	private List<int> scores;
	private List<string> scoreNames;

	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		scores = new List<int> ();
		scoreNames = new List<string>();
		gameOver = false;
		if (!isPlayerPrefsDefined ())
			ResetScoreBoard ();
	}

	void Start() {
		ExitGameOver ();
		titleMenu.SetActive (true);

		getScoreboardValues ();
		UpdateScore ();
		UpdateLives ();
	}

	void SpawnEnemy() {
		Vector2 cameraBounds = new Vector2 (
			Camera.main.orthographicSize,
			Camera.main.aspect*Camera.main.orthographicSize);
		float zPos = (Random.Range (-1.0F, 1.0F) > 0)? cameraBounds [1]: -cameraBounds [1];
		Vector3 spawnPosition = new Vector3 (
				Random.Range (-cameraBounds [0], cameraBounds [0]),
				0,
				zPos
		);
		GameObject enemySpawn = (Random.Range (-1.0F, 2.0F) > 0) ? enemy1 : enemy2;
		Instantiate (enemySpawn, spawnPosition, Quaternion.identity);
	}

	void SpawnAsteroids() {
		for (int i = 0; i < hazardCount; i++)
			Instantiate (hugeAsteroid, Vector3.zero, Quaternion.identity);
	}

	public void BeginGame() {
		titleMenu.SetActive (false);

		SpawnAsteroids ();
		Invoke ("SpawnPlayer", 2);
		Invoke ("SpawnEnemy", 30.0F);
	}

	public void SubtractAsteroidCount() {
		int numLeft = GameObject.FindGameObjectsWithTag ("Asteroid").Length;
		if (numLeft == 0) {
			CancelInvoke ("SpawnEnemy");
			hazardCount += 1;
			Invoke("SpawnAsteroids", 2);
			Invoke ("SpawnEnemy", 30.0F);
		}
	}

	public void TakeLife() {
		lifeCount -= 1;
		if (lifeCount >= 0) {
			UpdateLives ();
			Invoke ("SpawnPlayer", 1.5F);
		} else
			GameOver ();
	}

	public void AddScore(int value) {
		score += value;
		UpdateScore();
		if (score / newLife >= 1) {
			lifeCount++;
			newLife = newLife * 2;
			UpdateLives ();
		}
	}

	void SpawnPlayer() {
		Instantiate (player, Vector3.zero, Quaternion.identity);
		player.SetActive(true);
	}

	bool isPlayerPrefsDefined() {
		if (PlayerPrefs.HasKey ("Asteroids Scoreboard Name 1"))
			return true;
		else
			return false;
	}

	public void ResetScoreBoard() {
		string[] defaultNames = "The Quick Brown Fox Jumps Over The Lazy Dog Asteroids".Split (' ');
		for (int i = 0; i < scores.Count; i++) {
			PlayerPrefs.SetString ("Asteroids Scoreboard Name " + i, defaultNames[i]);
			PlayerPrefs.SetInt ("Asteroids Scoreboard " + i, 4000 - (200*i));
		}
		getScoreboardValues ();
	}

	void getScoreboardValues() {
		scoreNames.Clear ();
		scores.Clear ();
		for (int i = 0; i < scoreNameObjects.Length; i++) {
			scoreNames.Add (PlayerPrefs.GetString ("Asteroids Scoreboard Name " + i));
			scores.Add (PlayerPrefs.GetInt ("Asteroids Scoreboard " + i));
		}
		PopulateScoreboard ();
	}

	void setScoreboardValues() {
		for (int i = 0; i < scoreNameObjects.Length; i++) {
			PlayerPrefs.SetString ("Asteroids Scoreboard Name " + i, scoreNames[i]);
			PlayerPrefs.SetInt ("Asteroids Scoreboard " + i, scores[i]);
		}
		PopulateScoreboard ();
	}

	void PopulateScoreboard() {
		for (int i = 0; i < scoreNameObjects.Length; i++) {
			var stmp = scoreNameObjects[i].transform.Find ("Score").GetComponent<Text> ();
			var ntmp = scoreNameObjects[i].transform.Find ("Name").GetComponent<Text>();
			stmp.text = scores[i].ToString ();
			ntmp.text = scoreNames[i];
		}
		highScoreText.text = PlayerPrefs.GetInt ("Asteroids Scoreboard 0").ToString ();
	}

	public void UpdateScoreboard() {
		gameOverInputName.SetActive (false);
		gameOverTitle.gameObject.SetActive (false);
		ShowHighScore ();

		scores.Sort ();
		scoreNames.Reverse ();
		int index = scores.BinarySearch(score);
		if (index < 0) {
			scores.Insert (~index, score);
			scores.RemoveAt (0);
			scoreNames.Insert (~index, newName.text);
			scoreNames.RemoveAt (0);
		}
		scores.Reverse ();
		scoreNames.Reverse ();

		score = 0;
		setScoreboardValues ();
	}

	void UpdateScore() {
		scoreText.text = "Score: " + score;
		if (score > System.Convert.ToInt32(highScoreText.text))
			highScoreText.text = score.ToString ();
	}

	void UpdateLives() {
		livesText.text = "Lives: " + lifeCount;
	}

	void GameOver() {
		gameOver = true;
		gameOverTitle.gameObject.SetActive (true);
		CancelInvoke ("SpawnEnemy");
		if (score > scores[scores.Count - 1]) 
			gameOverInputName.SetActive (true);
		else
			gameOverGeneral.SetActive (true);
	}

	public void ExitTitleMenu() {
		titleMenu.SetActive (false);
	}

	public void ExitGameOver() {
		gameOverTitle.gameObject.SetActive (false);
		gameOverGeneral.SetActive (false);
		gameOverInputName.SetActive (false);
	}

	public void ShowHighScore() {
		highScoreMenu.gameObject.SetActive (true);
	}

	public void ExitHighScore() {
		highScoreMenu.SetActive (false);
		if (gameOver)
			GameOver ();
		else
			titleMenu.SetActive (true);
	}

	public void Reset() {
		gameOverTitle.gameObject.SetActive (false);
		Application.LoadLevel (Application.loadedLevel);
	}
}
