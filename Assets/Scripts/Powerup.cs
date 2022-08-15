using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
	class Powerup : MonoBehaviour
	{
		[SerializeField]
		float timeActivatedLeft = 0;

		GameManager manager;
		Paddle paddle;

		bool activated = false;
		bool complete = false;

		private void Start()
		{
			manager = GameObject.Find("GameManager").GetComponent<GameManager>();
			paddle = GameObject.FindGameObjectWithTag("Paddle").GetComponent<Paddle>();
		}

		public void Activate()
		{
			activated = true;

			if (transform.name.ToLower().Contains("extra life"))
			{
				manager.UpdateLives(manager.GetLives + 1);
			}
			else if (transform.name.ToLower().Contains("multiball"))
			{
				GameObject ball;
				for (int i = 0; i < 2; i++)	//TO DO: Change how they are moved 
				{
					ball = Instantiate(Resources.Load("Ball", typeof(GameObject))) as GameObject;
					ball.GetComponent<Rigidbody2D>().AddForce(Vector2.up * ball.GetComponent<Ball>().Speed);
				}
			}
			else if (transform.name.ToLower().Contains("speed up"))
			{
				paddle.ChangeSpeed(paddle.OriginalSpeed * 1.5f);
			}
		}

		void Deactivate()
		{
			complete = true;

			if (transform.name.ToLower().Contains("multiball"))
			{

			}
			else if (transform.name.ToLower().Contains("speed up"))
			{
				paddle.ChangeSpeed(paddle.OriginalSpeed);
			}
		}

		void Update()
		{
			if (activated && !complete)
				timeActivatedLeft -= Time.deltaTime;

			if (timeActivatedLeft <= 0)
				Deactivate();
		}
	}
}
