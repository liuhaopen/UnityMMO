package.path = package.path ..';../?.lua;../../?.lua;../../../?.lua;../../lualib/ECS/?.lua';
require "Lua.Common.Util.util"
local lu = require('Common.luaunit')
local aoi = require('aoi')

--在本目录运行本文件即可：lua aoi_test.lua
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
	lu.assertNotNil(around)
	local aoi_info = around[someone]
	lu.assertNotNil(aoi_info)
    lu.assertEquals(aoi_info, 1)
end

local assertSomeoneLeave = function ( around, someone )
	lu.assertNotNil(around)
	local aoi_info = around[someone]
	lu.assertNotNil(aoi_info)
    lu.assertEquals(aoi_info, 2)
end

function test_all(  )
	--地点
	local pos_library = {x=1,y=2,z=3}
	local pos_hotel = {x=-500,y=-123,z=7800}
	local pos_dongguan = {x=1000,y=-2000,z=3000}
	local pos_lake = {x=-2000,y=1000,z=-4000}
	local pos_cemetery = {x=-300,y=100,z=500}
	aoi:init()
	local girl = aoi:add()
	local around = aoi:get_around_offset(girl, 100, 120)
    assertNothingChange(around)
    --女主在图书馆溜达
	aoi:set_pos(girl, pos_library.x, pos_library.y, pos_library.z)
	local around = aoi:get_around_offset(girl, 100, 120)
    assertNothingChange(around)
	aoi:set_pos(girl, pos_library.x+1, pos_library.y-1, pos_library.z+50)
	local around = aoi:get_around_offset(girl, 100, 120)
    assertNothingChange(around)
    aoi:remove(girl)--无聊死了
    
    --男主帅气出场
    local boy = aoi:add()
    aoi:set_pos(boy, pos_library.x-1, pos_library.y+1, pos_library.z-10)
	local around = aoi:get_around_offset(boy, 100, 120)
	lu.assertEquals(get_tbl_size(around), 0)
	--女主嗅到帅哥味，于是复活
	girl = aoi:add()
	aoi:set_pos(girl, pos_library.x, pos_library.y, pos_library.z)
    --与男主相遇，确定过眼神
	local around = aoi:get_around_offset(girl, 100, 120)
	lu.assertEquals(get_tbl_size(around), 1)
	assertSomeoneEnter(around, boy)
    local around = aoi:get_around_offset(boy, 150, 150)
	lu.assertEquals(get_tbl_size(around), 1)
	assertSomeoneEnter(around, girl)

	--两人整天相互了解彼此的生理结构
	aoi:set_pos(girl, pos_hotel.x-1, pos_hotel.y-1, pos_hotel.z+10)
    aoi:set_pos(boy, pos_hotel.x-1, pos_hotel.y-1, pos_hotel.z+10)
	local around = aoi:get_around_offset(girl, 100, 120)
    assertNothingChange(around)
	local around = aoi:get_around_offset(boy, 100, 120)
    assertNothingChange(around)
	aoi:set_pos(girl, pos_hotel.x-2, pos_hotel.y+1, pos_hotel.z-10)
    local around = aoi:get_around_offset(girl, 100, 120)
    assertNothingChange(around)
    aoi:set_pos(boy, pos_hotel.x-3, pos_hotel.y+1, pos_hotel.z+12)
	local around = aoi:get_around_offset(boy, 100, 120)
    assertNothingChange(around)

    --男主腻了，于是跑去东莞
    aoi:set_pos(boy, pos_dongguan.x, pos_dongguan.y, pos_dongguan.z)
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
    aoi:set_pos(lady, pos_dongguan.x+10, pos_dongguan.y-8, pos_dongguan.z-15)
    local around = aoi:get_around_offset(lady, 150, 150)
	assertSomeoneEnter(around, boy)

    --女主去东莞找男主，结果捉奸在床
    aoi:set_pos(girl, pos_dongguan.x-4, pos_dongguan.y+8, pos_dongguan.z-4)
    local around = aoi:get_around_offset(girl, 150, 150)
    lu.assertEquals(get_tbl_size(around), 2)
	assertSomeoneEnter(around, boy)
	assertSomeoneEnter(around, lady)
	
    --女主接受不了，去湖边准备自杀
    aoi:set_pos(girl, pos_lake.x, pos_lake.y, pos_lake.z)
    local around = aoi:get_around_offset(girl, 150, 150)
    lu.assertEquals(get_tbl_size(around), 2)
	assertSomeoneLeave(around, boy)
	assertSomeoneLeave(around, lady)

    --女主的备胎前去安慰
    local bench = aoi:add()
    aoi:set_pos(bench, pos_lake.x+100, pos_lake.y-100, pos_lake.z+50)
    local around = aoi:get_around_offset(bench, 150, 150)
    lu.assertEquals(get_tbl_size(around), 1)
	assertSomeoneEnter(around, girl)
	local around = aoi:get_around_offset(girl, 150, 150)
    lu.assertEquals(get_tbl_size(around), 1)
	assertSomeoneEnter(around, bench)
	
	--备胎跑去告诉第三者说男主是个渣男
    aoi:set_pos(bench, pos_dongguan.x-30, pos_dongguan.y-18, pos_dongguan.z+25)
    local around = aoi:get_around_offset(bench, 150, 150)
    lu.assertEquals(get_tbl_size(around), 3)
	assertSomeoneEnter(around, boy)
	assertSomeoneEnter(around, lady)
	assertSomeoneLeave(around, girl)

	--第三者得知真相后离开男主跑去和女主道歉
    aoi:set_pos(lady, pos_lake.x+10, pos_lake.y+10, pos_lake.z-50)
    local around = aoi:get_around_offset(lady, 150, 150)
    lu.assertEquals(get_tbl_size(around), 2)
	assertSomeoneEnter(around, girl)
	assertSomeoneLeave(around, boy)

	--女主和第三者相逢恨晚，百合花开
	local around = aoi:get_around_offset(girl, 150, 150)
    lu.assertEquals(get_tbl_size(around), 2)
	assertSomeoneEnter(around, lady)
	assertSomeoneLeave(around, bench)

    --备胎和男主没了女人后颓废渡日，一起玩游戏培养了深厚的基情
 	local around = aoi:get_around_offset(boy, 150, 150)
    lu.assertEquals(get_tbl_size(around), 1)
	assertSomeoneEnter(around, bench)
	local around = aoi:get_around_offset(bench, 150, 150)
    lu.assertEquals(get_tbl_size(around), 1)
	assertSomeoneLeave(around, lady)--上次第三者跑去找女主时的离开

    --男主突然被车撞死了（我也不想这么狗血的，但remove函数必须得测试啊）
    aoi:remove(boy)
    local around = aoi:get_around_offset(boy, 11150, 11150)
    assertNothingChange(around)
    local around = aoi:get_around_offset(bench, 150, 150)
    lu.assertEquals(get_tbl_size(around), 1)
	assertSomeoneLeave(around, boy)

	--剩下的三人出席葬礼后过了上幸福的生活
    aoi:set_pos(girl, pos_cemetery.x, pos_cemetery.y, pos_cemetery.z)
    aoi:set_pos(lady, pos_cemetery.x, pos_cemetery.y, pos_cemetery.z)
    aoi:set_pos(bench, pos_cemetery.x, pos_cemetery.y, pos_cemetery.z)
    local around = aoi:get_around_offset(girl, 150, 150)
    lu.assertEquals(get_tbl_size(around), 1)
	assertSomeoneEnter(around, bench)
	local around = aoi:get_around_offset(lady, 150, 150)
    lu.assertEquals(get_tbl_size(around), 1)
	assertSomeoneEnter(around, bench)
	local around = aoi:get_around_offset(bench, 150, 150)
    lu.assertEquals(get_tbl_size(around), 2)
	assertSomeoneEnter(around, girl)
	assertSomeoneEnter(around, lady)

	--备胎在996血汗工厂工作了几年后猝死在键盘前
	aoi:remove(bench)
	local around = aoi:get_around_offset(girl, 150, 150)
    lu.assertEquals(get_tbl_size(around), 1)
	assertSomeoneLeave(around, bench)

	--女主生下了备胎的小轮胎
	local baby = aoi:add()
    aoi:set_pos(baby, pos_cemetery.x, pos_cemetery.y, pos_cemetery.z)
    local around = aoi:get_around_offset(baby, 150, 150)
    lu.assertEquals(get_tbl_size(around), 2)
	assertSomeoneEnter(around, girl)
	assertSomeoneEnter(around, lady)
end

os.exit( lu.LuaUnit.run() )
