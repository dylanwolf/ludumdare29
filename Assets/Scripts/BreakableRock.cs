using UnityEngine;
using System.Collections;

public class BreakableRock : MonoBehaviour {

	const float CastingCost = 0.2f;
	public string Contains;
	
	void OnMouseDown()
	{
		if (PlayerController.Current.MagicPower < CastingCost)
			return;

		PlayerController.Current.MagicPower -= CastingCost;

		DestroyObject(gameObject);
		PlayerController.Current.CastAnimation();
		Soundboard.PlayRockBreak();

		Cloud c = (Cloud)Instantiate(PlayerController.Current.CloudPrefab);
		c.transform.position = this.transform.position;

		if (Contains == "Ruby")
		{
			Transform r = (Transform)Instantiate (PlayerController.Current.RubyPrefab);
			r.transform.position = this.transform.position;
		}
		else if (Contains == "Monster")
		{
			RockMonster r = (RockMonster)Instantiate (PlayerController.Current.MonsterPrefab);
			r.transform.position = this.transform.position;
		}
	}
}
