local TaskDialogView = BaseClass(UINode)

function TaskDialogView:Constructor(  )
	self.UIConfig = {
		prefab_path = "Assets/AssetBundleRes/ui/task/TaskDialogView.prefab",
		canvas_name = "Normal",
		components = {
			{UI.HideOtherView},
		},
	}
end

return TaskDialogView