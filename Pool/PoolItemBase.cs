using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolItem
{
    void Spawn();
    void Recycle();
}
public abstract class PoolItemBase : MonoBehaviour, IPoolItem
{
    public virtual void Spawn()
    {

    }
    public virtual void Recycle()
    {

    }
    private void OnEnable()
    {
        Spawn();
    }
    private void OnDisable()
    {
        Recycle();
    }
}
