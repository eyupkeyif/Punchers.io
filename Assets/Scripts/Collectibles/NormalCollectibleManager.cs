using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCollectibleManager : MonoBehaviour
{
   public static NormalCollectibleManager Instance { get; private set; }
    private bool isNormalCollected = false;
    private PuncherBase puncherBase;
   public GameObject normalParticle;
    private void Start()
    {
        isNormalCollected = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        puncherBase = other.gameObject.GetComponent<PuncherBase>();

        if (other.gameObject.tag == "Player"|| other.gameObject.tag=="Enemy")
        {
            isNormalCollected = true;

            NormalCollectedItem();

                puncherBase.UpgradePunchRange(1);
            


        }

        else
        {
            isNormalCollected = false;
        }
    }

    protected virtual void NormalCollectedItem()
    {
        GameObject normalCollectibleEffect = Instantiate(normalParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(normalCollectibleEffect,3f);
        LevelManager.Instance.RemoveNormalCollectedItem(this);

    }


}
