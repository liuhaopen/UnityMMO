local Ac = require "Action"
local Hurt = require "game.scene.fight.Hurt"
local PickTarget = require "game.scene.fight.PickTarget"
local Buff = require "game.scene.fight.Buff"
local SceneConst = require "game.scene.SceneConst"
local If = Ac.If
local Delay = Ac.Delay
local Random = Ac.Random
local Sequence = Ac.Sequence
local CallFunc = Ac.CallFunc
local And = Ac.And
local Or = Ac.Or
local Repeat = Ac.Repeat

local SkillActions = {}
function SkillActions:Init(  )
	self.actions = {}		
	--[[
		Sequence:
			队列，后面可带不定数量的Action
		Delay:
			延迟，参数为延迟的毫秒
		PickTarget:
			选取目标,必须要有的，运行后会更新目标列表，大部分的Action都会用到该目标列表，参数有目标类型(敌方、自己、我方等等)、选取形状(圆、直线等)、最大目标数量等，为空时就用该技能的默认配置
		If:
			第一参数是个条件Action，true则运行第二参数的Action否则跑第三参数的
		Hurt:
			直接扣血,第一参数为伤害比率，不传的话将读取技能配置里的damage_rate字段
		Buff:
			添加Buff，当然每个Buff也像技能一样是个Action，里面也有触发条件等判定
		条件Action：
			可以是个function或者重写了__call的table
		Random:
			条件Action,参数为概率万分比
		CheckAttr:
			条件Action,判断某属性和某个数值的关系
		BreakSkill:
			打断目标的技能
		注：之所以每个技能都做成一个 function 的形式提供是因为一个技能可能有多个等级，不同等级会有不同效果，所以创建一个技能 action 时需要传入该技能的特定等级的配置即 cfg,其是从 config_skill.lua 里该技能的 detail 字段里当前等级的那条
	--]]
	local normal_skill_list = {
		110000, 110001, 110002, 110003, 110012, 
		120000, 120001, 120002, 120003, 120012, 
	}
	--普通的技能，选中攻击目标，然后直接扣血
	for i,v in ipairs(normal_skill_list) do
		self.actions[v] = function( cfg )
			return Sequence { 
					PickTarget {}, 
					Hurt {} 
				}
		end
	end

	local skill_list_with_delay = {
		[200000]=300, [200001]=800,
		[200100]=600, [200101]=600, 
		[200200]=500, [200201]=600, 
		[200300]=350, [200301]=400, 
		[200400]=400, [200401]=300, 
		[200500]=500, [200501]=300,
	}
	--怪物的普通技能，因为是单体攻击所以就从调用 FightMgr:CastSkill 时就已经传入攻击目标了，直接扣血
	for k,v in pairs(skill_list_with_delay) do
		self.actions[k] = function( cfg )
			return Sequence { 
				Delay {v}, 
				Hurt {} 
			}
		end
	end
	
	--扣血后有万份之 x 概率触发400000buff(降低防御),具体降低的数值根据技能等级读取配置的
	self.actions[110010] = function( cfg )
		return Sequence {
			Delay {100}, 
			PickTarget {}, 
			Hurt {}, 
			If { Random{cfg.buff.probability}, Buff{400000} } 
		}
	end
	self.actions[120010] = self.actions[110010]

	--扣血后有万份之 x 概率触发400002buff(晕眩)
	self.actions[110011] = function( cfg )
		return Sequence {
			Delay {500}, 
			PickTarget {}, 
			Hurt {}, 
			If { Random{cfg.buff.probability}, Buff{400002} } 
		}
	end
	self.actions[120011] = self.actions[110011]

	--男主大招，300毫秒后，重复5次攻击，每次间隔700毫秒
	self.actions[110013] = function( cfg )
		return Sequence { 
			Delay {300}, 
			Repeat {5, 
				Sequence { 
					PickTarget {}, 
					Hurt {}, 
					If { Random{cfg.buff.probability}, Buff{400001} },
					Delay {700} 
				}
			}
		}
	end
	self.actions[120013] = self.actions[110013]

	--女主大招，重复5次攻击，每次间隔700毫秒，每次都有一定概率加吸血 buff
	self.actions[120013] = function( cfg )
		return Sequence { 
			Delay {300}, 
			Repeat {5, 
				Sequence { 
					PickTarget {}, 
					Hurt {}, 
					If { Random{cfg.buff.probability}, Buff{400001} },
					Delay {700} 
				}
			}
		}
	end
end

function SkillActions:GetActionCreator( skillID )
	return self.actions[skillID]
end

return SkillActions