using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnemyAgentMovementData
{
    public float Speed;
    public float AngularSpeed;
    public float FleeSpeed;
    public float ChaseSpeed;

    public float Acceleration;

    public float FleeRadius;
    public float ChaseRadius;
    public float WanderRadius;
}
