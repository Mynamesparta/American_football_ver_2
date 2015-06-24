using UnityEngine;
using System.Collections;

public class Vector : MonoBehaviour {
	public SpriteRenderer sprite;
	public static Game_Controller contr;
	public static Strategy strategy;
	bool BuildTime=false;
	Vector3 startPoint;
	Vector3 vector;
	public void setColor(Color col)
	{
		sprite.color = col;
	}
	public void setBuildTime(bool b)
	{
		BuildTime = b;
	}

	public void setStartPoint(Vector3 vec)
	{
		startPoint = vec;
		transform.position = vec;
	}
	public Vector3 getVector()
	{
		return vector;
	}
	public void setVector(Vector3 vec)
	{
		vector = vec;
		transform.LookAt (startPoint+vec);
	}
	float timer;
	void Update()
	{
		if (BuildTime)
		{
			if (Input.GetMouseButtonDown (0)) {
				timer = 0;
			}
		
			if (Input.GetMouseButton (0)) {
				if (timer >= strategy.OPTIONS.TimePause_before_paint) {
					Vector3 vec = contr.getCurrentMousePosition ();
					vector = vec - startPoint;
					transform.LookAt (vec);
					timer = 0;
				} else {
					timer += Time.deltaTime;
				}
			}
		}
	}

}
