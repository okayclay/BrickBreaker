﻿using System.Collections;
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
	int numBalls = 1;
	bool gameFinished = false;
	int levelIndex = 0;
	GameObject curLevel;

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
	[SerializeField]
	Text gameOverText;
	[SerializeField]
	GameObject[] levels;
	[SerializeField]
	Text brickLabel;

	//TODO: 
	public Mode			CurrentMode {	get { return curMode; } }
	public int			GetLives	{	get { return lives; } }
	public int			GetScore	{	get { return score; } }
	public  Transform[] Powerups	{	get { return powerups; } }
	public int			NumberBalls {	get { return numBalls; } set { numBalls = value; } }
	public bool			GameFinished {  get { return gameFinished; } }

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
		Time.timeScale = 1;
		SetDefaultUI();

		CountBricks();
		curLevel = GameObject.FindWithTag("Level");

		gameOverPanel.SetActive(false);
	}

	private void CountBricks()
	{
		var temp = GameObject.FindGameObjectsWithTag("Brick");
		numBricks = temp.Length;
		UpdateBricks();
	}

	public void ChangeMode(Mode newMode)
	{
		//Debug.Log("Changing mode to " + newMode);
		curMode = newMode;

		if (curMode == Mode.GameEnd)
		{
			if (lives < 1)
				Finish("GAME OVER");
			else if (numBricks < 1)
			{
				StartCoroutine( NextLevel() );
			}
		}
	}

	private void Finish(string title)
	{
		gameOverPanel.SetActive(true);
		gameOverText.text = title;

		Time.timeScale = 0;
		gameFinished = true;
	}

	private IEnumerator NextLevel()
	{
		if (levelIndex >= levels.Length)
		{
			Finish("YOU WIN!");	//no more levels, you win
		}
		else
		{
			GameObject prevLevel = curLevel;	//Store the current level elsewhere
			curLevel = Instantiate(levels[levelIndex], curLevel.transform.position, Quaternion.identity);	//bring up the new level
			Destroy(prevLevel); //destroy previous level
			yield return new WaitForSeconds(1.5f);
			ChangeMode(Mode.Move);
			levelIndex++;
			CountBricks();
		}
	}

	public void BrickDestroyed(Transform brick)
	{
		numBricks--;
		PowerupRandomizer(brick);

		Instantiate(brick.GetComponent<Brick>().Explosion, brick.position, Quaternion.identity);
		Destroy(brick.gameObject);  //no more brick

		if (numBricks < 1)
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
		Debug.Log("Play again");
		SceneManager.LoadScene("Level 1");
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
				Time.timeScale = 0;
				break;

			case Mode.Move:
				Time.timeScale = 1;	//just make sure the timer is going again after changing levels - K.Clay
				break;

			default:
				curTime += Time.deltaTime;
				break;
		}
		UpdateTime();
	}

	public void UpdateBricks()
	{
		brickLabel.text = "Bricks Left: " + numBricks;
	}

	private void UpdateTime()
	{
		int minutes = Mathf.FloorToInt(curTime / 60F);
		timeLabel.text = string.Format("Time: {00:0}:{1:00}", minutes, curTime - minutes * 60);
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