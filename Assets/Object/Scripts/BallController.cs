using UnityEngine;
using System.Collections;

[System.Serializable]
public enum State_of_Ball {Player,Ground,Air};

public class BallController : MonoBehaviour {
	public GameObject mainCamera;
	public float speed;
	public State_of_Ball state=State_of_Ball.Player;
	public Counter counter;

	
	private Transform position;
	private Transform rotation;
	private float startTime;
	private float journeyLength;
	private Vector3 Forse;
	private Rigidbody rigidbody;
	void Awake()
	{
		GetComponent<CapsuleCollider> ().isTrigger = true;
		rigidbody = GetComponent<Rigidbody> ();
		//mainCamera=GameObject.FindGameObjectWithTag(Tags.camera);
	}
	public void SetPosition(Transform _position)
	{
		position = _position;
	}
	public void SetRotation(Transform _rotation)
	{
		rotation = _rotation;
	}
	void LateUpdate()
	{
		switch(state)
		{
		case State_of_Ball.Player:
		{
			if(position==null||rotation==null)
				return;
			if (Vector3.Distance (transform.position, position.position) < speed * Time.deltaTime) 
			{
				transform.position = position.position;
			}
			else
			{
				startTime = Time.time;
				journeyLength=Vector3.Distance (transform.position, position.position);
				transform.position = Vector3.Lerp(transform.position,position.position,
				                                  Time.deltaTime*speed/journeyLength);
			}

			if(true)//position.name=="Ball_1")
				transform.rotation=rotation.rotation;
			else
				transform.rotation=position.rotation;
			break;
		}
		case State_of_Ball.Ground:
		{
			break;
		}
		case State_of_Ball.Air:
		{
			break;
		}
		}
		counter.float_counter_2 = transform.position.x;

	}
	public void Pass(Vector3 speed)
	{
		rigidbody.isKinematic = false;
		//GetComponent<Rigidbody>().AddForce (Forse, ForceMode.Impulse);
		rigidbody.velocity = speed;
		state = State_of_Ball.Air;
		GetComponent<CapsuleCollider> ().isTrigger = false;
	}
	public State_of_Ball getState()
	{
		return state;
	}
	public void Take()
	{
		state = State_of_Ball.Player;
	}

}
