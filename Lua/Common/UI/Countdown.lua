--倒计时组件,内置管理定时器,界面销毁时会自动移除,请放心使用
UI.Countdown = UI.Countdown or BaseClass(UI.UIComponent)
--传入结束时间end_time,截至该时间前每秒都会触发传入的step_call_back回调,其回调函数的参数为剩余时间
function UI.Countdown:CountdownByEndTime( end_time, step_call_back, duration )
	if not end_time or not step_call_back then 
		print('Cat:UIComponents.lua[UI.Countdown:CountdownByEndTime] failed! some arge nil:', tostring(end_time), tostring(step_call_back))
		return 
	end

	self.end_time = end_time
	self.step_call_back = step_call_back
	if not self.timer then
		local function step_countdown( )
			local curTime = Time:GetServerTime()
			local left_time = self.end_time - curTime/1000
			if left_time <= 0 then
				self:OnClose()
			else
				self.is_counting = true
			end
			self.step_call_back(left_time)
		end
		self.timer = Timer.New(step_countdown, duration and duration or 1, -1)
		self.timer:Start()
		step_countdown()
	elseif duration then
		self.timer:SetDuration(duration)
	end
end

function UI.Countdown:CountdownByLeftTime( left_time, step_call_back, duration )
	local curTime = Time:GetServerTime()
	self:CountdownByEndTime(curTime/1000+left_time, step_call_back, duration)
end

function UI.Countdown:DelayCallByLeftTime( left_time, step_call_back, duration )
	self:CountdownByLeftTime(left_time, function(_left_time)
		if _left_time<=0 and step_call_back then
			step_call_back()
		end
	end, duration)
end

function UI.Countdown:IsCounting(  )
	return self.is_counting
end

function UI.Countdown:Stop(  )
	self:OnClose()
end

function UI.Countdown:OnClose(  )
	if self.timer then
		self.timer:Stop()
		self.timer = nil
	end
	self.is_counting = false
end