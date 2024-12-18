using System;
using UnityEngine;
using UnityHelper;

namespace _GameAssets._Scripts.ObjectSimulation
{
    public class TankCtrl : MonoBehaviour
    {
        [SerializeField]
        private Transform _juiceTf;

        [SerializeField]
        private float _speedSlider = 1.5f;
        
        private float _currentValueSlider = 1;
        private float _nextValueSlider = 1;
        private bool  _hasNoti;

        private void OnEnable()
        {
            EventDispatcher.Instance.RegisterListener(EventID.UpdateJuice,UpdateJuiceNoNoti);
            EventDispatcher.Instance.RegisterListener(EventID.UpdateJuiceNoti,UpdateJuiceNoti);
            EventDispatcher.Instance.RegisterListener(EventID.UpdateJuiceNoAnim,UpdateJuiceNoAnim);
        }

        private void OnDisable()
        {
            EventDispatcher.Instance.RemoveListener(EventID.UpdateJuice,UpdateJuiceNoNoti);
            EventDispatcher.Instance.RemoveListener(EventID.UpdateJuiceNoti,UpdateJuiceNoti);
            EventDispatcher.Instance.RemoveListener(EventID.UpdateJuiceNoAnim,UpdateJuiceNoAnim);
        }
        

        private void UpdateJuiceNoNoti(object obj)
        {
            _hasNoti         = false;
            _nextValueSlider = (float)obj;
        }

        private void UpdateJuiceNoti(object obj)
        {
            _hasNoti         = true;
            _nextValueSlider = (float)obj;
        }
        
        private void UpdateJuiceNoAnim(object obj)
        {
            _hasNoti            = false;
            _nextValueSlider    = (float)obj;
            _currentValueSlider = _nextValueSlider;
            _juiceTf.localScale = GetLocalScale();
        }

        private void Update()
        {
            if (_nextValueSlider - _currentValueSlider != 0)
            {
                _currentValueSlider = Mathf.MoveTowards(_currentValueSlider, _nextValueSlider, _speedSlider * Time.deltaTime);
                _juiceTf.localScale = GetLocalScale();

                if (_nextValueSlider - _currentValueSlider == 0)
                {
                    CheckAndPostNoti();
                }
            }
        }

        private void CheckAndPostNoti()
        {
            if(!_hasNoti) return;
            _hasNoti = false;
            EventDispatcher.Instance.PostEvent(EventID.FinishUpdateCapacityJuice);
        }

        private Vector3 GetLocalScale()
        {
            return new(1, 1, _currentValueSlider);
        }
        
    }
}