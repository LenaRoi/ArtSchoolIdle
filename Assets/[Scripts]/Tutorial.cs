using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using System.Linq;
using EKTemplate;

public class Tutorial : MonoBehaviour
{
    public RectTransform textRect;
    public RectTransform handRect;
    public Text instructionText;
    public void CloseTutorial()
    {
        handRect.DOKill();
        textRect.DOKill();
        UIManager.instance.tutorialPanel.ActiveSmooth(false);
        if (PlayerPrefs.GetInt("tutorial") == 0) PlayerPrefs.SetInt("tutorial", 1);
    }
    public void HelpTutorial()
    {
        handRect.DOKill();
        textRect.DOKill();
        instructionText.text = "TAP TO TUTOR";

        textRect.anchoredPosition = new Vector2(0, -750);
        handRect.anchoredPosition = new Vector2(-40000, 0);

        UIManager.instance.tutorialPanel.ActiveSmooth(true);

        //handRect.DOMoveX(handRect.anchoredPosition.x + 800, 1f).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);

        textRect.DOScale(0.9f, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
}
