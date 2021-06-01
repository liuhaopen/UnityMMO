#!/bin/sh
export ROOT=$(cd `dirname $0`; pwd)
export DAEMON=false
if [ ! -d "log" ]; then
  mkdir log
fi
if [ $(ps e -u ${USER} | grep -v grep | grep $(pwd) | grep skynet | wc -l) != 0 ]
then
    echo "server is already running, please execute ./stop.sh"
else
$ROOT/skynet/skynet $ROOT/config > log/$(date "+%Y%m%d-%H%M").log &
fi
