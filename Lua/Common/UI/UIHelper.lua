local GameObject = GameObject
local Util = Util
local LuaEventListener = LuaEventListener
local LuaClickListener = LuaClickListener
local LuaDragListener = LuaDragListener
local WordManager = WordManager
local Jury = require("Common.Util.Jury")

UI = BaseClass(CS.XLuaFramework.UIHelper)
--下列接口会尝试多种设置的，如果你已经知道你的节点是什么类型的就别用下列接口了
function UI.SetActive( obj, is_show )
	if not obj then return end
	if obj.SetActive then
		obj:SetActive(is_show)
	elseif obj.SetActive then
		obj:SetActive(is_show)
	end
end
--上列接口会尝试多种设置的，如果你已经知道你的节点是什么类型的就别用上列接口了

UI.UpdateVisibleJuryTable = {}
UI.UpdateVisibleJuryTableForValue = {}
function UI.InitForUIHelper(  )
	print('Cat:UIHelper.lua[InitForUIHelper]')
	setmetatable(UI.UpdateVisibleJuryTable, {__mode = "k"})   
	setmetatable(UI.UpdateVisibleJuryTableForValue, {__mode = "v"})   
end

--你显示隐藏前都要给我一个理由,我会综合考虑,只会在没有任何理由隐藏时我才会真正地显示出来
function UI.UpdateVisibleByJury( obj, is_show, reason )
	if not obj then return end
	local tab_str = tostring(obj)
	-- print('Cat:UIHelper.lua[33] tab_str, obj, is_show, reason', tab_str, obj, is_show, reason)
	UI.UpdateVisibleJuryTableForValue[tab_str] = obj
	if not UI.UpdateVisibleJuryTable[obj] then
		local jury = Jury.New()
		--当陪审团的投票结果变更时触发 
		local on_jury_change = function (  )
			--之所以用UpdateVisibleJuryTableForValue弱表是因为直接引用obj的话将影响到obj的gc,因为Jury等着obj释放时跟着自动释放,但Jury引用了本函数,而本函数又引用obj的话就循环引用了(想依赖弱引用做自动回收是会有这个问题的)
			local showNode = UI.UpdateVisibleJuryTableForValue[tab_str]
			-- print('Cat:UIHelper.lua[40] showNode', showNode, jury:IsNoneVote())
			if showNode then
				--没人投票的话就说明可以显示啦
				showNode:SetActive(jury:IsNoneVote())
			end
		end
		jury:CallBackOnResultChange(on_jury_change)
		UI.UpdateVisibleJuryTable[obj] = jury
	end
	if is_show then
		UI.UpdateVisibleJuryTable[obj]:UnVote(reason)
	else
		--想隐藏就投票
		UI.UpdateVisibleJuryTable[obj]:Vote(reason)
	end
end

local find = string.find
local gsub = string.gsub
local Split = Split
UI.G_ComponentMapForGetChildren = {
	img = "Image", txtnav = "Text", tog = "Toggle", imgex = "ImageExtend", outline = "Outline", raw = "RawImage", scroll = "ScrollRect", input = "InputField", txt = "TextMeshProUGUI", inp = "TMP_InputField",
}
function UI.GetChildren( self, parent, names, ignoreExist )
	assert(parent, "UIHelper:GetChildren() cannot find transform!")
	for i=1,#names do
		local name_parts = Split(names[i], ":")
		local full_name = name_parts[1]
		local short_name = full_name
		if short_name and find(short_name,"/") then
			short_name = gsub(short_name,".+/","")
		end
		if not ignoreExist then
			assert(self[short_name] == nil, short_name .. " already exists")
		end
		if short_name then
			self[short_name] = parent:Find(full_name)
		end
		assert(self[short_name], "cannot find child : "..short_name)
		for j=2,#name_parts do
			if name_parts[j] == "obj" then
				self[short_name.."_"..name_parts[j]] = self[short_name].gameObject
			elseif UI.G_ComponentMapForGetChildren[name_parts[j]] then
				local component = self[short_name]:GetComponent(UI.G_ComponentMapForGetChildren[name_parts[j]])
				if component==nil then
					print("cannot find component "..name_parts[j].." in child "..short_name)
				end
				self[short_name.."_"..name_parts[j]] = component
			else
				assert(false, "cannot find this component short name : "..name_parts[j])
			end
		end
	end
end

