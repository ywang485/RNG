using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Opening : MonoBehaviour {

	private Text scoreDisplay = null;
	private int score = 0;

	void Start() {
		GameObject scoreDisplayObj = GameObject.Find ("ScoreDisplay");
		if (scoreDisplayObj != null) {
			scoreDisplay = scoreDisplayObj.GetComponent<Text> ();
		}
		if (scoreDisplay != null) {
			if (GamePlay.playerData.bossDefeated) {
				score = 400;
			} else if (GamePlay.playerData.yetiDefeated) {
				score = 300;
			} else if (GamePlay.playerData.slimeDefeated) {
				score = 200;
			} else if (GamePlay.playerData.batDefeated) {
				score = 100;
			} else {
				score = 0;
			}
		}
	}

	void Update() {
		if (scoreDisplay != null) {
			scoreDisplay.text = "Score: " + score;
		}
	}

	public void startGame() {
		SceneManager.LoadScene ("GamePlay");
	}
}
