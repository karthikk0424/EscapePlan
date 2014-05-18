using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	#region Singleton created on access
	private static LevelManager instance = null;
	private LevelManager() {}
	
	public static LevelManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new LevelManager();
			}
			return instance;
		}
	}
	#endregion
}
