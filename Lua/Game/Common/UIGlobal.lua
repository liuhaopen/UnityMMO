Util = CS.XLuaFramework.Util
AppConfig = CS.XLuaFramework.AppConfig
ResMgr = CS.XLuaFramework.ResourceManager.GetInstance()
NetMgr = CS.XLuaFramework.NetworkManager.GetInstance()
UIHelper = CS.XLuaFramework.UIHelper
cookiesManager = CS.XLuaFramework.CookiesManager.GetInstance()
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

Entity = CS.Unity.Entities.Entity

require "Common.UI.UIHelper"
-- SkillMgr = CS.UnityMMO.SkillManager
require "Common.Util.TableUtil"
GlobalEvents = require("Game.Common.GlobalEvents")
EventDispatcher = require("Game.Common.EventDispatcher")

UI.AlertView = require("Game.Common.AlertView")
MainRole = require("Game.Scene.MainRole")
print('Cat:UIGlobal.lua[4] UI.AlertView', UI.AlertView)

UINode = require("Common.UI.UINode")
UILooksNode = require("Common.UI.UILooksNode")
GoodsItem = require("Game.Common.UI.GoodsItem")

ResPath = require("Game.Common.ResPath")

ConfigMgr = require("Tools.ConfigMgr")
NoError = 0