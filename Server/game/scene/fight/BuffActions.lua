local BuffActions = {}

function BuffActions:Init(  )
	self.actions = {}		

	--700000Buff：20%的概率触发（为了不用浮点数，数值单位为万分比），如果
	self.actions[700000] = function( skillCfgArge )
		return If { Random{2000}, 
			Repeat{5, Sequence{ Hurt{skillCfgArge.HurtRate}, Delay{500} } },
		}
	end

	--700001Buff：如果hp小于等于万分之10时，伤害比率提升万分之skillCfgArge.BuffArge1，否则只提升skillCfgArge.BuffArge2
	self.actions[700001] = function( skillCfgArge )
		return If { CheckAttr{"hp","<=", 10, "%"}, 
			PowerUp{skillCfgArge.BuffArge1}, 
			PowerUp{skillCfgArge.BuffArge2}, 
		}
	end
end

function BuffActions:GetActionCreator( skillID )
	return self.actions[skillID]
end

return BuffActions