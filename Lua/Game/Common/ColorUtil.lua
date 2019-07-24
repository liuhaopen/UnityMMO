local ColorUtil = {
	--ColorUtil.Str.Gray
	Str = {
		Gray = "#3e3e3e", ---灰色
		White = "#ffffff",--白色
		Green = "#0a953e",--绿色
		Blue = "#2971e6",--蓝色
		Purple = "#e036f4",--紫色
		Orange = "#ff6519",--橙色
		Red = "#fe1a1a",--红色
		Pink = "#ff7b7b",--粉色
		Brown = "#95562A", -- 棕色
		BrownBlack = "#3F2816", --黑褐色
		Yellow = "#ffe222", --黄色
		LYellow = "#ffedb6", --浅黄色
		LBlue = "#bed4e7",  --浅蓝色
	},
	--ColorUtil.Color.Green
	Value = {
		White = Color(1, 1, 1, 1),
		Gray = Color(62/255, 62/255, 62/255, 1),
		Green = Color(10/255, 149/255, 62/255, 1),
		Blue = Color(100/255, 159/255, 255/255, 1),
		Purple = Color(224/255, 54/255, 244/255, 1),
		Orange = Color(255/255, 101/255, 25/255, 1),
		Red = Color(254/255, 26/255, 26/255, 1),
		Pink = Color(1, 123/255, 123/255, 1),
		BrownBlack = Color(63/255, 40/255, 22/255, 1),
		Yellow = Color(255 / 255, 226 / 255, 34 / 255, 1),
		LYellow = Color(1, 237/255, 182/255, 1),
		LBlue = Color(190/255, 212/255, 231/255, 1),
	},
	--ColorUtil.Index.Green
	Index = {
		Green = 0,
		Blue = 1,
		Purple = 2,
		Orange = 3,
		Red = 4,
		Pink = 5,
	},	
	--ColorUtil.Name[ColorUtil.Index.Green]
	Name = {
		[0] = "绿",
		[1] = "蓝",
		[2] = "紫",
		[3] = "橙",
		[4] = "红",
		[5] = "粉",
	}
}

--获取颜色类型对应的颜色
function ColorUtil:GetStr(index)
	if index == ColorUtil.Index.Green then --绿色
		return  ColorUtil.Str.Green
	elseif index == ColorUtil.Index.Blue then --蓝色
		return  ColorUtil.Str.Blue
	elseif index == ColorUtil.Index.Purple then --紫色
		return  ColorUtil.Str.Purple
	elseif index == ColorUtil.Index.Orange then --橙色
		return  ColorUtil.Str.Orange
	elseif index == ColorUtil.Index.Red then --红色
		return  ColorUtil.Str.Red
	elseif index == ColorUtil.Index.Pink then --粉色
		return  ColorUtil.Str.Pink
	else
		return ColorUtil.Str.White
	end
end

--将字符串颜色值转化为Color颜色，支持#FFFFFF和FFFFFF输入，支持6和8位字符
function ColorUtil:ConvertHexToRGBColor(Hex_str)
	if not Hex_str then return ColorUtil.Value.White end
	local _hex = string.gsub(Hex_str,"#","")
	string.lower(_hex)
	local r,g,b,a = 1,1,1,1
	if string.len(_hex) == 6 then
		r = tonumber(string.sub(_hex,1,2), 16)/255
		g = tonumber(string.sub(_hex,3,4), 16)/255
		b = tonumber(string.sub(_hex,5,6), 16)/255
	elseif string.len(_hex) == 8 then
		r = tonumber(string.sub(_hex,1,2), 16)/255
		g = tonumber(string.sub(_hex,3,4), 16)/255
		b = tonumber(string.sub(_hex,5,6), 16)/255
		a = tonumber(string.sub(_hex,7,8), 16)/255
	end
	local color = Color(r,g,b,a)
	return color
end

return ColorUtil