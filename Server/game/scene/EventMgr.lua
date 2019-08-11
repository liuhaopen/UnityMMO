local SceneEventMgr = BaseClass()
local table_insert = table.insert

function SceneEventMgr:Init( sceneMgr )
	self.sceneMgr = sceneMgr
	self.event_list = {}
	self.skill_event_list = {}
	self.hurt_event_list = {}
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

function SceneEventMgr:AddSkillEvent( uid, eventInfo )
	self.skill_event_list[uid] = self.skill_event_list[uid] or {}
	table_insert(self.skill_event_list[uid], eventInfo)
end

function SceneEventMgr:GetSkillEvent( uid )
	return self.skill_event_list[uid]
end

function SceneEventMgr:ClearAllSkillEvents(  )
	self.skill_event_list = {}
end

function SceneEventMgr:AddHurtEvent( uid, eventInfo )
	self.hurt_event_list[uid] = self.hurt_event_list[uid] or {}
	table_insert(self.hurt_event_list[uid], eventInfo)
end

function SceneEventMgr:GetHurtEvent( uid )
	return self.hurt_event_list[uid]
end

function SceneEventMgr:ClearAllHurtEvents(  )
	self.hurt_event_list = {}
end

return SceneEventMgr