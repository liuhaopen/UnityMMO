
-- CtrlNames = {
-- 	Prompt = "PromptCtrl",
-- 	Message = "MessageCtrl"
-- }

-- PanelNames = {
-- 	"PromptPanel",	
-- 	"MessagePanel",
-- }

--协议类型--
-- ProtocalType = {
-- 	BINARY = 0,
-- 	PB_LUA = 1,
-- 	PBC = 2,
-- 	SPROTO = 3,
-- }
--当前使用的协议类型--
-- TestProtoType = ProtocalType.BINARY;

Util = LuaFramework.Util;
AppConst = LuaFramework.AppConst;
LuaHelper = LuaFramework.LuaHelper;
UIHelper = LuaFramework.UIHelper;
ByteBuffer = LuaFramework.ByteBuffer;

ResMgr = LuaHelper.GetResManager();
PanelMgr = LuaHelper.GetPanelManager();
SoundMgr = LuaHelper.GetSoundManager();
NetMgr = LuaHelper.GetNetManager();

WWW = UnityEngine.WWW;
GameObject = UnityEngine.GameObject;

-- local old_assert = assert
-- assert = function ( statement, comment )
-- 	print('Cat:define.lua[38]', statement)
-- 	if not statement then
-- 		Util.LogError(comment.." in "..debug.traceback())
-- 	end
-- 	old_assert(statement, comment)
-- end