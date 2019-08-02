local Window = BaseClass(UINode)

local StyleInfo = {
	["WindowStyle.Big"] = {
		widgetName = "WindowBig", nodes = {
			"tab_scroll/Viewport/tab_scroll_con","tab_con","tab_scroll","close:obj","tab_line","bg:raw","title:raw",
		},
	},
	["WindowStyle.NoTab"] = {
		widgetName = "WindowNoTab", nodes = {
			"close:obj","bg:raw","title:raw",
		},
	},
}
function Window:Constructor( )
	self:Reset()
end

function Window.Create( )
	return LuaPool:Get("Window")
end

function Window:Reset()
	self.lastData = nil
	self.data = nil
end

--[[
data可含字段：
style:见文件上部分的StyleInfo，目前有两大窗体，一个带标签页一个没有
tabInfo:标签页数据,详见SetTabInfo函数
onSwitchTab:切换标签页时触发的回调
onClose:关闭按钮点击回调
bg:背景图
title:标题图
--]]
function Window:Load( data )
	self.lastData = self.data
	self.data = data
	if not self.data then 
		LogError("Window:Load data nil")
		return 
	end

	if self.data.style then
		self:SetStyle(self.data.style)
	end
	if self.data.bg then
		self:SetBG(self.data.bg)
	end
	if self.data.title then
		self:SetTitle(self.data.title)
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
		if bg ~= "default" then
			UI.SetBg(self, self.bg_raw, ResPath.GetBgPath(bg))
		else
			UI.SetRawImage(self, self.bg_raw, "Assets/AssetBundleRes/ui/common/com_default_win_bg.png")
			self.bg_raw.gameObject:SetActive(true)
		end
	end
end

function Window:SetTitle( title )
	if self.isLoaded then
		UI.SetRawImage(self, self.title_raw, title, true)
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
		self.curStyleInfo = styleInfo
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
	if not self.curStyleInfo then return end
	
	UI.GetChildren(self, self.transform, self.curStyleInfo.nodes, true)

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