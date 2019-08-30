local Ac = require "Action"
local Attr = require "game.scene.fight.Attr"
local NotifyBuff = require "game.scene.fight.NotifyBuff"
local SuckHP = require "game.scene.fight.SuckHP"
local If = Ac.If
local Delay = Ac.Delay
local Random = Ac.Random
local Sequence = Ac.Sequence
local CallFunc = Ac.CallFunc
local And = Ac.And
local Or = Ac.Or
local Repeat = Ac.Repeat

local BuffActions = {}

function BuffActions:Init(  )
	self.actions = {}		

	--属性 Buff：增加或减少万分之 x 属性 n 毫秒
	self.actions[400000] = function( cfg )
		return Attr { duration=cfg.duration, attr_id=cfg.attr_id, value=cfg.value, is_percent=cfg.is_percent }
	end

	--吸血 buff
	self.actions[400001] = function( cfg )
		return Sequence { 
			Repeat { cfg.suck_count, 
				Sequence { 
					SuckHP {value=cfg.value, is_percent=cfg.is_percent},
					Delay {cfg.suck_interval} 
				}
			}
		}
	end

	--晕眩 buff
	self.actions[400002] = function( cfg )
		return Sequence { 
			NotifyBuff {SceneConst.Buff.Dizzy},
			Speed { cfg }
		}
	end

	--回血 buff
	self.actions[400003] = function( cfg )
		return Repeat {-1, 
				Sequence { 
					HP {target=SceneConst.SkillTargetType.Me, value=cfg.value, is_percent=cfg.is_percent},
					Delay {700} 
				}
			}
	end

end

function BuffActions:GetActionCreator( buffID )
	return self.actions[buffID]
end

return BuffActions