using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class Ball : MonoBehaviour
{
    Rigidbody2D body;
    Transform paddle;
    [SerializeField]
    float speed;
    [SerializeField]
    Transform explosionParticle;

    // Start is called before the first frame update
    void Start()
    {
        setUp();
    }

    // Update is called once per frame
    void Update()
    {
        switch(Engine.CurrentMode)
		{
            case Engine.Mode.Play:
                break;

            default:
                transform.position = new Vector2(paddle.position.x, paddle.position.y + .3f);   //keep the ball on the paddle and move it around

                if (Input.GetButtonDown("Jump"))
                {
                    Engine.ChangeMode(Engine.Mode.Play);
                    body.AddForce(Vector2.up * speed);
                }
                break;
		}
    }

    void setUp()
	{
        body = transform.GetComponent<Rigidbody2D>();
        paddle = GameObject.FindGameObjectWithTag("Paddle").transform;
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
		switch(other.tag)
		{
            case "Bottom":
                Debug.Log("LOSE A LIFE");
                body.AddForce(Vector2.zero);            //stop the force
                Engine.ChangeMode(Engine.Mode.Move);    //move it to the paddle
                break;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		switch(collision.gameObject.tag)
		{
            case "Brick":
                StartCoroutine(RemoveBrick(collision.transform));
                break;
		}
	}

    IEnumerator RemoveBrick(Transform brick)
	{
        Transform explosion = Instantiate(explosionParticle, brick.position, Quaternion.identity);
        Destroy(brick.gameObject);  //no more brick
        yield return new WaitForSeconds(2);
        Destroy(explosion.gameObject);
    }
}
