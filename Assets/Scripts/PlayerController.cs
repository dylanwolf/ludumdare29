using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public Cloud CloudPrefab;
	public Transform RubyPrefab;
	public RockMonster MonsterPrefab;

	public static PlayerController Current;

	SpriteRenderer renderer;
	public TileLayerFader OverworldTileFader;
	public ObjectLayerFader OverworldObjectFader;
	public ObjectLayerFader UnderworldObjectFader;

	public AudioSource OverworldMusic;
	public AudioSource UnderworldMusic;

	Vector3 lastPosition = Vector3.zero;
	Animator anim;

	public int Rubies = 0;

	float PushbackTimer;
	const float MaxPushbackTimer = 0.25f;
	const float PushbackAmount = 10.0f;
	Vector2 pushbackDirection;

	public LavaFlow LavaFlowPrefab;

	public float Speed = 3.0f;

	public float Life = 1.0f;
	const float OverworldLifeRegen = 0.05f;
	const float LifeDamage_Lava = 0.25f;
	const float LifeDamage_Monster = 0.25f;

	public float MagicPower = 0f;
	const float OverworldMagicRegen = 0.025f;
	const float UnderworldMagicRegen = 0.1f;
	const float CastingCost_Transfer = 0.1f;
	const float MagicDamage_Lava = 0.1f;
	const float MagicDamage_Monster = 0.25f;

	Color tmpColor;
	float minNonRedDamage = 0.5f;
	float minOpacityDamage = 0.75f;



	public enum LocationTypes
	{
		Overworld,
		Underworld
	}

	public enum PlayerState
	{
		OnLevel,
		Arrived,
		ArrivedAndSet,
		Transition,
		Paused
	}

	const string AnimParam_Speed = "Speed";
	const string AnimParam_Jump = "Jumping";
	const string AnimParam_Cast = "Casting";

	public LocationTypes CurrentLocation;
	public PlayerState CurrentState;
	float TransitionTimer;
	const float TransitionTimerMax = 1;
	float DamageTimer;
	const float DamageTimerMax = 1.0f;

	float CastTimer;
	const float CastTimerMax = 0.5f;

	// Use this for initialization
	void Awake () {
		anim = GetComponent<Animator>();
		renderer = GetComponent<SpriteRenderer>();
		Current = this;

		foreach (UTiledLayerSettings uls in FindObjectsOfType(typeof(UTiledLayerSettings)))
        {
			uls.gameObject.SetActive(true);
			if (uls.gameObject.name == "Overworld")
			{
				OverworldTileFader = uls.GetComponent<TileLayerFader>();
			}
			if (uls.gameObject.name == "Underworld Objects")
			{
				UnderworldObjectFader = uls.GetComponent<ObjectLayerFader>();
			}
			if (uls.gameObject.name == "Overworld Objects")
			{
				OverworldObjectFader = uls.GetComponent<ObjectLayerFader>();
			}
		}

	}
	void Start() {
		ResetLevel();
	}

	void Crossfade(float overworldAmt)
	{
		OverworldMusic.volume = overworldAmt;
		UnderworldMusic.volume = 1 - overworldAmt;
	}

	void ResetLevel()
	{
		Rubies = 0;
		Life = 1.0f;
		MagicPower = 0.25f;
		CurrentLocation = LocationTypes.Overworld;
		CurrentState = PlayerState.OnLevel;
		UnderworldObjectFader.SetActive(false, false);
		OverworldObjectFader.SetActive(true, true);
		OverworldObjectFader.SetOpacity(1);
		OverworldTileFader.SetOpacity(1);
		Crossfade(1);
	}

	private bool forcedTransition = false;

	private Vector2 lastMove;
	private Vector2 tmpForce;
	private float tmpOpacity;
	private Vector3 tmpScale;
	public void CastAnimation()
	{
		CastTimer = CastTimerMax;
		anim.SetBool(AnimParam_Cast, true);
	}

	void Update()
	{
		if (CurrentState == PlayerState.OnLevel)
		{
			// Switch levels
			if (Input.GetButtonUp("Jump") && CastTimer <= 0)
			{
				if (MagicPower > CastingCost_Transfer)
				{
					UnderworldObjectFader.SetActive(false, false);
					OverworldObjectFader.SetActive(false, true);
					CurrentState = PlayerState.Transition;
					Soundboard.PlayTransition();
					TransitionTimer = TransitionTimerMax;
					MagicPower -= CastingCost_Transfer;
					anim.SetBool(AnimParam_Jump, true);
				}
			}
		}
	}

	private float tmpTween;
	void FixedUpdate () {
		if (CurrentState == PlayerState.OnLevel && CastTimer <= 0 && PushbackTimer <= 0)
		{
			// Move the character
			tmpForce.x = Input.GetAxis("Horizontal");
			tmpForce.y = Input.GetAxis("Vertical");
			lastPosition = transform.position;
			rigidbody2D.velocity = (tmpForce * Speed);
			if (tmpForce.x != 0)
			{
				tmpScale = transform.localScale;
				tmpScale.x = -Mathf.Sign (tmpForce.x);
				if (Mathf.Sign (tmpScale.x) != Mathf.Sign (transform.localScale.x))
				{
					transform.localScale = tmpScale;
					Camera.main.transform.localScale = tmpScale;
				}
			}
			if (tmpForce.magnitude > 0)
			{
				lastMove = tmpForce;
			}
			anim.SetFloat(AnimParam_Speed, tmpForce.magnitude);
		}
		else
		{
			rigidbody2D.velocity = Vector2.zero;
			anim.SetFloat(AnimParam_Speed, 0);
		}
		
		if (PushbackTimer > 0 && CurrentState != PlayerState.Paused)
		{
			PushbackTimer -= Time.fixedDeltaTime;
			if (PushbackTimer > 0)
			{
				rigidbody2D.velocity -= (pushbackDirection * Mathf.Sin ((Mathf.PI/2) * (PushbackTimer/MaxPushbackTimer)) * PushbackAmount);
			}
		}

		if (CurrentState == PlayerState.Paused)
			return;

		if (Life <= 0)
		{
			MessageWindow.Current.ShowWindow("Perished...", Application.loadedLevelName);
			Soundboard.PlayFailure();
		}

		if (DamageTimer > 0)
		{
			DamageTimer -= Time.fixedDeltaTime;

			if (DamageTimer > 0)
			{
				tmpColor = Color.white;
				tmpTween = (Mathf.Sin (24*Mathf.PI * (DamageTimer/DamageTimerMax))+1)/2;
				tmpColor.a = ((1 - minOpacityDamage) * tmpTween) + minOpacityDamage;
				tmpColor.b = ((1 - minNonRedDamage) * tmpTween) + minOpacityDamage;
				tmpColor.g = ((1 - minNonRedDamage) * tmpTween) + minOpacityDamage;
				renderer.color = tmpColor;
			}
			else
			{
				renderer.color = Color.white;
			}
		}

		if (CastTimer > 0)
		{
			CastTimer -= Time.fixedDeltaTime;
			if (CastTimer <= 0)
			{
				anim.SetBool(AnimParam_Cast, false);
			}
		}

		if (CurrentState == PlayerState.OnLevel)
		{
			MagicPower += (CurrentLocation == LocationTypes.Overworld ? OverworldMagicRegen : UnderworldMagicRegen) * Time.fixedDeltaTime;
			Life += (CurrentLocation == LocationTypes.Overworld ? OverworldLifeRegen : 0) * Time.fixedDeltaTime;
			if (MagicPower > 1) { MagicPower = 1; }
			if (Life > 1) { Life = 1; }
		}
		else if (CurrentState == PlayerState.Transition)
		{
			TransitionTimer -= Time.fixedDeltaTime;
			tmpOpacity = TransitionTimer/TransitionTimerMax;
			Crossfade ((CurrentLocation == LocationTypes.Underworld) ? (1-tmpOpacity) : tmpOpacity);
			OverworldTileFader.SetOpacity((CurrentLocation == LocationTypes.Underworld) ? (1-tmpOpacity) : tmpOpacity);
			OverworldObjectFader.SetOpacity((CurrentLocation == LocationTypes.Underworld) ? (1-tmpOpacity) : tmpOpacity);

			if (TransitionTimer <= 0)
			{
				CurrentState = PlayerState.Arrived;
				CurrentLocation = (CurrentLocation == LocationTypes.Overworld) ? LocationTypes.Underworld : LocationTypes.Overworld;
				UnderworldObjectFader.SetActive(CurrentLocation == LocationTypes.Underworld, false);
				OverworldObjectFader.SetActive(CurrentLocation == LocationTypes.Overworld, true);
				anim.SetBool(AnimParam_Jump, false);
			}
		}
		else if (CurrentState == PlayerState.Arrived)
		{
			CurrentState = PlayerState.ArrivedAndSet;
		}
		else if (CurrentState == PlayerState.ArrivedAndSet)
		{
			CurrentState = PlayerState.OnLevel;
			forcedTransition = false;
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == RubyTag)
		{
			Rubies += 1;
			DestroyObject(collider.gameObject);
			Soundboard.PlayRubyPickup();
		}
	}

	const string MonsterTag = "Monster";
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (CurrentState == PlayerState.Transition)
			return;

		Debug.Log(collision.collider.tag);

		if ((CurrentState == PlayerState.Arrived || CurrentState == PlayerState.ArrivedAndSet) && !forcedTransition)
		{
			UnderworldObjectFader.SetActive(false, false);
			OverworldObjectFader.SetActive(false, true);
			CurrentState = PlayerState.Transition;
			TransitionTimer = TransitionTimerMax;
			Soundboard.PlayTransition();
			anim.SetBool(AnimParam_Jump, true);
			transform.position = lastPosition;
			forcedTransition = true;
		}

		if (collision.collider.tag == MonsterTag)
		{
			if (DamageTimer <= 0)
			{
				MagicPower -= MagicDamage_Monster;
				Life -= LifeDamage_Monster;
				DamageTimer = DamageTimerMax;
				Soundboard.PlayHurt();
			}
			if (!forcedTransition)
				Pushback();
		}
		else if (collision.collider.tag == LavaTag)
		{
			if (DamageTimer <= 0)
			{
				MagicPower -= MagicDamage_Lava;
				Life -= LifeDamage_Lava;
				DamageTimer = DamageTimerMax;
				Soundboard.PlayHurt();
			}
			if (!forcedTransition)
				Pushback();
		}
	}

	const string LavaTag = "Lava";
	const string PlayerAttackTag = "PlayerAttack";
	void OnTriggerStay2D(Collider2D collider)
	{
		if (CurrentState == PlayerState.Paused || CurrentState == PlayerState.Transition)
			return;

		if (collider.tag == PlayerAttackTag)
		{
			if (DamageTimer <= 0)
			{
				MagicPower -= MagicDamage_Lava;
				Life -= LifeDamage_Lava;
				DamageTimer = DamageTimerMax;
			}
			Pushback();
		}
	}

	const string RubyTag = "Ruby";

	public void Pushback()
	{
		PushbackTimer = MaxPushbackTimer;
		pushbackDirection = lastMove.normalized;
	}

	public static void DuckMusic()
	{
		if (Current != null)
		{
			Current.OverworldMusic.volume = (Current.CurrentLocation == LocationTypes.Overworld) ?  0.25f : 0;
			Current.UnderworldMusic.volume = (Current.CurrentLocation == LocationTypes.Underworld) ?  0.25f : 0;
		}
	}
}
