using ComponentUtilitys;
using UnityEngine;
using UnityEngine.UI;

public class CardItemObjectMenu : CardBase
{
    [SerializeField]
    private Image _cardImageRotate45Degrees;
    
    public void Rotate45Degrees(bool isSet)
    {
        _cardImage.enabled                = !isSet;
        _cardImageRotate45Degrees.enabled = isSet;
    }

    protected override void SetCardImage(Sprite sprite)
    {
        base.SetCardImage(sprite);
        _cardImageRotate45Degrees.sprite = sprite;
    }

    public override void OnClick()
    {
        EventManager.selectedObjectIndex?.Invoke(_cardID);
        EventManager.goToGamePlayScene?.Invoke();
    }
}