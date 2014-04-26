using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LavaTile : MonoBehaviour {

	const float CastingCost = 0.2f;

	void OnMouseUpAsButton()
	{
		if (PlayerController.Current.MagicPower > CastingCost)
		{
			LavaFlow lf = (LavaFlow)Instantiate(PlayerController.Current.LavaFlowPrefab);
			lf.transform.position = this.transform.position;
			PlayerController.Current.MagicPower -= CastingCost;
		}
	}
}