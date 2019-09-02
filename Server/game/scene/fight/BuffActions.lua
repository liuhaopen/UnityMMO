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
	self.clearers = {}

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
			NotifyBuff {SceneConst.Buff.Dizzy, cfg.duration},
			Speed { bod="buff_"..SceneConst.Buff.Dizzy, is_set=true, value=0 },
			Delay { cfg.duration },
			Speed { bod="buff_"..SceneConst.Buff.Dizzy, is_set=false },
		}
	end

	--冰冻 buff,减速并扣几次血
	self.actions[400003] = function( cfg )
		return Sequence { 
			NotifyBuff { SceneConst.Buff.Forzen, cfg.duration*cfg.hurt_count },
			Speed { bod="buff_"..SceneConst.Buff.Forzen, is_set=true, value=cfg.decelerate },
			Repeat { cfg.hurt_count, 
				Sequence { 
					Hurt {},
					Delay { cfg.duration },
				}
			},
			Speed { bod="buff_"..SceneConst.Buff.Forzen, is_set=false },
		}
	end
	--消除冰冻 buff
	self.clearers[400003] = function( buffAction )
		--先看看该 action 是否已经减了速度了，是的话就要还原速度
		if buffAction[2]:IsDone() then
			buffAction[#buffAction]:Start(buffAction.buffData)
			buffAction[#buffAction]:Update()
		end
		buffAction.buffData.sceneMgr.actionMgr:RemoveAction(buffAction)
	end

	--回血 buff
	self.actions[400004] = function( cfg )
		return Repeat {-1, 
				Sequence { 
					HP {target=SceneConst.SkillTargetType.Me, value=cfg.value, is_percent=cfg.is_percent},
					Delay {700} 
				}
			}
	end

	--清除目标身上 buff
	self.actions[400005] = function( cfg )
		return ClearBuff {target=cfg.target, clear_type=cfg.clear_type}
	end

end

function BuffActions:GetActionCreator( buffID )
	return self.actions[buffID]
end

function BuffActions:GetActionClearer( buffID )
	return self.clearers[buffID]
end

return BuffActions