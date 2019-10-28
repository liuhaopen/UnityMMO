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
        "red:obj","effect","icon:img",
    }
    UI.GetChildren(self, self.transform, nodes)
	UI.SetLocalPositionXYZ(self.transform, 0, 0, 0)

	self:AddEvents()
	if self.cache_move_to_info then
    	self:MoveToPos(self.cache_move_to_info[1], self.cache_move_to_info[2])
    	self.cache_move_to_info = nil
    end
end

function MainUIMenuIcon:AddEvents()
	local function OnItemClick(target)
		if target == self.gameObject then
			self:BtnCall()
		end
	end
    UI.BindClickEvent(self.gameObject, OnItemClick)

    -- local UpdateFunIcon = function (icon_type)
    -- 	if self.icon_type == icon_type then
    -- 		self:RefreshRedPoint()
    -- 	end
    -- end
    -- self:BindEvent(GlobalEventSystem, EventName.REFRESH_FUNCTION_ICON_RED_DOT, UpdateFunIcon)
end

function MainUIMenuIcon:BtnCall(  )
	if not self.data then return end
	
	--Cat_Todo : open func
	local open_func = self.data.open_func
	if open_func then
		local ids = Split(open_func, "@")
		OpenFun.Open(ids[1], ids[2])
	else
		print('Cat:MainUIMenuIcon.lua[has no open func!]')
	end
end

function MainUIMenuIcon:OnUpdate()
	print('Cat:MainUIMenuIcon.lua[53] self.data', self.data)
	if not self.data then return end
	
	self.gameObject.name = self.data.icon_type
	local resPath = ResPath.GetFullUIPath("mainui/"..self.data.icon_res..".png")
	print('Cat:MainUIMenuIcon.lua[57] resPath', resPath)
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