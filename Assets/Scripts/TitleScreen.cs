using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0))
		{
			Application.LoadLevel("Level1");
		}
	}
}
