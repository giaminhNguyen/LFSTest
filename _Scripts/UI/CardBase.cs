using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

namespace ComponentUtilitys
{
    public abstract class CardBase : MonoBehaviour,IOnClick
    {
        [SerializeField]
        protected Image _cardImage;
        
        protected int _cardID;
        
        public virtual void SetUp(int id, Sprite sprite)
        {
            _cardID = id;
            SetCardImage(sprite);
        }
        
        protected virtual void SetCardImage(Sprite sprite)
        {
            _cardImage.sprite = sprite;
        }

        public abstract void OnClick();
    }
}