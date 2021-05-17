using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Subtitles : MonoBehaviour {
    [SerializeField] Transform house;
    [SerializeField] float distanceToHouseMin = 40;
    [SerializeField] float timePerLetter = 0.03f;

    private TextMeshProUGUI[] textMeshArray;
    private int currentIndex = 0;
    private float timer = 0f;
    private Transform camTransform;

    private void Start()
    {
        camTransform = Camera.main.transform;
        textMeshArray = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var textMesh in textMeshArray) {
            textMesh.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Vector3.Distance(camTransform.position, house.position) < distanceToHouseMin) {
            timer += Time.deltaTime;
            if (timer < textMeshArray[currentIndex].text.Length * timePerLetter) {
                textMeshArray[currentIndex].gameObject.SetActive(true);
            } else {
                textMeshArray[currentIndex].gameObject.SetActive(false);
                timer = 0f;
                currentIndex++;
            }
            if (currentIndex >= textMeshArray.Length) {
                gameObject.SetActive(false);
            } else {
                textMeshArray[currentIndex].gameObject.SetActive(true);
            }
        } else {
            foreach (var textMesh in textMeshArray) {
                textMesh.gameObject.SetActive(false);
            }
        }
    }
}
