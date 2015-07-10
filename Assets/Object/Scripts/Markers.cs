using UnityEngine;
using System.Collections;

public class Markers : MonoBehaviour {

	private Game_Controller contr;
	private Counter counter;
	public float leftMarker
	{
		get
		{
			return transform.position.x-5;
		}
		set
		{
			transform.position=new Vector3(value+5,transform.position.y,transform.position.z);
		}
	}
	public float rightMarker
	{
		get
		{
			return transform.position.x+5;
		}
		set
		{
			transform.position=new Vector3(value-5,transform.position.y,transform.position.z);
		}
	}
	void Start () 
	{
		contr = Game_Controller.current_contr;
		counter = contr.SCRIPTS.counter;
		contr.EndDown += EndDown;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	public void EndDown()
	{
		MonoBehaviour.print ("hello!!!!!!!!!!!!!!!!!!!!!!!!!!");
		float line = counter.real_X;
		switch(contr.currentSide)
		{
		case Game_Controller.Side.Left:
		{
			if(line>rightMarker)
			{
				leftMarker=line;
			}
			break;
		}
		case Game_Controller.Side.Right:
		{
			if(line<leftMarker)
			{
				rightMarker=line;
			}
			break;
		}
		}
	}

}
