#!/bin/sh
export ROOT=$(cd `dirname $0`; pwd)
export DAEMON=false

while getopts "Dk" arg
do
	case $arg in
		D)
			export DAEMON=true
			;;
		k)
			kill `cat $ROOT/skynet.pid`
			exit 0;
			;;
	esac
done

$ROOT/skynet/skynet $ROOT/config

