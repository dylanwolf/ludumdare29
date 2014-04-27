using UnityEngine;
using System.Collections;

public class RockMonsterSpawner : MonoBehaviour {

	void Start()
	{
		RockMonster rm = (RockMonster)Instantiate(PlayerController.Current.MonsterPrefab);
		rm.transform.position = transform.position;
		rm.transform.parent = transform.parent;

		if (transform.parent.name.StartsWith("Underworld"))
		{
			rm.gameObject.layer = 11;
		}

		transform.parent.gameObject.GetComponent<ObjectLayerFader>().ResetObjects = true;

		DestroyObject(this.gameObject);
	}
}
