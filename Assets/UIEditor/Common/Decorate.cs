#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;

namespace U3DExtends
{
    [RequireComponent(typeof(RectTransform))]
    [ExecuteInEditMode]
    public class Decorate : MonoBehaviour
    {
        string spr_path = "";
        [SerializeField]
        [HideInInspector]
        private Image _image;

        public string SprPath
        {
            get { return spr_path; }
            set 
            {
                LoadSpr(value);
            }
        }

        public void LoadSpr(string path)
        {
            InitComponent();
            //Debug.Log("path : " + path);
            if (spr_path != path)
            {
                spr_path = path;
                _image.sprite = UIEditorHelper.LoadSpriteInLocal(path);
                _image.SetNativeSize();
                gameObject.name = CommonHelper.GetFileNameByPath(path);
                //Debug.Log("_image.sprite :" + (_image.sprite != null).ToString());
            }
        }

        protected void Start()
        {
            InitComponent();
        }

        protected void InitComponent()
        {
            if (_image == null)
                _image = GetComponent<Image>();
        }


    }
}
#endif