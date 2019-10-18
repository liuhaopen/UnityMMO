cc = cc or {}
cc.Wrapper = cc.Wrapper or {}

--大部分action都会调用节点的某些接口，比如MoveTo就是每帧调用节点改变坐标的接口，而不同引擎其接口都不一样的，所以为了action们能够通用就增加了此中间层，为你的引擎实现本文件的所有接口就可以了。

function cc.Wrapper.SetLocalPosition( node, x, y, z )
	UIHelper.SetLocalPosition(node, x, y, z)
end

function cc.Wrapper.GetLocalPosition( node )
	local pos = node.localPosition
	return pos.x, pos.y, pos.z
end

function cc.Wrapper.SetAnchoredPosition( node, x, y )
	UIHelper.SetAnchoredPosition(node, x, y)
end

function cc.Wrapper.GetAnchoredPosition( node )
	local pos = node.anchoredPosition
	return pos.x, pos.y
end

function cc.Wrapper.SetPosition( node, x, y, z )
	UIHelper.SetPosition(node, x, y, z)
end

function cc.Wrapper.GetPosition( node )
	local pos = node.position
	return pos.x, pos.y, pos.z
end

function cc.Wrapper.SetActive( node, is_show )
	UI.SetActive(node, is_show)
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
		if cc.Wrapper.TryGet(node, "color") and node.color then
			alpha = node.color.a
		elseif cc.Wrapper.TryGet(node, "alpha") and node.alpha then
			alpha = node.alpha
		else
			local image = node:GetComponent("Image")
			if image ~= nil then
				local r,g,b,a = image.color:Get()
				alpha = a
			else
				local text = node:GetComponent("Text")
				if text ~= nil then
					alpha = text.color.a
				else
					local imageEx = node:GetComponent("ImageExtend")
					if imageEx then
						alpha = imageEx.alpha
					end
				end
			end
		end
		return alpha
	end
end

function cc.Wrapper.SetChildrenAlpha( node, alpha, childCon )
	local childrenList
	if childCon and childCon.childrenListForActionWrapper then
		childrenList = childCon.childrenListForActionWrapper
	else
		childrenList = node:GetComponentsInChildren(typeof(UnityEngine.RectTransform))
		if childCon then
			childCon.childrenListForActionWrapper = childrenList
		end
	end
	if not childrenList then return end
    for i=0,childrenList.Length-1 do
        local child = childrenList[i]
        cc.Wrapper.SetAlpha(child, alpha)
    end
end

function cc.Wrapper.SetAlpha( node, alpha )
	if node and alpha then
		if cc.Wrapper.TryGet(node, "color") and node.color then
			node.color = Color(node.color.r,node.color.g,node.color.b,alpha)
		elseif cc.Wrapper.TryGet(node, "alpha") and node.alpha then
			node.alpha = alpha
		elseif cc.Wrapper.TryGet(node, "SetAlpha") and node.SetAlpha then	
			node:SetAlpha(alpha)
		else
			--尽量为特定的action用setTarget传入特定的组件
			local image = node:GetComponent("Image")
			if image ~= nil then
				local r,g,b,a = image.color:Get()
				image.color = Color.New(r,g,b,alpha)
			else
				local text = node:GetComponent("Text")
				if text ~= nil then
					text.color = Color(text.color.r,text.color.g,text.color.b,alpha)
				else
					local imageEx = node:GetComponent("ImageExtend")
					if imageEx ~= nil then
						imageEx.alpha = alpha
					end
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