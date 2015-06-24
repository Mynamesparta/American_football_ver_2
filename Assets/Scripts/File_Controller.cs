using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
public class File_Controller : MonoBehaviour {
	public GameObject clone_of_file_name;
	public Transform transform_list;
	public InputField name;
	public Scrollbar scrollbar;
	public float number_of_file_name;
	public float size_of_file_name;
	public float deltaRefresh=10f;
	public Animator anim;
	public Game_Controller contr;
	public Strategy strategy;

	private float size_of_Panel;
	private VerticalLayoutGroup verticalLG;
	private List<File_Input> list_of_file;
	private DirectoryInfo directory;
	private File_Input current;
	private float current_time;
	private bool refresh_time=true;
	private bool isFirst=true;
	void Awake()
	{
		MonoBehaviour.print ("Awake files");
		verticalLG = GetComponent<VerticalLayoutGroup> ();
		directory = new DirectoryInfo ("Assets//docs//Action_information");
		list_of_file = new List<File_Input> ();
		//StartCoroutine ("_RefreshFile");
		RefreshFile();
	}
	void Update()
	{
		if(false&&refresh_time)
		{
			current_time += Time.deltaTime;
			if(current_time>deltaRefresh)
			{
				current_time=0;
				RefreshFile();
			}
		}
		if(isFirst)
		{
			isFirst=false;
		}
	}
	IEnumerator _RefreshFile()
	{
		yield return new WaitForSeconds(2f);
		RefreshFile();
	}
	void Refresh()
	{
		float y = 0;
		for(int i=0;i<list_of_file.Count;i++)
		{
			y=-size_of_file_name*i;
			list_of_file[i].transform.localPosition=new Vector3(list_of_file[i].transform.localPosition.x,
			                                            y,
			                                            list_of_file[i].transform.localPosition.z);
		}
	}
	string getInputText()
	{
		string text = name.text;
		name.text = "";
		if (text == "")
			text = "None name";
		return text;
	}
	void size_of_listChanged()
	{
		scrollbar.size = number_of_file_name / (number_of_file_name > list_of_file.Count ? number_of_file_name : list_of_file.Count);
		size_of_Panel = list_of_file.Count * size_of_file_name;
		scrollbar.value = 0;
		Refresh ();
	}
	public void RefreshFile()
	{
		//MonoBehaviour.print ("Refresh File");
		/*/
		if (list_of_file.Count > 0)
			return;
		/*/
		//MonoBehaviour.print ("REfresh File");
		if (list_of_file == null)
			list_of_file = new List<File_Input> ();
		for(int i=0;i<list_of_file.Count;i++)
			Object.Destroy(list_of_file[i].gameObject);
		list_of_file.Clear();
		//MonoBehaviour.print (directory.FullName);
		if (!directory.Exists) 
		{
			directory.Create();
			MonoBehaviour.print("create new Folder");
		}
		FileInfo[] files = directory.GetFiles ("*.txt");
		File_Input file_name;
		//MonoBehaviour.print("Lenght:"+files.Length);
		for(int i=0 ; i<files.Length;i++)
		{
			MonoBehaviour.print("File:"+files[i].Name);
			file_name = (Object.Instantiate (clone_of_file_name)as GameObject).GetComponent<File_Input>();
			file_name.transform.SetParent (transform_list);
			file_name.transform.localPosition = new Vector3 (0, 0, 0);
			file_name.transform.localScale = new Vector3 (1, 1, 1);
			file_name.setFile(files[i]);
			list_of_file.Add(file_name);
		}
		size_of_listChanged();
	}
	public void Add()
	{
		if(!directory.Exists)
			directory.Create ();
		File_Input file_name = (Object.Instantiate (clone_of_file_name)as GameObject).GetComponent<File_Input>();
		file_name.transform.SetParent (transform_list);
		file_name.transform.localPosition = new Vector3 (0, 0, 0);
		file_name.transform.localScale = new Vector3 (1, 1, 1);
		if(file_name.setName (getInputText(),directory))
		{
			list_of_file.Insert (0,file_name);
			size_of_listChanged ();
			Save(file_name);
		}
		else
		{
			Object.Destroy(file_name.gameObject);
		}
	}
	public void scrollbarChanged()
	{
		float y = size_of_Panel * scrollbar.value *(1 - scrollbar.size);
		y = (y >= 0 ? y : 0);
		transform_list.localPosition = new Vector3 (transform_list.localPosition.x,
		                                        	y,
		                                         	transform_list.localPosition.z);
	}
	public void setCurrent(File_Input fi)
	{
		if (current != null)
			current.setToCurrentFile (false);
		current = fi;
	}
	public void deleteCurrent()
	{
		if (current == null)
			return;
		list_of_file.Remove (current);
		current.Delete ();
		Object.Destroy (current.gameObject);
		current = null;
		size_of_listChanged ();
	}	
	void Save(File_Input file_input)
	{
		file_input.Write (strategy.toString());
	}
	public void saveCurrent()
	{
		if(current!=null)
		{
			current.Write (strategy.toString());
		}
	}
	public void renameCurrent()
	{
		if(current!=null)
		{
			string name = getInputText ();
			current.setName(name,directory);
		}
	}
	public void setActive()
	{
		bool b = !anim.GetBool ("inScene");
		anim.SetBool("inScene",b);
	}
	public void _setActive(bool b)
	{
		gameObject.SetActive (b);
	}	
	public void  readCurrent()
	{
		if(current!=null)
		{
			strategy.fromString(current.Read());
		}
	}
	public void Read(string name)
	{
		foreach(File_Input file in list_of_file)
		{
			//print(file.getName().Replace(" ","")+"=="+name);
			if(file.getName().Replace(" ","")==name)
			{
				strategy.fromString(file.Read());
				break;
			}
		}
	}

}
