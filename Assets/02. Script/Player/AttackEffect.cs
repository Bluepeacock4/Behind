using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public Sprite[] effectSprites;
    public float frameRate = 60f;
    public float delay;

    private SpriteRenderer spriteRenderer;
    private Coroutine playAnimationCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlayEffect()
    {
        if (playAnimationCoroutine != null)
        {
            StopCoroutine(playAnimationCoroutine);
        }

        playAnimationCoroutine = StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        int frameIndex = 0;
        float frameDelay = 1f / frameRate;
        spriteRenderer.sprite = null;
        yield return new WaitForSeconds(delay);

        while (frameIndex < effectSprites.Length)
        {
            spriteRenderer.sprite = effectSprites[frameIndex];
            frameIndex++;

            yield return new WaitForSeconds(frameDelay);
        }
        gameObject.SetActive(false);
    }
}
