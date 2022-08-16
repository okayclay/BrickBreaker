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
    [SerializeField]
    Transform explosionParticle;

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

                if (Input.GetButtonDown("Jump"))
                {
                    manager.ChangeMode(GameManager.Mode.Play);
                    body.AddForce(Vector2.up * speed);
                }
                break;
		}
    }

    void setUp()
	{
        body = transform.GetComponent<Rigidbody2D>();
        Debug.Log("Rigidbody Found: " + (body != null));
        paddle = GameObject.FindGameObjectWithTag("Paddle").transform;
        Debug.Log("Paddle found: " + (paddle != null));
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Debug.Log("Game Manager found: " + (manager != null));
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
                if (manager.CurrentMode != GameManager.Mode.GameEnd)   //Dont count multiball loses against your lives
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
                StartCoroutine(HitBrick(collision.transform));
                break;
		}
	}

    IEnumerator HitBrick(Transform brick)
	{
        manager.BrickDestroyed(brick);
        manager.UpdateScore(brick.GetComponent<Brick>().Points);
        Transform explosion = Instantiate(explosionParticle, brick.position, Quaternion.identity);
        Destroy(brick.gameObject);  //no more brick
        yield return new WaitForSeconds(2);
        Destroy(explosion.gameObject);  //clean up explosion
    }
}
