--下面基础组件间的require有依赖顺序相关,闲着没事也别换顺序,要加新的往文件尾加就好
Util = CS.XLuaFramework.Util
AppConfig = CS.XLuaFramework.AppConfig
ResMgr = CS.XLuaFramework.ResourceManager.GetInstance()
UIHelper = CS.XLuaFramework.UIHelper
GameObject = CS.UnityEngine.GameObject

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
require("Common.util.util")
require("Common.util.LuaUtil")
list = require("Common.util.list")
require("Common.util.event")
require("Common.util.Timer")
UpdateManager = require "Common.UpdateManager"

require("Common.action.ActionTweenFunction")
require("Common.action.Action")
require("Common.action.ActionInterval")
require("Common.action.ActionEase")
require("Common.action.ActionInstant")
require("Common.action.ActionManager")
require("Common.action.ActionExtend")
require("Common.action.ActionCatmullRom")
require("Common.action.ActionNodeWrapper")

