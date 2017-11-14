using System.Collections;
using UnityEngine;

public class LightingHandler : MonoBehaviour
{
    private void Start()
    {
        StartCycle();
    }

    [ContextMenu("StartCycle")]
    public void StartCycle() 
    {
        StartCoroutine(NightCoroutine());
    }

    private IEnumerator NightCoroutine()
    {
        Camera t_camera = Camera.main;

        Color t_backgroundColor = t_camera.backgroundColor;
        while (true)
        {
            t_backgroundColor.g -= .0005f / 2;
            t_backgroundColor.b -= .0005f;

            Color t_ambient = RenderSettings.ambientSkyColor;
            if (t_ambient.r > .3f)
            {
                t_ambient.r -= .0005f;
                t_ambient.g -= .0005f;
                t_ambient.b -= .0005f;
            }
            RenderSettings.ambientSkyColor = t_ambient;

            yield return new WaitForSeconds(.00001f);

            t_camera.backgroundColor = t_backgroundColor;

            if (t_backgroundColor.b <= 0) {
                yield return new WaitForSeconds(10f);
                StartCoroutine(DayCoroutine());
                break;
            } 

        }
    }
    private IEnumerator DayCoroutine()
    {
        Camera t_camera = Camera.main;

        Color t_backgroundColor = t_camera.backgroundColor;
        while (true)
        {
            t_backgroundColor.g += .0005f / 2;
            t_backgroundColor.b += .0005f;

            Color t_ambient = RenderSettings.ambientSkyColor;
            if (t_ambient.r >= t_backgroundColor.b)
            {
                t_ambient.r += .0005f;
                t_ambient.g += .0005f;
                t_ambient.b += .0005f;
            }
            RenderSettings.ambientSkyColor = t_ambient;

            yield return new WaitForSeconds(.00001f);

            t_camera.backgroundColor = t_backgroundColor;

            if (t_backgroundColor.b >= 1) {
                yield return new WaitForSeconds(10f);
                StartCoroutine(NightCoroutine());
                break;
            }
        }
    }
}
