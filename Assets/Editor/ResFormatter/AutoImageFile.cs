using UnityEngine;
using UnityEditor;
using System.Xml;
using System.IO;


public class AutoImageFile : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        //自动设置为Sprite2D类型 
        TextureImporter textureImporter = (TextureImporter)assetImporter;
        textureImporter.textureType = TextureImporterType.Sprite;
    }
   
}
