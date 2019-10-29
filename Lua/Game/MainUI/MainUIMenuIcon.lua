local MainUIMenuIcon = BaseClass(UINode)
MainUIMenuIcon.Width = 90
function MainUIMenuIcon:Constructor( parentTrans )
	self.viewCfg = {
		parentTrans = parentTrans,
		prefabPath = "Assets/AssetBundleRes/ui/mainui/MainUIMenuIcon.prefab",
	}
end

function MainUIMenuIcon:OnLoad()
	local nodes = {
        "red:obj","effect","icon:img:obj",
    }
    UI.GetChildren(self, self.transform, nodes)
	UI.SetLocalPositionXYZ(self.transform, 0, 0, 0)

	self.red_obj:SetActive(false)
	self:AddEvents()
	if self.cache_move_to_info then
    	self:MoveToPos(self.cache_move_to_info[1], self.cache_move_to_info[2])
    	self.cache_move_to_info = nil
    end
end

function MainUIMenuIcon:AddEvents()
	local function OnItemClick(target)
		if target == self.icon_obj then
			self:BtnCall()
		end
	end
    UI.BindClickEvent(self.icon_obj, OnItemClick)

    -- local UpdateFunIcon = function (icon_type)
    -- 	if self.icon_type == icon_type then
    -- 		self:RefreshRedPoint()
    -- 	end
    -- end
    -- self:BindEvent(GlobalEventSystem, EventName.REFRESH_FUNCTION_ICON_RED_DOT, UpdateFunIcon)
end

function MainUIMenuIcon:BtnCall(  )
	if not self.data then return end
	
	do 
		Message:Show("功能未开启")
		return
	end
	--Cat_Todo : open func
	local func_id = self.data.func_id
	if func_id then
		local ids = Split(open_func, "@")
		OpenFun.Open(ids[1], ids[2])
	else
		print('Cat:MainUIMenuIcon.lua[has no open func!]')
	end
end

function MainUIMenuIcon:OnUpdate()
	if not self.data then return end
	
	self.gameObject.name = self.data.icon_type
	local resPath = ResPath.GetFullUIPath("mainui/"..self.data.icon_res..".png")
	UI:SetImage(self.icon_img, resPath, true)
	self:UpdateRedDot()
end

function MainUIMenuIcon:MoveToPos( pos, animate_time )
	if self.isLoaded then
    	cc.ActionManager:getInstance():removeAllActionsFromTarget(self.transform)
    	local action = cc.MoveTo.createAnchoredType(animate_time, pos.x, pos.y)
    	self:AddAction(action, self.transform)
    else
    	self.cache_move_to_info = {pos, animate_time}
    end
end

function MainUIMenuIcon:UpdateRedDot()
	-- local isRed = MainUIModel:GetInstance():GetRedDot(self.data.icon_type)
	-- self.red_obj:SetActive(isRed)
end

function MainUIMenuIcon:GetVisualIndex(  )
	return self.data and self.data.visual_index or 1
end

return MainUIMenuIcon