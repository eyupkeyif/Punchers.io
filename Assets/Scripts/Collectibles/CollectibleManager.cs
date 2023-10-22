using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance { get; private set; }
    
   
   
    private bool isCollected=false;
    private PuncherBase puncherBase;
    private void Start()
    {
        

        isCollected = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        puncherBase = other.gameObject.GetComponent<PuncherBase>();

        if (other.gameObject.tag == "Player" || other.gameObject.tag=="Enemy")
        {
            isCollected = true;


            CollectedItem();

            if (gameObject.tag == "SpeedBoost")
            {
                //if (other.gameObject.tag=="Enemy")
                //{
                //    EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
                //    puncherBase.SpeedBooster(ref enemy.movementSpeed);
                //}
                //else if (other.gameObject.tag == "Player")
                //{
                //    Controller controller = other.gameObject.GetComponent<Controller>();
                //    puncherBase.SpeedBooster(ref controller.movementSpeed);
                //    Debug.Log(controller.movementSpeed);
                //}

                if(puncherBase != null)
                {
                    puncherBase.SpeedBooster(2f);
                }
               
            }
            
            if (gameObject.tag=="jojoPunch")
            {
                puncherBase.JojoPunch();
            }
            if (gameObject.tag=="HealthBoost")
            {
                puncherBase.GetHeal();

            }
 

            
        }
       
        else
        {
            isCollected = false;
        }
    }

   protected virtual void CollectedItem()
    {
        Destroy(gameObject);

        LevelManager.Instance.RemoveCollectedItem(this) ;

    }
}
