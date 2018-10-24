cc = cc or {}
cc.Wrapper = cc.Wrapper or {}

--大部分action都会调用节点的某些接口，比如MoveTo就是每帧调用节点改变坐标的接口，而不同引擎其接口都不一样的，所以为了action们能够通用就增加了此中间层，为你的引擎实现本文件的所有接口就可以了。

function cc.Wrapper.SetLocalPosition( node, x, y, z )
	if node ~= nil then
		node:SetLocalPosXYZ(x, y, z)
	end
end

function cc.Wrapper.GetLocalPosition( node )
	return GetLocalPosition(node)
end

function cc.Wrapper.SetAnchoredPosition( node, x, y )
	SetAnchoredPosition(node, x, y)
end

function cc.Wrapper.GetAnchoredPosition( node )
	return GetAnchoredPosition(node)
end

function cc.Wrapper.SetPosition( node, x, y, z )
	SetGlobalPosition(node, x, y, z)
end

function cc.Wrapper.GetPosition( node )
	return GetGlobalPosition(node)
end

function cc.Wrapper.SetVisible( node, is_show )
	SetVisible(node, is_show)
end

function cc.Wrapper.GetVisible( node )
	return GetVisible(node)
end

function cc.Wrapper.GetLocalScale( node )
	GetLocalScale(node)
end

function cc.Wrapper.SetLocalScale( node, x, y, z )
	SetLocalScale(node, x, y, z)
end

function cc.Wrapper.GetLocalRotation( node )
	GetLocalRotation(node)
end

function cc.Wrapper.SetLocalRotation( node, x, y, z )
	SetLocalRotation(node, x, y, z)
end

function cc.Wrapper.TryGetReal(tbl, key)
	return tbl[key]
end

function cc.Wrapper.TryGet( tbl, key )
	local is_ok = pcall(cc.Wrapper.TryGetReal, tbl, key)
	return is_ok
end

function cc.Wrapper.GetAlpha( node )
	if node then
		local alpha = nil
		if cc.Wrapper.TryGet(node, "alpha") then
			alpha = node.alpha
		elseif cc.Wrapper.TryGet(node, "color") then
			alpha = node.color.a
		else
			local image = node:GetComponent("Image")
			if image ~= nil then
				local r,g,b,a = image.color:Get()
				alpha = a
			else
				local imageEx = node:GetComponent("ImageExtend")
				alpha = imageEx.alpha
				if imageEx ~= nil then
					local text = node:GetComponent("Text")
					alpha = text.alpha
				end
			end
		end
		return alpha
	end
end

function cc.Wrapper.SetAlpha( node, alpha )
	if node and alpha then
		if cc.Wrapper.TryGet(node, "color") then
			node.color = Color(node.color.r,node.color.g,node.color.b,alpha)
		elseif cc.Wrapper.TryGet(node, "SetAlpha") then	
			node:SetAlpha(alpha)
		else
			--尽量为特定的action用setTarget传入特定的组件
			local image = node:GetComponent("Image")
			if image ~= nil then
				local r,g,b,a = image.color:Get()
				image.color = Color.New(r,g,b,alpha)
			else
				local imageEx = node:GetComponent("ImageExtend")
				if imageEx ~= nil then
					imageEx.alpha = alpha
				else
					local text = node:GetComponent("Text")
					text.color = Color(text.color.r,text.color.g,text.color.b,alpha)
				end
			end
		end
	end
end

function cc.Wrapper.GetSize( node )
	
end

function cc.Wrapper.SetSize( node, w, h )
	
end

function cc.Wrapper.Delete( node )
	if node ~= nil then
		node:DeleteMe()
		node = nil
	end
end

function cc.Wrapper.DestroyObject( node )
	if node and node.gameObject then
		destroy(node.gameObject)
	end
end

function cc.Wrapper.SetText( node, txt )
	if node then
		node.text = txt
	end
end