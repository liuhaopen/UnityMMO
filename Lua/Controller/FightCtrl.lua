FightCtrl = {}
-- local this = FightCtrl
FightCtrl.Instance = nil
function FightCtrl:GetInstance()
	if not FightCtrl.Instance then
		self:Init()
		FightCtrl.Instance = self
	end
	return FightCtrl.Instance
end

function FightCtrl:Init(  )
	self:RegisterCocosActions()

end


-- -- FightCtrl:GetInstance():AddAction(main_role, ConfigFight[1001])
-- function FightCtrl:AddAction( fighter, action_cfg, extra_param )
-- 	if action_cfg == nil then
-- 		print("Cat_Error:FightCtrl.lua [AddAction] action_cfg is nil", debug.traceback())
-- 		return
-- 	end
-- 	if fighter == nil then
-- 		print("Cat_Error:FightCtrl.lua [AddAction] fighter is nil", debug.traceback())
-- 		return
-- 	end
-- 	local element = self._targets[fighter]
--     if not element then
--     	element = {fighter = fighter, param = {action_cfg = action_cfg, extra_param=extra_param}, elapsed = 0}
-- 		self._targets[fighter] = element
--     end
-- end

-- function FightCtrl:Update( deltaTime )
-- 	for k,v in pairs(self._targets) do
--         self._currentTarget = v
--         self._currentTargetSalvaged = false

--         if not self._currentTarget.paused then
--         	for i,v in ipairs(self._currentTarget.interval_actions or {}) do
--         		v.step_func()
--         	end
--         	self._currentTarget.elapsed = self._currentTarget.elapsed + deltaTime
--         	self._currentTarget.clip_index = self._currentTarget.clip_index or 1
--         	-- if self._currentTarget.param.action_cfg[self._currentTarget.clip_index] then
--         	--每帧最多只跑一个clip
--     		local cur_clip = self._currentTarget.param.action_cfg[self._currentTarget.clip_index]
--     		if cur_clip and cur_clip.time <= self._currentTarget.elapsed then
-- 	        	self._currentTarget.actionIndex =  1
-- 	        	while self._currentTarget.actionIndex <= #(cur_clip.actions) do 
-- 	        		local curAction = self._currentTarget.param.action_cfg[self._currentTarget.actionIndex]
-- 	                self._currentTarget.currentAction = curAction
-- 	                repeat
-- 				        if curAction == nil then
-- 	                		self._currentTarget.actionIndex = self._currentTarget.actionIndex + 1 
-- 				            break
-- 				        end

-- 		                self._currentTarget.currentActionSalvaged = false
-- 		                local func = self.action_funcs[curAction.name]
-- 		                if func then
-- 			                if curAction.duration then
-- 			                	self._currentTarget.interval_actions = self._currentTarget.interval_actions or {}
-- 			                	local interval_action = {step_func = func, }
-- 			                	table.insert(self._currentTarget.interval_actions, interval_action)
-- 			                	func(self._currentTarget.fighter, self._currentTarget.param, curAction.param)
--     							-- local updateDt = math.max(0,math.min(1,self._elapsed / math.max(self._duration,cc.ActionInterval.FLT_EPSILON)))
-- 			                else
-- 			                	func(self._currentTarget.fighter, self._currentTarget.param, curAction.param)
-- 			                end
-- 		                end
-- 		                -- self._currentTarget.currentAction:step(dt)

-- 		                -- if self._currentTarget.currentActionSalvaged then
-- 		                --     self._currentTarget.currentAction = nil
-- 		                -- elseif self._currentTarget.currentAction:isDone() then
-- 		                --     self._currentTarget.currentAction:stop()

-- 		                --     local action = self._currentTarget.currentAction
-- 		                --     self._currentTarget.currentAction = nil
-- 		                --     self:removeAction(action)
-- 		                -- end

-- 		                -- self._currentTarget.currentAction = nil
-- 		                self._currentTarget.actionIndex = self._currentTarget.actionIndex + 1 
-- 				    until true
-- 	            end
--             end
--         end
--         -- only delete currentTarget if no actions were scheduled during the cycle (issue #481)
--         if self._currentTargetSalvaged and #(self._currentTarget.actions) == 0 then
-- 			self._targets[k] = nil
--     	end
-- 	end

--     self._currentTarget = nil
-- end

