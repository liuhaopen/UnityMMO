local TabBar = BaseClass(UINode)

function TabBar:Constructor( parentTrans )

end

function TabBar:SetTabInfo( info )
	self.tabInfo = info
end

function TabBar:BindClickEvent( onClick )
	self.onClick = onClick
end

return TabBar