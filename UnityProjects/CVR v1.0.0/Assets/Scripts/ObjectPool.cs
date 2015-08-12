

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Programmer: Wesley Allard
/// Last Edited: July 25 2015
/// 
/// Description:
/// This Class is an template object pool where free objects are daisy chained in a one way list format.
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectPool<T> {
    public PoolObject<T>[] Pool;
    PoolObject<T> firstObject;
    PoolObject<T> lastObject;
    
    
    public ObjectPool(int PoolSize)
    {
        if (PoolSize > 0)
        {
            Pool = new PoolObject<T>[PoolSize];            
            for (int i = 0; i < PoolSize; i++)
            {
                Pool[i] = new PoolObject<T>(this);
            }
            firstObject = Pool[0];//Set head and tail values
            lastObject = Pool[PoolSize - 1];

            //Set each PoolObject's _NextObject to the next one in the array since at this moment all objects are not InUse
            for (int i = 0; i < PoolSize; i++)
            {
                if (i < PoolSize - 1)
                    Pool[i]._NextObject = Pool[i + 1];
                else Pool[i]._NextObject = null;
            }
        }
        else
        {
            Debug.Log("PoolSize incorrect value: " + PoolSize + ", Pool was not initialized");
        }        
    }
    /// <summary>
    /// Returns an object to the pool 
    /// </summary>
    /// <param name="_obj"></param>
    public void ReturnObjectToPool(PoolObject<T> _obj)
    {
        _obj.InUse = false;
        _obj._NextObject = null;//Reset _NextObject values to destroy leftover infomation
            
        //Check to see if the end of the chain has been set
        if (firstObject == null)
        {//If the pool chain is completely empty then set the firstObject's value
            firstObject = _obj;
        }
        if (lastObject == null)
        {
            lastObject = _obj; 
        }
        else 
        {//Add the returned PoolObject to the end of the list 
            lastObject._NextObject = _obj;
            lastObject = _obj;
            
        }
    }
    /// <summary>
    /// Returns a free Objects, will return null if no object is free
    /// </summary>
    /// <returns></returns>
    public PoolObject<T> GetFreeObject()
    {
        if (firstObject != null)//If the first object is null then there are no free objects in the pool
        {
            PoolObject<T> _obj = firstObject;
            firstObject = firstObject._NextObject;
            return _obj;
        }
        else return null; //Returning null is temporary
        //A more perminate solution should be resetting the oldest pool object and returning that instead.
    }
	
}
public class PoolObject<T>
{
    public T _Object;
    public PoolObject<T> _NextObject;
    //In C++ a union would be used to ommit the rest of the class until initialization in order to save on memory while pool is stagnant(not in use) 

    public bool InUse; 
    private ObjectPool<T> pool;
      
    public PoolObject(ObjectPool<T> _pool)
    {
        pool = _pool;
        InUse = false;
    }
    public virtual void ReturnToPool()
    {        
        pool.ReturnObjectToPool(this);
    }
}

