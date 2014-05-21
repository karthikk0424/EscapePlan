
/// <summary>
/// An object recycler to pool object that could be reused several times. 
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#region Interface

/// <summary>
/// The contract for the class to implement the necessary methods and variables. 
/// </summary>
interface RecyclerContract
{
	int TotalRecycled
	{	get;}
	
	int TotalActive
	{ 	get;}
	
	int TotalInactive
	{	get;}
	
	GameObject LastActiveGameObject
	{	get;}
	
	//void Spawn();
	GameObject Spawn(Vector3 _worldPosition, Quaternion _rotation);
	//void Spawn(Vector3 _worldPosition, Quaternion _rot);

	void Despawn(GameObject go);
	void DespawnAll();
}

#endregion

public class ObjectRecycler : RecyclerContract
{	
	#region Variables

	private GameObject theObject, parentGameObject, lastActiveObject;
	private List<GameObject> recyledObjects = new List<GameObject>();
	private int totalRecyledObjects = 0, totalActiveObjects = 0, totalInactiveObjects = 0;
	private string gameObjectName = System.String.Empty;

	#endregion

	#region Getters & Setters

	/// <summary>
	/// Gets the number of recylced objects.
	/// </summary>
	/// <value>The total recycled gameobject.</value>
	public int TotalRecycled
	{
		get {	return totalRecyledObjects;}
	}

	/// <summary>
	/// Gets the total active game objects. 
	/// </summary>
	/// <value>The total active game objects.</value>
	public int TotalActive
	{
		get {	return totalActiveObjects;}
	}

	/// <summary>
	/// Gets the total inactive game objects.
	/// </summary>
	/// <value>The total inactive game objects.</value>
	public int TotalInactive
	{
		get {	return totalInactiveObjects;}
	}

	/// <summary>
	/// Gets the last active game object.
	/// </summary>
	/// <value>The last active game object.</value>
	public GameObject LastActiveGameObject
	{
		get { return lastActiveObject;}
	}

	#endregion
	
	#region Constructors	

	/// <summary>
	/// Initializes a new instance of the <see cref="ObjectRecycler"/> class.
	/// </summary>
	/// <param name="go">Game object to be recyled.</param>
	/// <param name="count">Number of objects to be recylced</param>
	/// <param name="parent">The parent for all the recycled objects.</param>
	internal ObjectRecycler(GameObject go, int count, GameObject parent)
	{
		if((count < 2) || (go == null))
		{	return;}

		parentGameObject = parent;
		theObject = go;
		gameObjectName = go.name;

		for(int i = 0; i < count; i++)
		{
			GameObject temp  = Object.Instantiate(theObject, Vector3.zero, Quaternion.identity) as GameObject;
			recyledObjects.Add(temp);
			totalRecyledObjects++;
			temp.name = (gameObjectName + "_" + totalRecyledObjects);
			temp.transform.parent = parent.transform;
			temp.SetActive(false);
		}
		totalInactiveObjects = count;
		totalActiveObjects = 0;
	}

	#endregion
	
	
	#region Spawn & Despawn Methods

	/// <summary>
	/// Spawns the game object at the specified _worldPosition and _rotation.
	/// </summary>
	/// <param name="_worldPosition">The vector3 world co-ordinates.</param>
	/// <param name="_rotation">The angle of rotation to be spawned at.</param>
	public GameObject Spawn(Vector3 _worldPosition, Quaternion _rotation)
	{
		// LINQ to find free objects in the list
		GameObject _go =  (from item in recyledObjects
		                   where item.activeSelf == false
		                   select item.gameObject).FirstOrDefault();
		
		if(_go == null)
		{
			_go  = Object.Instantiate(theObject) as GameObject;
			recyledObjects.Add(_go);
			totalRecyledObjects++;
			_go.name =  (gameObjectName + "_" + totalRecyledObjects);
			_go.transform.parent = parentGameObject.transform; 
		}
		
		_go.transform.position = _worldPosition;
		_go.transform.rotation = _rotation;
		_go.SetActive (true);
		lastActiveObject = _go;
		recountTheCounter ();
		return _go;
	}

	/// <summary>
	/// Despawn the specified game object
	/// </summary>
	/// <param name="_go">Game Object.</param>
	public void Despawn(GameObject _go)
	{
		if(_go != null && _go.activeSelf == true)
		{
			_go.transform.position = Vector3.zero;
			_go.transform.rotation = Quaternion.identity;
			_go.SetActive (false);
			totalActiveObjects--;
			totalInactiveObjects++;
		}
	}

	/// <summary>
	/// Despawns all the active game objects. 
	/// </summary>
	public void DespawnAll()
	{
		foreach(GameObject item in recyledObjects)
		{
			if(item.activeSelf == true)
			{	//Despawn(item);
				item.transform.position = Vector3.zero;
				item.transform.rotation = Quaternion.identity;
				item.SetActive(false);
			}
			totalActiveObjects = 0;
			totalInactiveObjects = totalRecyledObjects;
		}
	}

	#endregion

	#region Calculation

	/// <summary>
	/// Counts the active and inactive game objects thro' LINQ.
	/// </summary>
	private void recountTheCounter()
	{
		totalActiveObjects = (from item in recyledObjects
		                      where item.activeSelf == true
		                      select item).Count();
		
		totalInactiveObjects = (from item in recyledObjects
		                        where item.activeSelf == false
		                        select item).Count();	 
	}

	#endregion
}
