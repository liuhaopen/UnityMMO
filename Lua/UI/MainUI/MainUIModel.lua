MainUIModel = MainUIModel or {}

MainUIModel.Instance = nil
function MainUIModel:GetInstance(  )
	if not MainUIModel.Instance then
		MainUIModel.Instance = self
	end
	return MainUIModel.Instance
end
