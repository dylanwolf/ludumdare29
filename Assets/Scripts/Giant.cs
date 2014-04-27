using UnityEngine;
using System.Collections;

public class Giant : MonoBehaviour {

	public int RubyGoal = 1;
	public string NewLevel = "test";

	string message;
	void Awake()
	{
		if (RubyGoal == 1)
		{
			message = "If you want to pass, give me THE RUBY.";
		}
		else
		{
			message = string.Format("If you want to pass, give me {0} RUBIES.", RubyGoal);
		}
	}

	bool canInteract = true;
	const string PlayerTag = "Player";
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (PlayerController.Current.CurrentState == PlayerController.PlayerState.Transition)
			return;

		if (collision.collider.tag == PlayerTag)
		{
			if (PlayerController.Current.Rubies >= RubyGoal)
			{
				MessageWindow.Current.ShowWindow("Crunch, crunch, crunch, mmm, it tastes so sweet. Rubies are my favorite.", NewLevel);
				Soundboard.PlaySuccess();
			}
			else
			{
				MessageWindow.Current.ShowWindow(message, string.Empty);
				canInteract = false;
			}
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.collider.tag == PlayerTag)
			canInteract = true;
	}
}
