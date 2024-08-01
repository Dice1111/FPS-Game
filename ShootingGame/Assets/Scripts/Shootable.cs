using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Shootable : MonoBehaviour
{
    [SerializeField] int health = 10;

    public float impulseStrength = 5.0f;
    public void ReduceHealth(int damage)
    {
        health -= damage;
    }

    public int GetHealth()
    {
        return health;
    }


    public void SetHealth(int value)
    {
        health += value;
    }




}

