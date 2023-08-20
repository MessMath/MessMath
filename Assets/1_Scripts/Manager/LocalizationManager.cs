using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MessMathI18n
{
    public enum Language
    {
        KOREAN = 0,
        ENGLISH = 1
    }
    public enum ResLoadType
    {
        /// Serialized in scriptable object
        Serialized = 1,
        /// Load from Resources 
        Resources = 2,
        /// Load from Assetbundle
        Assetbundle = 3,
    }

    [Serializable]
    public class ResParam
    {
        public string IndexName;
        public string ResName;
    }

    [Serializable]
    public class ObjectParam : ResParam
    {
        public GameObject Prefab;
    }

    [Serializable]
    public class TextResParam : ResParam
    {
        public TextAsset TextFile;
    }

    [Serializable]
    public class TextResWithSelectionParam : TextResParam
    {
        public bool Selected;
    }

    [CreateAssetMenu(menuName = "I18n/ResSettings", fileName = "ResSettings")]
    public class LocalizationManager : ScriptableObject
    {
        private Language selectedLanguage = Language.KOREAN;
        [SerializeField]
        private ResLoadType m_LoadType;
        [SerializeField]
        private List<TextResWithSelectionParam> m_I18nFiles;

        private Func<string, UnityEngine.Object> mABSyncLoad;
        private Action<string, Action<UnityEngine.Object>> mABASyncLoad;
        private Action<string> mABUnload;

        /// Set the delegate for synchronously loading object from assetbundles
        public void SetAssetbundleSyncLoadDelegate(Func<string, UnityEngine.Object> del)
        {
            mABSyncLoad = del;
        }

        /// Set the delegate for asynchronously loading object from assetbundles
        public void SetAssetbundleASyncLoadDelegate(Action<string, Action<UnityEngine.Object>> del)
        {
            mABASyncLoad = del;
        }

        /// Set the delegate for unloading object from assetbundle
        public void SetAssetbundleUnloadDelegate(Action<string> del)
        {
            mABUnload = del;
        }

        public void SetLanguage(Language language)
        {
            selectedLanguage = language;
        }

        public Language GetSelectedLaguage()
        {
            return selectedLanguage;
        }

        #region I18n Files
        public void LoadI18n()
        {
            if (m_I18nFiles == null || m_I18nFiles.Count == 0)
            {
                Debug.LogError("LoadI18n failed. Please assign i18n files to ResSettings.asset.");
                return;
            }

            var i18nSelected = m_I18nFiles.FindAll(file => file.Selected);
            if (i18nSelected.Count == 0)
            {
                Debug.LogWarning("Please select an i18n file in ResSettings.asset. Default select \'kor\'.");
                i18nSelected.Add(m_I18nFiles.Find(file => file.IndexName == "kor"));
            }
            else if (i18nSelected.Count > 1)
            {
                //Debug.LogWarning("You have selected more than one i18n files in ResSettings.asset. The first one will be used.");
                i18nSelected.Add(m_I18nFiles.Find(file => file.IndexName == "kor"));
                i18nSelected.Add(m_I18nFiles.Find(file => file.IndexName == "en"));
            }

            var resParam = i18nSelected[(int)selectedLanguage];
            TextAsset textAsset = null;
            switch (m_LoadType)
            {
                case ResLoadType.Assetbundle:
                    if (mABSyncLoad != null)
                        textAsset = mABSyncLoad(resParam.ResName) as TextAsset;
                    break;
                case ResLoadType.Resources:
                    textAsset = Resources.Load<TextAsset>(resParam.ResName);
                    break;
                case ResLoadType.Serialized:
                    textAsset = resParam.TextFile;
                    break;
            }
            if (textAsset != null)
            {
                I18n.AddI18nFile(textAsset.text);
                if (m_LoadType == ResLoadType.Assetbundle && mABUnload != null)
                    mABUnload(resParam.ResName);
            }

            Debug.Log("Select I18n: " + resParam.IndexName);
        }
        #endregion

        private static LocalizationManager mInstance = null;
        public static LocalizationManager Get()
        {
            if (mInstance == null)
                mInstance = Resources.Load<LocalizationManager>("ResSettings");
            if (mInstance == null)
                throw new Exception("There is no \"ResSettings\" ScriptObject under Resources folder");

            return mInstance;
        }

        public static void Dispose()
        {
            mInstance = null;
        }
    }
}