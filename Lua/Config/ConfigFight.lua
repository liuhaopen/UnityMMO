ConfigFight = ConfigFight or {
	-- DefaultPath = "Logic/FightAction/",
	-- DefaultFile = "FightActions",
	[1000] = {
		{time=0, actions={
			{name="CircleSelector", param={radio=5}},
			{name="MoveTo", param={}},
		}},
	},
	[1001] = {
		{time = 0, actions = {
			{name="CircleSelector", param={radio=10}},
			{name="PreSwing", duration=0.5},
			{name="PlayAnimate", param={animate_name="Skill_1001"}},
			{name="PlaySound", param={sound_name="sound_1001"}},
		}},
		{time = 0.1, actions = {
			{name="ShowHurtNum", param={}},
			{name="Shake", param={angle=160, max_range=10}, duration=4}
		}},
		{time = 0.2, actions={
			{name="CallFighterFunc", param={func_name="SetSpeed", arge=100}},
			{name="ShowParticle", param={particle_name="effect1001"}},
		}},
		[1.1] = {actions=
			{name="Rush", param={distance=150}, duration=2}
		},
		[1.6] = {actions=
			{name="Jump", param={height=20, speed=0.2}}
		},
		[1.7] = {actions=
			{name="HitedDown", param={back_distance=120}}
		},
		[1.8] = {actions={
			{name="BackSwing", duration=0.5},
			{name="Suck", param={radio=40, suck_distance=20}}
		},
		[2.0] = {actions={
			{name="Sequene", param={
				{name="BackSwing", duration=0.5},
				{name="BackSwing", duration=0.5},
			}}
		},
		[2.1] = {actions=
			{name="IfElse", param=
			{
				True = {

				},
				False = {

				},
			}
			},
		},
	}
}

