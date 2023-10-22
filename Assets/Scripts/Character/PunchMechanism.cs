using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class PunchMechanism : MonoBehaviour
{
    public GameObject leftPunch;
    public GameObject rightPunch;
    public int mudamuda;
    public float timeBtwPunches = 0.2f; //0.2f
    public float punchDuration = 0.2f;
    public float punchCoolDown = 1f;
    private float punchRange = 3f;
    [SerializeField] SliderSpawn punchSlider;
    public int vibration = 15;
    public float elasticity = 0.4f;

    public bool skipCooldownTrigger = false;

    [HideInInspector] public bool isPunching = false;
    private void Awake()
    {
       
         Invoke("StartPunch", UnityEngine.Random.Range(0f, 2f));
    }

    public void SetupStats(Stats stats)
    {
        punchRange = stats.punchRange;

        punchDuration = punchRange * 0.04f;
    }

    private void StartPunch()
    {
        StartCoroutine(PunchCoroutine());
        
    }

    private void EndPunch()
    {
        StopAllCoroutines();
    }

   

    private IEnumerator PunchCoroutine()
    {
         while (true)
        {
   
            if (punchCoolDown > 0)
            {
                yield return PunchSliderCoroutine(punchCoolDown, punchCoolDown / 20f);
            }

            isPunching = true;
           
            Punch(leftPunch);

            yield return new WaitForSeconds(timeBtwPunches);

            Punch(rightPunch);

            yield return new WaitForSeconds(punchDuration);

            isPunching = false;


        }
    }

    

    private void Punch(GameObject punch)
    {
        Vector3 punchDirection = Vector3.forward * punchRange - new Vector3(punch.transform.localPosition.x, 0, punch.transform.localPosition.z);

        punch.transform.DOPunchPosition(punchDirection, punchDuration, vibration, elasticity).SetLink(gameObject);
    }

    private IEnumerator PunchSliderCoroutine(float time, float deltaTime = 0.05f)
    {
        float timer = 0;



        punchSlider.slider.value = punchSlider.slider.minValue;
        while (timer <= time)
        {

            punchSlider.slider.value = Mathf.Lerp(punchSlider.slider.minValue, punchSlider.slider.maxValue, timer / time);

            if (skipCooldownTrigger)
            {
                skipCooldownTrigger = false;
                break;
            }

            timer += deltaTime;
            yield return new WaitForSeconds(deltaTime);
        }
        punchSlider.slider.value = punchSlider.slider.maxValue;

        yield return null;

    }


}
