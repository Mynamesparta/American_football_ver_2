using UnityEngine;
using System.Collections;
using System;
[System.Serializable]
public enum name_of_Button{Counter=0,Menu=1,Editor=2,Editor_Panel=3,FileList=4,File_Panel=5};
public class Canvas : MonoBehaviour {

	public Options_In_Scene IN_SCENE;
	public Animator[] anim;
	public Animator[] anim_recorder;
	private bool[] editTime;

	void Awake()
	{
		anim = GetComponentsInChildren<Animator> ();
		editTime=new bool[anim.Length];
		for(int i=0;i<editTime.Length;i++)
		{
			editTime[i]=false;
		}
		inScene (name_of_Button.FileList, IN_SCENE.File_list);
		inScene (name_of_Button.Counter, IN_SCENE.Counter);
		inScene (name_of_Button.Editor, IN_SCENE.Editor);
		inScene (name_of_Button.Menu, IN_SCENE.Menu);
	}
	int getNumber_of_Button(name_of_Button name)
	{
		return (int)name;
		print ("something wrong:name of Button null");
		return -1;
	}
	public name_of_Button fromStringToButton(string _name)
	{
		return (name_of_Button)Enum.Parse (typeof(name_of_Button), _name);
	}
	public void Edit(name_of_Button name)
	{
		int index = getNumber_of_Button (name);
		if (editTime[index]) 
		{
			editTime[index]=false;
			anim[index].SetBool("isActive",false);
		}
		else
		{
			editTime[index]=true;
			anim[index].SetBool("isActive",true);
		}
	}
	public void Edit(name_of_Button name,bool b)
	{
		int index=getNumber_of_Button(name);
		editTime[index]=b;
		anim[index].SetBool("isActive",b);
	}
	public void Edit(string name)
	{
		int index = getNumber_of_Button (fromStringToButton(name));
		if (editTime[index]) 
		{
			editTime[index]=false;
			anim[index].SetBool("isActive",false);
		}
		else
		{
			editTime[index]=true;
			anim[index].SetBool("isActive",true);
		}
	}
	public void Edit(string name,bool b)
	{
		int index=getNumber_of_Button(fromStringToButton(name));
		editTime[index]=b;
		anim[index].SetBool("isActive",b);
	}
	public void inScene(name_of_Button name,bool set)
	{
		MonoBehaviour.print (name.ToString()+" inScene:" + set.ToString ());
		int index=getNumber_of_Button(name);
		if(set)
		{
			anim[index].gameObject.SetActive(true);
		}
		anim [index].SetBool ("inScene", set);//!anim [index].GetBool ("inScene"));
		if(!set)
		{
			anim[index].gameObject.SetActive(false);
		}

	}
	public Animator getAnimator(name_of_Button name)
	{
		int index = getNumber_of_Button (name);
		return anim [index];
	}
	void LateUpdate()
	{
	}
	//===============================struct======================
	[System.Serializable]
	public struct Options_In_Scene
	{
		public bool File_list;
		public bool Counter;
		public bool Editor;
		public bool Menu;
	}

}
