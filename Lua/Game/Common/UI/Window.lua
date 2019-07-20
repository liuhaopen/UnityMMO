local Window = BaseClass(UINode)
-- Window.Style = {
-- 	Big = 1,
-- 	Normal = 2,
-- 	Small = 3,
-- }
local StyleInfo = {
	["WindowStyle.Big"] = {
		widgetName = "WindowBig",
	},
}
function Window:Constructor( parentTrans )
	self:Reset()
end

function Window.Create( )
	return LuaPool:Get("Pool-Window")
end

function Window:Reset()
	self.lastData = nil
	self.data = nil
end

function Window:Load( data )
	self.lastData = self.data
	self.data = data
	if not self.data or not self.data.style then 
		LogError("Window:Load data nil")
		return 
	end

	if self.data.style then
		self:SetStyle(self.data.style)
	end
	if self.data.bg then
		self:SetBG(self.data.bg)
	end
	if self.data.tabInfo then
		self:SetTabInfo(self.data.tabInfo)
	end
	if self.data.onSwitchTab then
		self:BindSwitchTabEvent(self.data.onSwitchTab)
	end
	if self.data.onClose then
		self:BindCloseEvent(self.data.onClose)
	end
end

function Window:SetBG( bg )
	if self.isLoaded then
		UI.SetBg(self, self.bg_raw, ResPath.GetBgPath(bg))
	end
end

function Window:SetStyle( style )
	self.lastStyle = self.style
	self.style = style
	local isStyleSameAsLast = self.lastStyle and self.style and self.lastStyle==self.style
	if not isStyleSameAsLast then
		local styleInfo = StyleInfo[self.data.style]
		if not styleInfo then
			LogError("Window:SetData cannot find style info by : "..self.data.style)
		end
		if self.gameObject then
			GameObject.Destroy(self.gameObject)
			self.gameObject = nil
		end
		local window = PrefabPool:Get(styleInfo.widgetName)
		print('Cat:Window.lua[33] window', window)
		self:Init(window.gameObject)
	end
end

function Window:OnLoad(  )
	local names = {
		"tab_scroll/Viewport/tab_scroll_con","tab_con","tab_scroll","close:obj","tab_line","sub_view_con","bg:raw",
	}
	UI.GetChildren(self, self.transform, names, true)

	self:AddEvents()
end

function Window:AddEvents(  )
	local on_click = function ( click_obj )
		if self.close_obj == click_obj then
			if self.onClose then
				self.onClose()
			end
		end
	end
	UI.BindClickEvent(self.close_obj, on_click)
	
end

function Window:SetTabInfo( info )
	self.tabInfo = info
end

function Window:BindSwitchTabEvent( onSwitchTab )
	self.onSwitchTab = onSwitchTab
end

function Window:BindCloseEvent( onClose )
	self.onClose = onClose
end

return Window