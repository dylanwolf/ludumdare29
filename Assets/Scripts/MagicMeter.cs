using UnityEngine;
using System.Collections;

[RequireComponent(typeof(tk2dTiledSprite))]
public class MagicMeter : MonoBehaviour {

	private tk2dTiledSprite meterSprite;

	public float MaxWidth = 500;

	private float lastMagicPower = 0;

	void Awake () {
		meterSprite = GetComponent<tk2dTiledSprite>();
	}

	static Vector2 tmpDim;
	void Update()
	{
		if (PlayerController.Current != null)
		{
			if (PlayerController.Current.MagicPower == lastMagicPower)
				return;

			lastMagicPower = PlayerController.Current.MagicPower;
			tmpDim = meterSprite.dimensions;
			tmpDim.x = PlayerController.Current.MagicPower * MaxWidth;
			if (tmpDim.x < 0) tmpDim.x = 0;
			meterSprite.dimensions = tmpDim;
		}
	}
}
