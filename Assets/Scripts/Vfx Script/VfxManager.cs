using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxManager : MonoBehaviour
{
    public GameObject deathParticle;

    public List<GameObject> damageParticles;

    public void DamageParticle(Vector3 position, Quaternion rotation)
    {
        int randParticle = Random.Range(0, damageParticles.Count);
        GameObject damage = Instantiate(damageParticles[randParticle], position, rotation);

        Destroy(damage,2f);

    }

    public void DeathParticle(Vector3 position, Quaternion rotation)
    {
        GameObject death = Instantiate(deathParticle, position, rotation);
        Destroy(death, 4f);

    }
}
