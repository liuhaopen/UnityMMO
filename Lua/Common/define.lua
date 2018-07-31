Util = LuaFramework.Util
AppConst = LuaFramework.AppConst
LuaHelper = LuaFramework.LuaHelper
UIHelper = LuaFramework.UIHelper
ByteBuffer = LuaFramework.ByteBuffer

ResMgr = LuaHelper.GetResManager()
PanelMgr = LuaHelper.GetPanelManager()
SoundMgr = LuaHelper.GetSoundManager()
NetMgr = LuaHelper.GetNetManager()

WWW = UnityEngine.WWW
GameObject = UnityEngine.GameObject

require "Common/BaseClass"
require("action/ActionTweenFunction")
require("action/Action")
require("action/ActionInterval")
require("action/ActionEase")
require("action/ActionInstant")
require("action/ActionManager")
require("action/ActionExtend")
require("action/ActionCatmullRom")
require("action/ActionNodeWrapper")