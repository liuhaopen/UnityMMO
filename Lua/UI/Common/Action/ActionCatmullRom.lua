cc = cc or {}

--Cat_Todo : 本文件暂时没用，所以没测试过的

local DeepCopy = function ( object )
    local lookup_table = {}
    local function _copy(object)
        if type(object) ~= "table" then
            return object
        elseif lookup_table[object] then
            return lookup_table[object]
        end

        local new_table = {}
        lookup_table[object] = new_table
        for index, value in pairs(object) do
            new_table[_copy(index)] = _copy(value)
        end

        return setmetatable(new_table, getmetatable(object))
    end
    return _copy(object)
end

cc.PointArray = cc.PointArray or BaseClass()
function cc.PointArray:Constructor( points )
    self._controlPoints = points
end
function cc.PointArray:clone()
    return DeepCopy(self)
end

function cc.PointArray:getControlPoints()
    return _controlPoints;
end

function cc.PointArray:setControlPoints(controlPoints)
    self._controlPoints = controlPoints
end

function cc.PointArray:addControlPoint(controlPoint)
    table.insert( self._controlPoints, controlPoint )
end

function cc.PointArray:insertControlPoint(controlPoint, index)
    -- self._controlPoints[index] = controlPoint
    table.insert( self._controlPoints, index, controlPoint )
end

function cc.PointArray:getControlPointAtIndex(index)
    index = math.floor( index )
    local count = 0
    if self._controlPoints then
        count = #self._controlPoints
    end
    if index > count then
        index = count
    end
    if index <= 0 then
        index = 1
    end
    -- index = math.min(count, index)
    -- index = index>=1 and index or 1
    -- print("Cat:ActionCatmullRom.lua [34][start] self._controlPoints", self._controlPoints,index)
    -- PrintTable(self._controlPoints)
    -- print("Cat:ActionCatmullRom.lua [34][end]")
    return self._controlPoints[index]
end

function cc.PointArray:replaceControlPoint(controlPoint, index)
    self._controlPoints[index] = controlPoint
end

function cc.PointArray:removeControlPointAtIndex(index)
    if self._controlPoints[index] then
        table.remove( self._controlPoints, index )
    end
end

function cc.PointArray:count()
    return #self._controlPoints
end

function cc.PointArray:reverse()
    local newArray = {}
    for i=#self._controlPoints,1,-1 do
        table.insert( newArray, self._controlPoints[i])
    end
    local config = cc.PointArray.New()
    config:setControlPoints(newArray);
    
    return config;
end

-- function cc.PointArray:reverseInline()
-- {
--     size_t l = _controlPoints->size();
--     Vec2 *p1 = nullptr;
--     Vec2 *p2 = nullptr;
--     float x, y;
--     for (size_t i = 0; i < l/2; ++i)
--     {
--         p1 = _controlPoints->at(i);
--         p2 = _controlPoints->at(l-i-1);
        
--         x = p1->x;
--         y = p1->y;
        
--         p1->x = p2->x;
--         p1->y = p2->y;
        
--         p2->x = x;
--         p2->y = y;
--     }
-- }

-- CatmullRom Spline formula:
function cc.CardinalSplineAt(p0, p1, p2, p3, tension, t)
    print("Cat:ActionCatmullRom [97] t: ",t)
    local t2 = t * t
    local t3 = t2 * t
    --Formula: s(-ttt + 2tt - t)P1 + s(-ttt + tt)P2 + (2ttt - 3tt + 1)P2 + s(ttt - 2tt + t)P3 + (-2ttt + 3tt)P3 + s(ttt - tt)P4

    local s = (1 - tension) / 2
	
    local b1 = s * ((-t3 + (2 * t2)) - t);                      -- s(-t3 + 2 t2 - t)P1
    local b2 = s * (-t3 + t2) + (2 * t3 - 3 * t2 + 1);          -- s(-t3 + t2)P2 + (2 t3 - 3 t2 + 1)P2
    local b3 = s * (t3 - 2 * t2 + t) + (-2 * t3 + 3 * t2);      -- s(t3 - 2 t2 + t)P3 + (-2 t3 + 3 t2)P3
    local b4 = s * (t3 - t2);                                   -- s(t3 - t2)P4
    
    local x = (p0.x*b1 + p1.x*b2 + p2.x*b3 + p3.x*b4);
    local y = (p0.y*b1 + p1.y*b2 + p2.y*b3 + p3.y*b4);
	
    return {x=x, y=y}
end

--points是控制点列表，tension是松紧程度。tension==1时，样条线是分段直线。tension<1向外松弛弯曲，tension>1向内缩紧弯曲。By动作是以当前坐标为新坐标原点
cc.CardinalSplineTo = cc.CardinalSplineTo or BaseClass(cc.ActionInterval)
function cc.CardinalSplineTo:Constructor(duration, points, tension)
    self:initWithDuration(duration, points, tension)
end

function cc.CardinalSplineTo:initWithDuration(duration, points, tension)
    self._deltaT = 0.0
    -- self._tension = 0.0
    -- CCASSERT(points->count() > 0, "Invalid configuration. It must at least have one control point");
    cc.ActionInterval.initWithDuration(self, duration)
    self._points = cc.PointArray.New(points)
    self._tension = tension
end

function cc.CardinalSplineTo:startWithTarget(target)
    cc.ActionInterval.startWithTarget(self, target);
