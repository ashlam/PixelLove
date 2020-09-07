using UnityEngine;
using System.Collections;
using UnityEditor;

namespace TeaSoft.Code.Sample
{
    public class Editor_Audio_SoundAsset
    {
        [MenuItem("Asset/Create/SoundAsset")]
        public static void CreateAsset()
        {
            TeaSoft.CustomAssetUtility.CreateAsset<Proty_Asset_SoundConfig>();
        }
    }
}