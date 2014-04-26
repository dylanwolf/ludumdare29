using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public static PlayerController Current;

	public TileLayerFader OverworldTileFader;
	public ObjectLayerFader OverworldObjectFader;
	public ObjectLayerFader UnderworldObjectFader;

	public LavaFlow LavaFlowPrefab;

	public float Speed = 3.0f;

	public float MagicPower = 0f;
	const float OverworldRegen = 0.025f;
	const float UnderworldRegen = 0.1f;
	const float CastingCost_Transfer = 0.25f;
	const float MagicDamage_Lava = 0.5f;

	public enum LocationTypes
	{
		Overworld,
		Underworld
	}

	public enum PlayerState
	{
		OnLevel,
		Arrived,
		Transition
	}

	public LocationTypes CurrentLocation;
	public PlayerState CurrentState;
	float TransitionTimer;
	const float TransitionTimerMax = 1;
	float DamageTimer;
	const float DamageTimerMax = 5.0f;

	// Use this for initialization
	void Start () {
		ResetLevel();
		Current = this;
	}

	void ResetLevel()
	{
		MagicPower = 0.25f;
		CurrentLocation = LocationTypes.Overworld;
		CurrentState = PlayerState.OnLevel;
		UnderworldObjectFader.SetActive(false);
		OverworldObjectFader.SetActive(true);
		OverworldObjectFader.SetOpacity(1);
		OverworldTileFader.SetOpacity(1);
	}

	private Vector2 tmpForce;
	private float tmpOpacity;
	void FixedUpdate()
	{
		if (CurrentState == PlayerState.OnLevel)
		{
			// Move the character
			tmpForce.x = Input.GetAxis("Horizontal");
			tmpForce.y = Input.GetAxis("Vertical");
			rigidbody2D.velocity = (tmpForce * Speed);
		}
		else
		{
			rigidbody2D.velocity = Vector2.zero;
		}
	}

	void Update () {
		if (DamageTimer > 0)
		{
			DamageTimer -= Time.deltaTime;
		}

		if (CurrentState == PlayerState.OnLevel)
		{
			MagicPower += (CurrentLocation == LocationTypes.Overworld ? OverworldRegen : UnderworldRegen) * Time.deltaTime;
			if (MagicPower > 1) { MagicPower = 1; }

			// Switch levels
			if (Input.GetKeyUp (KeyCode.Return))
			{
				if (MagicPower > CastingCost_Transfer)
				{
					UnderworldObjectFader.SetActive(false);
					OverworldObjectFader.SetActive(false);
					CurrentState = PlayerState.Transition;
					Debug.Log (CurrentState);
					TransitionTimer = TransitionTimerMax;
					MagicPower -= CastingCost_Transfer;
				}
			}
		}
		else if (CurrentState == PlayerState.Transition)
		{
			TransitionTimer -= Time.deltaTime;
			tmpOpacity = TransitionTimer/TransitionTimerMax;
			OverworldTileFader.SetOpacity((CurrentLocation == LocationTypes.Underworld) ? (1-tmpOpacity) : tmpOpacity);
			OverworldObjectFader.SetOpacity((CurrentLocation == LocationTypes.Underworld) ? (1-tmpOpacity) : tmpOpacity);

			if (TransitionTimer <= 0)
			{
				CurrentState = PlayerState.Arrived;
				Debug.Log (CurrentState);
			}
		}
		else if (CurrentState == PlayerState.Arrived)
		{
			CurrentLocation = (CurrentLocation == LocationTypes.Overworld) ? LocationTypes.Underworld : LocationTypes.Overworld;
			UnderworldObjectFader.SetActive(CurrentLocation == LocationTypes.Underworld);
			OverworldObjectFader.SetActive(CurrentLocation == LocationTypes.Overworld);
			CurrentState = PlayerState.OnLevel;
			Debug.Log (CurrentState);
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		Debug.Log ("Collided with "+ collider.name);
	}

	private string LavaTag = "Lava";
	void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.tag == LavaTag && DamageTimer <= 0)
		{
			MagicPower -= MagicDamage_Lava;
			DamageTimer = DamageTimerMax;
			Debug.Log (string.Format("Magic: {0} Damage Timer: {1}", MagicPower, DamageTimer));
		}
	}
}
