using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;

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
	[SerializeField]
	Transform[] powerups;

	public Mode			CurrentMode {	get { return curMode; } }
	public int			GetLives	{	get { return lives; } }
	public int			GetScore	{	get { return score; } }
	public  Transform[] Powerups	{	get { return powerups; } }

	public enum Mode
	{
		None,
		Play,
		Move,
		Pause,
		GameEnd
	}

	void Start()
	{
		ChangeMode(Mode.Pause);
		SetDefaultUI();

		var temp = GameObject.FindGameObjectsWithTag("Brick");
		numBricks = temp.Length;
		Debug.Log(numBricks + " found");

		gameOverPanel.SetActive(false);
	}

	public void ChangeMode(Mode newMode)
	{
		curMode = newMode;

		if (curMode == Mode.GameEnd)
		{
			if (lives < 1)
				GameOver();
			else if (numBricks < 1)
				GameWon();
		}
	}

	void GameOver()
	{
		gameOverPanel.SetActive(true);
	}

	void GameWon()
	{

	}

	public void BrickDestroyed(Transform brick)
	{
		numBricks--;
		PowerupRandomizer(brick);
		if(numBricks < 1)
		{
			numBricks = 0;
			ChangeMode(Mode.GameEnd);
		}
	}

	void PowerupRandomizer(Transform brick)
	{
		int chance = Random.Range(1, 11);  //max exclusive
		int index = Random.Range(0, powerups.Length);

		if (chance < 6)
		{
			Instantiate(powerups[index], brick.position, brick.rotation);
		}
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

			case Mode.GameEnd:
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
			ChangeMode(Mode.GameEnd);
		}
	}

	public void UpdateScore(int amount)
	{
		score += amount;
		scoreLabel.text = "Score: " + score.ToString();
	}
}