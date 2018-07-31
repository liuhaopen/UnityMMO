cc = cc or {}

M_PI_X_2 = math.pi * 2.0
M_PI_2 = math.pi / 2.0
M_PI = math.pi

cc.ActionEase = cc.ActionEase or BaseClass(cc.ActionInterval)

function cc.ActionEase:Constructor(action)
    -- self:initWithAction(action)
end

function cc.ActionEase:initWithAction(action)
    cc.ActionInterval.initWithDuration(self, action:getDuration())
    self._inner = action
    -- print("Cat:CCActionEase.lua [initWithAction] self._inner: ",self._inner)
end

function cc.ActionEase:clone()
    print("Cat_Error:CCActionEase.lua [reverse] cannot clone ease action!")
    return nil;
end
    
function cc.ActionEase:reverse()
    print("Cat_Error:CCActionEase.lua [reverse] cannot reverse ease action!")
    return nil;
end

-- function cc.ActionEase:step(time)
--     cc.ActionInterval.step(self, time)
-- end

function cc.ActionEase:startWithTarget(target)
	-- print("Cat:CCActionEase.lua [startWithTarget]")
    cc.ActionInterval.startWithTarget(self, target);
    self._inner:startWithTarget(self._target);
end

function cc.ActionEase:stop(void)
    self._inner:stop();
    cc.ActionInterval.stop(self)
end

function cc.ActionEase:update(time)
    self._inner:update(time)
end

function cc.ActionEase:getInnerAction()
    return self._inner
end

------------------------EaseRateAction start---------------------------
cc.EaseRateAction = cc.EaseRateAction or BaseClass(cc.ActionEase)

function cc.EaseRateAction:Constructor(action, rate)
    self:initWithAction(action, rate)
end

function cc.EaseRateAction:initWithAction(action, rate)
    cc.ActionEase.initWithAction(self, action)
    self._rate = rate
end

function cc.EaseRateAction:setRate(rate) 
    self._rate = rate
end
    
function cc.EaseRateAction:getRate() 
    return self._rate
end

--in
cc.EaseIn = cc.EaseIn or BaseClass(cc.EaseRateAction)

function cc.EaseIn:Constructor(action, rate)
    self:initWithAction(action, rate)
end

function cc.EaseIn:clone()
    return EaseIn.New(self._inner:clone(), self._rate)
end

function cc.EaseIn:update(time)
    self._inner:update(cc.tweenfunc.easeIn(time, self._rate))
end

function cc.EaseIn:reverse()
    return cc.EaseIn.New(self._inner:reverse(), 1/self._rate)
end

--out
cc.EaseOut = cc.EaseOut or BaseClass(cc.EaseRateAction)

function cc.EaseOut:Constructor(action, rate)
    self:initWithAction(action, rate)
end

function cc.EaseOut:clone()
    return EaseOut.New(self._inner:clone(), self._rate)
end

function cc.EaseOut:update(time)
    self._inner:update(cc.tweenfunc.easeOut(time, self._rate))
end

function cc.EaseOut:reverse()
    return cc.EaseOut.New(self._inner:reverse(), 1/self._rate)
end

--in out
cc.EaseInOut = cc.EaseInOut or BaseClass(cc.EaseRateAction)

function cc.EaseInOut:Constructor(action, rate)
    self:initWithAction(action, rate)
end

function cc.EaseInOut:clone()
    return EaseInOut.New(self._inner:clone(), self._rate)
end

function cc.EaseInOut:update(time)
    self._inner:update(cc.tweenfunc.easeInOut(time, self._rate))
end

function cc.EaseInOut:reverse()
    return cc.EaseInOut.New(self._inner:reverse(), self._rate)
end
------------------------EaseRateAction end---------------------------

------------------------EaseExponential start---------------------------
--in
cc.EaseExponentialIn = cc.EaseExponentialIn or BaseClass(cc.ActionEase)

function cc.EaseExponentialIn:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseExponentialIn:clone()
    return EaseExponentialIn.New(self._inner:clone())
end

function cc.EaseExponentialIn:update(time)
    self._inner:update(cc.tweenfunc.expoEaseIn(time))
end

