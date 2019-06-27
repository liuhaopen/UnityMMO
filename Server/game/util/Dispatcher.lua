local Dispatcher = {}

function Dispatcher:Init(  )
	self.sprotoHandlers = {}
	self.publicFuncs = {}
end

function Dispatcher:RegisterSprotoHandler( handler )
	for k,v in pairs(handler) do
		self.sprotoHandlers[k] = v
	end	
end

function Dispatcher:GetSprotoHandler( sprotoName )
	return self.sprotoHandlers[sprotoName]
end

function Dispatcher:RegisterPublicFuncs( publicClassName, publicFuncs )
	self.publicFuncs[publicClassName] = publicFuncs
end

function Dispatcher:GetPublicFunc( publicClassName, funcName )
	return self.publicFuncs[publicClassName] and self.publicFuncs[publicClassName][funcName]
end

return Dispatcher