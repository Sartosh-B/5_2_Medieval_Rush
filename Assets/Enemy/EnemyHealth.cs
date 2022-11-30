using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitpoints = 5;
    [SerializeField] int currentHitpoints = 0;
    
    void OnEnable()
    {
        currentHitpoints = maxHitpoints;
    }



    void OnParticleCollision(GameObject other)
    {
        
        currentHitpoints--;
        if (currentHitpoints < 1)
        {
            KillEnemy();
        }
    }
    void KillEnemy()
    {
        gameObject.SetActive(false);
    }


}
