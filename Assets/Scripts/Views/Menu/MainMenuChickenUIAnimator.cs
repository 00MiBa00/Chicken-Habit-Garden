using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Menu
{
    public class MainMenuChickenUIAnimator : MonoBehaviour
    {
        [Header("Refs")] [SerializeField] Image img; // UI Image с курочкой
        [SerializeField] List<Sprite> frames; // 0:neutral, 1:side, 2:wink, 3:smile

        [Header("Frame Loop")] [SerializeField]
        float frameTime = 0.14f; // длительность кадра

        [Header("Idle Motion")] [SerializeField]
        float bobAmplitude = 8f; // px по Y (UI, в анкерных пикселях)

        [SerializeField] float bobPeriod = 1.2f; // сек
        [SerializeField] float squashAmount = 0.06f; // 0..0.2

        [Header("Random Emotes")] [SerializeField]
        Vector2 emoteDelayRange = new Vector2(3f, 7f);

        [SerializeField] float headTilt = 4f; // градусы
        [SerializeField] float peckDown = 10f; // px смещение вниз

        RectTransform rt;
        Vector2 startAnchoredPos;
        Vector3 startScale, startEuler;

        Sequence seqFrames, seqBob, seqSquash;
        Coroutine emoteRoutine;

        void Awake()
        {
            rt = GetComponent<RectTransform>();
            if (!img) img = GetComponent<Image>();

            startAnchoredPos = rt.anchoredPosition;
            startScale = rt.localScale;
            startEuler = rt.localEulerAngles;
        }

        void OnEnable()
        {
            PlayBaseFrames();
            PlayBob();
            PlaySquash();
            emoteRoutine = StartCoroutine(EmoteLoop());
        }

        void OnDisable()
        {
            seqFrames?.Kill();
            seqBob?.Kill();
            seqSquash?.Kill();
            if (emoteRoutine != null) StopCoroutine(emoteRoutine);

            rt.anchoredPosition = startAnchoredPos;
            rt.localScale = startScale;
            rt.localEulerAngles = startEuler;
            if (frames != null && frames.Count > 0) img.sprite = frames[0];
        }

        // ---------- Базовый покадровый цикл ----------
        void PlayBaseFrames()
        {
            if (img == null || frames == null || frames.Count < 4) return;

            seqFrames?.Kill();
            seqFrames = DOTween.Sequence().SetLink(gameObject).SetLoops(-1);

            int[] order = { 0, 1, 0, 3, 0 }; // neutral → side → neutral → smile → neutral
            foreach (var idx in order)
            {
                int i = idx;
                seqFrames.AppendCallback(() => img.sprite = frames[i])
                    .AppendInterval(frameTime);
            }
        }

        // ---------- Мягкий bob по Y (UI: anchor-pos) ----------
        void PlayBob()
        {
            seqBob?.Kill();
            seqBob = DOTween.Sequence().SetLink(gameObject).SetLoops(-1, LoopType.Yoyo)
                .Append(rt.DOAnchorPosY(startAnchoredPos.y + bobAmplitude, bobPeriod * 0.5f)
                    .SetEase(Ease.InOutSine))
                .Append(rt.DOAnchorPosY(startAnchoredPos.y, bobPeriod * 0.5f)
                    .SetEase(Ease.InOutSine));
        }

        // ---------- Squash/Stretch ----------
        void PlaySquash()
        {
            seqSquash?.Kill();
            seqSquash = DOTween.Sequence().SetLink(gameObject).SetLoops(-1, LoopType.Yoyo);
            seqSquash.Append(rt.DOScale(new Vector3(
                    startScale.x * (1f + squashAmount),
                    startScale.y * (1f - squashAmount),
                    startScale.z), bobPeriod * 0.5f).SetEase(Ease.InOutSine))
                .Append(rt.DOScale(startScale, bobPeriod * 0.5f).SetEase(Ease.InOutSine));
        }

        // ---------- Редкие эмоции ----------
        IEnumerator EmoteLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(emoteDelayRange.x, emoteDelayRange.y));
                int pick = Random.Range(0, 3);
                if (pick == 0) LookSideEmote();
                else if (pick == 1) WinkEmote();
                else PeckEmote();
            }
        }

        void LookSideEmote()
        {
            if (frames.Count > 1) img.sprite = frames[1]; // side
            rt.DOLocalRotate(new Vector3(0, 0, headTilt), 0.12f).SetEase(Ease.OutSine)
                .OnComplete(() => rt.DOLocalRotate(startEuler, 0.16f).SetEase(Ease.InSine));
            DOVirtual.DelayedCall(0.28f, () =>
            {
                if (frames.Count > 0) img.sprite = frames[0];
            });
        }

        void WinkEmote()
        {
            if (frames.Count > 2) img.sprite = frames[2]; // wink/angry
            rt.DOPunchScale(new Vector3(0.05f, -0.05f, 0f), 0.22f, 6, 0.8f);
            DOVirtual.DelayedCall(0.24f, () =>
            {
                if (frames.Count > 3) img.sprite = frames[3];
            });
            DOVirtual.DelayedCall(0.44f, () =>
            {
                if (frames.Count > 0) img.sprite = frames[0];
            });
        }

        void PeckEmote()
        {
            float down = startAnchoredPos.y - peckDown;
            DOTween.Sequence().SetLink(gameObject)
                .Append(rt.DOAnchorPosY(down, 0.08f).SetEase(Ease.InBack))
                .Join(rt.DOLocalRotate(new Vector3(0, 0, -headTilt), 0.08f))
                .Append(rt.DOAnchorPosY(startAnchoredPos.y, 0.10f).SetEase(Ease.OutBack))
                .Join(rt.DOLocalRotate(startEuler, 0.10f));

            if (frames.Count > 3)
            {
                img.sprite = frames[3];
                DOVirtual.DelayedCall(0.16f, () =>
                {
                    if (frames.Count > 0) img.sprite = frames[0];
                });
            }
        }
    }
}