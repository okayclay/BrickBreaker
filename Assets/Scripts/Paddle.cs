using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField]    //so we can change it in the inspector
    float speed;
    [SerializeField]
    float rightBound, leftBound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");                             //get input from the input manager every frame
        transform.Translate(Vector2.right * horizontal * Time.deltaTime * speed);   //move the paddle in the direction that we pressed
        CheckBoundary();
    }
    /// <summary>
    /// Stops the paddle from going off screen
    /// </summary>
    void CheckBoundary()
	{
        if(transform.position.x >= rightBound)
		{
            transform.position = new Vector2( rightBound, transform.position.y);
		}
        else if (transform.position.x <= leftBound)
		{
            transform.position = new Vector3(leftBound, transform.position.y);
		}
	}
}
