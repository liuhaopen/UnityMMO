--下面基础组件间的require有依赖顺序相关,闲着没事也别换顺序,要加新的往文件尾加就好
Util = CS.XLuaFramework.Util
AppConfig = CS.XLuaFramework.AppConfig
ResMgr = CS.XLuaFramework.ResourceManager.GetInstance()
NetMgr = CS.XLuaFramework.NetworkManager.GetInstance()
UIHelper = CS.XLuaFramework.UIHelper
GameObject = CS.UnityEngine.GameObject
GameConst = CS.UnityMMO.GameConst
GameVariable = CS.UnityMMO.GameVariable
SceneMgr = CS.UnityMMO.SceneMgr

Mathf		= require "Common.UnityEngine.Mathf"
Vector2		= require "Common.UnityEngine.Vector2"
Vector3 	= require "Common.UnityEngine.Vector3"
Vector4		= require "Common.UnityEngine.Vector4"
Quaternion	= require "Common.UnityEngine.Quaternion"
Color		= require "Common.UnityEngine.Color"
Ray			= require "Common.UnityEngine.Ray"
Bounds		= require "Common.UnityEngine.Bounds"
RaycastHit	= require "Common.UnityEngine.RaycastHit"
Touch		= require "Common.UnityEngine.Touch"
LayerMask	= require "Common.UnityEngine.LayerMask"
Plane		= require "Common.UnityEngine.Plane"
Time		= require "Common.UnityEngine.Time"
Object		= require "Common.UnityEngine.Object"

require("Common/BaseClass")
require("Common.Util.util")
require("Common.Util.LuaUtil")
list = require("Common.Util.list")
require("Common.Util.event")
require("Common.Util.Timer")
UpdateManager = require "Common.UpdateManager"
require "Common.GlobalEventSystem"

require("Common.Action.ActionTweenFunction")
require("Common.Action.Action")
require("Common.Action.ActionInterval")
require("Common.Action.ActionEase")
require("Common.Action.ActionInstant")
require("Common.Action.ActionManager")
require("Common.Action.ActionExtend")
require("Common.Action.ActionCatmullRom")
require("Common.Action.ActionNodeWrapper")

--顺序无关的
require("Common.Util.Functor")


