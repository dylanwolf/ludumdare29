using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class RockMonster : MonoBehaviour {

	public enum AIState
	{
		Wander,
		Wait
	}

	const float Speed = 2.0f;
	float timer;
	const float WaitTimerMin = 0.2f;
	const float WaitTimerMax = 0.4f;
	const float WanderTimerMin = 0.75f;
	const float WanderTimerMax = 1.5f;
	float reverseThreshold = 0.5f;
	Vector2 direction = Vector2.zero;
	AIState CurrentState;

	Vector3 lastPosition;

	void RandomizeDirection()
	{
		r = Random.Range(0, 1.0f);
		if (r < 0.25)
		{
			direction.x = -1; direction.y = 0;
		}
		else if (r < 0.5)
		{
			direction.x = 1; direction.y = 0;
		}
		else if (r < 0.75)
		{
			direction.x = 0; direction.y = 1;
		}
		else
		{
			direction.x = 0; direction.y = -1;
		}
	}

	float r;
	void BeginWander()
	{
		CurrentState = AIState.Wander;
		timer = Random.Range(WanderTimerMin, WanderTimerMax);

		r = Random.Range(0, 1.0f);
		if (r < reverseThreshold)
		{
			direction *= -1;
			reverseThreshold = 0.2f;
		}
		else
		{
			direction.x = (direction.x == 0) ? ((Random.Range(0, 1.0f) < 0.5) ? -1 : 1) : 0;
			direction.y = (direction.y == 0) ? ((Random.Range(0, 1.0f) < 0.5) ? -1 : 1) : 0;
			reverseThreshold = 0.5f;
		}
	}

	void BeginWait()
	{
		rigidbody2D.velocity = Vector2.zero;
		CurrentState = AIState.Wait;
		timer = Random.Range(WaitTimerMin, WaitTimerMax);
	}

	void Awake () {
		GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("RockGuyAnimation");

		RandomizeDirection();
		BeginWander();
	}
	
	void FixedUpdate () {
		if (PlayerController.Current.CurrentState == PlayerController.PlayerState.Paused)
		{
			rigidbody2D.velocity = Vector2.zero;
			return;
		}

		// Move
		if (CurrentState == AIState.Wander)
		{
			lastPosition = transform.position;
			rigidbody2D.velocity = direction * Speed;
		}
	}

	void Update()
	{
		if (PlayerController.Current.CurrentState == PlayerController.PlayerState.Paused)
			return;

		timer -= Time.deltaTime;
		if (timer < 0)
		{
			if (CurrentState == AIState.Wait)
			{
				BeginWander();
			}
			else
			{
				BeginWait();
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collide)
	{
		if (PlayerController.Current.CurrentState == PlayerController.PlayerState.Paused)
			return;

		BeginWander();
		transform.position = lastPosition;
	}

	const string LavaTag = "Lava";
	const string PlayerAttackTag = "PlayerAttack";
	void OnTriggerEnter2D(Collider2D collide)
	{
		if (PlayerController.Current.CurrentState == PlayerController.PlayerState.Paused)
			return;

		Debug.Log (collide.tag);

		if (collide.tag == PlayerAttackTag)
		{
			Cloud c = (Cloud)Instantiate(PlayerController.Current.CloudPrefab);
			c.transform.position = this.transform.position;
			DestroyObject (gameObject);
		}
	}
}
