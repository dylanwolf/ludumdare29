using UnityEngine;
using System.Collections;

[RequireComponent(typeof(tk2dTextMesh))]
public class RubyCount : MonoBehaviour {

	tk2dTextMesh textMesh;

	void Start () {
		textMesh = GetComponent<tk2dTextMesh>();
		textMesh.text = "0";
	}
	
	private int lastCount = 0;
	void Update () {
		if (PlayerController.Current.Rubies != lastCount)
		{
			textMesh.text = PlayerController.Current.Rubies.ToString();
			lastCount = PlayerController.Current.Rubies;
		}
	}
}
