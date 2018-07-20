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

-- local old_assert = assert
-- assert = function ( statement, comment )
-- 	print('Cat:define.lua[38]', statement)
-- 	if not statement then
-- 		Util.LogError(comment.." in "..debug.traceback())
-- 	end
-- 	old_assert(statement, comment)
-- end