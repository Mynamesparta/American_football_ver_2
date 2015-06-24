using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Counter : MonoBehaviour {
	public Text text_counter_1;
	public Text text_counter_2;
	public LineRenderer Ball_Line;
	public Vector3[] pos_of_Ball_Line=new Vector3[2];
	private float real_X;
	private Game_Controller contr;
	//public float const_coef_Foot_to_world_Point;
	public Vector3 center
	{
		get{return new Vector3(real_X,0,0);}
	}

	public float float_counter_1
	{
		get{return _float_counter_1;}
		set
		{
			real_X=value;
			_float_counter_1=50-Mathf.Abs( value);//*const_coef_Foot_to_world_Point;
			text_counter_1.text=_float_counter_1.ToString();
			pos_of_Ball_Line[0].x=pos_of_Ball_Line[1].x=value;
			Ball_Line.SetPosition(0,pos_of_Ball_Line[0]);
			Ball_Line.SetPosition(1,pos_of_Ball_Line[1]);
		}
	}
	private  float some_coef;
	public float float_counter_2
	{
		get{return _float_counter_2;}
		set
		{
			_float_counter_2=Mathf.Abs(value)-_float_counter_1+50;//*const_coef_Foot_to_world_Point;
			text_counter_2.text=_float_counter_2.ToString();
		}
	}

	float _float_counter_1;
	float _float_counter_2;

	void Awake()
	{
		float_counter_1 = 0f;
		float_counter_2 = 0f;
		Strategy.Activities.counter = this;
		Strategy.Action.counter = this;
		Line.counter = this;
		contr = GameObject.FindWithTag (Tags.gameController).GetComponent < Game_Controller> ();
		contr.SCRIPTS.counter = this;
	}
	void Start()
	{
	}
}
