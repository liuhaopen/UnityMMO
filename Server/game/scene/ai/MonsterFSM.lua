local MonsterFSM = {
	nodes = {
		{
			id = 1,
			type = "Blueprint.State.PatrolState",
			name = "PatrolState",
		},
		{
			id = 2,
			type = "Blueprint.State.FightState",
			name = "FightState",
		},
		{
			id = 3,
			type = "Blueprint.State.DeadState",
			name = "DeadState",
		},
	},
}
return MonsterFSM