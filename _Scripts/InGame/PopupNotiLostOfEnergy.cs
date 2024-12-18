using TMPro;
using UnityEngine;
using UnityHelper;

namespace _GameAssets._Scripts.InGame
{
    public class PopupNotiLostOfEnergy : LayerPopup
    {
        [SerializeField]
        private TextMeshProUGUI _textContent;

        [SerializeField]
        private TextMeshProUGUI _textButtonReload;
        [Header("--")]
        [SerializeField]
        private string _textContentVapeObject;
        [SerializeField]
        private string _textButtonVapeObject;
        
        [SerializeField]
        private string _textContentGunObject;
        [SerializeField]
        private string _textButtonGunObject;
        
        [SerializeField]
        private string _textContentLightSaberObject;
        [SerializeField]
        private string _textButtonLightSaberObject;
        

        protected override void Start()
        {
            base.Start();

            switch (EventManager.getSelectedObjectType())
            {
                case ObjectSimulationType.VapeAndPod: 
                    _textContent.text = _textContentVapeObject;
                    _textButtonReload.text = _textButtonVapeObject;
                    break;
                case ObjectSimulationType.MachineGun: 
                    _textContent.text      = _textContentGunObject;
                    _textButtonReload.text = _textButtonGunObject;
                    break;
                case ObjectSimulationType.ScifiGun: 
                    _textContent.text      = _textContentGunObject;
                    _textButtonReload.text = _textButtonGunObject;
                    break;
                
                case ObjectSimulationType.LightSaber: 
                    _textContent.text      = _textContentLightSaberObject;
                    _textButtonReload.text = _textButtonLightSaberObject;
                    break;
                
            }
        }
    }
}