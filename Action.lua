cc = cc or {}

cc.Action = cc.Action or BaseClass()

function cc.Action:Constructor()
	self._originalTarget = nil
	self._target = nil
	self._tag = cc.Action.INVALID_TAG
	self._flags = 0
    self._classType = "Action"
end

function cc.Action:description()
    return "Action"
end

function cc.Action:clone()
    print("Cat_Error:CCAction.lua [Action:clone] should not exec this method!")
    return nil
end

function cc.Action:reverse()
    print("Cat_Error:CCAction.lua [Action:reverse] should not exec this method!")
    return nil
end

function cc.Action:isDone()
    return true
end

function cc.Action:startWithTarget(target)
    --如果已经设置过了就不要再覆盖了
    if not self._target then
	    self._target = target
    end
    self._originalTarget = target
end

function cc.Action:stop()
    self._target = nil
end

function cc.Action:step(dt)
    --override me
end

function cc.Action:update(time)
    --override me
end

function cc.Action:getTarget()
    return self._target
end

function cc.Action:setTarget(target)
    self._target = target
end

function cc.Action:getOriginalTarget()
    return self._originalTarget
end

function cc.Action:setOriginalTarget(originalTarget)
    self._originalTarget = originalTarget
end

function cc.Action:getTag()
    return self._tag
end

function cc.Action:setTag(tag)
	self._tag = tag
end

function cc.Action:getFlags()
    return self._flags
end

function cc.Action:setFlags(flags)
    self._flags = flags
end

cc.FiniteTimeAction = cc.FiniteTimeAction or BaseClass(cc.Action)

function cc.FiniteTimeAction:Constructor()
    self._duration = 0
    self._classType = "FiniteTimeAction"
end

function cc.FiniteTimeAction:getDuration()
    return self._duration
end

function cc.FiniteTimeAction:setDuration(duration)
    self._duration = duration
end

function cc.FiniteTimeAction:reverse()
	print("Cat_Error:CCAction.lua [FiniteTimeAction:reverse] should not exec this method!")
    return nil;
end

function cc.FiniteTimeAction:clone()
    print("Cat_Error:CCAction.lua [FiniteTimeAction:clone] should not exec this method!")
    return nil;
end
