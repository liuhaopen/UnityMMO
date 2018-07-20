UIManager = UIManager or {}
UIMgr = UIManager

function UIManager.Show( view )
	assert(view, "cannot open a nil view")
	assert(view.UIConfig, "cannot find UIConfig in view", view)
	assert(view.UIConfig.prefab, "cannot find UIConfig.prefab in view", view)
	
	local on_load_succeed = function ( transform )
		print('Cat:UIManager.lua[on_load_succeed]')
		view.transform = transform

		if view.OnEnter then
			view:OnEnter()
		end
	end
	PanelMgr:CreatePanel(view.UIConfig.prefab, on_load_succeed)
end

function UIManager.Close( view )
	assert(view, "cannot close a nil view")
    GameObject.Destroy(view.transform)
	if view.OnLeave then
		view:OnLeave()
	end
end

return UIMgr