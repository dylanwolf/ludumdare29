using UnityEngine;
using System.Collections;

public class LifeMeter : MonoBehaviour {

	private tk2dTiledSprite meterSprite;
	
	public float MaxWidth = 500;
	
	private float lastLife = -1;
	
	void Awake () {
		meterSprite = GetComponent<tk2dTiledSprite>();
	}
	
	static Vector2 tmpDim;
	void Update()
	{
		if (PlayerController.Current != null)
		{
			if (PlayerController.Current.Life == lastLife)
				return;
			
			lastLife = PlayerController.Current.Life;
			tmpDim = meterSprite.dimensions;
			tmpDim.x = PlayerController.Current.Life * MaxWidth;
			if (tmpDim.x < 0) tmpDim.x = 0;
			meterSprite.dimensions = tmpDim;
		}
	}
}
