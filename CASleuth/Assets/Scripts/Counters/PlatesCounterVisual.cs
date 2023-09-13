using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform redVisualPrefab;
    [SerializeField] private Transform yellowVisualPrefab;
    [SerializeField] private Transform greenVisualPrefab;
    [SerializeField] private Transform whiteVisualPrefab;

    private List<GameObject> plateVisualGameObjectsList;
    private int createType = 0;

    private void Awake()
    {
        plateVisualGameObjectsList = new List<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateSpawned(object sender, EventArgs e)
    {
        //∞¥À≥–Ú≤°∂æ÷÷¿‡
        Transform plateVisualTransform=null;
        platesCounter.SetVirusType(createType);
        switch (createType) {
            case 0:
                plateVisualTransform = Instantiate(redVisualPrefab, counterTopPoint);
                break;
            case 1:
                plateVisualTransform = Instantiate(yellowVisualPrefab, counterTopPoint);
                break;
            case 2:
                plateVisualTransform = Instantiate(greenVisualPrefab, counterTopPoint);
                break;
            case 3:
                plateVisualTransform = Instantiate(whiteVisualPrefab, counterTopPoint);
                break;

        }
        if (createType < 3)
        {
            createType++;
        }
        else
        {
            createType = 0;
        }
        float plateOffsetY = 0.8f;
        plateVisualTransform.localPosition = new Vector3(0f, plateOffsetY * plateVisualGameObjectsList.Count, 0f);

        plateVisualGameObjectsList.Add(plateVisualTransform.gameObject);


    }

    private void PlatesCounter_OnPlateRemoved(object sender, EventArgs e)
    {
        if (plateVisualGameObjectsList.Count > 0)
        {
            GameObject plateGameObject = plateVisualGameObjectsList[plateVisualGameObjectsList.Count - 1];
            plateVisualGameObjectsList.Remove(plateGameObject);
            Destroy(plateGameObject);
        }
    }
}
