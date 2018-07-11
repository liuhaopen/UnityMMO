local MovementSystem = ecs.System.create({
	filter = function(entity)
		return entity.move_direction and entity.position and entity.move_speed and entity.state
	end,
	update = function(entities)
		for k,v in pairs(entities) do
			local target_pos = v.position + entity.move_direction*entity.move_speed
			--计算跳跃

			--判断目的坐标是否障碍区

			--通知服务端

			v.is_running = true
			v.position = target_pos
		end
	end,
})
return MovementSystem