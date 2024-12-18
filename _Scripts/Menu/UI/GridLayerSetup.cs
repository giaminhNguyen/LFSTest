using System;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class GridLayerSetup : MonoBehaviour
{
    [Foldout("Info")]
    [SerializeField,Variants(0,1)]
    protected int _match = 1;
    [SerializeField]
    protected Vector2 _resolutionReference = new (1080,1920);
    [EndFoldout]
    [Header("Reference")]
    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup;

    [Header("SetUp")]
    [SerializeField]
    private int _columnCountMax = 6;
    [SerializeField]
    private int _columnCountPrefer = 3;
    [SerializeField]
    private int _cellWidthMax;
    [SerializeField]
    private int _cellWidthMin;
    [SerializeField]
    private int _spacing;
    
    private Vector2 _screenCalculate;
    

    private void Awake()
    {
        CalculateScreenSize();
        SetUp();
    }
    
    private void CalculateScreenSize()
    {
        var screenHeightCalculate = 0f;
        var screenWidthCalculate  = 0f;
            
        if (_match - 1 == 0)
        {
            screenHeightCalculate = _resolutionReference.y;
            var ratio = screenHeightCalculate / Screen.height;
            screenWidthCalculate = Screen.width * ratio;
        }
        else
        {
            screenWidthCalculate = _resolutionReference.x;
            var ratio = screenWidthCalculate / Screen.width;
            screenHeightCalculate = Screen.height * ratio;
        }
        _screenCalculate = new Vector2(screenWidthCalculate, screenHeightCalculate);
    }

    [Button]
    private void SetUp()
    {
        var screenWidth = (int)_screenCalculate.x;
        int widthCell   = _cellWidthMax + _spacing;
        var columnCount = screenWidth / (widthCell);
        if(columnCount < _columnCountPrefer)
        {
            columnCount = _columnCountPrefer;
            widthCell   = Mathf.Max(screenWidth / _columnCountPrefer,_cellWidthMin);
        }else if (columnCount > _columnCountMax)
        {
            columnCount = _columnCountMax;
        }

        widthCell -= _spacing;
        _gridLayoutGroup.cellSize = new Vector2(widthCell, widthCell + 100);
        _gridLayoutGroup.constraintCount = columnCount;
    }
}