function cc.EaseExponentialIn:reverse()
    return cc.EaseExponentialIn.New(self._inner:reverse())
end

--out
cc.EaseExponentialOut = cc.EaseExponentialOut or BaseClass(cc.ActionEase)

function cc.EaseExponentialOut:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseExponentialOut:clone()
    return EaseExponentialOut.New(self._inner:clone())
end

function cc.EaseExponentialOut:update(time)
    self._inner:update(cc.tweenfunc.expoEaseOut(time))
end

function cc.EaseExponentialOut:reverse()
    return cc.EaseExponentialOut.New(self._inner:reverse())
end

--in out
cc.EaseExponentialInOut = cc.EaseExponentialInOut or BaseClass(cc.ActionEase)

function cc.EaseExponentialInOut:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseExponentialInOut:clone()
    return EaseExponentialInOut.New(self._inner:clone())
end

function cc.EaseExponentialInOut:update(time)
    self._inner:update(cc.tweenfunc.expoEaseInOut(time))
end

function cc.EaseExponentialInOut:reverse()
    return cc.EaseExponentialInOut.New(self._inner:reverse())
end
------------------------EaseExponential end---------------------------

------------------------EaseSine start---------------------------
--in
cc.EaseSineIn = cc.EaseSineIn or BaseClass(cc.ActionEase)

function cc.EaseSineIn:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseSineIn:clone()
    return EaseSineIn.New(self._inner:clone())
end

function cc.EaseSineIn:update(time)
    self._inner:update(cc.tweenfunc.sineEaseIn(time))
end

function cc.EaseSineIn:reverse()
    return cc.EaseSineIn.New(self._inner:reverse())
end

--out
cc.EaseSineOut = cc.EaseSineOut or BaseClass(cc.ActionEase)

function cc.EaseSineOut:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseSineOut:clone()
    return EaseSineOut.New(self._inner:clone())
end

function cc.EaseSineOut:update(time)
    self._inner:update(cc.tweenfunc.sineEaseOut(time))
end

function cc.EaseSineOut:reverse()
    return cc.EaseSineOut.New(self._inner:reverse())
end

--in out
cc.EaseSineInOut = cc.EaseSineInOut or BaseClass(cc.ActionEase)

function cc.EaseSineInOut:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseSineInOut:clone()
    return EaseSineInOut.New(self._inner:clone())
end

function cc.EaseSineInOut:update(time)
    self._inner:update(cc.tweenfunc.sineEaseInOut(time))
end

function cc.EaseSineInOut:reverse()
    return cc.EaseSineInOut.New(self._inner:reverse())
end
------------------------EaseSine end---------------------------

------------------------EaseElastic start---------------------------
cc.EaseElastic = cc.EaseElastic or BaseClass(cc.ActionEase)

function cc.EaseElastic:Constructor(action, period)
    --抽象类所以不能直接New本类
    -- self:initWithAction(action, period)
end

function cc.EaseElastic:initWithAction(action, period)
    -- print("Cat:CCActionEase.lua [54] action,period: ",action,period)
    cc.ActionEase.initWithAction(self, action)
    period = period or 0.3
    self._period = period
end

function cc.EaseElastic:getPeriod()
    return self._period
end

function cc.EaseElastic:setPeriod(fPeriod)
    self._period = fPeriod 
end

cc.EaseElasticOut = cc.EaseElasticOut or BaseClass(cc.EaseElastic)

function cc.EaseElasticOut:Constructor(action, period)
    self:initWithAction(action, period)
end

function cc.EaseElasticOut:clone()
    return EaseElasticOut.New(self._inner:clone(), self._period)
end

function cc.EaseElasticOut:update(time)
    self._inner:update(cc.tweenfunc.elasticEaseOut(time, self._period));
end

function cc.EaseElasticOut:reverse()
    return cc.EaseElasticIn.New(self._inner:reverse(), self._period);
end

cc.EaseElasticIn = cc.EaseElasticIn or BaseClass(cc.EaseElastic)

function cc.EaseElasticIn:Constructor(action, period)
    self:initWithAction(action, period)
end

function cc.EaseElasticIn:clone()
    return cc.EaseElasticIn.New(self._inner:clone(), self._period)
