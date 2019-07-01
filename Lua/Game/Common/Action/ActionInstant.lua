cc = cc or {}

cc.ActionInstant = cc.ActionInstant or BaseClass(cc.FiniteTimeAction)

function cc.ActionInstant:Constructor()
    self._classType = "ActionInstant"
end
    
function cc.ActionInstant:isDone()
    return true
end

function cc.ActionInstant:step(dt)
    self:update(1)
end

function cc.ActionInstant:update(time)
    -- nothing
end

--CallFunc start
cc.CallFunc = cc.CallFunc or BaseClass(cc.ActionInstant)

function cc.CallFunc:Constructor()
end

function cc.CallFunc.create(call_back, param, is_need_unpack)
    local action = cc.CallFunc.New()
    action:initWithFunction(call_back, param, is_need_unpack)
    return action
end

function cc.CallFunc:initWithFunction( call_back, param, is_need_unpack )
    self.call_back = call_back
    self.param = param
    self.is_need_unpack = is_need_unpack
end

function cc.CallFunc:clone()
    return cc.CallFunc.New(self.call_back)
end

function cc.CallFunc:reverse()
    return self:clone()
end

function cc.CallFunc:update(time)
    if self.call_back then
        if self.is_need_unpack then
           self.call_back(unpack(self.param))
        else
    	   self.call_back(self.param)
        end
    end
end
--CallFunc end


--Place start
cc.Place = cc.Place or BaseClass(cc.ActionInstant)

function cc.Place:Constructor(new_x, new_y, new_z)
    self:initWithPosition(new_x, new_y, new_z)
end

function cc.Place:initWithPosition( new_x, new_y, new_z )
    self.new_x = new_x
    self.new_y = new_y
    self.new_z = new_z
end

function cc.Place:clone()
    return cc.Place.New(self.new_x, self.new_y, self.new_z)
end

function cc.Place:reverse()
    return self:clone()
end

function cc.Place:update(time)
    if not self._target then return end
    cc.Wrapper.SetLocalPosition(self._target, self.new_x, self.new_y, self.new_z)
    -- if self.new_x then
    --     self._target:SetVectorL(WidgetProperty.Position, self.new_x)
    -- end

    --  if self.new_y then
    --     self._target:SetVectorR(WidgetProperty.Position, self.new_y)
    -- end
end
--Place end

--Show start
cc.Show = cc.Show or BaseClass(cc.ActionInstant)

function cc.Show:Constructor()
end

function cc.Show:clone()
    return cc.Show.New()
end

function cc.Show:reverse()
    return cc.Hide.New()
end

function cc.Show:update(time)
    cc.Wrapper.SetActive(self._target, true)
end
--Show end

--Hide start
cc.Hide = cc.Hide or BaseClass(cc.ActionInstant)

function cc.Hide:Constructor()
end

function cc.Hide:clone()
    return cc.Hide.New()
end

function cc.Hide:reverse()
    return cc.Show.New()
end

function cc.Hide:update(time)
    cc.Wrapper.SetActive(self._target, false)
end
--Hide end

--Delete start
cc.Delete = cc.Delete or BaseClass(cc.ActionInstant)

function cc.Delete:Constructor()
end

function cc.Delete:clone()
    return cc.Delete.New()
end

function cc.Delete:reverse()
    -- return cc.Show.New()
end

function cc.Delete:update(time)
    cc.Wrapper.Delete(self._target)
end
--Delete end

--Alpha start
cc.Alpha = cc.Alpha or BaseClass(cc.ActionInstant)

function cc.Alpha:Constructor(new_alpha)
    self:initWithAlpha(new_alpha)
end

function cc.Alpha:initWithAlpha( new_alpha )
    self.new_alpha = new_alpha
end

function cc.Alpha:clone()
    return cc.Alpha.New(self.new_alpha)
end

function cc.Alpha:reverse()
    return self:clone()
end

function cc.Alpha:update(time)
    if not self._target then return end
    cc.Wrapper.SetAlpha(self._target, self.new_alpha)
end
--Alpha end

--Scale start
cc.Scale = cc.Scale or BaseClass(cc.ActionInstant)

function cc.Scale:__init(scale_x, scale_y, scale_z)
    self:initWithScale(scale_x, scale_y, scale_z)
end

function cc.Scale:initWithScale( scale_x, scale_y, scale_z )
    self.scale_x = scale_x or 1
    self.scale_y = scale_y or 1
    self.scale_z = scale_z or 1
end

function cc.Scale:clone()
    return cc.Scale.New(self.scale_x, self.scale_y, self.scale_z)
end

function cc.Scale:update(time)
    if not self._target then return end
    cc.Wrapper.SetLocalScale(self._target, self.scale_x, self.scale_y, self.scale_z)
end
--Scale end

--Text start
cc.Text = cc.Text or BaseClass(cc.ActionInstant)

function cc.Text:__init(new_txt)
    self:initWithText(new_txt)
end

function cc.Text:initWithText( new_txt )
    self.new_txt = new_txt
end

function cc.Text:clone()
    return cc.Text.New(self.new_txt)
end

function cc.Text:update(time)
    if not self._target then return end
    cc.Wrapper.SetText(self._target, self.new_txt)
end
--Text end