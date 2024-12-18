using UnityEngine;

namespace UnityHelper
{
    public static class DataSaveGame
    {
        public static bool ActiveMusic
        {
            get => PlayerPrefs.GetInt("ActiveMusic", 1) == 1;
            set => PlayerPrefs.SetInt("ActiveMusic", value ? 1 : 0);
        }
        
        public static bool ActiveSFX
        {
            get => PlayerPrefs.GetInt("ActiveSFX", 1) == 1;
            set => PlayerPrefs.SetInt("ActiveSFX", value ? 1 : 0);
        }
        
        public static bool ActiveVibrate
        {
            get => PlayerPrefs.GetInt("ActiveVibrate", 0) == 1;
            set => PlayerPrefs.SetInt("ActiveVibrate", value ? 1 : 0);
        }
        
        public static bool ActiveFlash
        {
            get => PlayerPrefs.GetInt("ActiveFlash", 1) == 1;
            set => PlayerPrefs.SetInt("ActiveFlash", value ? 1 : 0);
        }
        
        public static bool ActiveScreenFlash
        {
            get => PlayerPrefs.GetInt("ActiveScreenFlash", 1) == 1;
            set => PlayerPrefs.SetInt("ActiveScreenFlash", value ? 1 : 0);
        }

        public static int HumanIndexSelected
        {
            get => PlayerPrefs.GetInt("HumanIndexSelected", 0);
            set => PlayerPrefs.SetInt("HumanIndexSelected", value);
        }
        
        public static bool IsAcceptNotiAndroid13
        {
            get => PlayerPrefs.GetInt("IsAcceptNotiAndroid13", 0) == 1;
            set => PlayerPrefs.SetInt("IsAcceptNotiAndroid13", value ? 1 : 0);
        }
        
        
    }
}