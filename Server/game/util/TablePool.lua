local TablePool = {
	pools = {}
}

function TablePool:Get( name )
	name = name or ""

	local pool = self.pools[name]
	if pool and #pool > 0 then
		return table.remove(pool, #pool)
	else
		return nil
	end
end

function TablePool:Recycle( name, tbl )
	local pool = self.pools[name]
	pool = pool or {}
	pool[#pool+1] = tbl
	self.pools[name] = pool
end

return TablePool