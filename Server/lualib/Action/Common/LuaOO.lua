-- See details for additional information : http://angg.twu.net/LATEX/dednat6/eoo.lua.html
local OO = {}

OO.Class = {
    type   = "Class",
    __call = function (class, o) return setmetatable(o, class) end,
}
setmetatable(OO.Class, OO.Class)

OO.Type = function (o)
	local  mt = getmetatable(o)
	return mt and mt.type or type(o)
end

OO.Over = function (uppertable)
	return function (lowertable)
	    setmetatable(uppertable, {__index=lowertable})
	    return uppertable
  	end
end

OO.ClassOver = function (upperclassmt)
	return function (lowerclass)
	    setmetatable(upperclassmt.__index, {__index=lowerclass.__index})
	    return OO.Class(upperclassmt)
  	end
end

return OO