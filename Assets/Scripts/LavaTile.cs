using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LavaTile : MonoBehaviour {

	const float CastingCost = 0.2f;

	Vector3 tmp;
	void OnMouseDown()
	{
		if (PlayerController.Current.MagicPower > CastingCost)
		{
			LavaFlow lf = (LavaFlow)Instantiate(PlayerController.Current.LavaFlowPrefab);
			//lf.transform.position = this.transform.position;
			tmp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			tmp.z = transform.position.z;
			lf.transform.position = tmp;
		

			PlayerController.Current.MagicPower -= CastingCost;
			Soundboard.PlayLava();
			PlayerController.Current.CastAnimation();
		}
	}
}