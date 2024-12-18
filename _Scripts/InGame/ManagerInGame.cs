using System;
using UnityEngine;
using UnityHelper;

namespace _GameAssets._Scripts.InGame
{
    public class ManagerInGame : MonoBehaviour
    {
        private void Start()
        {
            AudioManager.Instance.PauseMusic();
        }
    }
}