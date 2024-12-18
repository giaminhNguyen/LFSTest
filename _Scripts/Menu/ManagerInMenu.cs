using System;
using UnityEngine;
using UnityHelper;

namespace _GameAssets._Scripts.Menu
{
    public class ManagerInMenu : MonoBehaviour
    {
        private void Start()
        {
            if (!AudioManager.Instance.MusicState)
            {
                AudioManager.Instance.PlayMusic();
            }
            
        }
    }
}