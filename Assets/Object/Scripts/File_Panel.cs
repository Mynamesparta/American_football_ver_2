using UnityEngine;
using System.Collections;

public class File_Panel : MonoBehaviour {
	public File_Controller file_controller;
	public float timePauseBeforeRefresh;

	public void updateFiles()
	{
		for_timer = true;
		//MonoBehaviour.print ("Update Files");
	}
	float timer=0;
	bool for_timer;
	void Update()
	{
		if(for_timer)
		{
			if(timer>=timePauseBeforeRefresh)
			{
				timer = 0;
				for_timer=false;
				file_controller.RefreshFile ();
				MonoBehaviour.print("Hello Refresh");
			}
			else
			{
				MonoBehaviour.print(timer+"<"+timePauseBeforeRefresh);
				timer+=Time.deltaTime;
			}
		}
	}
}
