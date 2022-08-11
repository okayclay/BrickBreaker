using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	private int lives = 3;
	private int score;
	private Mode curMode;
	private float curTime;
	int numBricks;

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
		Pause
	}

	void Start()
	{
		ChangeMode(Mode.Pause);
		SetDefaultUI();
		var temp = GameObject.FindObjectsOfType<Brick>();
		numBricks = temp.Length;
	}

	public void ChangeMode(Mode newMode)
	{
		curMode = newMode;
	}

	private void Update()
	{
		switch(curMode)
		{
			case Mode.Pause:
				break;

			default:
				curTime += Time.deltaTime;
				int minutes = Mathf.FloorToInt(curTime / 60F);
				timeLabel.text = string.Format("Tmie: {0:0}:{1:00}", minutes, curTime - minutes * 60);
				break;
		}
	}

	private void SetDefaultUI()
	{
		scoreLabel.text = "Score: " + score.ToString();
		livesLabel.text = "Lives: " + lives.ToString();
	}

	public void UpdateLives(int newLifeAmount)
	{
		lives = newLifeAmount;
		if(lives < 1)
		{
			Debug.Log("End the game here");
		}
		livesLabel.text = "Lives: " + lives.ToString();
	}

	public void UpdateScore(int amount)
	{
		score += amount;
		scoreLabel.text = "Score: " + score.ToString();
	}
}
