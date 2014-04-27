using UnityEngine;
using System.Collections;

public class ObjectLayerFader : MonoBehaviour {

	SpriteRenderer[] sprites;
	BoxCollider2D[] colliders;
	Color tmpColor = new Color(1, 1, 1, 1);

	public bool ResetObjects = false;

	public void SetOpacity(float opacity)
	{
		tmpColor.a = opacity;
		for (int i = 0; i < sprites.Length; i++)
		{
			sprites[i].color = tmpColor;
		}
	}

	const int DefaultLayer = 0;
	const int OtherWorldLayer = 11;
	public void SetActive(bool active, bool disable)
	{
		if (sprites == null || ResetObjects)
		{
			sprites = transform.GetComponentsInChildren<SpriteRenderer>();
		}
		if (colliders == null || ResetObjects)
		{
			colliders = transform.GetComponentsInChildren<BoxCollider2D>();
		}
		ResetObjects = false;

		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i] != null)
			{
				if (disable)
				{
					colliders[i].enabled = active;
				}
				else
				{
					colliders[i].gameObject.layer = active ? DefaultLayer : OtherWorldLayer;
					colliders[i].enabled = false;
					colliders[i].enabled = true;
				}
			}
		}
	}
}
