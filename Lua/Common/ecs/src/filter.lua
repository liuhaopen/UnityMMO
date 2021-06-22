local mt = {}

function mt.all(...)
	return {
		type = "all",
		com_name_or_filters = {...},
	}
end

function mt.any(...)
	return {
		type = "any",
		com_name_or_filters = {...},
	}
end

function mt.no(...)
	return {
		type = "no",
		com_name_or_filters = {...},
	}
end

return mt