using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    public float scaleNumber = 0.1f;
    private Vector3 originalScale;
    void Start()
    {
        originalScale = transform.localScale;
        Sequence seq = DOTween.Sequence();
        seq.SetLink(gameObject);
        seq.Append(transform.DOScale(originalScale + new Vector3(scaleNumber, scaleNumber, 0), 0.3f));
        seq.Append(transform.DOScale(originalScale - new Vector3(scaleNumber, scaleNumber, 0), 0.6f));
        seq.Append(transform.DOScale(originalScale, 0.3f));
        seq.SetLoops(-1);
        seq.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
