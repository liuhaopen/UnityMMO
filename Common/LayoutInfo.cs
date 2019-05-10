#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace U3DExtends
{
    [RequireComponent(typeof(Canvas))]
    [ExecuteInEditMode]
    public class LayoutInfo : MonoBehaviour
    {
        //[HideInInspector]
        [SerializeField]
        private string _layoutPath = string.Empty;
        public static bool IsShowLayoutName = false;

        Vector3 _lastRealLayoutPos = new Vector3(-1, -1);
        Vector2 _lastRealLayoutSize = Vector2.zero;
        UnityEngine.UI.Text _viewNameLabel = null;

        const string RealPosStartStr = "RealLayoutPosStart ";
        const string RealPosEndStr = " RealLayoutPosEnd\n";

        static string configPath = string.Empty;
        static string ConfigPath
        {
            get
            {
                if (configPath == string.Empty)
                    configPath = Application.temporaryCachePath + "/Decorates";
                return configPath;
            }
        }

        public string LayoutPath
        {
            get
            {
                return _layoutPath;
            }

            set
            {
                _layoutPath = value;
            }
        }

        public GameObject EditingView
        {
            get
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    if (child.GetComponent<Decorate>() != null || (_viewNameLabel!=null && _viewNameLabel.transform==child))
                        continue;
                    return child.gameObject;
                }
                return null;
            }
        }

        private void Start() {
            Transform name_trans = transform.Find("ViewName");
            if (name_trans!=null)
                _viewNameLabel = name_trans.GetComponent<UnityEngine.UI.Text>();
        }

        //本来坐标或大小变更时也需要再保存一下,但是需要监听pos size等变更又要io保存,怕太多参照图时影响性能,所以还是界面保存时再一起保存吧
        public bool SaveToConfigFile()
        {
            string select_path = FileUtil.GetProjectRelativePath(LayoutPath);
            string layout_path_md5 = UIEditorHelper.GenMD5String(select_path);
            RectTransform real_layout = UIEditorHelper.GetRealLayout(gameObject) as RectTransform;//先拿到真实的界面prefab
            if (select_path == "" || real_layout == null)
            {
                //界面还未保存,等保存时再调用本函数
                return false;
            }
            RectTransform curTrans = transform as RectTransform;
            bool hadDecorateTransChanged = false;
            bool hadTransChanged = true;
            if (real_layout.localPosition == _lastRealLayoutPos && real_layout.sizeDelta == _lastRealLayoutSize)
                hadTransChanged = false;
            _lastRealLayoutPos = real_layout.localPosition;
            _lastRealLayoutSize = real_layout.sizeDelta;
            if (!Directory.Exists(ConfigPath))
                Directory.CreateDirectory(ConfigPath);
            string savePath = ConfigPath + "/" + layout_path_md5 + ".txt";
            StringBuilder content = new StringBuilder();
            content.Append(RealPosStartStr);
            content.Append(real_layout.localPosition.x.ToString());
            content.Append(' ');
            content.Append(real_layout.localPosition.y.ToString());
            content.Append(RealPosEndStr);
            Decorate[] decorates = transform.GetComponentsInChildren<Decorate>();
            for (int i = 0; i < decorates.Length; i++)
            {
                RectTransform rectTrans = decorates[i].GetComponent<RectTransform>();
                if (rectTrans != null)
                {
                    content.Append(decorates[i].SprPath);
                    content.Append('#');
                    content.Append(rectTrans.localPosition.x.ToString());
                    content.Append(' ');
                    content.Append(rectTrans.localPosition.y.ToString());
                    content.Append('#');
                    content.Append(rectTrans.sizeDelta.x.ToString());
                    content.Append(' ');
                    content.Append(rectTrans.sizeDelta.y.ToString());
                    content.Append('*');//分隔不同的参照图
                    if (decorates[i].IsChangedTrans())
                    {
                        decorates[i].SaveTrans();
                        hadDecorateTransChanged = true;
                    }
                }
            }
            if (hadTransChanged || hadDecorateTransChanged)
            {
                if (content[content.Length - 1] == '*')
                    content.Remove(content.Length - 1, 1);//删掉最后一个分隔符
                File.WriteAllText(savePath, content.ToString());
                return true;
            }
            //当真实界面的坐标和参照图的变换没变的话就不需要保存了
            return false;
        }

        public Decorate GetDecorateChild(string picPath)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                Decorate decor = child.GetComponent<Decorate>();
                if (decor != null && decor.SprPath == picPath)
                {
                    return decor;
                }
            }
            return null;
        }

        //打开界面时,从项目临时文件夹找到对应界面的参照图配置,然后生成参照图
        public void ApplyConfig(string view_path)
        {
            string layout_path_md5 = UIEditorHelper.GenMD5String(view_path);
            string confighFilePath = ConfigPath + "/" + layout_path_md5 + ".txt";
            if (!File.Exists(confighFilePath))
                return;
            string content = File.ReadAllText(confighFilePath);
            int pos_end_index = content.IndexOf(RealPosEndStr);
            if (pos_end_index == -1)
            {
                Debug.Log("cannot find real layout pos config on ApplyConfig : " + view_path);
                return;
            }
            string real_layout_pos_str = content.Substring(RealPosStartStr.Length, pos_end_index - RealPosStartStr.Length);
            string[] pos_cfg = real_layout_pos_str.Split(' ');
            if (pos_cfg.Length == 2)
            {
                RectTransform real_layout = UIEditorHelper.GetRealLayout(gameObject) as RectTransform;//先拿到真实的界面prefab
                if (real_layout == null)
                {
                    Debug.Log("cannot find real layout on ApplyConfig : " + view_path);
                    return;
                }
                real_layout.localPosition = new Vector3(float.Parse(pos_cfg[0]), float.Parse(pos_cfg[1]), real_layout.localPosition.z);
            }
            else
            {
                Debug.Log("cannot find real layout pos xy config on ApplyConfig : " + view_path);
                return;
            }
            content = content.Substring(pos_end_index + RealPosEndStr.Length);
            if (content == "")
                return;//有些界面没参考图也是正常的,直接返回
            string[] decorate_cfgs = content.Split('*');
            for (int i = 0; i < decorate_cfgs.Length; i++)
            {
                string[] cfgs = decorate_cfgs[i].Split('#');
                if (cfgs.Length == 3)
                {
                    string decorate_img_path = cfgs[0];
                    if (!File.Exists(decorate_img_path))
                    {
                        Debug.Log("LayoutInfo:ApplyConfig() cannot find decorate img file : " + decorate_img_path);
                        continue;
                    }
                    Decorate decor = GetDecorateChild(decorate_img_path);
                    if (decor == null)
                        decor = UIEditorHelper.CreateEmptyDecorate(transform);
                    decor.SprPath = decorate_img_path;
                    RectTransform rectTrans = decor.GetComponent<RectTransform>();
                    if (rectTrans != null)
                    {
                        //IFormatter formatter = new BinaryFormatter();//使用序列化工具的话就可以保存多点信息,但实现复杂了,暂用简单的吧
                        string[] pos = cfgs[1].Split(' ');
                        if (pos.Length == 2)
                            rectTrans.localPosition = new Vector2(float.Parse(pos[0]), float.Parse(pos[1]));

                        string[] size = cfgs[2].Split(' ');
                        if (size.Length == 2)
                            rectTrans.sizeDelta = new Vector2(float.Parse(size[0]), float.Parse(size[1]));
                    }
                }
                else
                {
                    Debug.Log("warning : detect a wrong decorate config file!");
                    return;
                }
            }
        }

        private void OnDrawGizmos() {
            if (_viewNameLabel==null)
                return;
            // bool is_show_name = Event.current!=null && (Event.current.control) && !Event.current.alt && !Event.current.shift;
            if (IsShowLayoutName)
            {
                string show_name = transform.name.Substring(0, transform.name.Length-("_Canvas").Length);
                _viewNameLabel.text = show_name;
                _viewNameLabel.transform.SetAsLastSibling();
                _viewNameLabel.gameObject.SetActive(true);
            }
            else
            {
                _viewNameLabel.gameObject.SetActive(false);
            }
        }
      
    }
}
#endif