using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField]
    int points;
    [SerializeField]
    int totalHits;
    [SerializeField]
    Transform explosionParticle;
    [SerializeField]
    Sprite hitImage;

    GameManager manager;

    public int          Points      {  get { return points; } }
    public Transform    Explosion   {  get { return explosionParticle; } }
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit()
	{
        totalHits--;
        if(totalHits <= 0)
		{
            totalHits = 0;
            manager.BrickDestroyed(this.transform);
		}
        else
		{
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = hitImage;
		}
	}
}