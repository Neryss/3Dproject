﻿using UnityEngine;

public class TargetScript : MonoBehaviour
{
	public float health = 50f;

    public void TakeDamage(float damage)
    {
    	health -= damage;
    	if(health <= 0)
    		Destroy(gameObject);
    }
}
