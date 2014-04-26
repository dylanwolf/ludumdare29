using UnityEngine;
using System.Collections;
using System.Collections;

public class LavaFlow : MonoBehaviour {

	const float MaxTimer = 2.0f;
	float timer = 0;

	// Use this for initialization
	void Start () {
		timer = MaxTimer;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer < 0)
		{
			DestroyObject(this.gameObject);
		}
	}
}
