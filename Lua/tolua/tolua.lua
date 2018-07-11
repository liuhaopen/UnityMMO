--------------------------------------------------------------------------------
--      Copyright (c) 2015 - 2016 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------
if jit then		
	if jit.opt then		
		jit.opt.start(3)				
	end		
	
	print("ver"..jit.version_num.." jit: ", jit.status())
	print(string.format("os: %s, arch: %s", jit.os, jit.arch))
end

if DebugServerIp then  
  require("mobdebug").start(DebugServerIp)
end

require "tolua.misc.functions"
Mathf		= require "tolua.UnityEngine.Mathf"
Vector3 	= require "tolua.UnityEngine.Vector3"
Quaternion	= require "tolua.UnityEngine.Quaternion"
Vector2		= require "tolua.UnityEngine.Vector2"
Vector4		= require "tolua.UnityEngine.Vector4"
Color		= require "tolua.UnityEngine.Color"
Ray			= require "tolua.UnityEngine.Ray"
Bounds		= require "tolua.UnityEngine.Bounds"
RaycastHit	= require "tolua.UnityEngine.RaycastHit"
Touch		= require "tolua.UnityEngine.Touch"
LayerMask	= require "tolua.UnityEngine.LayerMask"
Plane		= require "tolua.UnityEngine.Plane"
Time		= reimport "tolua.UnityEngine.Time"

list		= require "tolua.list"
utf8		= require "tolua.misc.utf8"

require "tolua.event"
require "tolua.typeof"
require "tolua.slot"
require "tolua.System.Timer"
require "tolua.System.coroutine"
require "tolua.System.ValueType"
require "tolua.System.Reflection.BindingFlags"

--require "misc.strict"