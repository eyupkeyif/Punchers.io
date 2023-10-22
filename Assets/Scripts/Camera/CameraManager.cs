using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject fadeToBlackCanvasObj;
    [SerializeField] Camera mainCam;
    [SerializeField] float modifiedOffsetCoeff, modifierOffsetLerpCoeff;
    [SerializeField] float lerpCoef;
    [SerializeField] Vector3 firstPosition;

    Coroutine shakeCoroutine;

    //Follow
    bool isFollowing;
    GameObject targetObj;
    public Vector3 offset;
    Vector3 modifiedOffsetDirection, modifiedOffset;

    private Vector3 initialPosition;


    void Awake()
    {
        isFollowing = false;

        initialPosition = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isFollowing)
        {
            Following();
        }
    }

    public Camera GetMainCam()
    {
        return mainCam;
    }

    #region Fade To Black

    IEnumerator FadeToBlackCoroutine(float time)
    {
        float timer = 0;
        fadeToBlackCanvasObj.SetActive(true);
        CanvasGroup canvasGroup = fadeToBlackCanvasObj.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        while (timer <= time)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, timer / time);
            timer += 0.025f;
            yield return new WaitForSeconds(0.025f);
        }
        yield return null;
    }

    public void FadeToBlack(float time)
    {
        if (fadeToBlackCanvasObj != null)
        {
            StartCoroutine(FadeToBlackCoroutine(time));
        }
    }

    #endregion



    #region Camera Follow

    private void Following()
    {
        modifiedOffset = Vector3.Lerp(modifiedOffset, modifiedOffsetDirection * modifiedOffsetCoeff, modifierOffsetLerpCoeff * Time.fixedDeltaTime);

        Vector3 desiredPosition = targetObj.transform.position + offset + modifiedOffset;


        // Vector3 lerpedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, lerpTime * Time.fixedDeltaTime);// for smooth lerp
        Vector3 lerpedPosition = Vector3.Lerp(transform.position, desiredPosition, lerpCoef * Time.deltaTime);// for smooth lerp

        

        float clampedZ = Mathf.Clamp(lerpedPosition.z, -22f, 4f);
        float clampedX = Mathf.Clamp(lerpedPosition.x, -23f, 23f);
        lerpedPosition = new Vector3(clampedX, lerpedPosition.y, clampedZ);



        transform.position = lerpedPosition;
    }

    public void SetModifiedOffsetDirection(Vector3 direction)
    {
        modifiedOffsetDirection = direction.normalized;
    }

    public void StartFollow(GameObject obj)
    {
        isFollowing = true;
        targetObj = obj;

        offset = mainCam.transform.position - obj.transform.position;
        // transform.position = obj.transform.position + offset;

        // firstPosition = transform.position;
    }

    public void StartFollow(GameObject obj, Vector3 offset)
    {
        isFollowing = true;
        targetObj = obj;
        this.offset = offset;
    }

    public void StopFollow()
    {
        isFollowing = false;
    }

    public void SpawnToInitialPosition()
    {
        transform.position = initialPosition;
    }
    #endregion


    #region Camera Shake

    public void ShakeCamera(float duration = 1, float magnitude = 1, bool decreasingMagnitude = false)
    {
        shakeCoroutine = StartCoroutine(CameraShakeCoroutine(duration, magnitude, decreasingMagnitude));
    }

    public void StopCameraShake()
    {
        StopCoroutine(shakeCoroutine);
    }

    private IEnumerator CameraShakeCoroutine(float duration = 1, float magnitude = 1, bool decreasingMagnitude = false)
    {
        float timer = 0;

        while (timer < duration)
        {
            Vector2 randomShake = Random.insideUnitCircle * magnitude;

            if (decreasingMagnitude)
            {
                randomShake *= (1 - (timer / duration));
            }
            transform.Translate((Vector3)randomShake, Space.Self);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        yield return null;
    }

    #endregion

    public void ReturnToFirstPoint()
    {
        transform.position = firstPosition;
    }


}
