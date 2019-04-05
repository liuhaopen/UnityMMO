package.path = package.path ..';../?.lua;../../?.lua;../../lualib/ECS/?.lua';
local lu = require('Tests.luaunit')
local aoi = require('aoi')

local get_tbl_size = function ( tbl )
	local count = 0
	for k,v in pairs(tbl) do
		count = count + 1
	end
	return count
end

local assertNothingChange = function ( around )
    lu.assertEquals(get_tbl_size(around), 0)
end

local assertSomeoneEnter = function ( around, someone )
	lu.assertEquals(get_tbl_size(around), 1)
    lu.assertEquals(around[1][1], true)
    lu.assertEquals(around[1][2], someone)
end

local assertSomeoneLeave = function ( around, someone )
	lu.assertEquals(get_tbl_size(around), 1)
    lu.assertEquals(around[1][1], false)
    lu.assertEquals(around[1][2], someone)
end

function test_all(  )
	--
	aoi:init()
	local girl = aoi:add()
	aoi:set_pos(girl, {x=1,y=2,z=3})
	local around = aoi:get_around_offset(girl, 100, 120)
    -- lu.assertEquals(get_tbl_size(around), 0)
    assertNothingChange(around)

    --与男主相遇，你中有我，我中有你
    local boy = aoi:add()
    aoi:set_pos(boy, {x=-1, y=-2, z=-3})
	local around = aoi:get_around_offset(girl, 100, 120)
	assertSomeoneEnter(around, boy)

    local around = aoi:get_around_offset(boy, 50, 50)
	assertSomeoneEnter(around, girl)

    --男主腻了，于是跑路
    aoi:set_pos(boy, {x=1000, y=-2000, z=-3000})
    --男主已经看不见女主了
    local around = aoi:get_around_offset(boy, 150, 150)
	assertSomeoneLeave(around, girl)
    
    --但女主还是忘不了那个负心汉
    local around = aoi:get_around_offset(girl, 50, 123500)
    assertNothingChange(around)--因为get_around_offset只返回和上次的差异集，而且传入的radius_long超大，所以这次查询结果不会有男主的离开
    --因为终日以泪洗脸导致眼角膜发炎视力下降，这时就看不见男主了
    local around = aoi:get_around_offset(girl, 50, 60)
	assertSomeoneLeave(around, boy)

    --第三者插足
    local lady = aoi:add()
    aoi:set_pos(lady, {x=1001, y=-2001, z=-3001})
    local around = aoi:get_around_offset(lady, 150, 150)
	assertSomeoneEnter(around, boy)

    --女主捉奸在床
    aoi:set_pos(girl, {x=1010, y=-2010, z=-3010})
    local around = aoi:get_around_offset(girl, 150, 150)
    lu.assertEquals(get_tbl_size(around), 2)
    lu.assertEquals(around[1][1], true)
    lu.assertEquals(around[1][2], boy)
    lu.assertEquals(around[2][1], true)
    lu.assertEquals(around[2][2], lady)
    
end

os.exit( lu.LuaUnit.run() )
