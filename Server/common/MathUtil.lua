local clamp = function ( value, min, max )
	if value > max then
		value = max 
	end
	if value < min then
		value = min
	end
	return value
end

math.clamp = clamp
