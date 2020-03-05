local skynet = require "skynet"
local mysql = require "skynet.db.mysql"
require "skynet.manager"
require "common.util"

--[[
用法:
local dbserver = skynet.localname(".your db name")
local is_succeed = skynet.call(dbserver, "lua", "insert", "Account", {account_id=7, password="123"})
print('Cat:main.lua[insert] is_succeed', is_succeed)
local is_succeed, result = skynet.call(dbserver, "lua", "select_all", "Account")
if is_succeed then
	print("Cat:main [start:30] result:", result)
	PrintTable(result)
	print("Cat:main [end]")
end
--]]
local db

local function ping()
	while true do
		if db then
			db:query("select l;")
		end
		skynet.sleep(3600*1000)
	end
end

local CMD = {}

function CMD.open( conf )
	-- print("Cat:dbserver [start:18] conf:", conf)
	-- PrintTable(conf)
	-- print("Cat:dbserver [end]")
	db = mysql.connect(conf)
	skynet.fork(ping)

	skynet.register(conf.name or "."..conf.database)
end

function CMD.close( conf )
	if db then
		db:disconnect()
		db = nil
	end
end

function CMD.insert( tablename, rows )
	local cols = {}
	local vals = {}
	for k, v in pairs(rows) do
		table.insert(cols, k)
		if type(v) == "string" then
			v = mysql.quote_sql_str(v)
		end
		table.insert(vals, v)
	end
	vals = table.concat(vals, ",")
	cols = table.concat(cols, ",")
	local sql = string.format("insert into %s(%s) values(%s);", tablename, cols, vals)
	local result = db:query(sql)
	if result.errno then
		skynet.error(result.err)
		return false
	end
	return true
end

function CMD.delete( tablename, key, value )
	local sql = string.format("delete from %s where %s = %s;", tablename, key, mysql.quote_sql_str(tostring(value)))
	local result = db:query(sql)
	if result.errno then
		skynet.error(result.err)
		return false
	end
	return true
end

function CMD.query(command)
	local result = db:query(command)
	if result.errno then
		skynet.error(result.err)
		return false
	end
	return true, result
end

function CMD.update( tablename, key, value, row )
	local t = {}
	for k,v in pairs(row) do
		if type(v) == "string" then
			v = mysql.quote_sql_str(v)
		end
		table.insert(t, k.."="..v)
	end
	local setvalues = table.concat(t, ",")
	local sql = string.format("update %s set %s where %s = '%s';", tablename, setvalues, key, value)
	local result = db:query(sql)
	if result.errno then
		skynet.error(result.err)
		return false
	end
	return true 
end

function CMD.select_by_key( tablename, key, value )
	local sql = string.format("select * from %s where %s = '%s';", tablename, key, value)
	local result = db:query(sql)
	if result.errno then
		skynet.error(result.err)
		return false
	end
	return true, result
end

function CMD.select_one_by_key( tablename, key, value )
	local sql = string.format("select * from %s where %s = '%s';", tablename, key, value)
	local result = db:query(sql)
	if result.errno then
		skynet.error(result.err)
		return false
	end
	return true, result and result[1]
end

function CMD.select_by_condition( tablename, condition )
	local sql = string.format("select * from %s where %s;", tablename, condition)
	local result = db:query(sql)
	if result.errno then
		skynet.error(result.err)
		return false
	end
	return true, result
end

function CMD.select_all( tablename )
	local sql = string.format("select * from %s;", tablename)
	local result = db:query(sql)
	if result.errno then
		skynet.error(result.err)
		return false
	end
	return true, result
end

skynet.start(function()
	skynet.dispatch("lua", function(session, source, cmd, ...)
		local f = assert(CMD[cmd], "can't not find cmd :"..(cmd or "empty"))
		if session == 0 then
			f(...)
		else
			skynet.ret(skynet.pack(f(...)))
		end
	end)
end)

