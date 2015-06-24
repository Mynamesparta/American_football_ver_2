using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Line : MonoBehaviour 
{
	public List<Vector3> list_of_points;
	public static Game_Controller contr;
	public static Strategy strategy;
	public static Counter counter;
	Vector3 startPoint;
	LineRenderer line_renderer;
	bool BuildTime=false;
	bool ViewTime=true;
	public void Awake()
	{
		list_of_points = new List<Vector3> ();
		line_renderer = GetComponent<LineRenderer> ();
	}
	public void setColor(Color col)
	{
		line_renderer.SetColors (col, col);
	}
	public void BuildLine(bool b)
	{
		if(!b)
		{
			line_renderer.SetVertexCount(0);
		}
		else
		{
			line_renderer.SetVertexCount(list_of_points.Count+1);
			line_renderer.SetPosition(0,startPoint);
			for(int i=0;i<list_of_points.Count;i++)
			{
				line_renderer.SetPosition(i+1,list_of_points[i]+startPoint);
			}
		}
	}
	public void Add(Vector3 vec)
	{
		list_of_points.Add (vec-startPoint);
		if(BuildTime)
		{
			line_renderer.SetVertexCount(list_of_points.Count+1);
			line_renderer.SetPosition(list_of_points.Count,vec);
		}
		lastPos=currentPos;
	}
	public void RemoveLast()
	{
		if (list_of_points.Count == 0)
			return;
		list_of_points.RemoveAt (list_of_points.Count - 1);
		line_renderer.SetVertexCount (list_of_points.Count+1);
		lastPos=LastPoint;
	}
	public void setStartPoint(Vector3 vec)
	{
		startPoint = vec;
		line_renderer.SetVertexCount (1);
		line_renderer.SetPosition (0, startPoint);
	}
	public List<string> toString()
	{
		List<string> list = new List<string> ();
		list.Add (startPoint.ToString ());
		for(int i=0;i<list_of_points.Count;i++)
		{
			list.Add(list_of_points[i].ToString());
		}
		return list;
	}
	public void fromString(List<string> list)
	{
		startPoint = fromString_to_Vector (list [0])+counter.center;
		list_of_points.Clear ();
		for(int i=1;i<list.Count;i++)
		{
			list_of_points.Add(fromString_to_Vector(list[i]));
		}
		if(ViewTime)
		{
			BuildLine(true);
		}

	}
	public static Vector3 fromString_to_Vector(string text)
	{
		text = text.Remove (0, 1);
		text = text.Remove (text.Length-1, 1);
		string[] textVec = text.Split (",".ToCharArray());
		return new Vector3 (float.Parse (textVec [0]), float.Parse (textVec [1]), float.Parse (textVec [2]));
	}
	public void setBuildTime(bool b)
	{
		BuildTime = b;
		if(b)
		{
			lastPos=LastPoint;
		}
	}
	public Vector3 LastPoint
	{
		get
		{
			return list_of_points.Count == 0 ? startPoint : (list_of_points [list_of_points.Count - 1] + startPoint);
		}
	}
	public Vector3 BeginPoint
	{
		get
		{
			return startPoint;
		}
	}
	public Vector3 getVec(int Index)
	{
		if (Index == 0)
			return startPoint;
		return list_of_points [Index - 1] + startPoint;
	}
	public int Count
	{
		get{return 1+list_of_points.Count;}
	}
	Vector3 lastPos;
	Vector3 currentPos;
	float timer;
	public void Update()
	{
		if(BuildTime)
		{
			if(Input.GetMouseButtonDown(0))
			{
				timer=0;
			}

			if(Input.GetMouseButton(0))
			{
				if(timer>=strategy.OPTIONS.TimePause_before_paint)
				{
					currentPos=contr.getCurrentMousePosition();
					currentPos.y=0;
					//MonoBehaviour.print(currentPos.ToString());
					//return;
					if(Vector3.Distance(lastPos,currentPos)>strategy.OPTIONS.Lenght_of_line)
					{
						Add(currentPos);
					}
				}
				else
				{
					timer+=Time.deltaTime;
				}
			}

			if(Input.GetMouseButton(1))
			{
				currentPos=contr.getCurrentMousePosition();
				currentPos.y=0;
				if(Vector3.Distance(lastPos,currentPos)<strategy.OPTIONS.Zone_of_remove_point)
				{
					RemoveLast();
				}
			}
		}
	}
}
