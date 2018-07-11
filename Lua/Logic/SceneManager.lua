SceneManager = SceneManager or {}

function SceneManager:GetInstance(  )
	if not SceneManager.Instance then
		self:Init()
		SceneManager.Instance = self
	end
	return SceneManager.Instance
end

function SceneManager:Init(  )
	
end

function SceneManager:AddRole(  )
	
end

function SceneManager:AddEnemy(  )
	
end