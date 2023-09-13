using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingCloudUI : MonoBehaviour
{
    [SerializeField] private Image timerImage;

    private void Update() {
        timerImage.fillAmount = KitchenGameManager.Instance.GetGamePlayingTimerNormalized();
    }
    private void Start() {
        timerImage.fillAmount = 0f;
    }
}
