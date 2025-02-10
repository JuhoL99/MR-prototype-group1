using UnityEngine;
using TMPro;
using Unity.Mathematics;
using System;
using System.Collections;

public class ScorePopup : MonoBehaviour
{
    [SerializeField] private GameObject scoreObjectPrefab;
    private Transform cameraTransform;
    private float popupDuration = 1.5f;
    private float riseSpeed = 0.5f;
    private AnimationCurve fadeOutCurve = AnimationCurve.EaseInOut(0,1,1,0);
    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }
    public void OnTargetHit(Vector3 hitPos)
    {
        float distance = Vector3.Distance(cameraTransform.position, hitPos);
        Debug.Log("spawned score");
        SpawnScorePopup(distance, hitPos);
    }
    private void SpawnScorePopup(float distance, Vector3 hitPos)
    {
        GameObject scoreObject = Instantiate(scoreObjectPrefab, hitPos, Quaternion.identity);
        TMP_Text scoreText = scoreObject.GetComponent<TMP_Text>();
        scoreText.text = distance.ToString("F1") + " m";
        StartCoroutine(AnimatePopup(scoreObject, scoreText));
    }
    private IEnumerator AnimatePopup(GameObject scoreObject, TMP_Text scoreText)
    {
        float timer = 0f;
        Vector3 startPosition = scoreObject.transform.position;
        Color startColor = scoreText.color;
        while (timer < popupDuration)
        {
            scoreObject.transform.position = startPosition + Vector3.up * (timer * riseSpeed);
            float alpha = fadeOutCurve.Evaluate(timer / popupDuration);
            scoreText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(scoreObject);
    }
}
