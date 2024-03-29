using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSkybox : MonoBehaviour
{
    public Skybox skyboxComponent;

    public Material[] skyboxes;
    //public Material skyboxDay;
    //public Material skyboxNight;
    public float fadeDuration;
    int exposureID;
    int count = 1;

    void Start()
    {
        exposureID = Shader.PropertyToID("_Exposure");
        StartCoroutine(TransitionToNewSkyBox(skyboxes[count]));
    }

    IEnumerator TransitionToNewSkyBox(Material newSkybox)
    {
        yield return new WaitForSeconds(15);
        yield return StartCoroutine(FadeExposure(0));

        skyboxComponent.material.SetFloat(exposureID, 1); // Resets the old skybox
        skyboxComponent.material = newSkybox;
        skyboxComponent.material.SetFloat(exposureID, 0);

        yield return StartCoroutine(FadeExposure(1));

        count++;

        if (count == skyboxes.Length)
        {
            count = 0;
        }

        StartCoroutine(TransitionToNewSkyBox(skyboxes[count]));

        /*
        if (count == 0)
        {
            count++;
            StartCoroutine(TransitionToNewSkyBox(skyboxDay));
        }
        else
        {
            count = 0;
            StartCoroutine(TransitionToNewSkyBox(skyboxNight));
        }*/

    }

    IEnumerator FadeExposure(float targetValue)
    {
        float timer = 0;
        float startValue = skyboxComponent.material.GetFloat(exposureID);

        while (timer < fadeDuration)
        {
            float newValue = Mathf.Lerp(startValue, targetValue, timer / fadeDuration);
            skyboxComponent.material.SetFloat(exposureID, newValue);

            timer += Time.deltaTime;
            yield return null;
        }

        skyboxComponent.material.SetFloat(exposureID, targetValue);
    }
}