--     _deltaT = (float) 1 / _points->count();
    self._deltaT =  1 / (self._points:count() - 1);
    self._previousPosition = {}
    self._previousPosition.x, self._previousPosition.y, self._previousPosition.z = cc.Wrapper.GetLocalPosition(target)
    self._accumulatedDiff = {x=0, y=0}
end

function cc.CardinalSplineTo:clone()
    local a = cc.CardinalSplineTo.New()
    a:initWithDuration(self._duration, self._points:clone(), self._tension)
    return a
end

function cc.CardinalSplineTo:update(time)
    local p;
    local lt;
	
    -- eg.
    -- p..p..p..p..p..p..p
    -- 1..2..3..4..5..6..7
    -- want p to be 1, 2, 3, 4, 5, 6
    if (time == 1) then
        p = self._points:count()-1
        lt = 1;
    else 
        p = math.floor(time / self._deltaT)
        lt = (time - self._deltaT * p) / self._deltaT
    end
    -- print("Cat:ActionCatmullRom.lua [150] time,p,lt:", time,p,lt)
    -- Interpolate
    local addition = 0
    local pp0 = DeepCopy(self._points:getControlPointAtIndex(p+0+addition))
    local pp1 = DeepCopy(self._points:getControlPointAtIndex(p+1+addition))
    local pp2 = DeepCopy(self._points:getControlPointAtIndex(p+2+addition))
    local pp3 = DeepCopy(self._points:getControlPointAtIndex(p+3+addition))
	
    print("Cat:ActionCatmullRom [166] pp0, pp1, pp2, pp3: ", pp0.x, pp0.y, pp1.x, pp1.y, pp2.x, pp2.y, pp3.x, pp3.y, p)
    pp0.y = 720-pp0.y
    pp1.y = 720-pp1.y
    pp2.y = 720-pp2.y
    pp3.y = 720-pp3.y
    local newPos = cc.CardinalSplineAt(pp0, pp1, pp2, pp3, self._tension, lt)
    newPos.y = 720-newPos.y
	-- print("Cat:ActionCatmullRom.lua [157] newPos.x,newPos.y:", newPos.x,newPos.y, lt)
-- #if CC_ENABLE_STACKABLE_ACTIONS
--     -- Support for stacked actions
--     Node *node = _target;
--     local diff = node->getPosition() - _previousPosition;
--     if( diff.x !=0 || diff.y != 0 ) 
--         _accumulatedDiff = _accumulatedDiff + diff;
--         newPos = newPos + _accumulatedDiff;
--     end
-- #endif
    self:updatePosition(newPos)
end

function cc.CardinalSplineTo:updatePosition(newPos)
    if self._target.SetVectorValue then
        self._target:SetVectorValue(WidgetProperty.Position, newPos.x, newPos.y)
    elseif self._target.setPosition then
        self._target:setPosition(newPos.x, newPos.y)
    end
    self._previousPosition = newPos
end

function cc.CardinalSplineTo:reverse()
    local pReverse = self._points:reverse()  
    return cc.CardinalSplineTo.New(self._duration, pReverse, self._tension)
end


-- function cc.CardinalSplineBy:create(duration, points, tension)

--     CardinalSplineBy *ret = new (std:nothrow) CardinalSplineBy();
--     if (ret)
    
--         if (ret->initWithDuration(duration, points, tension))
        
--             ret->autorelease();
--         else 
        
--             CC_SAFE_RELEASE_NULL(ret);
--         end
--     end

--     return ret;
-- end

-- CardinalSplineBy:CardinalSplineBy() : _startPosition(0,0)

-- end

-- function cc.CardinalSplineBy:updatePosition(cocos2d:Vec2 &newPos)

--     Vec2 p = newPos + _startPosition;
--     _target->setPosition(p);
--     _previousPosition = p;
-- end

-- CardinalSplineBy* CardinalSplineBy:reverse() const

--     PointArray *copyConfig = _points->clone();
	
--     //
--     // convert "absolutes" to "diffs"
--     //
--     Vec2 p = copyConfig->getControlPointAtIndex(0);
--     for (ssize_t i = 1; i < copyConfig->count(); ++i)
    
--         Vec2 current = copyConfig->getControlPointAtIndex(i);
--         Vec2 diff = current - p;
--         copyConfig->replaceControlPoint(diff, i);
        
--         p = current;
--     end
	
--     // convert to "diffs" to "reverse absolute"
	
--     PointArray *pReverse = copyConfig->reverse();
	
--     // 1st element (which should be 0,0) should be here too
    
--     p = pReverse->getControlPointAtIndex(pReverse->count()-1);
--     pReverse->removeControlPointAtIndex(pReverse->count()-1);
    
--     p = -p;
--     pReverse->insertControlPoint(p, 0);
    
--     for (ssize_t i = 1; i < pReverse->count(); ++i)
    
--         Vec2 current = pReverse->getControlPointAtIndex(i);
--         current = -current;
--         Vec2 abs = current + p;
--         pReverse->replaceControlPoint(abs, i);
        
--         p = abs;
--     end
	
--     return CardinalSplineBy:create(_duration, pReverse, _tension);
-- end

-- function cc.CardinalSplineBy:startWithTarget(cocos2d:Node *target)
    
--     CardinalSplineTo:startWithTarget(target);
--     _startPosition = target->getPosition();
-- end

-- CardinalSplineBy* CardinalSplineBy:clone() const

--     // no copy constructor
--     auto a = new (std:nothrow) CardinalSplineBy();
--     a->initWithDuration(this->_duration, this->_points->clone(), this->_tension);
--     a->autorelease();
--     return a;
-- end