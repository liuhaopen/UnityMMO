using UnityEditor;
using UnityEngine;

public class AutoSpriteFormat : AssetPostprocessor
{
    private void OnPostprocessTexture(Texture2D texture)
    {
        if (assetPath.ToLower().IndexOf("assets/assetbundleres/ui/") != -1)
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spriteImportMode = SpriteImportMode.Single;
            textureImporter.alphaIsTransparency = true;
            textureImporter.mipmapEnabled = false;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
