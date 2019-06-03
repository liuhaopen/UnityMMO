local AlertView = BaseClass()

function AlertView:DefaultVar( )
	return {
	UIConfig = {
			prefab_path = "Assets/AssetBundleRes/ui/common/AlertView.prefab",
			canvas_name = "Normal",
			components = {
				{UI.Background, {is_click_to_close=false, alpha=0.5}},
			},
		},
	}
end

function AlertView.Show( data )
	local view = AlertView.New()
	view:SetData(data)
    UIMgr:Show(view)
    return view
end

function AlertView:SetData( data )
	self.data = data
end

function AlertView:OnLoad(  )
	local names = {"cancel:obj","ok:obj","content:txt","cancel/cancel_label:txt","ok/ok_label:txt",}
	UI.GetChildren(self, self.transform, names)
	
	self:AddEvents()
	self:UpdateView()
end

function AlertView:AddEvents(  )
	local on_click = function ( click_obj )
		if click_obj == self.ok_obj then
	        if self.data and self.data.on_ok then
	        	self.data.on_ok()
	        end
		elseif click_obj == self.cancel_obj then
            if self.data and self.data.on_cancel then
	        	self.data.on_cancel()
	        end
		end
	end
	UIHelper.BindClickEvent(self.ok_obj, on_click)
	UIHelper.BindClickEvent(self.cancel_obj, on_click)
end

function AlertView:UpdateView(  )
	if not self.data then return end
	
	self.content_txt.text = self.data.content
	self.ok_label_txt.text = self.data.ok_btn_text
	self.cancel_label_txt.text = self.data.cancel_btn_text
end
        
return AlertView