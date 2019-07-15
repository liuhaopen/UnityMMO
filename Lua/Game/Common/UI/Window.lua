local Window = BaseClass(UINode)
Window.Style = {
	Big = 1,
	Normal = 2,
	Small = 3,
}
function Window:Constructor( parentTrans )

end

function Window.Create( style )
	local window = Window.New()
	return window
end

function Window:SetTabInfo( info )
	self.tabInfo = info
end

function Window:BindClickEvent( onClick )
	self.onClick = onClick
end

return Window