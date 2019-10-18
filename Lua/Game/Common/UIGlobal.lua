Util = CS.XLuaFramework.Util
AppConfig = CS.XLuaFramework.AppConfig
ResMgr = CS.XLuaFramework.ResourceManager.GetInstance()
NetMgr = CS.XLuaFramework.NetworkManager.GetInstance()
UIHelper = CS.XLuaFramework.UIHelper
cookiesManager = CS.XLuaFramework.CookiesManager.GetInstance()
CSLuaBridge = CS.XLuaFramework.CSLuaBridge
GameObject = CS.UnityEngine.GameObject
GameConst = CS.UnityMMO.GameConst
GameVariable = CS.UnityMMO.GameVariable
SceneMgr = CS.UnityMMO.SceneMgr
TextAnchor = CS.UnityEngine.TextAnchor
Camera = CS.UnityEngine.Camera
SceneHelper = CS.UnityMMO.SceneHelper
RoleMgr = CS.UnityMMO.RoleMgr
MonsterMgr = CS.UnityMMO.MonsterMgr
NPCMgr = CS.UnityMMO.NPCMgr
LogError = CS.UnityEngine.Debug.LogError
Entity = CS.Unity.Entities.Entity
SkillManager = CS.UnityMMO.SkillManager
Cocos = CS.Cocos

LRoleMgr = require("Game.Scene.LRoleMgr")
ErrorCode = require("Game.Error.ErrorCode")
ColorUtil = require("Game.Common.ColorUtil")
require "Common.UI.UIHelper"
require "Common.Util.TableUtil"

GlobalEvents = require("Game.Common.GlobalEvents")
EventDispatcher = require("Game.Common.EventDispatcher")

UI.AlertView = require("Game.Common.AlertView")
MainRole = require("Game.Scene.MainRole")
UINode = require("Common.UI.UINode")
UILooksNode = require("Common.UI.UILooksNode")
GoodsItem = require("Game.Common.UI.GoodsItem")

ResPath = require("Game.Common.ResPath")
ConfigMgr = require("Tools.ConfigMgr")
NoError = 0

UI.Window = require("Game.Common.UI.Window")