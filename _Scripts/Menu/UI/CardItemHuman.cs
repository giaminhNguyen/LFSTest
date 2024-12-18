using System;
using ComponentUtilitys;
using UnityEngine;
using UnityEngine.UI;

namespace _GameAssets._Scripts.Menu.UI
{
    public class CardItemHuman : CardBase
    {
        [SerializeField]
        private GameObject _on;
        
        [SerializeField]
        private GameObject _off;

        private void OnEnable()
        {
            EventManager.selectedHumanIndex += SelectedHuman;
        }

        private void OnDisable()
        {
            EventManager.selectedHumanIndex -= SelectedHuman;
        }

        private void SelectedHuman(int obj)
        {
            SetState(obj == _cardID);
        }

        public override void SetUp(int id, Sprite sprite)
        {
            base.SetUp(id, sprite);
            SetState(EventManager.getSelectedHumanIndex() == _cardID);
        }

        public override void OnClick()
        {
            EventManager.selectedHumanIndex?.Invoke(_cardID);
        }
        
        private void SetState(bool state)
        {
            _on.SetActive(state);
            _off.SetActive(!state);
        }
        
    }
}