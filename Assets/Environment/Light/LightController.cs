using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour
{
    Light sunLight;

    public float defaultIntensity = 1f;
    public float highIntensity = 2f;
    public float lowIntensity = .1f;


    [Header("win parameters")]
    public float winAnimTime = 12f;

    [Header("fail parameters")]
    public float failAnimTime = 4f;
    public float minFlickerOnTime = .2f;
    public float maxFlickerOnTime = .6f;
    public float minFlickerOffTime = .01f;
    public float maxFlickerOffTime = .15f;

    void Start()
    {
        sunLight = GetComponent<Light>();
        //Fail();
        Win();
    }

    // trigger a flicker animation on simon fail
    public void Win()
    {
        StartCoroutine(SineIntensity(winAnimTime));
    }

    // trigger a flicker animation on simon fail
    public void SmallWin()
    {
        StartCoroutine(SineIntensity(winAnimTime * .4f));
    }

    IEnumerator SineIntensity(float timeToWait)
    {
        StartCoroutine(GoToIntensity(highIntensity, timeToWait * .4f));
        yield return new WaitForSeconds(timeToWait * .4f);
        StartCoroutine(GoToIntensity(defaultIntensity, timeToWait * .6f));
    }


    // trigger a flicker animation on simon fail
    public void Fail()
    {
        StartCoroutine(FlickerForSeconds(failAnimTime));
    }

    IEnumerator FlickerForSeconds(float timeToWait)
    {
        float startTime = Time.time;

        while (Time.time < startTime + timeToWait)
        {
            sunLight.intensity = highIntensity;
            yield return new WaitForSeconds(Random.Range(minFlickerOnTime, maxFlickerOnTime));
            sunLight.intensity = lowIntensity;
            yield return new WaitForSeconds(Random.Range(minFlickerOffTime, maxFlickerOffTime));
        }
        StartCoroutine(GoToIntensity(defaultIntensity, 5f));
    }

    IEnumerator GoToIntensity(float targetIntensity, float timeToWait)
    {
        float startTime = Time.time;
        float startIntensity = sunLight.intensity;
        float waitedTime = 0f;

        while (waitedTime < timeToWait)
        {
            sunLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, waitedTime / timeToWait);
            waitedTime = Time.time - startTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        sunLight.intensity = targetIntensity;
    }
}