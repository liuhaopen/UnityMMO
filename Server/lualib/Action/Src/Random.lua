local Random = Ac.OO.Class {
	type 	= "Random",
	__call = function(self)
		local random = math.random(1, 10000)
		return random <= self[1]
	end
}
return Random