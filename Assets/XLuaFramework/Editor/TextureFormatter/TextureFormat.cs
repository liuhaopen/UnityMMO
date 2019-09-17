using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;

namespace AssetFormatter {
    [Serializable]
    public class TextureFormat : ICloneable
    {
        public TextureImporterFormat format = TextureImporterFormat.Automatic;
        public int maxSize = 2048, compressionQuality = 50;
        public AudioClipLoadType loadType = AudioClipLoadType.DecompressOnLoad;

        public void Apply(TextureImporter ti, string platformName)
        {
            TextureImporterPlatformSettings settings = new TextureImporterPlatformSettings();
            settings.overridden = true;
            settings.name = platformName;
            settings.maxTextureSize = maxSize;
            settings.format = format;
            settings.compressionQuality = compressionQuality;
            settings.allowsAlphaSplitting = format == TextureImporterFormat.ETC2_RGB4_PUNCHTHROUGH_ALPHA;
            ti.SetPlatformTextureSettings(settings);
            //ti.SetPlatformTextureSettings(platformName, maxSize, format, compressionQuality, format == TextureImporterFormat.ETC2_RGB4_PUNCHTHROUGH_ALPHA);
        }

        //The options for the platform string are "Web", "Standalone", "iPhone" and "Android"
        public static string GetPlatformName(TFormatterPlatform platform)
        {
            switch (platform)
            {
                case TFormatterPlatform.Standalone:
                    return "Standalone";
                case TFormatterPlatform.Android:
                    return "Android";
                case TFormatterPlatform.iPhone:
                    return "iPhone";
                default:
                    return "Standalone";
            }
        }

