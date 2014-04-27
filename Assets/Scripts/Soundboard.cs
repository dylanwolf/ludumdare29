using UnityEngine;
using System.Collections;

public class Soundboard : MonoBehaviour {

	static Soundboard Current;

	public AudioSource[] RockBreak;
	public AudioSource RubyPickup;
	public AudioSource[] Hurt;
	public AudioSource Success;
	public AudioSource Fail;
	public AudioSource[] Lava;
	public AudioSource Transition;

	void Awake()
	{
		Current = this;
	}

	static void PlayRandom(AudioSource[] source)
	{
		source[Random.Range(0, source.Length)].Play ();
	}

	public static void PlayRubyPickup()
	{
		if (Current != null && Current.RubyPickup != null)
		{
			Current.RubyPickup.Play();
		}
	}

	public static void PlayTransition()
	{
		if (Current != null && Current.Transition != null)
		{
			Current.Transition.Play();
		}
	}

	public static void PlaySuccess()
	{
		if (Current != null && Current.Success != null)
		{
			Current.Success.Play();
			PlayerController.DuckMusic();
		}
	}

	public static void PlayFailure()
	{
		if (Current != null && Current.Fail != null)
		{
			PlayerController.DuckMusic();
			Current.Fail.Play();
			PlayerController.DuckMusic();
		}
	}

	public static void PlayRockBreak()
	{
		if (Current != null && Current.RockBreak != null)
		{
			PlayRandom(Current.RockBreak);
		}
	}

	public static void PlayHurt()
	{
		if (Current != null && Current.Hurt != null)
		{
			PlayRandom(Current.Hurt);
		}
	}

	public static void PlayLava()
	{
		if (Current != null && Current.Lava != null)
		{
			PlayRandom(Current.Lava);
		}
	}
}
