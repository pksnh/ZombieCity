using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleMove : MonoBehaviour
{
    // Sequence sequence;

    private Vector3 targetPosition = new Vector3 (0f, 260f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalMove(targetPosition, 3f).SetEase(Ease.Linear).SetLoops(-1,LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
