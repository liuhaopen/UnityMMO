# Copyright (C) 2011-2012 LuaDist.
# Created by Peter Kapec
# Redistribution and use of this file is allowed according to the terms of the MIT license.
# For details see the COPYRIGHT file distributed with LuaDist.
# Please note that the package source code is licensed under its own license.

project ( lpeg C )
cmake_minimum_required ( VERSION 2.8 )
include ( cmake/dist.cmake )
include ( lua )

install_lua_module ( lpeg lpvm.c lpcap.c lptree.c lpcode.c lpprint.c lpeg.def )
install_lua_module ( re re.lua )
install_doc ( lpeg.html re.html lpeg-128.gif )
install_data ( HISTORY )
install_test ( test.lua )

# for tests
file ( WRITE "${CMAKE_CURRENT_BINARY_DIR}/strict.lua" "" )
add_lua_module ( strict ${CMAKE_CURRENT_BINARY_DIR}/strict.lua )

add_lua_test ( test.lua )