end

function cc.EaseElasticIn:update(time)
    self._inner:update(cc.tweenfunc.elasticEaseIn(time, self._period));
end

function cc.EaseElasticIn:reverse()
    return cc.EaseElasticOut.New(self._inner:reverse(), self._period);
end

cc.EaseElasticInOut = cc.EaseElasticInOut or BaseClass(cc.EaseElastic)

function cc.EaseElasticInOut:Constructor(action, period)
    self:initWithAction(action, period)
end

function cc.EaseElasticInOut:clone()
    return cc.EaseElasticInOut.New(self._inner:clone(), self._period)
end

function cc.EaseElasticInOut:update(time)
    self._inner:update(cc.tweenfunc.elasticEaseInOut(time, self._period));
end

function cc.EaseElasticInOut:reverse()
    return cc.EaseElasticInOut.New(self._inner:reverse(), self._period);
end

------------------------EaseElastic end---------------------------

------------------------EaseBounce start---------------------------
--in
cc.EaseBounceIn = cc.EaseBounceIn or BaseClass(cc.ActionEase)

function cc.EaseBounceIn:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseBounceIn:clone()
    return EaseBounceIn.New(self._inner:clone())
end

function cc.EaseBounceIn:update(time)
    self._inner:update(cc.tweenfunc.bounceEaseIn(time))
end

function cc.EaseBounceIn:reverse()
    return cc.EaseBounceIn.New(self._inner:reverse())
end

--out
cc.EaseBounceOut = cc.EaseBounceOut or BaseClass(cc.ActionEase)

function cc.EaseBounceOut:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseBounceOut:clone()
    return EaseBounceOut.New(self._inner:clone())
end

function cc.EaseBounceOut:update(time)
    self._inner:update(cc.tweenfunc.bounceEaseOut(time))
end

function cc.EaseBounceOut:reverse()
    return cc.EaseBounceOut.New(self._inner:reverse())
end

--in out
cc.EaseBounceInOut = cc.EaseBounceInOut or BaseClass(cc.ActionEase)

function cc.EaseBounceInOut:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseBounceInOut:clone()
    return EaseBounceInOut.New(self._inner:clone())
end

function cc.EaseBounceInOut:update(time)
    self._inner:update(cc.tweenfunc.bounceEaseInOut(time))
end

function cc.EaseBounceInOut:reverse()
    return cc.EaseBounceInOut.New(self._inner:reverse())
end
------------------------EaseBounce end---------------------------

------------------------EaseBack start---------------------------
--in
cc.EaseBackIn = cc.EaseBackIn or BaseClass(cc.ActionEase)

function cc.EaseBackIn:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseBackIn:clone()
    return EaseBackIn.New(self._inner:clone())
end

function cc.EaseBackIn:update(time)
    self._inner:update(cc.tweenfunc.backEaseIn(time))
end

function cc.EaseBackIn:reverse()
    return cc.EaseBackIn.New(self._inner:reverse())
end

--out
cc.EaseBackOut = cc.EaseBackOut or BaseClass(cc.ActionEase)

function cc.EaseBackOut:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseBackOut:clone()
    return EaseBackOut.New(self._inner:clone())
end

function cc.EaseBackOut:update(time)
    self._inner:update(cc.tweenfunc.backEaseOut(time))
end

function cc.EaseBackOut:reverse()
    return cc.EaseBackOut.New(self._inner:reverse())
end

--in out
cc.EaseBackInOut = cc.EaseBackInOut or BaseClass(cc.ActionEase)

function cc.EaseBackInOut:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseBackInOut:clone()
    return EaseBackInOut.New(self._inner:clone())
end

function cc.EaseBackInOut:update(time)
    self._inner:update(cc.tweenfunc.backEaseInOut(time))
end

function cc.EaseBackInOut:reverse()
    return cc.EaseBackInOut.New(self._inner:reverse())
end
------------------------EaseBack end---------------------------

------------------------EaseQuadraticAction start---------------------------
--in
cc.EaseQuadraticActionIn = cc.EaseQuadraticActionIn or BaseClass(cc.ActionEase)

function cc.EaseQuadraticActionIn:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseQuadraticActionIn:clone()
    return EaseQuadraticActionIn.New(self._inner:clone())
