local FLT_EPSILON = 1.192092896e-07

local ActionInterval = Ac.OO.Class{
	type 	= "ActionInterval",
	__index = {
		_firstTick = true, _elapsed = 0, _duration = 0,
		Start = function(self)

		end,
		IsDone = function(self)
			return self._elapsed >= self._duration
		end,
		Step = function(self, dt)
			if not self._firstTick then
		    	self._firstTick = nil
		        self._elapsed = 0
		    else
		        self._elapsed = self._elapsed + dt
		    end
		    --把经历的时间转化为0~1的进度
		    local updateDt = math.max(0,math.min(1,self._elapsed / math.max(self._duration, FLT_EPSILON)))
		    self:Update(updateDt)
		end,
	},
}
return ActionInterval