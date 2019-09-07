local Ac = require "Action"
local Attr = require "game.scene.fight.Attr"
local NotifyBuff = require "game.scene.fight.NotifyBuff"
local SuckHP = require "game.scene.fight.SuckHP"
local Speed = require "game.scene.fight.Speed"
local Ability = require "game.scene.fight.Ability"
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
	self.counter = {}

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
		local bod = self:GetBOD(400002, SceneConst.Buff.Dizzy)
		return Sequence { 
			NotifyBuff { SceneConst.Buff.Dizzy, cfg.duration },
			Speed { bod=bod, is_set=true, value=0 },
			Ability { bod=bod, is_set=true, value=false },
			Delay { cfg.duration },
			Speed { bod=bod, is_set=false },
			Ability { bod=bod, is_set=false },
		}
	end

	--冰冻 buff,减速并扣几次血
	self.actions[400003] = function( cfg )
		local bod = self:GetBOD(400003, SceneConst.Buff.Forzen)
		return Sequence { 
			NotifyBuff { SceneConst.Buff.Forzen, cfg.duration*cfg.hurt_count },
			Speed { bod=bod, is_set=true, value=cfg.decelerate },
			Ability { bod=bod, is_set=true, value=false },
			Repeat { cfg.hurt_count, 
				Sequence { 
					Hurt {},
					Delay { cfg.duration },
				}
			},
			Speed { bod=bod, is_set=false },
			Ability { bod=bod, is_set=false },
		}
	end
	--消除冰冻 buff
	self.clearers[400003] = function( buffAction )
		--先看看该 action 是否已经减了速度和攻击能力，是的话就要还原
		if buffAction[2]:IsDone() then
			local actionNum = #buffAction
			for i=actionNum,actionNum-1,-1 do
				buffAction[i]:Start(buffAction.buffData)
				buffAction[i]:Update()
			end
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

	--沉默 buff
	self.actions[400006] = function( cfg )
		local bod = self:GetBOD(400006, SceneConst.Buff.Silence)
		return Sequence { 
			NotifyBuff { SceneConst.Buff.Silence, cfg.duration },
			Ability { bod=bod, is_set=true, value=false },
			Delay { cfg.duration },
			Speed { bod=bod, is_set=false },
			Ability { bod=bod, is_set=false },
		}
	end

	--毒 buff，定时扣血
	self.actions[400007] = function( cfg )
		return Sequence { 
			NotifyBuff { SceneConst.Buff.Poison, cfg.duration*cfg.hurt_count },
			Repeat { cfg.hurt_count, 
				Sequence { 
					Hurt {},
					Delay { cfg.duration },
				}
			}
		}
	end

	--火 buff，定时扣血
	self.actions[400008] = function( cfg )
		return Sequence { 
			NotifyBuff { SceneConst.Buff.Fire, cfg.duration*cfg.hurt_count },
			Repeat { cfg.hurt_count, 
				Sequence { 
					Hurt {},
					Delay { cfg.duration },
				}
			}
		}
	end
end

function BuffActions:GetActionCreator( buffID )
	return self.actions[buffID]
end

function BuffActions:GetActionClearer( buffID )
	return self.clearers[buffID]
end

function BuffActions:GetBOD( buffID, buffType )
	self.counter[buffID] = self.counter[buffID] or 0
	self.counter[buffID] = self.counter[buffID] + 1
	local id = self.counter[buffID]
	local bod = string.format("buff_%s_%s", buffType, id)
	return bod
end

return BuffActions