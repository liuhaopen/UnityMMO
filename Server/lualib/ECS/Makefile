MINGW=/usr/local
LUA_INC=-I $(MINGW)/include
LUA_LIB=-L $(MINGW)/bin

ECSCore.so : ECSCore.c
	gcc -g -Wall -fPIC --shared -o $@ $^ $(LUA_INC) $(LUA_LIB)

clean :
	rm -f ECSCore.so