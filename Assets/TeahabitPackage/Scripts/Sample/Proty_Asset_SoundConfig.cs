using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeaSoft.Code.Sample
{

    public class Proty_Asset_SoundConfig : ScriptableObject
    {
        public System.Collections.Generic.List<SoundConfigData> soundDatas;


        internal void PlaySound(GlobalSoundType t)
        {
            //Proty_Audio_Sound tempSound = null;
            //foreach (SoundConfigData x in soundDatas)
            //{
            //    if (x.soundType == t)
            //    {
            //        tempSound = x.sound;
            //        break;
            //    }
            //}
            //Manager_Game_Audio.CreateInstance().CreateSound(tempSound);
        }

    }

    [System.Serializable]
    public class SoundConfigData
    {
        public GlobalSoundType soundType;
        //public Proty_Audio_Sound sound;
    }


    public enum GlobalSoundType
    {
        None,
        ClickButton,
        StartingWindow,
        BiSha
    }
}