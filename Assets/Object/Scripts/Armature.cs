using UnityEngine;
using System.Collections;
public class Armature : MonoBehaviour {

	PlayerMovement player;
	void Start () 
	{
		player = GetComponentInParent<PlayerMovement> ();
	}
	public bool itsMyTeam(PlayerMovement.Team team)
	{
		return player.team == team;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
