local LoadingView = BaseClass()

function LoadingView:DefaultVar( )
	return {
	UIConfig = {
			prefab_path = "Assets/AssetBundleRes/ui/common/LoadingView.prefab",
			canvas_name = "Normal",
			components = {
				{UI.Background, {is_click_to_close=false, alpha=0.5}},
			},
		},
	}
end

function LoadingView.Show( data )
	local view = LoadingView.New()
	view:SetData(data)
    UIMgr:Show(view)
    return view
end

function LoadingView:SetData( data )
	self.data = data
end

function LoadingView:OnLoad(  )
	local names = {"cancel:obj","ok:obj","content:txt","cancel/cancel_label:txt","ok/ok_label:txt",}
	UI.GetChildren(self, self.transform, names)
	
	self:AddEvents()
	self:UpdateView()
end

function LoadingView:AddEvents(  )
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

function LoadingView:UpdateView(  )
	if not self.data then return end
	
	self.content_txt.text = self.data.content
	self.ok_label_txt.text = self.data.ok_btn_text
	self.cancel_label_txt.text = self.data.cancel_btn_text
end
        
return LoadingView