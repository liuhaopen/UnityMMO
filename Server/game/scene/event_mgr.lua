local scene_event_mgr = {}
local table_insert = table.insert

function scene_event_mgr:Init( sceneMgr )
	self.sceneMgr = sceneMgr
	self.event_list = {}
	self.fight_event_list = {}
end

function scene_event_mgr:AddSceneEvent( uid, eventInfo )
	self.event_list[uid] = self.event_list[uid] or {}
	table_insert(self.event_list[uid], eventInfo)
end

function scene_event_mgr:GetSceneEvent( uid )
	return self.event_list[uid]
end

function scene_event_mgr:ClearAllSceneEvents(  )
	self.event_list = {}
end

function scene_event_mgr:AddFightEvent( uid, eventInfo )
	self.fight_event_list[uid] = self.fight_event_list[uid] or {}
	table_insert(self.fight_event_list[uid], eventInfo)
end

function scene_event_mgr:GetFightEvent( uid )
	return self.fight_event_list[uid]
end

function scene_event_mgr:ClearAllFightEvents(  )
	self.fight_event_list = {}
end

return scene_event_mgr