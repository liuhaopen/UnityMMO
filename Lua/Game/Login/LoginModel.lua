LoginModel = LoginModel or {}

LoginModel.Instance = nil
function LoginModel:GetInstance(  )
	if not LoginModel.Instance then
		LoginModel.Instance = self
	end
	return LoginModel.Instance
end

function LoginModel:GetRoleList( )
	return self.role_list
end

function LoginModel:SetRoleList( value )
	self.role_list = value
end