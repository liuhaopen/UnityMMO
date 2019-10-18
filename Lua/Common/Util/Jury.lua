-- Jury = Jury or BaseClass()
--用BaseClass的话New了就要手动DeleteMe啊,好烦,lua本来就帮我们管理内存了还要多此一举
local Jury = {}

--陪审团类，类似于“位操作”对象，因为StateFlag.H1的语义太不友好了，所以就专门弄多一个，建议对每个用途弄一个枚举作位名，但我知道大家都很懒的，用字符串作位名也可以但最好写具体写长点以免重复了
--用法：
--[[
	self.is_need_hide_jury = Jury.New()--判断是否需要隐藏的陪审团
	--在A情况下需要隐藏，那么A就投票了
	self.is_need_hide_jury:Vote("HideBecauseA")
	--A情况过后可以取消投票
	self.is_need_hide_jury:UnVote("HideBecauseA")
	--这时通过IsNoneVote得知是否没人投票，真的话就可以显示节点：
	node:SetBool(WidgetProperty.Visible, self.is_need_hide_jury:IsNoneVote())
	--但其实不用每次投票后都手动显示隐藏节点，你可以设置投票结果变更时的回调：
	local on_jury_change = function (  )
		--每次调用了Vote或UnVote方法后导致投票结果变更时就会触发此函数
		node:SetBool(WidgetProperty.Visible, self.is_need_hide_jury:IsNoneVote())
	end
	self.is_need_hide_jury:CallBackOnResultChange(on_jury_change)
--]]
Jury.__index = Jury

function Jury.New(  )
	local jury = {}
	setmetatable(jury, {__index = Jury})
	jury:Reset()
	return jury
end

--重置
function Jury:Reset(  )
	self.vote_table = {}
	self.is_none_vote = nil
end

--投票结果变更时触发，即从无人投切换到有人或有人投切换到无人投时触发，注意1人变到N人投也不会触发，因为结果都一样的
function Jury:CallBackOnResultChange( call_back )
	self.call_back_on_result_change = call_back
end

--投票情况变更时触发，一般用CallBackOnResultChange就足够了并且性能会好点，你确定要用这个回调吗？我都没用过
function Jury:CallBackOnVoteChange( call_back )
	self.call_back_on_vote_change = call_back
end

--某人name投票
function Jury:Vote( name )
	if not name then return end
	self.vote_table[name] = true
	--如果之前是没人投票的，现在有人投所以结果就变更了
	local is_result_change = self.is_none_vote or (self.is_none_vote==nil)--self.is_none_vote的值需要在回调执行前修改，因为有可能回调里会调用本类的Reset，回调后再修改本类的值的话前面的Reset就没用了
	self.is_none_vote = false
	if is_result_change and self.call_back_on_result_change then
		self.call_back_on_result_change()
	end
	if self.call_back_on_vote_change then
		self.call_back_on_vote_change()
	end
end

--某人name取消投票
function Jury:UnVote( name )
	if not name then return end
	self.vote_table[name] = nil
	local cur_is_none_vote = self:IsNoneVote()
	local is_result_change = self.is_none_vote ~= cur_is_none_vote
	self.is_none_vote = cur_is_none_vote
	if is_result_change and self.call_back_on_result_change then
		self.call_back_on_result_change()
	end
	if self.call_back_on_vote_change then
		self.call_back_on_vote_change()
	end
end

--是否有人投票了
function Jury:HasAnyVote(  )
	return next(self.vote_table)~=nil
end

--是否没人投票
function Jury:IsNoneVote(  )
	return next(self.vote_table)==nil
end

--某人是否投票了
function Jury:HadVote( name )
	return self.vote_table[name] ~= nil
end

function Jury:PrintVote(  )
	-- print("Cat:Jury [PrintVote] self.vote_table:", self.vote_table)
	-- PrintTable(self.vote_table)
	-- print("Cat:Jury [end]")
end

return Jury