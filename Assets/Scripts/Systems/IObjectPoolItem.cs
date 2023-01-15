using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.Systems
{
    public interface IObjectPoolItem
    {
        string PoolObjectType { get; }
        void GetFromPool(ObjectPool pool);
        void ReturnToPool();
        GameObject GetGameObject();
    }
}