--返回对齐方式字符串
function UI.AlignTypeToStr( alignment )
	local vert_align = "Upper"
	if alignment == TextAnchor.UpperLeft
	or alignment == TextAnchor.UpperCenter
	or alignment == TextAnchor.UpperRight then
		vert_align = "Upper"
	elseif alignment == TextAnchor.MiddleLeft
		or alignment == TextAnchor.MiddleCenter
		or alignment == TextAnchor.MiddleRight then
		vert_align = "Middle"
	elseif alignment == TextAnchor.LowerLeft
		or alignment == TextAnchor.LowerCenter
		or alignment == TextAnchor.LowerRight then
		vert_align = "Lower"
	end

	local hori_align = "Left"
	if alignment == TextAnchor.UpperLeft
	or alignment == TextAnchor.MiddleLeft
	or alignment == TextAnchor.LowerLeft then
		hori_align = "Left"
	elseif alignment == TextAnchor.UpperCenter
		or alignment == TextAnchor.MiddleCenter
		or alignment == TextAnchor.LowerCenter then
		hori_align = "Center"
	elseif alignment == TextAnchor.UpperRight
		or alignment == TextAnchor.MiddleRight
		or alignment == TextAnchor.LowerRight then
		hori_align = "Right"
	end
	return hori_align, vert_align
end

function UI.SetPositionXYZ( transform, x, y, z )
	if transform then
		transform.position = Vector3.New(x, y, z)
	end
end

function UI.SetPositionXY( transform, x, y )
	UI.SetPositionXYZ(transform, x, y, 0)
end

function UI.SetPositionX( transform, value )
	if transform then
		local curPos = transform.position
		UI.SetPositionXYZ(transform, value, curPos.y, curPos.z)
	end
end

function UI.SetPositionY( transform, value )
	if transform then
		local curPos = transform.position
		UI.SetPositionXYZ(transform, curPos.x, value, curPos.z)
	end
end

function UI.SetPositionZ( transform, value )
	if transform then
		local curPos = transform.position
		UI.SetPositionXYZ(transform, curPos.x, curPos.y, value)
	end
end

function UI.SetPosition(transform, pos)
	UI.SetPositionXYZ(transform, pos.x, pos.y, pos.z)
end

function UI.SetLocalPositionXYZ( transform, x, y, z )
	if transform then
		transform.localPosition = Vector3.New(x, y, z)
	end
end

function UI.SetLocalPositionXY( transform, x, y )
	UI.SetLocalPositionXYZ(transform, x, y, 0)
end

function UI.SetLocalPositionX( transform, value )
	if transform then
		local curPos = transform.localPosition
		UI.SetLocalPositionXYZ(transform, value, curPos.y, curPos.z)
	end
end

function UI.SetLocalPositionY( transform, value )
	if transform then
		local curPos = transform.localPosition
		UI.SetLocalPositionXYZ(transform, curPos.x, value, curPos.z)
	end
end

function UI.SetLocalPositionZ( transform, value )
	if transform then
		local curPos = transform.localPosition
		UI.SetLocalPositionXYZ(transform, curPos.x, curPos.y, value)
	end
end

function UI.SetLocalPosition(transform, pos)
	UI.SetLocalPositionXYZ(transform, pos.x, pos.y, pos.z)
end

function UI.GetPositionXYZ(transform)
	if transform then
		local pos = transform.position
		return pos.x, pos.y, pos.z
	end
	return 0
end

function UI.GetLocalPositionX(transform)
	if transform then
		local pos = transform.localPosition
		return pos.x
	end
	return 0
end

function UI.GetLocalPositionY(transform)
	if transform then
		local pos = transform.localPosition
		return pos.y
	end
	return 0
end

function UI.GetLocalPositionZ(transform)
	if transform then
		local pos = transform.localPosition
		return pos.z
	end
	return 0
end

function UI.GetLocalPositionXY(transform)
	if transform then
		local pos = transform.localPosition
		return pos.x, pos.y
	end
	return 0, 0
end

function UI.GetLocalPositionXYZ(transform)
	if transform then
		local pos = transform.localPosition
		return pos.x, pos.y, pos.z
	end
	return 0, 0, 0
end

function UI.SetAnchoredPositionXY( transform, x, y )
	if transform then
		transform.anchoredPosition = Vector2.New(x, y)
	end
end

function UI.SetAnchoredPositionX( transform, value )
	if transform then
		local curPos = transform.anchoredPosition
		UI.SetLocalPositionXY(transform, value, curPos.y, curPos.z)
	end
end

function UI.SetAnchoredPositionY( transform, value )
	if transform then
		local curPos = transform.anchoredPosition
		UI.SetAnchoredPositionXY(transform, curPos.x, value, curPos.z)
	end
end

