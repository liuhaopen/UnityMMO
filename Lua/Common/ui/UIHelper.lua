local GameObject = GameObject
local Util = Util
local LuaEventListener = LuaEventListener
local LuaClickListener = LuaClickListener
local LuaDragListener = LuaDragListener
local WordManager = WordManager

--设置position位置
function SetGlobalPosition(transform, x, y, z)
	if transform then
		Util.SetPosition(transform, x, y, z)
	end
end

function SetGlobalPositionX(transform, x)
	if transform then
		x = x or 0
		Util.SetPositionX(transform, x)
	end
end

function SetGlobalPositionY(transform, y)
	if transform then
		y = y or 0
		Util.SetPositionY(transform, y)
	end
end

function SetGlobalPositionZ(transform, z)
	if transform then
		z = z or 0
		Util.SetPositionZ(transform, z)
	end
end

function GetGlobalPosition( transform )
	if not transform then return end
	return Util.GetPosition(transform)
end

function GetGlobalPositionX(transform)
	return Util.GetPositionX(transform)
end


function GetGlobalPositionY(transform)
	return Util.GetPositionY(transform)
end


function GetGlobalPositionZ(transform)
	return Util.GetPositionZ(transform)
end

--设置localPosition位置
function SetLocalPosition(transform, x, y, z)
	if transform then
		x = x or 0
		y = y or x
		z = z or x
		Util.SetLocalPosition(transform, x, y, z)
	end
end

function SetLocalPositionX(transform, x)
	if transform then
		x = x or 0
		Util.SetLocalPositionX(transform, x)
	end
end

function SetLocalPositionY(transform, y)
	if transform then
		y = y or 0
		Util.SetLocalPositionY(transform, y)
	end
end

function SetLocalPositionZ(transform, z)
	if transform then
		z = z or 0
		Util.SetLocalPositionZ(transform, z)
	end
end

function GetLocalPosition( transform )
	if not transform then return end
	return Util.GetLocalPosition(transform)
end

function GetLocalPositionX(transform)
	return Util.GetLocalPositionX(transform)
end

function GetLocalPositionY(transform)
	return Util.GetLocalPositionY(transform)
end

function GetLocalPositionZ(transform)
	return Util.GetLocalPositionZ(transform)
end

--设置localscale
function SetLocalScale(transform, x, y, z)
	if transform then
		x = x or 1
		y = y or x
		z = z or x
		Util.SetLocalScale(transform, x, y, z)
	end
end

function GetLocalScale(transform)
	if transform then
		return Util.GetLocalScale(transform)
	end
end

--获取包围盒的y值
function GetRenderBoundsSize(render)
	if render then
		return Util.GetRenderBoundsSize(render)
	end
end

--设置localRotation
function SetLocalRotation(transform, x, y, z, w)
	if transform then
		x = x or 0
		y = y or x
		z = z or x
		w = w or 1
		Util.SetLocalRotation(transform, x, y, z, w)
	end
end

function GetLocalRotation(transform)
	if transform then
		return Util.GetLocalRotation(transform)
	end
end

--设置rotate
function SetRotate(transform, x, y, z)
	if transform then
		Util.SetRotate(transform, x, y, z)
	end
end

function GetRotate(transform)
	if transform then
		Util.GetRotate(transform)
	end
end

function SetSizeDelta(transform, x, y)
	if transform then
		Util.SetSizeDelta(transform, x, y)
	end
end

function SetSizeDeltaX(transform, x)
	if transform then
		Util.SetSizeDeltaX(transform, x)
	end
end

function SetSizeDeltaY(transform, y)
	if transform then
		Util.SetSizeDeltaY(transform, y)
	end
end


function GetSizeDeltaX(transform)
	if transform then
		return Util.GetSizeDeltaX(transform)
	end
end

function GetSizeDeltaY(transform)
	if transform then
		return Util.GetSizeDeltaY(transform)
	end
end

--添加点击事件
function AddClickEvent(target,call_back,use_sound)
	if target then
		if use_sound  then
			local function call_back_2(target,...)
				EventSystem.Fire(GlobalEventSystem,EventName.PLAY_UI_EFFECT_SOUND,use_sound)
				call_back(target,...)
			end

			LuaClickListener.Get(target).onClick = call_back_2
		else
			LuaClickListener.Get(target).onClick = call_back
		end
	end
end

function RemoveClickEvent(target)
	if target then
		LuaClickListener.Remove(target)
	end
end

--添加按下事件
function AddDownEvent(target,call_back,use_sound)
	if target then
		if use_sound  then
			local function call_back_2(target,...)
				EventSystem.Fire(GlobalEventSystem,EventName.PLAY_UI_EFFECT_SOUND,use_sound)
				call_back(target,...)
			end

			LuaEventListener.Get(target).onDown = call_back_2
		else
			LuaEventListener.Get(target).onDown = call_back
		end
	end
end

function RemoveEvent(target)
	if target then
		LuaEventListener.Remove(target)
	end
end

--添加进入事件
function AddEnterEvent(target,call_back)
	if target then
		LuaEventListener.Get(target).onEnter = call_back
	end
end

--添加离开事件
function AddExitEvent(target,call_back)
	if target then
		LuaEventListener.Get(target).onExit = call_back
	end
end

