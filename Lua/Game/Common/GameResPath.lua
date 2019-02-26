GameResPath = GameResPath or {}

function GameResPath.GetFullUIPath( relativePath )
	return "Assets/AssetBundleRes/ui/"..relativePath
end

function GameResPath.GetRoleHeadRes( career, head_id )
	local res_id = career*1000 + head_id
	return "Assets/AssetBundleRes/ui/roleHead/head_"..res_id..".png"
end

return GameResPath