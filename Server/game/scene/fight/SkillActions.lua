local Ac = require "Action"

local SkillActions = {}
local If = Ac.If
local Delay = Ac.Delay
local Random = Ac.Random
local Sequence = Ac.Sequence
local CallFunc = Ac.CallFunc

function SkillActions:Init(  )
	self.actions = {}		
	do return end

	--[[
	Sequence:队列，后面可带不定数量的Action
	Delay:延迟，参数为延迟的毫秒
	PickTarget:选取目标,必须要有的，运行后会更新目标列表，大部分的Action都会用到该目标列表，参数有目标类型(敌方、自己、我方等等)、选取形状(圆、直线等)、最大目标数量等，为空时就用该技能的默认配置
	If:第一参数是个条件Action，true则运行第二参数的Action否则跑第三参数的
	Hurt:直接扣血
	Buff:添加Buff，当然每个Buff也像技能一样是个Action，里面也有触发条件等判定
	条件Action：可以是个function或者重写了__call的table
	Random:条件Action,参数为概率万分比
	CheckAttr:条件Action,判断某属性和某个数值的关系
	--]]

	--110000技能：100毫秒后造成直接伤害(伤害系数是根据不同等级配置不同的值，所以cfg是根据施法者当前的技能等级传入的值：config_skill.lua里的detail[skill_lv].arge)
	self.actions[110000] = function( cfg )
		return Sequence { Delay{100}, PickTarget{1,1,cfg.MaxTarget}, Hurt{cfg.HurtRate} }
	end

	--100毫秒后造成直接伤害后尝试触发Buff1，后面的为传入参数
	self.actions[110001] = function( cfg )
		return Sequence { Delay{100}, Hurt{cfg.HurtRate}, Buff{1,2,3} }
	end

	--如果hp小于等于万分之10时，延迟100毫秒后伤害HurtRate1值并触发Buff1，否则伤害HurtRate2且触发Buff2
	self.actions[110002] = function( cfg )
		return If { CheckAttr{"hp","<=", 10, "%"}, 
			Sequence { Delay{100}, Hurt{cfg.HurtRate1}, Buff{1} },
			Sequence { Delay{100}, Hurt{cfg.HurtRate2}, Buff{2} },
		}
	end

	--一共触发5次，每次有20%的概率暴发（伤害系数为HurtRateBig）并触发Buff1,另外的80%造成HurtRateSmall伤害但没有Buff
	self.actions[110003] = function( cfg )
		return Repeat{5, If { Random{2000}, 
				Sequence{ Hurt{cfg.HurtRateBig}, Buff{1}, Delay{1000} } ,
				Sequence{ Hurt{cfg.HurtRateSmall}, Delay{1000} } ,
			}
		}
	end
end

function SkillActions:GetActionCreator( skillID )
	return self.actions[skillID]
end

local Hurt = AC.OO.Class{
	type 	= "Hurt",
	__index = {
		Execute = function(self)

		end,
	},
}

return SkillActions