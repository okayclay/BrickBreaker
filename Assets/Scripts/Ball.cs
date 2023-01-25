using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody2D body;
    Transform paddle;
    GameManager manager;
    [SerializeField]
    float speed;

    public float Speed {  get { return speed; } }

    // Start is called before the first frame update
    void Start()
    {
        setUp();
    }

    // Update is called once per frame
    void Update()
    {
        switch(manager.CurrentMode)
		{
            case GameManager.Mode.Play:
                break;

            case GameManager.Mode.GameEnd:
                ReturnToPaddle();
                break;

            default:
                transform.position = new Vector2(paddle.position.x, paddle.position.y + .3f);   //keep the ball on the paddle and move it around

                if (Input.GetButtonDown("Jump") && !manager.GameFinished)
                {
                    manager.ChangeMode(GameManager.Mode.Play);
                    body.AddForce(Vector2.up * speed);
                   // Debug.Log("Play mode");
                }
                break;
		}
    }

    void setUp()
	{
        body = transform.GetComponent<Rigidbody2D>();
        paddle = GameObject.FindGameObjectWithTag("Paddle").transform;
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void ReturnToPaddle()
	{
        body.velocity = Vector2.zero;                        //stop the force
        manager.ChangeMode(GameManager.Mode.Move);          //move it to the paddle
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
		switch(other.tag)
		{
            case "Bottom":
                if (!manager.GameFinished)   //Dont count multiball loses against your lives
                {
                    if (manager.NumberBalls < 2)
                    {
                        ReturnToPaddle();
                        manager.UpdateLives(manager.GetLives - 1);           //lose a life
                    }
                    else
					{
                        Destroy(gameObject);
                        manager.NumberBalls--;
					}
                }
                break;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		switch(collision.gameObject.tag)
		{
            case "Brick":
                HitBrick(collision.transform);
                break;
		}
	}

    void HitBrick(Transform brick)
	{
        brick.GetComponent<Brick>().Hit();  //the brick was hit, some take more than 1 hit to destroy
        manager.UpdateScore(brick.GetComponent<Brick>().Points);
    }
}