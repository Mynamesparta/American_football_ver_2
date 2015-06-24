using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System;

public class File_Input : MonoBehaviour {
	private File_Controller Fcon;
	private Animator anim;
	private Text text;
	private FileInfo file;
	void Awake()
	{
		Fcon = GameObject.FindGameObjectWithTag ("File_Controller").GetComponent<File_Controller>();
		anim = GetComponent<Animator> ();
		text = GetComponent<Text> ();
	}
	public void setToCurrentFile(bool b)
	{
		//print ("nice:"+b.ToString());
		anim.SetBool ("isActive", b);
		if (b) 
		{
			Fcon.setCurrent (this);
		}
	}
	public void setFile(FileInfo _file)
	{
		if(_file!=null)
		{
			file=_file;
			string name=file.Name;
			name=name.Replace(".txt"," ");
			//MonoBehaviour.print("name_of_file:"+name);
			//print(name);
			text.text=name;
		}
	}
	public bool setName(string name,DirectoryInfo direc)
	{
		name=name.Replace (" ", "_");
		if (file == null) 
		{
			file = new FileInfo (direc.FullName + "\\" + name + ".txt");
			if (!file.Exists) 
			{
				text.text = name;
				return true;
			} 
			return false;
		}
		else
		{
			FileInfo _file=new FileInfo (direc.FullName + "\\" + name + ".txt");
			print("_file:"+_file.FullName);
			if(!_file.Exists)
				file.MoveTo(_file.FullName);
			else
				return false;
			text.text = name;
			return true;
		}
	}
	public void Delete()
	{
		if (file != null&&file.Exists) 
		{
			file.Delete();
		}
	}
	public void Write(List<String> list)
	{
		StringBuilder text=new StringBuilder();
		for(int i=0;i<list.Count;i++)
		{
			text.AppendLine(list[i]);
		}
		if (file != null)
		{
			print (file.FullName);
			File.WriteAllText(file.FullName,text.ToString());
		}
	}
	public List<String> Read()
	{
		string[] text = File.ReadAllLines (file.FullName);
		List<String> list = new List<String> ();
		for(int i=0;i<text.Length;i++)
		{
			//MonoBehaviour.print(text[i]);
			list.Add(text[i]);
		}
		return list;
	}
	public string getName()
	{
		return text.text;
	}

}
