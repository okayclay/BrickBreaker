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
	int numBalls = 1;
	bool gameFinished = false;

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

	//TODO: Double check play again button
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

		var temp = GameObject.FindGameObjectsWithTag("Brick");
		numBricks = temp.Length;

		gameOverPanel.SetActive(false);
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
				Finish("YOU WIN!");
		}
	}

	private void Finish(string title)
	{
		gameOverPanel.SetActive(true);
		gameOverText.text = title;

		Time.timeScale = 0;
		gameFinished = true;
	}

	public IEnumerator BrickDestroyed(Transform brick)
	{
		numBricks--;
		PowerupRandomizer(brick);

		Transform explosion = Instantiate(brick.GetComponent<Brick>().Explosion, brick.position, Quaternion.identity);
		Destroy(brick.gameObject);  //no more brick
		yield return new WaitForSeconds(2);
		Destroy(explosion.gameObject);  //clean up explosion

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
				Time.timeScale = 0;
				break;

			default:
				curTime += Time.deltaTime;
				break;
		}
		UpdateTime();
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