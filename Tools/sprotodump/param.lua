------------- stream -------------
local stream_mt = {}
local function new_stream(...)
  local raw = {...}
  raw.count = #raw
  raw.cur_idx = 1
  return setmetatable(raw, {__index = stream_mt})
end


function stream_mt:read()
  local len = self.count
  if self.cur_idx <= len then
    local ret = self[self.cur_idx]
    self.cur_idx = self.cur_idx + 1
    return ret
  end
end


function stream_mt:read_value()
  if not self:is_opt() then
    return self:read()
  end
end


function stream_mt:is_opt()
  local cur_v = self[self.cur_idx]
  if cur_v then
    return string.match(cur_v, "^%-.+$")
  end
end


------------- parser -------------
local function _parser_opt(stream, n)
  if stream:is_opt() then
    if n then
      for i=1,n do
        if not stream[stream.cur_idx+i] then
          return false
        end
      end
    end

    local ret = {
      opt = stream:read(),
    }

    if not n then
      while true do
        local v = stream:read_value()
        if not v then break end
        table.insert(ret, v)
      end
    else
      for i=1,n do
        local v = stream:read_value()
        if not v then break end
        table.insert(ret, v)
      end
    end
    return ret
  end
end

local function parser_opt(stream)
  return _parser_opt(stream)
end


local function parser_outopt(stream)
  return _parser_opt(stream, 1)
end


local function parser_namespace(stream)
  return _parser_opt(stream, 0)
end


------------- param -------------
local function parse_param(...)
  local stream = new_stream(...)

  if #stream == 0 then
    return false
  end

  local ret = {
    dircetory = false,
    package = false,
    outfile = false,
    namespace = false,

    sproto_file = {},
    dump_type = false,
  }


  --- parser option
  local result = parser_opt(stream)
  if not result then
    return false
  else
    ret.dump_type = result.opt
    for i,v in ipairs(result) do
      ret.sproto_file[i] = v
    end
  end

  --- parser out option
  local out_option = {
    ["-d"] = "dircetory",
    ["-o"] = "outfile",
    ["-p"] = "package",
  }
  while true do
    result = parser_outopt(stream)
    if not result then break end
    local key = out_option[result.opt]
    local value = result[1]
    if not key then break end
    ret[key] = value
  end
  
  -- parser namespace
  result = parser_namespace(stream)
  ret.namespace = not not result

  return ret
end

return parse_param


