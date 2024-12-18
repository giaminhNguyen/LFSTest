using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;
using VInspector;
using EventDispatcher = UnityHelper.EventDispatcher;

namespace _GameAssets._Scripts.InGame.UI
{
    public class CapacityController : MonoBehaviour
    {
        [Header("Capacity")]
        [SerializeField,Variants("Text","Slider")]
        private string _type;
        
        [SerializeField]
        private TMP_Text _capacityText;
        
        [SerializeField]
        private Image _capacitySliderImage;

        [SerializeField]
        private float _speedSlider = 3;

        private float _currentValueSlider;
        private float _nextValueSlider;
        private bool  _hasNoti;

        private void OnEnable()
        {
            EventDispatcher.Instance.RegisterListener(EventID.UpdateEnergy, UpdateEnergyNoNoti);
            EventDispatcher.Instance.RegisterListener(EventID.UpdateEnergyNoti, UpdateEnergyNoti);
            EventDispatcher.Instance.RegisterListener(EventID.UpdateEnergyNoAnim, UpdateEnergyNoAnim);
        }
        
        private void OnDisable()
        {
            EventDispatcher.Instance.RemoveListener(EventID.UpdateEnergy, UpdateEnergyNoNoti);
            EventDispatcher.Instance.RemoveListener(EventID.UpdateEnergyNoti, UpdateEnergyNoti);
            EventDispatcher.Instance.RemoveListener(EventID.UpdateEnergyNoAnim, UpdateEnergyNoAnim);
        }
        

        private void Update()
        {
            if (_nextValueSlider - _currentValueSlider != 0)
            {
                _currentValueSlider = Mathf.MoveTowards(_currentValueSlider, _nextValueSlider, _speedSlider * Time.deltaTime);
                _capacitySliderImage.fillAmount = _currentValueSlider;

                if (_nextValueSlider - _currentValueSlider == 0)
                {
                    CheckAndPostNoti();
                }
            }
        }

        private void UpdateEnergyNoti(object obj)
        {
            _hasNoti = true;
            UpdateEnergy(obj);
        }

        private void UpdateEnergyNoNoti(object obj)
        {
            _hasNoti = false;
            UpdateEnergy(obj);
        }
        
        private void UpdateEnergyNoAnim(object obj)
        {
            _hasNoti = false;
            if (_type.Equals("Text"))
            {
                var bullet = (int)obj;
                _capacityText.text = $"x {bullet}";
            }
            else
            {
                _currentValueSlider             = (float)obj;
                _nextValueSlider                = _currentValueSlider;
                _capacitySliderImage.fillAmount = _currentValueSlider;
            }
        }
        
        private void UpdateEnergy(object obj)
        {
            if (_type.Equals("Text"))
            {
                var bullet = (int)obj;
                _capacityText.text = $"x {bullet}";
                CheckAndPostNoti();
            }
            else
            {
                _nextValueSlider                = (float)obj;
            }
        }

        private void CheckAndPostNoti()
        {
            if(!_hasNoti) return;
            _hasNoti = false;
            EventDispatcher.Instance.PostEvent(EventID.FinishUpdateCapacity);
        }
        
    }
}