local print = print
local tconcat = table.concat
local tinsert = table.insert
local srep = string.rep
local type = type
local pairs = pairs
local tostring = tostring
local next = next

local function print_r(root)
	print('Cat:print_r.lua[11] root', root)
	local cache = {  [root] = "." }
	print('Cat:print_r.lua[13]')
	local function _dump(t,space,name)
		print('Cat:print_r.lua[14] t,space,name', t,space,name)
		local temp = {}
		for k,v in pairs(t) do
			print('Cat:print_r.lua[17] v, k', v, k)
			local key = tostring(k)
			if cache[v] then
				tinsert(temp,"+" .. key .. " {" .. cache[v].."}")
			elseif type(v) == "table" then
				local new_key = name .. "." .. key
				cache[v] = new_key
				tinsert(temp,"+" .. key .. _dump(v,space .. (next(t,k) and "|" or " " ).. srep(" ",#key),new_key))
			else
				tinsert(temp,"+" .. key .. " [" .. tostring(v).."]")
			end
		end
		return tconcat(temp,"\n"..space)
	end
	print(_dump(root, "",""))
end

return print_r