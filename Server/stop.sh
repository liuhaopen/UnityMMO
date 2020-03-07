#!/bin/sh
PID=$(ps e -u ${USER} | grep -v grep | grep "$(pwd)" | grep skynet | awk '{print $1}')
kill ${PID}
