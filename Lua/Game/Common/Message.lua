Message = Message or {}

--container:为飘字所在容器
function Message:Init( container, max_at_the_same_time )
	print('Cat:Message.lua[Init]')
	assert(container, "Message:Init() : container cannot nil!")
	self.msg_list = {}
	self.container = container
	self.max_at_the_same_time = max_at_the_same_time
	self.last_pop_msg_time = 0

	self.__update_handle = BindCallback(self, Message.Update)
	UpdateManager:GetInstance():AddUpdate(self.__update_handle)	
end

function Message:Show( ... )
	local arg = {...}
	local str=""
	
	for key, var in ipairs(arg) do
		if type(var)=="table" then
			for key1, var1 in ipairs(var) do
				str=str.."[key]"..key1.."[var]"..var1.."\n"
			end
		elseif type(var)=="userdata" then
			str = str.."a userdata"
		else
			str = str .. var
		end
	end
	table.insert(self.msg_list, {msg=str, insert_time=Time.time})
end

function Message:PopAMsg( )
	if not self.msg_list or #self.msg_list <= 0 then 
		return false
	end
	local msg_info = table.remove(self.msg_list, 1)
	local msg_item = 
	{
		UIConfig =
		{
			prefab_path = "Assets/AssetBundleRes/ui/common/Message.prefab",
		},
		OnLoad = function(msg_item)
			UIHelper.SetParent(msg_item.transform, self.container)
			msg_item.label = msg_item.transform:GetComponent("Text")
			msg_item.label.text = msg_info.msg
			local action = cc.MoveBy.createLocalType(0.5, 0, 60)
			local on_end = function (  )
				GameObject.Destroy(msg_item.gameObject)
			end
			action = cc.Sequence.create(action, cc.DelayTime.create(0.5), cc.CallFunc.create(on_end))
			cc.ActionManager:getInstance():addAction(action, msg_item.transform)
		end
	}
	UIMgr:Show(msg_item)
	
	return true
end

function Message:Update( )
	local curTime = Time.time
	if curTime - self.last_pop_msg_time > 0.6 then
		self.last_pop_msg_time = curTime 
		self:PopAMsg()	
	end
end