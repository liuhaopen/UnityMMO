local MonsterFSM = {
	nodes = {
		{
			id = 1,
			type = "Blueprint.State.PatrolState",
			name = "Patrol",
		},
		{
			id = 2,
			type = "Blueprint.State.FightState",
			name = "Fight",
		},
		{
			id = 3,
			type = "Blueprint.State.DeadState",
			name = "Dead",
		},
	},
}
return MonsterFSM