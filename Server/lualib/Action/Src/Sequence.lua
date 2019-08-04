local Update = function(self, t)
	local found = 0
    local new_t = 0.0

    if( t < self._split ) then
        found = 0
        if( self._split ~= 0 ) then
            new_t = t / self._split
        else
            new_t = 1
        end
     else 
        found = 1
        if ( self._split == 1 ) then
            new_t = 1
        else
            new_t = (t-self._split) / (1 - self._split )
        end
    end

    if ( found==1 ) then
        if( self._last == -1 ) then
            -- action[0] was skipped, execute it.
            self._actions[0]:startWithTarget(self._target)
                self._actions[0]:update(1.0)
            self._actions[0]:stop()
        elseif( self._last == 0 ) then
            -- switching to action 1. stop action 0.
                self._actions[0]:update(1.0)
            self._actions[0]:stop()
        end
    elseif (found==0 and self._last==1 ) then
            self._actions[1]:update(0)
        self._actions[1]:stop()
    end
    -- Last action found and it is done.
    if( found == self._last and self._actions[found]:isDone() ) then
        return
    end

    -- Last action found and it is done
    if( found ~= self._last ) then
        self._actions[found]:startWithTarget(self._target)
    end
    self._actions[found]:update(new_t)
    self._last = found
end

local Sequence = Ac.ActionInterval {
	type 	= "Sequence",
	__index = {
		Update = Update,
	},
}

return Sequence