end

function cc.EaseQuadraticActionIn:update(time)
    self._inner:update(cc.tweenfunc.quadraticIn(time))
end

function cc.EaseQuadraticActionIn:reverse()
    return cc.EaseQuadraticActionIn.New(self._inner:reverse())
end

--out
cc.EaseQuadraticActionOut = cc.EaseQuadraticActionOut or BaseClass(cc.ActionEase)

function cc.EaseQuadraticActionOut:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseQuadraticActionOut:clone()
    return EaseQuadraticActionOut.New(self._inner:clone())
end

function cc.EaseQuadraticActionOut:update(time)
    self._inner:update(cc.tweenfunc.quadraticOut(time))
end

function cc.EaseQuadraticActionOut:reverse()
    return cc.EaseQuadraticActionOut.New(self._inner:reverse())
end

--in out
cc.EaseQuadraticActionInOut = cc.EaseQuadraticActionInOut or BaseClass(cc.ActionEase)

function cc.EaseQuadraticActionInOut:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseQuadraticActionInOut:clone()
    return EaseQuadraticActionInOut.New(self._inner:clone())
end

function cc.EaseQuadraticActionInOut:update(time)
    self._inner:update(cc.tweenfunc.quadraticInOut(time))
end

function cc.EaseQuadraticActionInOut:reverse()
    return cc.EaseQuadraticActionInOut.New(self._inner:reverse())
end
------------------------EaseQuadraticAction end---------------------------

------------------------EaseQuarticAction start---------------------------
--in
cc.EaseQuarticActionIn = cc.EaseQuarticActionIn or BaseClass(cc.ActionEase)

function cc.EaseQuarticActionIn:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseQuarticActionIn:clone()
    return EaseQuarticActionIn.New(self._inner:clone())
end

function cc.EaseQuarticActionIn:update(time)
    self._inner:update(cc.tweenfunc.quartEaseIn(time))
end

function cc.EaseQuarticActionIn:reverse()
    return cc.EaseQuarticActionIn.New(self._inner:reverse())
end

--out
cc.EaseQuarticActionOut = cc.EaseQuarticActionOut or BaseClass(cc.ActionEase)

function cc.EaseQuarticActionOut:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseQuarticActionOut:clone()
    return EaseQuarticActionOut.New(self._inner:clone())
end

function cc.EaseQuarticActionOut:update(time)
    self._inner:update(cc.tweenfunc.quartEaseOut(time))
end

function cc.EaseQuarticActionOut:reverse()
    return cc.EaseQuarticActionOut.New(self._inner:reverse())
end

--in out
cc.EaseQuarticActionInOut = cc.EaseQuarticActionInOut or BaseClass(cc.ActionEase)

function cc.EaseQuarticActionInOut:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseQuarticActionInOut:clone()
    return EaseQuarticActionInOut.New(self._inner:clone())
end

function cc.EaseQuarticActionInOut:update(time)
    self._inner:update(cc.tweenfunc.quartEaseInOut(time))
end

function cc.EaseQuarticActionInOut:reverse()
    return cc.EaseQuarticActionInOut.New(self._inner:reverse())
end
------------------------EaseQuarticAction end---------------------------

------------------------EaseQuinticAction start---------------------------
--in
cc.EaseQuinticActionIn = cc.EaseQuinticActionIn or BaseClass(cc.ActionEase)

function cc.EaseQuinticActionIn:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseQuinticActionIn:clone()
    return EaseQuinticActionIn.New(self._inner:clone())
end

function cc.EaseQuinticActionIn:update(time)
    self._inner:update(cc.tweenfunc.quintEaseIn(time))
end

function cc.EaseQuinticActionIn:reverse()
    return cc.EaseQuinticActionIn.New(self._inner:reverse())
end

--out
cc.EaseQuinticActionOut = cc.EaseQuinticActionOut or BaseClass(cc.ActionEase)

function cc.EaseQuinticActionOut:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseQuinticActionOut:clone()
    return EaseQuinticActionOut.New(self._inner:clone())
end

function cc.EaseQuinticActionOut:update(time)
    self._inner:update(cc.tweenfunc.quintEaseOut(time))
