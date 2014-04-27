using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

	float Timer;
	float MaxTimer = 0.25f;

	void Awake()
	{
		Timer = MaxTimer;
	}
	
	// Update is called once per frame
	void Update () {
		Timer -= Time.deltaTime;
		if (Timer <= 0)
		{
			DestroyObject(gameObject);
		}
	}
}
