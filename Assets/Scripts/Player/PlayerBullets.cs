using UnityEngine;
using System.Collections;

public class PlayerBullets : PlayerMotion
{
	public GameObject Bullets;

	protected override IEnumerator Start()
	{
		Debug.Log("Children");
		yield return null;
	}

	protected override void OnEnable()	{}

	protected override void Awake ()
	{
		//base.Awake ();
	}

	//protected void Awake() {}

}
