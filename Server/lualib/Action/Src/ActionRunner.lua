local ActionRunner = AC.OO.Class {
	type = "ActionRunner",
	__index    = {
		actions = {},
		AddAction = function( self, action )
			
		end,
		Update = function( self, deltaTime )

		end,
	},
}
return ActionRunner