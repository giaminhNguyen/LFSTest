using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

namespace _GameAssets._Scripts.Loading
{
    public class ManagerInLoading : MonoBehaviour
    {
        [SerializeField]
        private string _sceneName;
        [SerializeField]
        private Image _sliderLoadingImage;

        [SerializeField]
        private float _timeLoadingMin = 1.4f;
        [Header("Loading Steps")]
        
        [SerializeField]
        private float _speedStepStart;
        [SerializeField]
        private float _speedStepEnd;

        private float _speed;
        private bool  _loadFinish;
        private float _progress;
        private void Awake()
        {
            _progress                      = 0;
            _sliderLoadingImage.fillAmount = _progress;
        }
        
        private async void Start()
        {
            SetLimitFPS(60);
            _speed = _speedStepStart;
            
            this.LoadScene_LoadAsync(_sceneName,false);
            
            while (!this.LoadScene_CanDone())
            {
                await Task.Yield();
            }
            
            await Task.Delay((int)(_timeLoadingMin * 1000));

            _speed = _speedStepEnd;
        }

        private void SetLimitFPS(int fps)
        {
            QualitySettings.vSyncCount  = 0;
            Application.targetFrameRate = fps;
        }

        private void Update()
        {
            if(_loadFinish) return;
            _progress                      = Mathf.MoveTowards(_progress, 1, _speed * Time.deltaTime);
            _sliderLoadingImage.fillAmount = _progress;

            if (_progress >= 1)
            {
                _loadFinish = true;
                this.LoadScene_Activate();
            }
        }
    }
}