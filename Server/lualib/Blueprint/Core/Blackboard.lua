local Blackboard = BP.BaseClass()

function Blackboard:Constructor(  )
	self.variables = {}
end

function Blackboard:Init( luaData )
end

function Blackboard:SetVariable( name, val )
	self.variables[name] = val
end

function Blackboard:GetVariable( name )
	local val = self.variables[name]
	if type(val) ~= "function" then
		return val 
	else
		return val()
	end
end

return Blackboard