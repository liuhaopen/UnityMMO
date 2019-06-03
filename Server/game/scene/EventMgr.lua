local SceneEventMgr = BaseClass()
local table_insert = table.insert

function SceneEventMgr:Init( sceneMgr )
	self.sceneMgr = sceneMgr
	self.event_list = {}
	self.fight_event_list = {}
end

function SceneEventMgr:AddSceneEvent( uid, eventInfo )
	self.event_list[uid] = self.event_list[uid] or {}
	table_insert(self.event_list[uid], eventInfo)
end

function SceneEventMgr:GetSceneEvent( uid )
	return self.event_list[uid]
end

function SceneEventMgr:ClearAllSceneEvents(  )
	self.event_list = {}
end

function SceneEventMgr:AddFightEvent( uid, eventInfo )
	self.fight_event_list[uid] = self.fight_event_list[uid] or {}
	table_insert(self.fight_event_list[uid], eventInfo)
end

function SceneEventMgr:GetFightEvent( uid )
	return self.fight_event_list[uid]
end

function SceneEventMgr:ClearAllFightEvents(  )
	self.fight_event_list = {}
end

return SceneEventMgr