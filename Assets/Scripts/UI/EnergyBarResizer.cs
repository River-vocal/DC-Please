using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnergyBarResizer : MonoBehaviour
{
    [SerializeField] public FloatEventChannel extraPickupEventChannel;
    [SerializeField] public float ResizeSmoothTime = 1.0f;
    [SerializeField] public float ResizeStressScale = 1.1f;
    [SerializeField] public float ResizeThreshold = 2f;

    RectTransform rectTransform;
    float widthTarget;
    float resizeVelocity = 0.0f;
    float width => rectTransform.rect.width;

    private void Awake()
    {
        rectTransform = (RectTransform)transform;
        widthTarget = width;
    }

    private void Start()
    {
        widthTarget = LevelLoader.current.EnergyData.EnergyBarLength;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, widthTarget);
    }

    private void OnEnable()
    {
        extraPickupEventChannel.AddListener(Enlarge);
    }

    private void OnDisable()
    {
        extraPickupEventChannel.RemoveListener(Enlarge);
        LevelLoader.current.EnergyData.EnergyBarLength = widthTarget;
    }

    public void Enlarge(float enlargeWidth)
    {
        // GlobalAnalysis.energy_extender += 1;
        float newWidth = enlargeWidth + rectTransform.rect.width;
        Resize(newWidth);
    }

    public void Resize(float width)
    {
        widthTarget = width;
    }

    private void Update()
    {
        if (Math.Abs(width - widthTarget) > ResizeThreshold)
        {
            rectTransform.localScale = new Vector3(1, ResizeStressScale, 1);
            float newWidth = Mathf.SmoothDamp(width, widthTarget, ref resizeVelocity, ResizeSmoothTime);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
        }
        else
        {
            if (width != widthTarget)
            {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, widthTarget);
                rectTransform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}