end

function cc.EaseQuinticActionOut:reverse()
    return cc.EaseQuinticActionOut.New(self._inner:reverse())
end

--in out
cc.EaseQuinticActionInOut = cc.EaseQuinticActionInOut or BaseClass(cc.ActionEase)

function cc.EaseQuinticActionInOut:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseQuinticActionInOut:clone()
    return EaseQuinticActionInOut.New(self._inner:clone())
end

function cc.EaseQuinticActionInOut:update(time)
    self._inner:update(cc.tweenfunc.quintEaseInOut(time))
end

function cc.EaseQuinticActionInOut:reverse()
    return cc.EaseQuinticActionInOut.New(self._inner:reverse())
end
------------------------EaseQuinticAction end---------------------------

------------------------EaseCircleAction start---------------------------
--in
cc.EaseCircleActionIn = cc.EaseCircleActionIn or BaseClass(cc.ActionEase)

function cc.EaseCircleActionIn:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseCircleActionIn:clone()
    return EaseCircleActionIn.New(self._inner:clone())
end

function cc.EaseCircleActionIn:update(time)
    self._inner:update(cc.tweenfunc.circEaseIn(time))
end

function cc.EaseCircleActionIn:reverse()
    return cc.EaseCircleActionIn.New(self._inner:reverse())
end

--out
cc.EaseCircleActionOut = cc.EaseCircleActionOut or BaseClass(cc.ActionEase)

function cc.EaseCircleActionOut:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseCircleActionOut:clone()
    return EaseCircleActionOut.New(self._inner:clone())
end

function cc.EaseCircleActionOut:update(time)
    self._inner:update(cc.tweenfunc.circEaseOut(time))
end

function cc.EaseCircleActionOut:reverse()
    return cc.EaseCircleActionOut.New(self._inner:reverse())
end

--in out
cc.EaseCircleActionInOut = cc.EaseCircleActionInOut or BaseClass(cc.ActionEase)

function cc.EaseCircleActionInOut:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseCircleActionInOut:clone()
    return EaseCircleActionInOut.New(self._inner:clone())
end

function cc.EaseCircleActionInOut:update(time)
    self._inner:update(cc.tweenfunc.circEaseInOut(time))
end

function cc.EaseCircleActionInOut:reverse()
    return cc.EaseCircleActionInOut.New(self._inner:reverse())
end
------------------------EaseCircleAction end---------------------------

------------------------EaseCubicAction start---------------------------
--in
cc.EaseCubicActionIn = cc.EaseCubicActionIn or BaseClass(cc.ActionEase)

function cc.EaseCubicActionIn:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseCubicActionIn:clone()
    return EaseCubicActionIn.New(self._inner:clone())
end

function cc.EaseCubicActionIn:update(time)
    self._inner:update(cc.tweenfunc.cubicEaseIn(time))
end

function cc.EaseCubicActionIn:reverse()
    return cc.EaseCubicActionIn.New(self._inner:reverse())
end

--out
cc.EaseCubicActionOut = cc.EaseCubicActionOut or BaseClass(cc.ActionEase)

function cc.EaseCubicActionOut:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseCubicActionOut:clone()
    return EaseCubicActionOut.New(self._inner:clone())
end

function cc.EaseCubicActionOut:update(time)
    self._inner:update(cc.tweenfunc.cubicEaseOut(time))
end

function cc.EaseCubicActionOut:reverse()
    return cc.EaseCubicActionOut.New(self._inner:reverse())
end

--in out
cc.EaseCubicActionInOut = cc.EaseCubicActionInOut or BaseClass(cc.ActionEase)

function cc.EaseCubicActionInOut:Constructor(action)
    self:initWithAction(action)
end

function cc.EaseCubicActionInOut:clone()
    return EaseCubicActionInOut.New(self._inner:clone())
end

function cc.EaseCubicActionInOut:update(time)
    self._inner:update(cc.tweenfunc.cubicEaseInOut(time))
end

function cc.EaseCubicActionInOut:reverse()
    return cc.EaseCubicActionInOut.New(self._inner:reverse())
end
------------------------EaseQuinticAction end---------------------------
