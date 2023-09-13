using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private Image backgroundImage; // 新增的图片
    [SerializeField] private Image firstImage;
    [SerializeField] private Image secondImage;

    private int currentStage = 0; // 0: backgroundImage, 1: firstImage, 2: secondImage, 3: 都不显示

    private void ShowBackground() { // 显示新增的图片的方法
        backgroundImage.gameObject.SetActive(true);
        firstImage.gameObject.SetActive(false);
        secondImage.gameObject.SetActive(false);
        currentStage = 0;
    }

    private void ShowFirst() {
        backgroundImage.gameObject.SetActive(false); // 隐藏新增的图片
        firstImage.gameObject.SetActive(true);
        secondImage.gameObject.SetActive(false);
        currentStage = 1;
    }

    private void ShowSecond() {
        firstImage.gameObject.SetActive(false);
        secondImage.gameObject.SetActive(true);
        currentStage = 2;
    }

    private void HideAll() {
        backgroundImage.gameObject.SetActive(false); // 隐藏新增的图片
        firstImage.gameObject.SetActive(false);
        secondImage.gameObject.SetActive(false);
        currentStage = 3;
    }

    private void Update() {
        // 如果按下任何键
        if (Input.GetKeyDown(KeyCode.Space)) {
            switch (currentStage) {
                case 0:
                    ShowFirst();
                    break;
                case 1:
                    ShowSecond();
                    break;
                case 2:
                    HideAll();
                    break;
            }
        }
    }

    private void Start() {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        ShowBackground(); // 游戏开始时显示backgroundImage
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (KitchenGameManager.Instance.IsCountdownToStartActive()) {
            HideAll();
        }
    }
}
