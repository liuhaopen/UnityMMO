local global = require "global"
local ecs = require "ecs.ecs"

local mt = {}

function mt:init()
	self.world = ecs.world:new("main_world")
	self.entity_mgr = self.world.entity_mgr
	global.entity_mgr = self.entity_mgr
	global.world = self.world

	local on_start_game = function (  )
        self:start_game()
	end
    GlobalEventSystem:Bind(GlobalEvents.GameStart, on_start_game)
end

function mt:start_game()
	local systems = {
		"Game.systems.movement_system",
		"Game.systems.aoi_system",
		"Game.systems.visual_system",
		"Game.systems.aoi_system",
	}
	for i,sys_path in ipairs(systems) do
		local sys = require(sys_path):new()
		self.world:add_system(sys)
	end

	self.__update_handle = BindCallback(self, function()
		local dt = Time.deltaTime
		self.world:update(dt)
    end)
	UpdateManager:GetInstance():AddUpdate(self.__update_handle)	
end

return mt