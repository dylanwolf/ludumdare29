using UnityEngine;
using System.Collections;

public class TileLayerFader : MonoBehaviour {

	MeshRenderer[] tileMeshes;
	Color tmpColor = new Color(1, 1, 1, 1);

	void Awake()
	{
		tileMeshes = transform.GetComponentsInChildren<MeshRenderer>();
	}

	public void SetOpacity(float opacity)
	{
		tmpColor.a = opacity;
		for (int i = 0; i < tileMeshes.Length; i++)
		{
			tileMeshes[i].sharedMaterial.color = tmpColor;
		}
	}
}
