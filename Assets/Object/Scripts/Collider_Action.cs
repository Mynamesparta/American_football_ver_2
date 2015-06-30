using UnityEngine;
using System.Collections;

public class Collider_Action : MonoBehaviour 
{
	public delegate void Trigger(Collider other);
	public event Trigger Stay;
	public event Trigger Exit;
	public event Trigger Enter;
	//public event Trigger 
	bool Wellcome=true;
	//void OnTrigger
	//
	void OnTriggerStay(Collider other)
	{
		if(false&&Wellcome)
		{
			Wellcome=false;
			MonoBehaviour.print("Stay_"+name+":"+other.name);
			MonoBehaviour.print("tag:"+other.tag);
			MonoBehaviour.print((other.tag == Tags.football_field).ToString());
		}
		if (other.tag == Tags.football_field)
			return;
		if(Stay!=null)
		{
			Stay(other);
		}
	}
	Collider ok;
	void OnTriggerExit(Collider other)
	{
		Wellcome = true;
		if (other.tag == Tags.football_field)
			return;
		//MonoBehaviour.print ("Exit_"+name);
		if(Exit!=null)
		{
			Exit(other);
		}
	}
	void OnTriggerEnter( Collider other)
	{
		
		if (other.tag == Tags.football_field)
			return;
		if(Enter!=null)
		{
			Enter(other);
		}
	}
	//
}
