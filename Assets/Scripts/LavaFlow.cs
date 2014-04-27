using UnityEngine;
using System.Collections;
using System.Collections;

public class LavaFlow : MonoBehaviour {

	const float MaxTimer = 1.0f;
	float timer = 0;

	// Use this for initialization
	void Start () {
		timer = MaxTimer;
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerController.Current.CurrentState == PlayerController.PlayerState.Paused)
			return;

		timer -= Time.deltaTime;
		if (timer < 0)
		{
			DestroyObject(this.gameObject);
		}
	}	
}
