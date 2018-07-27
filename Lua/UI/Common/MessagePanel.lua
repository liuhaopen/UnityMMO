MessagePanel = MessagePanel or {}

function MessagePanel:Show( ... )
	local arg = {...}
	local str=""
	
	for key, var in ipairs(arg) do
		if type(var)=="table" then
			for key1, var1 in ipairs(var) do
				str=str.."[key]"..key1.."[var]"..var1.."\n"
			end
		elseif type(var)=="userdata" then
			str = str.."a userdata"
		else
			str = str .. var
		end
	end
end