function UI.SetAnchoredPosition(transform, pos)
	UI.SetAnchoredPositionXY(transform, pos.x, pos.y, pos.z)
end

function UI.GetAnchoredPositionX(transform)
	if transform then
		local pos = transform.anchoredPosition
		return pos.x
	end
	return 0
end

function UI.GetAnchoredPositionY(transform)
	if transform then
		local pos = transform.anchoredPosition
		return pos.y
	end
	return 0
end

function UI.GetAnchoredPositionXY(transform)
	if transform then
		local pos = transform.anchoredPosition
		return pos.x, pos.y
	end
	return 0, 0
end

function UI.SetLocalScaleXYZ( transform, x, y, z )
	if transform then
		transform.localScale = Vector3(x, y, z)
	end
end

function UI.SetSizeDeltaXY(transform, x, y)
	if transform then
        transform.sizeDelta = Vector2.New(x, y)
	end
end

function UI.SetSizeDelta(transform, size)
	if transform and size then
        transform.sizeDelta = Vector2.New(size.x, size.y)
	end
end

function UI.SetSizeDeltaX(transform, value)
	if transform then
        transform.sizeDelta = Vector2.New(value, transform.sizeDelta.y)
	end
end

function UI.SetSizeDeltaY(transform, value)
	if transform then
        transform.sizeDelta = Vector2.New(transform.sizeDelta.x, value)
	end
end

function UI.GetSizeDeltaXY(transform)
	if transform then
		local size = transform.sizeDelta
		return size.x, size.y
	end
	return 0, 0
end

function UI.GetSizeDelta(transform)
	if transform then
		return transform.sizeDelta
	end
	return Vector2.zero
end

function UI.GetSizeDeltaX(transform)
	if transform then
		local size = transform.sizeDelta
		return size.x
	end
	return 0
end

function UI.GetSizeDeltaY(transform)
	if transform then
		local size = transform.sizeDelta
		return size.y
	end
	return 0
end

function UI.SetBg( self, rawImg, bg_res, is_auto_size, loaded_call_back )
	print('Cat:UIHelper.lua[355] self, rawImg, bg_res, is_auto_size, loaded_call_back', self, rawImg, bg_res, is_auto_size, loaded_call_back)
	if not rawImg then return end
	rawImg.gameObject:SetActive(false)
	local on_loaded = function (  )
		print('Cat:UIHelper.lua[358] self.destroyed', self.destroyed)
		if self.destroyed then return end
		rawImg.gameObject:SetActive(true)
		if loaded_call_back then
			loaded_call_back()
		end
	end
	UI.SetRawImage(self, rawImg, bg_res, is_auto_size, on_loaded)
end

function UI.SetImage( self, img, resPath, isAutoSize, loadedCallBack )
	if not self or not img then 
		LogError("UI.SetImage failed "..tostring(self).." img:"..tostring(img).." resPath:"..resPath)
		return 
	end
	resPath = UI.FillUIResPath(resPath)
	self.__img_cur_res__ = self.__img_cur_res__ or {}
	self.__img_cur_res__[img] = resPath
	local onLoadedImg = function ( objs )
		if not objs or not objs[0] or self.destroyed or not img or img:IsDestroyed() or self.__img_cur_res__[img] ~= resPath then return end
		img.sprite = objs[0]
		if isAutoSize then
			img:SetNativeSize()
		end
		if loadedCallBack then
			loadedCallBack()
		end
	end
	ResMgr:LoadSprite(resPath, onLoadedImg)
end

function UI.SetRawImage( self, img, resPath, isAutoSize, loadedCallBack )
	-- print('Cat:UIHelper.lua[SetRawImage] self, img, resPath, isAutoSize', self, img, resPath, isAutoSize)
	if not self or not img then 
		LogError("UI.SetRawImage failed "..tostring(self).." img:"..tostring(img).." resPath:"..resPath)
		return 
	end
	if not self or not img then return end
	resPath = UI.FillUIResPath(resPath)
	self.__img_cur_res__ = self.__img_cur_res__ or {}
	self.__img_cur_res__[img] = resPath
	local onLoadedImg = function ( objs )
		-- print('Cat:UIHelper.lua[SetRawImage] self.__img_cur_res__[img], ', self.__img_cur_res__[img], resPath)
		if not objs or not objs[0] or self.destroyed or not img or img:IsDestroyed() or self.__img_cur_res__[img] ~= resPath then return end
		img.texture = objs[0]
		if isAutoSize then
			img:SetNativeSize()
		end
		if loadedCallBack then
			loadedCallBack()
		end
	end
	ResMgr:LoadTexture(resPath, onLoadedImg)
end
