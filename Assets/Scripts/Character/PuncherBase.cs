using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
[System.Serializable]
public class Stats
{
    public int maxHealth;
    public int currentHealth;
    public float punchRange;
    public float movementSpeed;
}


public class PuncherBase : MonoBehaviour
{
    [SerializeField] MMF_Player haptic;
    public delegate void ActionDelegate();
    public ActionDelegate OnEnemyDeath;
    public Stats stats = new();
    public HealthSystem healthSystem;
    private bool canHit = true;
    private bool getHeal = false;
    public float maxRange=10;
    public int numOfKills = 0,minHealth=1,maxHealth=5;
    private PuncherBase lastPuncher;
    private VfxManager vfxManager;
    private GameObject deathPunch;
    private PunchMechanism punchMechanism;
    private SliderSpawn punchsSlider;
    Vector3 direction,damagePunch;
    bool isJojoPunch = false;
    private void Awake()
    {
        
        SetupComponents();
        OnEnemyDeath += ViewController.instance.UpdateProgressBar;
    }

    protected virtual void SetupComponents()
    {
        stats.maxHealth = Random.Range(minHealth, maxHealth);
        stats.currentHealth = stats.maxHealth;
        healthSystem = GetComponent<HealthSystem>();


        punchMechanism = GetComponent<PunchMechanism>();
        vfxManager = GetComponent<VfxManager>();
        punchMechanism.SetupStats(stats);
        punchsSlider = GetComponent<SliderSpawn>();
        punchsSlider.SetupSlider();
        punchsSlider.DestroySlider(stats.currentHealth);
    }

    



    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject.GetComponent<PunchMechanism>())
        {
            if (other.transform.root.gameObject == gameObject || !other.transform.root.gameObject.GetComponent<PunchMechanism>().isPunching)
                return;
            if (other.gameObject.tag == "Punch")
            {

                if (other.transform.root.gameObject.GetComponent<Character>() != null)
                {
                    StartHaptic();
                }
                lastPuncher = other.transform.root.gameObject.GetComponent<PuncherBase>();
                
                deathPunch = other.transform.root.gameObject;

                //damagePunch = other.transform.position;

                 GetDamage();

                ShakeCam(other); // shake cam here

                direction = (transform.position - new Vector3(other.transform.root.gameObject.transform.position.x, transform.position.y, other.transform.root.gameObject.transform.position.z)).normalized;

                RaycastHit hit;

                if (Physics.Raycast(transform.position, direction, out hit, lastPuncher.stats.punchRange))
                {
                    Debug.Log("we saw the ring");
                    transform.DOMove(hit.point + hit.normal * 1, 0.3f).SetLink(gameObject).SetEase(Ease.OutBack).OnComplete(() =>
                    {
                        transform.DOMove(transform.position + hit.normal * 4f, 0.1f).SetLink(gameObject); // bounce off wall
                    });
                }
                else
                {
                    EnemyController enemyController = GetComponent<EnemyController>();

                    if (enemyController != null)
                    {
                        enemyController.canMove = false;
                    }

                    Controller controller = GetComponent<Controller>();

                    if(controller != null)
                    {
                        controller.canMove = false;
                    }

                    transform.DOMove(transform.position + direction * lastPuncher.stats.punchRange, lastPuncher.stats.punchRange / 20f).SetLink(gameObject).SetEase(Ease.OutBack).OnComplete(() =>
                    {
                        if (enemyController != null)
                        {
                            enemyController.canMove = true;
                        }


                        if (controller != null)
                        {
                            controller.canMove = true;
                        }
                    });

                }
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (canHit)
        {
            canHit = false;

            if (hit.collider.GetType() == typeof(BoxCollider) || hit.collider.GetType() == typeof(CapsuleCollider)) // knockback wall
            {
                Controller character = GetComponent<Controller>();
                if (character!=null)
                {
                    character.canMove = false;
                }

                Vector3 direction = transform.position - new Vector3(hit.point.x, transform.position.y, hit.point.z);
                transform.DOMove(transform.position + direction * 3f, 0.2f).SetLink(gameObject).SetEase(Ease.OutBack).OnComplete(() =>
                {

                    if (character != null)
                    {
                        character.canMove = true;
                    }

                });
            }
            else if (hit.collider.GetType() == typeof(CharacterController))
            {
                // Vector3 direction = transform.position - new Vector3(hit.point.x, transform.position.y, hit.point.z);

                // transform.DOMove(transform.position + direction * 1f, 0.2f).SetLink(gameObject);
            }

            DOVirtual.DelayedCall(0.03f, () => { canHit = true; });
        }

    }

    public virtual void ShakeCam(Collider collider)
    {

    }
    public void StartHaptic()
    {
        if (haptic)
        {
            haptic.PlayFeedbacks();
        }
    }

    private void GetDamage()
    {
        getHeal = false;
        stats.currentHealth -= 1;
        healthSystem.SetHealth(stats.currentHealth);

        if (stats.currentHealth>0)
        {
            //Vector3 offsetParticle = Vector3.forward;

            vfxManager.DamageParticle(gameObject.transform.position + new Vector3(0,2.5f,0), Quaternion.identity);
        }
        else if (stats.currentHealth <= 0)
        {
            Killed();
            punchsSlider.DestroySlider(stats.currentHealth);
        }
        
        
    }

    protected virtual void AddKill()
    {
        numOfKills += 1;

        UpgradePunchRange(10);
    }

    protected virtual void Killed()
    {
        lastPuncher.AddKill();
        Destroy(gameObject);
        Quaternion deathRotation = Quaternion.LookRotation(deathPunch.transform.forward, Vector3.up);
        Vector3 offset = gameObject.transform.position + new Vector3(0, 2f, 0);
        vfxManager.DeathParticle(offset,deathRotation);



        LevelManager.Instance.RemoveKilledEnemy(this);
    }

    public void UpgradePunchRange(float upgradePercent)
    {
        if (!isJojoPunch)
        {

            stats.punchRange += stats.punchRange * (upgradePercent / 100);

            stats.punchRange = Mathf.Clamp(stats.punchRange, 4f, maxRange);

        }



        punchMechanism.SetupStats(stats);
    }

    public void GetHeal()
    {
        if (stats.currentHealth<=0)
        {
            return;
        }

        if (stats.currentHealth<stats.maxHealth)
        {
            getHeal = true;
            stats.currentHealth += 1;
            healthSystem.SetHealth(stats.currentHealth) ;
            
        }

    }

    public void JojoPunch()
    {
        if (!isJojoPunch)
        {

            StartCoroutine(JojoPunchCoroutine());
        }        
    }
    public virtual void SpeedBooster(float movSpeed)
    {

    }

    IEnumerator JojoPunchCoroutine()
    {
        isJojoPunch = true;
        float originalCoolDown = punchMechanism.punchCoolDown;
        float originalDuration = punchMechanism.punchDuration;
        float originalPunchTime = punchMechanism.timeBtwPunches;
        punchMechanism.punchCoolDown = 0;
        punchMechanism.skipCooldownTrigger = true;

        punchMechanism.punchDuration = 0.15f;
        punchMechanism.timeBtwPunches = 0.15f;
        yield return new WaitForSeconds(5f);
        punchMechanism.timeBtwPunches = originalPunchTime;
        punchMechanism.punchDuration = originalDuration;

        punchMechanism.punchCoolDown = originalCoolDown;
        punchMechanism.skipCooldownTrigger = true;
        isJojoPunch = false;
    }



}
