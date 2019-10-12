local MainUIMenuIcon = BaseClass(UINode)
MainUIMenuIcon.Width = 90
function MainUIMenuIcon:Constructor( )
	self.viewCfg = {
		prefabPath = "Assets/AssetBundleRes/ui/mainui/MainUIMenuIcon.prefab",
	}
end

function MainUIMenuIcon:OnLoad()
	local nodes = {
        "red:obj","effect","icon:img",
    }
    self:GetChildren(nodes)

	self:AddEvents()
	if self.need_refreshData then
		self:SetData(self.data)
	end
end

function MainUIMenuIcon:AddEvents()
	local function OnItemClick(target)
		if target == self.gameObject then
			self:BtnCall()
		end
	end
    AddClickEvent(self.gameObject,OnItemClick,1)

    local UpdateFunIcon = function (icon_type)
    	if self.icon_type == icon_type then
    		self:RefreshRedPoint()
    	end
    end
    self:BindEvent(GlobalEventSystem, EventName.REFRESH_FUNCTION_ICON_RED_DOT, UpdateFunIcon)
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
	if not self.data then return end
	
	self.gameObject.name = self.data.icon_type
	UI:SetImage(self.icon_img, "mainUI_asset", self.data.icon_name, true)
	self:UpdateRedDot()
end

function MainUIMenuIcon:UpdateRedDot()
	local isRed = MainUIModel:GetInstance():GetRedDot(self.data.icon_type)
	self.red_obj:SetActive(isRed)
end

function MainUIMenuIcon:__delete()
end
