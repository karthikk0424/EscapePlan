using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
	void Spawn(Vector3 _worldPosition);
	//void Spawn(Vector3 _worldPosition, Quaternion _rot);

	void Despawn(GameObject go);
	void DespawnAll();
}

public class ObjectRecycler : RecyclerContract
{	
	private GameObject theObject, parentGameObject, lastActiveObject;
	private List<GameObject> recyledObjects = new List<GameObject>();
	private int totalRecyledObjects = 0, totalActiveObjects = 0, totalInactiveObjects = 0;
	
	#region Getters & Setters
	public int TotalRecycled
	{
		get {	return totalRecyledObjects;}
	}
	
	public int TotalActive
	{
		get {	return totalActiveObjects;}
	}
	
	public int TotalInactive
	{
		get {	return totalInactiveObjects;}
	}
	
	public GameObject LastActiveGameObject
	{
		get { return lastActiveObject;}
	}
	#endregion
	
	#region Constructors	

	// 1. Create objects under that parent. Thats it. 
	internal ObjectRecycler(GameObject go, int count, GameObject parent)
	{
		if((count < 2) || (go == null))
		{	return;}

		parentGameObject = parent;
		theObject = go;
		
		for(int i = 0; i < count; i++)
		{
			GameObject temp  = Object.Instantiate(theObject, Vector3.zero, Quaternion.identity) as GameObject;
			recyledObjects.Add(temp);
			totalRecyledObjects++;
			temp.name += ("_" + totalRecyledObjects);
			temp.transform.parent = parent.transform;
			temp.SetActive(false);
		}
		totalInactiveObjects = count;
		totalActiveObjects = 0;
		//RecountTheCounter();
	}

	/*
	internal ObjectRecycler(GameObject go, uint count)
	{
		if((count < 2) || (go == null))
		{	return;}
		
		parentGameObject = new GameObject((go.transform.name + "_Parent").ToString());
		theObject = go;
		
		for(int i = 0; i < count; i++)
		{
			GameObject temp  = Object.Instantiate(theObject, Vector3.zero, Quaternion.identity) as GameObject;
			recyledObjects.Add(temp);
			totalRecyledObjects++;
			temp.name = (temp.name + "_" + totalRecyledObjects);
			temp.transform.parent = parentGameObject.transform;
			temp.SetActive(false);
		}
		RecountTheCounter();
	}


	internal ObjectRecycler(GameObject go, uint count, string parentName)
	{
		if((count < 2) || (go == null))
		{	return;}
		
		parentGameObject = new GameObject(parentName);
		theObject = go;
		
		for(int i = 0; i < count; i++)
		{
			GameObject temp  = Object.Instantiate(theObject, Vector3.zero, Quaternion.identity) as GameObject;
			recyledObjects.Add(temp);
			totalRecyledObjects++;
			temp.name = (temp.name + "_" + totalRecyledObjects);
			temp.transform.parent = parentGameObject.transform;
			temp.SetActive(false);
		}
		RecountTheCounter();
	}
	
	internal ObjectRecycler(GameObject go, uint count, GameObject parent)
	{
		if((count < 2) || (go == null))
		{	return;}
		
		parentGameObject = parent;
		
		theObject = go;
		
		for(int i = 0; i < count; i++)
		{
			GameObject temp  = Object.Instantiate(theObject, Vector3.zero, Quaternion.identity) as GameObject;
			recyledObjects.Add(temp);
			totalRecyledObjects++;
			temp.name = (temp.name + "_" + totalRecyledObjects);
			temp.transform.parent = parentGameObject.transform;
			temp.SetActive(false);
		}
		RecountTheCounter();
	}
	
	internal ObjectRecycler(GameObject _go, uint count, GameObject parent, string parentName)
	{
		if((count < 2) || (_go == null))
		{	return;}
		
		parentGameObject = parent;
		parentGameObject.name = parentName;
		theObject = _go;
		
		for(int i = 0; i < count; i++)
		{
			GameObject temp  = Object.Instantiate(theObject, Vector3.zero, Quaternion.identity) as GameObject;
			recyledObjects.Add(temp);
			totalRecyledObjects++;
			temp.name = (temp.name + "_" + totalRecyledObjects);
			temp.transform.parent = parentGameObject.transform;
			temp.SetActive(false);
			
		}
		RecountTheCounter();
	}
	*/
	#endregion
	
	
	#region Spawn & Despawn Methods

	/*
	public void Spawn()
	{
		// LINQ to find free objects in the list
		GameObject _go =  (from item in recyledObjects
		                   where item.activeSelf == false
		                   select item.gameObject).FirstOrDefault();
		
		if(_go == null)
		{
			_go  = Object.Instantiate(theObject, Vector3.zero, Quaternion.identity) as GameObject;
			recyledObjects.Add(_go);
			totalRecyledObjects++;
			_go.name = (_go.name + "_" + totalRecyledObjects);
			_go.transform.parent = parentGameObject.transform; 
		}
		
		_go.transform.position = Vector3.zero;
		_go.transform.rotation = Quaternion.identity;
		_go.SetActive(true);
		lastActiveObject = _go;
		RecountTheCounter();
	}
	*/

	public void Spawn(Vector3 _worldPosition)
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
			_go.name +=  ("_" + totalRecyledObjects);
			_go.transform.parent = parentGameObject.transform; 
		}
		
		_go.transform.position = _worldPosition;
		_go.transform.rotation = Quaternion.identity;
		_go.SetActive(true);
		lastActiveObject = _go;
		recountTheCounter();
	}

	/*
	public void Spawn(Vector3 _worldPosition, Quaternion _rot)
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
			_go.name = (_go.name + "_" + totalRecyledObjects);
			_go.transform.parent = parentGameObject.transform; 
		}
		
		_go.transform.position = _worldPosition;
		_go.transform.rotation = _rot;
		_go.SetActive(true);
		lastActiveObject = _go;
		RecountTheCounter();
	}
	*/



	public void Despawn(GameObject _go)
	{
		if(_go != null && _go.activeSelf == true)
		{
			_go.transform.position = Vector3.zero;
			_go.transform.rotation = Quaternion.identity;
			_go.SetActive(false);
			totalActiveObjects--;
			totalInactiveObjects++;
		}
	//	RecountTheCounter();
	}
	
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
