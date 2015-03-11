using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class voxList <T>
{
	public List<T> vList;
	public int vcount;

	public voxList()
	{
		vList = new List<T>();
		vcount = 0;
	}
	public void Clear()
	{
		vcount = 0;
	}
	public void Clean()
	{
		vList.Clear();
	}
	public void Add(T item)
	{
		if(vcount >= vList.Count)
		{
			vList.Add(item);
			vcount++;
		}else
		{
			vList[vcount] = item;
			vcount++;
		}
	}
	public T[] ToArray(){
		return vList.GetRange(0,vcount).ToArray();
	}
}
