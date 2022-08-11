using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private int lives = 3;
	private int score;
	private Mode curMode;
	private float curTime;
	int numBricks;

	[SerializeField]
	GameObject gameOverPanel;
	[SerializeField]
	Text livesLabel;
	[SerializeField]
	Text scoreLabel;
	[SerializeField]
	Text timeLabel;

	public Mode CurrentMode { get { return curMode; } }
	public int GetLives { get { return lives;} }
	public int GetScore {  get { return score; } }

	public enum Mode
	{
		None,
		Play,
		Move,
		Pause,
		GameOver
	}

	void Start()
	{
		ChangeMode(Mode.Pause);
		SetDefaultUI();

		var temp = GameObject.FindObjectsOfType<Brick>();
		numBricks = temp.Length;

		gameOverPanel.SetActive(false);
	}

	public void ChangeMode(Mode newMode)
	{
		curMode = newMode;

		if (curMode == Mode.GameOver)
			GameOver();
	}

	void GameOver()
	{
		gameOverPanel.SetActive(true);
	}

	public void PlayAgain()
	{
		SceneManager.LoadScene("SampleScene");
	}

	public void Quit()
	{
		Application.Quit();
		Debug.Log("Good-bye!");
	}

	private void SetDefaultUI()
	{
		scoreLabel.text = "Score: " + score.ToString();
		livesLabel.text = "Lives: " + lives.ToString();
	}

	private void Update()
	{
		switch (curMode)
		{
			case Mode.Pause:
				break;

			case Mode.GameOver:
				break;

			default:
				curTime += Time.deltaTime;
				int minutes = Mathf.FloorToInt(curTime / 60F);
				timeLabel.text = string.Format("Time: {0:0}:{1:00}", minutes, curTime - minutes * 60);
				break;
		}
	}

	public void UpdateLives(int newLifeAmount)
	{
		lives = newLifeAmount;
		livesLabel.text = "Lives: " + lives.ToString();
		if (lives < 1)
		{
			lives = 0;
			ChangeMode(Mode.GameOver);
		}
	}

	public void UpdateScore(int amount)
	{
		score += amount;
		scoreLabel.text = "Score: " + score.ToString();
	}
}
