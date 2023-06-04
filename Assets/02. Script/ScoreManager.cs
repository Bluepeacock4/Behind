using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    public Text scoreUI;
    public Text comboText;
    public Image comboImage;
    private int score;
    private int combo;
    private float comboTimer;
    private Vector2 comboImageOriginalPosition;
    private Vector3 comboTextOriginalScale;
    private const float comboDuration = 2f;
    private float comboTime;
    private Vector3 initialComboImagePos;
    public RectTransform comboImageRect; 
    private Coroutine comboResetCoroutine;
    public CameraShake cameraShake;

    private void Start()
    {
        score = 0;
        combo = 0;
        comboImageOriginalPosition = comboImage.rectTransform.anchoredPosition;
        comboTextOriginalScale = comboText.transform.localScale;
        ResetComboUI();
    }

    public void scoreUp()
    {
        score += 100;
        scoreUI.text = score.ToString();
    }

    private void Update()
    {
        if (combo > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                combo = 0;
                ResetComboUI();
            }
        }
    }

    public void ComboUp()
    {
        cameraShake.Shake();
        combo++;
        comboTimer = comboDuration;
        comboText.text = combo.ToString();

        comboText.DOFade(1, 0);
        comboImage.DOFade(1, 0);

        comboText.transform.DOScale(comboTextOriginalScale * 1.2f, 0.2f).OnComplete(() =>
        {
            comboText.transform.DOScale(comboTextOriginalScale, 0.2f);
        });

        comboImage.rectTransform.DOAnchorPos(new Vector2(comboImageOriginalPosition.x + 20f, comboImageOriginalPosition.y), 0.2f).OnComplete(() =>
        {
            comboImage.rectTransform.DOAnchorPos(comboImageOriginalPosition, 0.2f);
        });
    }

    private void ResetComboUI()
    {
        comboText.DOFade(0, 0.5f).OnComplete(() => comboText.text = "0");
        comboImage.DOFade(0, 0.5f);
    }

}
