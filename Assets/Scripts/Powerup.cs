using System;
using System.Collections;
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

		static GameObject ball;

		GameManager manager;
		Paddle paddle;
		Vector3 pos;

		bool activated = false;
		bool complete = false;

		private void Start()
		{
			ball = Resources.Load("Ball", typeof(GameObject)) as GameObject;
			manager = GameObject.Find("GameManager").GetComponent<GameManager>();
			paddle = GameObject.FindGameObjectWithTag("Paddle").GetComponent<Paddle>();
		}

		public void Activate(Vector3 position)
		{
			activated = true;
			pos = position;

			if (transform.name.ToLower().Contains("extra life"))
			{
				manager.UpdateLives(manager.GetLives + 1);
			}
			else if (transform.name.ToLower().Contains("multiball"))
			{
				StartCoroutine(Wait());
			}
			else if (transform.name.ToLower().Contains("speed up"))
			{
				paddle.ChangeSpeed(paddle.OriginalSpeed * 2f);
			}
		}

		void Deactivate()
		{
			complete = true;

			if (transform.name.ToLower().Contains("speed up"))
			{
				paddle.ChangeSpeed(paddle.OriginalSpeed);
			}
		}

		void Multiball()
		{
			Instantiate(ball, pos, Quaternion.identity).GetComponent<Rigidbody2D>().AddForce(Vector2.down * ball.GetComponent<Ball>().Speed);
			manager.NumberBalls++;
		}

		IEnumerator Wait()
		{
			Multiball();
			yield return new WaitForSeconds(1.5f);
			Multiball();		
		}

		void Update()
		{
			if (activated && !complete)
				timeActivatedLeft -= Time.deltaTime;

			if (timeActivatedLeft <= 0)
				Deactivate();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			switch (other.tag)
			{
				case "Bottom":
					Destroy(gameObject);
					break;
			}
		}
	}
}