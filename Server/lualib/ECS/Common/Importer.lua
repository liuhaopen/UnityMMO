--本源码文件主要摘自云风的blog:https://blog.codingnow.com/2015/10/lua_require_env.html
local loaded = package.loaded
local searchpath = package.searchpath

local function load_env(filename)
    local f,err = loadfile(filename)
    if f == nil then
        return err
    end
    return function()
        return function(env)
            if env then
                debug.setupvalue(f, 1, env)
            end
            return f(filename)
        end
    end
end

local function searcher_env(name)
    local filename, err = package.searchpath(name, package.path)
    if filename == nil then
        return err
    else
        return load_env(filename)
    end
end

local function import(modname, env)
    return require(modname)(env)
end

local function enable()
    table.insert(package.searchers, 2, searcher_env)
end

local function disable()
    table.remove(package.searchers, 2)
end

return {enable=enable, disable=disable, require=import}
