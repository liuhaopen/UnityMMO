using UnityEngine;
using UnityEditor;

namespace U3DExtends
{
    [CustomEditor(typeof(Decorate))]
    public class DecorateEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Decorate widget = target as Decorate;
            if (GUILayout.Button("加载外部图片", GUILayout.Height(30)))
            {
                UIEditorHelper.SelectPicForDecorate(widget);
            }
        }
    }
    
    //Cat!TODO:给按钮文本等控件增加切换样式的功能
}