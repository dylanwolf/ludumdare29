using UnityEngine;
using System.Collections;

public class ObjectLayerFader : MonoBehaviour {

	SpriteRenderer[] sprites;
	BoxCollider2D[] colliders;
	Color tmpColor = new Color(1, 1, 1, 1);
	
	void Awake()
	{
		sprites = transform.GetComponentsInChildren<SpriteRenderer>();
		colliders = transform.GetComponentsInChildren<BoxCollider2D>();
	}
	
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
				}
			}
		}
	}
}
