using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

namespace _GameAssets._Scripts.Menu.UI
{
    public class ChangeHumanLayer : LayerBase
    {
        #region Properties

        [SerializeField]
        private CardItemHuman _cardItemHumanPrefab;

        [SerializeField]
        private Transform _contentList;
        
        [SerializeField]
        private Image _contentHuman;

        private List<CardItemHuman> _cardItemHumans;
        
        #endregion
        protected override void InitAwake()
        {
        }

        protected override void InitOnEnable()
        {
            EventDispatcher.Instance.RegisterListener(EventID.SelectedHuman, SelectedHuman);
        }

        private void OnDisable()
        {
            EventDispatcher.Instance.RemoveListener(EventID.SelectedHuman, SelectedHuman);
        }

        private void SelectedHuman(object obj)
        {
            var human = DataGame.Instance.GetHumanData(EventManager.getSelectedHumanIndex());
            _contentHuman.sprite = human.imageCollection;
        }

        protected override void InitStart()
        {
            
        }

        public override void Init()
        {
            if(!_hasContent || !DataGame.Instance) return;

            if (_cardItemHumans.IsNullOrEmpty())
            {
                _cardItemHumans = new();
            }
            
            var length = _cardItemHumans.Count;
            
            for(var i = 0; i < DataGame.Instance.HumanData.Length; i++)
            {
                var           human         = DataGame.Instance.GetHumanData(i);
                CardItemHuman cardItemHuman;
                
                if(i < length)
                {
                    cardItemHuman = _cardItemHumans[i];
                }
                else
                {
                    cardItemHuman = Instantiate(_cardItemHumanPrefab, _contentList);
                    _cardItemHumans.Add(cardItemHuman);
                }
                
                cardItemHuman.SetUp(i, human.icon);
            }
        }

        public override void Open()
        {
            SelectedHuman(null);
            base.Open();
            _content.SetActive(true);
        }

        public override void Close()
        {
            base.Close();
            _content.SetActive(false);
        }
    }
}