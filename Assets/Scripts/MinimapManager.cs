using System.Collections;
using UnityEngine;
public class MinimapManager : MonoBehaviour
{
    [SerializeField] Camera minimapCam;
    public void Clickk()
    {
        if(transform.localScale.x == 1)
        {
            iTween.ScaleTo(gameObject, iTween.Hash("x",3, "y", 3, "time", 0.3f));
            StartCoroutine(SmoothZoom(150, 0.2f));
        }
        else if (transform.localScale.x == 3)
        {
            iTween.ScaleTo(gameObject, iTween.Hash("x", 1, "y", 1, "time", 0.3f));
            StartCoroutine(SmoothZoom(35, 0.2f));
        }
    }
    IEnumerator SmoothZoom(float newSize, float time)
    {
        float elapsedTime = 0f;
        float startSize = minimapCam.orthographicSize;
        while (elapsedTime < time)
        {
            minimapCam.orthographicSize = Mathf.Lerp(startSize, newSize, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        minimapCam.orthographicSize = newSize;
    }
}