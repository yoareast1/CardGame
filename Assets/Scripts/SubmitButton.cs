using UnityEngine;
using DG.Tweening;

public class SubmitButton : MonoBehaviour
{
    private void Start()
    {
        transform.DOScale(0.1f, 1f)
            .SetRelative(true)
            .SetEase(Ease.OutQuad)
            .SetLoops(-1, LoopType.Restart);
    }
}
