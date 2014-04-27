using UnityEngine;
using System.Collections;

public class MessageWindow : MonoBehaviour {
	
	public string LevelChange;

	private tk2dTextMesh textMesh;

	public static MessageWindow Current;

	void Awake()
	{
		textMesh = GetComponentInChildren<tk2dTextMesh>();
		Current = this;
		ToggleSelfAndChildren(false);
	}

	private Renderer[] renderers;
	void ToggleSelfAndChildren(bool state)
	{
		if (renderers == null)
		{
			renderers = GetComponentsInChildren<Renderer>();
		}
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].enabled = state;
		}
	}

	private PlayerController.PlayerState lastState;
	public void ShowWindow(string message, string levelChange)
	{
		textMesh.text = message.ToUpper();
		ToggleSelfAndChildren(true);
		LevelChange = levelChange;
		lastState = PlayerController.Current.CurrentState;
		PlayerController.Current.CurrentState = PlayerController.PlayerState.Paused;
	}

	void LateUpdate()
	{
		if (!renderer.enabled)
			return;

		if (Input.GetButtonUp("Jump"))
		{
			ToggleSelfAndChildren(false);
			if (!string.IsNullOrEmpty(LevelChange))
			{
				Application.LoadLevel(LevelChange);
			}
			PlayerController.Current.CurrentState = lastState;
		}
	}
}
