using System;
using UnityEngine;

namespace _GameAssets._Scripts.InGame.UI
{
    public class UpdateSizeObjectUI : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _rawImageRtf;

        private void Awake()
        {
            var height = Screen.height;

            // _rawImageRtf.sizeDelta = new Vector2(height, height);

        }
    }
}