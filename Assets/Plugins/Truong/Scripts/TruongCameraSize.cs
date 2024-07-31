using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TruongCameraSize : TruongMonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float defaultWidth;
    [SerializeField] private float defaultHeight;
    [SerializeField] private float defaultSize;

    private float currentWidth;
    private float currentHeight;
    private float ratioHeight;
    private float width;
    private float ratioWidth;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadMainCamera();
    }

    private void LoadMainCamera()
    {
        this.mainCamera = GetComponent<Camera>();
    }

    protected override void Start()
    {
        UpdateSize();
    }

    // [Button]
    private void UpdateSize()
    {
        this.currentWidth = Screen.width;
        this.currentHeight = Screen.height;
        this.ratioHeight = defaultHeight / currentHeight;
        this.width = currentWidth * ratioHeight;
        this.ratioWidth = defaultWidth / width;
        mainCamera.orthographicSize = defaultSize * ratioWidth;
    }
}