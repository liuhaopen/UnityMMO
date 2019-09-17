using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace AssetFormatter {
    public class TFormatterSetting : ScriptableObject, ICloneable
    {

        public TextureImporterFormat standaloneDefault = TextureImporterFormat.Automatic;
        public TextureImporterFormat androidDefault = TextureImporterFormat.Automatic;
        public TextureImporterFormat iosDefault = TextureImporterFormat.Automatic;

        public List<FormatPathPair> formatList = new List<FormatPathPair>();

        public object Clone()
        {
            TFormatterSetting ts = (TFormatterSetting)MemberwiseClone();
            ts.formatList = new List<FormatPathPair>();
            for (int i = 0; i < formatList.Count; i++)
            {
                ts.formatList.Add((FormatPathPair)formatList[i].Clone());
            }
            return ts;
        }

        public void Copy(TFormatterSetting srcSetting)
        {
            standaloneDefault = srcSetting.standaloneDefault;
            androidDefault = srcSetting.androidDefault;
            iosDefault = srcSetting.iosDefault;
            formatList.Clear();
            for (int i = 0; i < srcSetting.formatList.Count; i++)
            {
                formatList.Add((FormatPathPair)srcSetting.formatList[i].Clone());
            }
        }

    }
}