--添加松开事件
function AddUpEvent(target,call_back)
	if target then
		LuaEventListener.Get(target).onUp = call_back
	end
end

--添加拖拽事件
function AddDragEvent(target,call_back)
	if target then
		LuaDragListener.Get(target).onDrag = call_back
	end
end

--添加拖拽开始事件
function AddDragBeginEvent(target,call_back)
	if target then
		LuaDragListener.Get(target).onDragBegin = call_back
	end
end

--添加拖拽结束事件
function AddDragEndEvent(target,call_back)
	if target then
		LuaDragListener.Get(target).onDragEnd = call_back
	end
end

function RemoveDragEvent(target)
	if target then
		LuaDragListener.Remove(target) 
	end
end

--添加Text首行缩进
function AddSpace(num,str)
	if str ~= "" then
		return Util.AddSpace(num,WordManager:changeWords(str)) 
	else
		return WordManager:changeWords(str)
	end
end

--下列接口会尝试多种设置的，如果你已经知道你的节点是什么类型的就别用下列接口了


-- function GetLocalPositionX( transform )
-- 	if not transform then return end
	
-- 	return GetLocalPositionX(transform)
-- end

-- function GetLocalPositionY( transform )
-- 	if not transform then return end
	
-- 	return GetLocalPositionY(transform)
-- end

-- function GetLocalPositionZ( transform )
-- 	if not transform then return end
	
-- 	return GetLocalPositionZ(transform)
-- end

function SetLocalPosition( transform, x, y, z )
	if not transform then return end
	
	SetLocalPositionX(transform, x)
	SetLocalPositionY(transform, y)
	SetLocalPositionZ(transform, z)
end

function SetVisible( obj, is_show )
	if not obj then return end
	if obj.SetActive then
		obj:SetActive(is_show)
	elseif obj.SetVisible then
		obj:SetVisible(is_show)
	end
end
--上列接口会尝试多种设置的，如果你已经知道你的节点是什么类型的就别用上列接口了

UpdateVisibleJuryTable = {}
UpdateVisibleJuryTableForValue = {}
function InitForUIHelper(  )
	print('Cat:UIHelper.lua[InitForUIHelper]')
	setmetatable(UpdateVisibleJuryTable, {__mode = "k"})   
	setmetatable(UpdateVisibleJuryTableForValue, {__mode = "v"})   
end

--你显示隐藏前都要给我一个理由,我会综合考虑,只会在没有任何理由隐藏时我才会真正地显示出来
function UpdateVisibleByJury( obj, is_show, reason )
	if not obj then return end
	local tab_str = tostring(obj)
	UpdateVisibleJuryTableForValue[tab_str] = obj
	if not UpdateVisibleJuryTable[obj] then
		local jury = Jury.New()
		--当陪审团的投票结果变更时触发 
		local on_jury_change = function (  )
			--之所以用UpdateVisibleJuryTableForValue弱表是因为直接引用obj的话将影响到obj的gc,因为Jury等着obj释放时跟着自动释放,但Jury引用了本函数,而本函数又引用obj的话就循环引用了(想依赖弱引用做自动回收是会有这个问题的)
			if UpdateVisibleJuryTableForValue[tab_str] then
				--没人投票的话就说明可以显示啦
				-- print('Cat:UIHelper.lua[obj] jury:IsNoneVote()', jury:IsNoneVote())
				SetVisible(UpdateVisibleJuryTableForValue[tab_str], jury:IsNoneVote())
			end
		end
		jury:CallBackOnResultChange(on_jury_change)
		UpdateVisibleJuryTable[obj] = jury
	end
	if is_show then
		UpdateVisibleJuryTable[obj]:UnVote(reason)
	else
		--想隐藏就投票
		UpdateVisibleJuryTable[obj]:Vote(reason)
	end
end

--滚动到目标点，让其尽量显示在滚动容器的中间
function ScrollToCenter( Content, item, item_width, size )--最后一个是容器 size 的大小
	-- print("huangcong: [318] Content, item, item_width: ",Content, item, item_width,size)
	--画布 item item_width scrollSize
	item_width = item_width or 80--默认把节点的高度当作100
	local scroll_size = size
	local scroll_real_size = Content.sizeDelta 
	local item_real_pos = nil
	item_real_pos = item:GetPosition()
	local new_fit_x = 0
	local condition1 = item_real_pos.x - scroll_size.x/2 --第一部分是判断左边极限
	local condition2 = item_real_pos.x + scroll_size.x/2 - scroll_real_size.x--右边极限
	if condition1 > 0 and condition2 < 0 then
		new_fit_x = -(item_real_pos.x - scroll_size.x/2) - item_width/2
	elseif condition1 <= 0 then
		new_fit_x = 0
	elseif condition2 >= 0 then
		new_fit_x = -(scroll_real_size.x - scroll_size.x)+1
	end
	Content.localPosition = Vector3(new_fit_x ,0 ,0)
end

local find = string.find
local gsub = string.gsub
function GetChildren( self, parent, names )
	for i=1,#names do
		local key = names[i]
		if key and find(key,"/") then
			key = gsub(key,".+/","")
		end
		assert(self[key] == nil, key .. " already exists")
		if key then
			self[key] = parent.Find(names[i])
		end
	end
end