        public static Enum GetPlatformEnum(int platform, int value)
        {
            switch (platform)
            {
                case 0:
                    return (StandAloneCompress)value;
                case 1:
                    return (AndroidCompress)value;
                case 2:
                    return (IosCompress)value;
                default:
                    return (StandAloneCompress)value;
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    [Serializable]
    public class FormatPathPair : ICloneable
    {
        public string name
        {
            get
            {
                if (string.IsNullOrEmpty(_name)) return string.Format("配置{0}", index);
                return _name;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _name = value;
                    index = -1;
                }
            }
        }
        [SerializeField]
        string _name = null;
        public int index;

        public ImporterType iType = ImporterType.Texture;
        [SerializeField]
        TextureFormat standaloneSet = new TextureFormat(), androidSet = new TextureFormat(), iosSet = new TextureFormat();
        public List<string> paths = new List<string>();
        public TextureImporterNPOTScale nonPowerOf2 = TextureImporterNPOTScale.None;
        public bool useMipMaps = true, isReadable = false;

        public FormatPathPair(int index)
        {
            this.index = index;
        }

        public TextureFormat GetFormatByPlatform(int platform)
        {
            switch (platform)
            {
                case 0:
                    return standaloneSet;
                case 1:
                    return androidSet;
                case 2:
                    return iosSet;
                default:
                    return standaloneSet;
            }
        }

        public bool Compare(TextureImporter ti, TextureFormat tf, string platformName)
        {
            TextureImporterFormat tif;
            int maxSize, compressionQuality;
            if (ti.GetPlatformTextureSettings(platformName, out maxSize, out tif, out compressionQuality))
            {
                return (
                    ti.mipmapEnabled == useMipMaps
                    && tif == tf.format
                    && compressionQuality == tf.compressionQuality
                    && ti.isReadable == isReadable
                    && ti.npotScale == nonPowerOf2
                );
            }
            else return false;
        }

        public void Apply(TextureImporter ti, TextureFormat tf, string platformName)
        {
            tf.Apply(ti, platformName);
            ti.mipmapEnabled = useMipMaps;
            ti.isReadable = isReadable;
            ti.npotScale = nonPowerOf2;
        }

        public object Clone()
        {
            FormatPathPair fpp = (FormatPathPair)MemberwiseClone();
            fpp.standaloneSet = (TextureFormat)standaloneSet.Clone();
            fpp.androidSet = (TextureFormat)androidSet.Clone();
            fpp.iosSet = (TextureFormat)iosSet.Clone();
            fpp.paths = new List<string>();
            for (int i = 0; i < paths.Count; i++)
            {
                fpp.paths.Add((string)paths[i].Clone());
            }
            return fpp;
        }
    }

    public enum ImporterType
    {
        Texture = 0,
        Model = 1,
        Audio = 2,
    }

    public enum TFormatterPlatform
    {
        Standalone = 0,
        Android = 1,
        iPhone = 2,
    }

    public enum StandAloneCompress
    {
        // 摘要:
        //     ///
        //     Alpha 8 bit texture format.
        //     ///
        Alpha8 = 1,

        //
        // 摘要:
        //     ///
        //     RGB 24 bit texture format.
        //     ///
        RGB24 = 3,
        //
        // 摘要:
        //     ///
        //     ARGB 32 bit texture format.
        //     ///
        RGBA32 = 4,
        //
        // 摘要:
        //     ///
        //     ARGB 32 bit texture format.
        //     ///
        ARGB16 = 2,
        //
        // 摘要:
        //     ///
        //     RGB 16 bit texture format.
        //     ///
        RGB16 = 7,
        //
        // 摘要:
        //     ///
        //     DXT1 compresed texture format.
        //     ///
        RGBCompresedDXT1 = 10,
        //
        // 摘要:
        //     ///
        //     DXT5 compresed texture format.
        //     ///
        RGBACompresedDXT5 = 12,
        //
        // 摘要:
        //     ///
        //     DXT1 compresed texture format with crunch compression for small storage sizes.
        //     ///
        DXT1Crunched = 28,
        //
        // 摘要:
        //     ///
        //     DXT5 compresed texture format with crunch compression for small storage sizes.
        //     ///
        DXT5Crunched = 29,
    }

    public enum AndroidCompress
    {
        //
        // 摘要:
        //     ///
        //     Alpha 8 bit texture format.
        //     ///
        Alpha8 = 1,
        //
        // 摘要:
        //     ///
        //     RGB 24 bit texture format.
        //     ///
        RGB24 = 3,
        //
        // 摘要:
        //     ///
        //     RGBA 32 bit texture format.
        //     ///
        RGBA32 = 4,
        //
        // 摘要:
        //     ///
        //     RGB 16 bit texture format.
        //     ///
        RGB16 = 7,
        //
        // 摘要:
        //     ///
        //     RGBA 16 bit (4444) texture format.
        //     ///
        RGBA16 = 13,
        //
        // 摘要:
        //     ///
        //     PowerVR (iPhone) 2 bits/pixel compressed color texture format.
        //     ///
        PVRTC_RGB2 = 30,
        //
        // 摘要:
        //     ///
        //     PowerVR (iPhone) 2 bits/pixel compressed with alpha channel texture format.
        //     ///
        PVRTC_RGBA2 = 31,
        //
        // 摘要:
        //     ///
        //     PowerVR (iPhone) 4 bits/pixel compressed color texture format.
        //     ///
        PVRTC_RGB4 = 32,
        //
        // 摘要:
        //     ///
        //     PowerVR (iPhone) 4 bits/pixel compressed with alpha channel texture format.
        //     ///
        PVRTC_RGBA4 = 33,
        //
        // 摘要:
        //     ///
        //     ETC (GLES2.0) 4 bits/pixel compressed RGB texture format.
        //     ///
        ETC_RGB4 = 34,
        //
        // 摘要:
        //     ///
        //     ATC (Android) 4 bits/pixel compressed RGB texture format.
        //     ///
        ATC_RGB4 = 35,
        //
        // 摘要:
        //     ///
        //     ATC (Android) 8 bits/pixel compressed RGBA texture format.
        //     ///
        ATC_RGBA8 = 36,
        //
        // 摘要:
        //     ///
        //     ETC2 compressed 4 bits / pixel RGB texture format.
        //     ///
        ETC2_RGB4 = 45,
        //
        // 摘要:
        //     ///
        //     ETC2 compressed 4 bits / pixel RGB + 1-bit alpha texture format.
        //     ///
        ETC2_RGB4_PUNCHTHROUGH_ALPHA = 46,
        //
        // 摘要:
        //     ///
        //     ETC2 compressed 8 bits / pixel RGBA texture format.
        //     ///
        ETC2_RGBA8 = 47,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGB texture format, 4x4 block size.
        //     ///
        ASTC_RGB_4x4 = 48,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGB texture format, 5x5 block size.
        //     ///
        ASTC_RGB_5x5 = 49,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGB texture format, 6x6 block size.
        //     ///
        ASTC_RGB_6x6 = 50,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGB texture format, 8x8 block size.
        //     ///
        ASTC_RGB_8x8 = 51,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGB texture format, 10x10 block size.
        //     ///
        ASTC_RGB_10x10 = 52,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGB texture format, 12x12 block size.
        //     ///
        ASTC_RGB_12x12 = 53,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGBA texture format, 4x4 block size.
        //     ///
        ASTC_RGBA_4x4 = 54,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGBA texture format, 5x5 block size.
        //     ///
        ASTC_RGBA_5x5 = 55,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGBA texture format, 6x6 block size.
        //     ///
        ASTC_RGBA_6x6 = 56,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGBA texture format, 8x8 block size.
        //     ///
        ASTC_RGBA_8x8 = 57,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGBA texture format, 10x10 block size.
        //     ///
        ASTC_RGBA_10x10 = 58,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGBA texture format, 12x12 block size.
        //     ///
        ASTC_RGBA_12x12 = 59
    }

    public enum IosCompress
    {
        //
        // 摘要:
        //     ///
        //     Alpha 8 bit texture format.
        //     ///
        Alpha8 = 1,

        //
        // 摘要:
        //     ///
        //     RGB 24 bit texture format.
        //     ///
        RGB24 = 3,
        //
        // 摘要:
        //     ///
        //     RGBA 32 bit texture format.
        //     ///
        RGBA32 = 4,

        //
        // 摘要:
        //     ///
        //     RGB 16 bit texture format.
        //     ///
        RGB16 = 7,
        //
        // 摘要:
        //     ///
        //     RGBA 16 bit (4444) texture format.
        //     ///
        RGBA16 = 13,
        //
        // 摘要:
        //     ///
        //     PowerVR (iPhone) 2 bits/pixel compressed color texture format.
        //     ///
        PVRTC_RGB2 = 30,
        //
        // 摘要:
        //     ///
        //     PowerVR (iPhone) 2 bits/pixel compressed with alpha channel texture format.
        //     ///
        PVRTC_RGBA2 = 31,
        //
        // 摘要:
        //     ///
        //     PowerVR (iPhone) 4 bits/pixel compressed color texture format.
        //     ///
        PVRTC_RGB4 = 32,
        //
        // 摘要:
        //     ///
        //     PowerVR (iPhone) 4 bits/pixel compressed with alpha channel texture format.
        //     ///
        PVRTC_RGBA4 = 33,
        //
        // 摘要:
        //     ///
        //     ATC (Android) 4 bits/pixel compressed RGB texture format.
        //     ///
        ATC_RGB4 = 35,
        //
        // 摘要:
        //     ///
        //     ATC (Android) 8 bits/pixel compressed RGBA texture format.
        //     ///
        ATC_RGBA8 = 36,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGB texture format, 4x4 block size.
        //     ///
        ASTC_RGB_4x4 = 48,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGB texture format, 5x5 block size.
        //     ///
        ASTC_RGB_5x5 = 49,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGB texture format, 6x6 block size.
        //     ///
        ASTC_RGB_6x6 = 50,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGB texture format, 8x8 block size.
        //     ///
        ASTC_RGB_8x8 = 51,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGB texture format, 10x10 block size.
        //     ///
        ASTC_RGB_10x10 = 52,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGB texture format, 12x12 block size.
        //     ///
        ASTC_RGB_12x12 = 53,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGBA texture format, 4x4 block size.
        //     ///
        ASTC_RGBA_4x4 = 54,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGBA texture format, 5x5 block size.
        //     ///
        ASTC_RGBA_5x5 = 55,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGBA texture format, 6x6 block size.
        //     ///
        ASTC_RGBA_6x6 = 56,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGBA texture format, 8x8 block size.
        //     ///
        ASTC_RGBA_8x8 = 57,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGBA texture format, 10x10 block size.
        //     ///
        ASTC_RGBA_10x10 = 58,
        //
        // 摘要:
        //     ///
        //     ASTC compressed RGBA texture format, 12x12 block size.
        //     ///
        ASTC_RGBA_12x12 = 59
    }
}
