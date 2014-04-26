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

	public void SetActive(bool active)
	{
		for (int i = 0; i < colliders.Length; i++)
		{
			colliders[i].enabled = active;
		}
	}
}
