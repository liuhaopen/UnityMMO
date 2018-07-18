local GameObject = GameObject
local Util = Util
local LuaEventListener = LuaEventListener
local LuaClickListener = LuaClickListener
local LuaDragListener = LuaDragListener
local WordManager = WordManager

--下列接口会尝试多种设置的，如果你已经知道你的节点是什么类型的就别用下列接口了
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