using System;
using UnityEngine;

[Serializable]
public struct EnemyStartData
{
    public Vector3 Position;

    public EnemyStartData(Vector3 position)
    {
        Position = position;
    }
}
