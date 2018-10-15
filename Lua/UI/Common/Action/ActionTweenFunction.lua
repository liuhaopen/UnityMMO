cc = cc or {}
cc.tweenfunc = cc.tweenfunc or BaseClass()


function cc.tweenfunc.easeIn(time, rate)
    return math.pow(time, rate)
end

function cc.tweenfunc.easeOut(time, rate)
    return math.pow(time, 1 / rate)
end
    
function cc.tweenfunc.easeInOut(time, rate)
    time = time * 2
    if (time < 1) then
        return 0.5 * math.pow(time, rate)
    else
        return (1.0 - 0.5 * math.pow(2 - time, rate))
    end
end

-- Sine Ease
function cc.tweenfunc.sineEaseIn(time)
    return -1 * math.cos(time * M_PI_2) + 1;
end
    
function cc.tweenfunc.sineEaseOut(time)
    return math.sin(time * M_PI_2);
end
    
function cc.tweenfunc.sineEaseInOut(time)
    return -0.5 * (math.cos(M_PI * time) - 1);
end


-- Quad Ease
function cc.tweenfunc.quadEaseIn(time)
    return time * time;
end
    
function cc.tweenfunc.quadEaseOut(time)
    return -1 * time * (time - 2);
end
    
function cc.tweenfunc.quadEaseInOut(time)
    time = time*2;
    if (time < 1) then
        return 0.5 * time * time;
    end
    time = time - 1
    return -0.5 * (time * (time - 2) - 1);
end



-- Cubic Ease
function cc.tweenfunc.cubicEaseIn(time)
    return time * time * time;
end
function cc.tweenfunc.cubicEaseOut(time)
    time = time - 1;
    return (time * time * time + 1);
end
function cc.tweenfunc.cubicEaseInOut(time)
    time = time*2;
    if (time < 1) then
        return 0.5 * time * time * time;
    end
    time = time - 2;
    return 0.5 * (time * time * time + 2);
end


-- Quart Ease
function cc.tweenfunc.quartEaseIn(time)
    return time * time * time * time;
end
    
function cc.tweenfunc.quartEaseOut(time)
    time = time - 1;
    return -(time * time * time * time - 1);
end
    
function cc.tweenfunc.quartEaseInOut(time)
    time = time*2;
    if (time < 1) then
        return 0.5 * time * time * time * time;
    end
    time = time - 2;
    return -0.5 * (time * time * time * time - 2);
end


-- Quint Ease
function cc.tweenfunc.quintEaseIn(time)
    return time * time * time * time * time;
end
    
function cc.tweenfunc.quintEaseOut(time)
    time = time - 1
    return (time * time * time * time * time + 1);
end
    
function cc.tweenfunc.quintEaseInOut(time)
    time = time*2;
    if (time < 1) then
        return 0.5 * time * time * time * time * time;
    end
    time = time - 2;
    return 0.5 * (time * time * time * time * time + 2);
end


-- Expo Ease
function cc.tweenfunc.expoEaseIn(time)
    return time == 0 and 0 or math.pow(2, 10 * (time/1 - 1)) - 1 * 0.001;
end
function cc.tweenfunc.expoEaseOut(time)
    return time == 1 and 1 or (-math.pow(2, -10 * time / 1) + 1);
end
function cc.tweenfunc.expoEaseInOut(time)
    time = time / 0.5;
    if (time < 1) then
        time = 0.5 * math.pow(2, 10 * (time - 1));
    else

        time = 0.5 * (-math.pow(2, -10 * (time - 1)) + 2);
    end

    return time;
end


-- Circ Ease
function cc.tweenfunc.circEaseIn(time)
    return -1 * (math.sqrt(1 - time * time) - 1);
end
function cc.tweenfunc.circEaseOut(time)
    time = time - 1;
    return math.sqrt(1 - time * time);
end
function cc.tweenfunc.circEaseInOut(time)
    time = time * 2;
    if (time < 1) then
        return -0.5 * (math.sqrt(1 - time * time) - 1);
    end
    time = time - 2;
    return 0.5 * (math.sqrt(1 - time * time) + 1);
end

