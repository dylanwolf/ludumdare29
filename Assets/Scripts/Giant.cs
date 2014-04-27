using UnityEngine;
using System.Collections;

public class Giant : MonoBehaviour {

	const string PlayerTag = "Player";
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.tag == PlayerTag)
			MessageWindow.Current.ShowWindow("If you want to pass, give me X RUBIES.", string.Empty);
	}
}
