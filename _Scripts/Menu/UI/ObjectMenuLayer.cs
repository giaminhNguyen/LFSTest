using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class ObjectMenuLayer : LayerBase
{
    #region Properties
    [Header("Reference")]
    [SerializeField]
    private Transform _objectListContent;
    
    [SerializeField]
    private CardItemObjectMenu _cardPrefab;
    
    private List<CardItemObjectMenu> _cardItems = new();

    private bool _isSet45Degrees;
    
    #endregion
    
    
    private void CreateCardItem(DataBase[] objectData)
    {
        if (objectData == null)
        {
            Debug.LogError("ObjectData is null");
            return;
        }

        var childLength = _cardItems.Count;
        var i           = 0;
        var cardPrefab  = _cardPrefab;

        for (;i < objectData.Length; i++)
        {
            CardItemObjectMenu child;
            if (i < childLength)
            {
                child = _cardItems[i];
                child.gameObject.SetActive(true);
            }
            else
            {
                child = Instantiate(cardPrefab, _objectListContent);
                _cardItems.Add(child);
            }
            child.SetUp(i, objectData[i].icon);
            child.Rotate45Degrees(_isSet45Degrees);
        }
        
        for(;i < _cardItems.Count; i++)
        {
            _cardItems[i].gameObject.SetActive(false);
        }
    }

    protected override void InitAwake()
    {
    }

    protected override void InitOnEnable()
    {
    }

    protected override void InitStart()
    {
    }

    public override void Init()
    {
        DataBase[] objectData;

        switch (EventManager.getSelectedObjectType())
        {
            case ObjectSimulationType.VapeAndPod:
                _isSet45Degrees = false;
                objectData      = DataGame.Instance.VapeAndPodData;
                break;
            case ObjectSimulationType.MachineGun:
                _isSet45Degrees = true;
                objectData      = DataGame.Instance.MachineData;
                break;
            case ObjectSimulationType.ScifiGun: 
                _isSet45Degrees = true;
                objectData      = DataGame.Instance.ScifiData;
                break;
            default: 
                _isSet45Degrees = false;
                objectData      = DataGame.Instance.LightSaberData;
                break;
        }
        CreateCardItem(objectData);
    }

    public override void Open()
    {
        base.Open();
        _content.SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        _content.SetActive(false);
    }
}