function cc.tweenfunc.elasticEaseOut( time, period )
	local newT = 0
    if time == 0 or time == 1 then
        newT = time
    else
        local s = period / 4
        newT = math.pow(2, -10 * time) * math.sin((time - s) * M_PI_X_2 / period) + 1;
    end
    return newT
end

function cc.tweenfunc.elasticEaseIn(time, period)
    local newT = 0;
    if (time == 0 or time == 1) then
        newT = time;
    else
        local s = period / 4;
        time = time - 1;
        newT = -math.pow(2, 10 * time) * math.sin((time - s) * M_PI_X_2 / period);
    end

    return newT
end

function cc.tweenfunc.elasticEaseInOut(time, period)
    local newT = 0;
    if (time == 0 or time == 1) then
        newT = time;
    else
        time = time * 2;
        if (not period or period == 0) then
            period = 0.3 * 1.5;
        end

        local s = period / 4;

        time = time - 1;
        if (time < 0) then
            newT = -0.5 * math.pow(2, 10 * time) * math.sin((time -s) * M_PI_X_2 / period);
        else
            newT = math.pow(2, -10 * time) * math.sin((time - s) * M_PI_X_2 / period) * 0.5 + 1;
        end
    end
    return newT;
end


-- Back Ease
function cc.tweenfunc.backEaseIn(time)
    local overshoot = 1.70158;
    return time * time * ((overshoot + 1) * time - overshoot);
end
function cc.tweenfunc.backEaseOut(time)
    local overshoot = 1.70158;

    time = time - 1;
    return time * time * ((overshoot + 1) * time + overshoot) + 1;
end
function cc.tweenfunc.backEaseInOut(time)
    local overshoot = 1.70158 * 1.525;

    time = time * 2;
    if (time < 1) then
        return (time * time * ((overshoot + 1) * time - overshoot)) / 2;
    else
   
        time = time - 2;
        return (time * time * ((overshoot + 1) * time + overshoot)) / 2 + 1;
    end
end



-- Bounce Ease
function cc.tweenfunc.bounceTime(time)
    if (time < 1 / 2.75) then
        return 7.5625 * time * time;
    elseif (time < 2 / 2.75) then
        time = time - 1.5 / 2.75;
        return 7.5625 * time * time + 0.75;
    elseif(time < 2.5 / 2.75) then
        time = time - 2.25 / 2.75;
        return 7.5625 * time * time + 0.9375;
    end
    time = time - 2.625 / 2.75;
    return 7.5625 * time * time + 0.984375;
end
function cc.tweenfunc.bounceEaseIn(time)
    return 1 - cc.tweenfunc.bounceTime(1 - time);
end

function cc.tweenfunc.bounceEaseOut(time)
    return cc.tweenfunc.bounceTime(time);
end

function cc.tweenfunc.bounceEaseInOut(time)
    local newT = 0;
    if (time < 0.5) then
   
        time = time * 2;
        newT = (1 - cc.tweenfunc.bounceTime(1 - time)) * 0.5;
    else
   
        newT = cc.tweenfunc.bounceTime(time * 2 - 1) * 0.5 + 0.5;
    end

    return newT;
end


-- Custom Ease
function cc.tweenfunc.customEase(time, easingParam)
    if (easingParam) then
        local tt = 1-time;
        return easingParam[1]*tt*tt*tt + 3*easingParam[3]*time*tt*tt + 3*easingParam[5]*time*time*tt + easingParam[7]*time*time*time;
    end
    return time;
end

   
function cc.tweenfunc.quadraticIn(time)
    return   math.pow(time,2);
end
    
function cc.tweenfunc.quadraticOut(time)
    return -time*(time-2);
end
    
function cc.tweenfunc.quadraticInOut(time)
    
    local resultTime = time;
    time = time*2;
    if (time < 1) then
        resultTime = time * time * 0.5;
    else
        time = time - 1
        resultTime = -0.5 * (time * (time - 2) - 1);
    end
    return resultTime;
end
    
function cc.tweenfunc.bezieratFunction( a, b, c, d, t )
    return (math.pow(1-t,3) * a + 3*t*(math.pow(1-t,2))*b + 3*math.pow(t,2)*(1-t)*c + math.pow(t,3)*d );
end
