#!/usr/bin/env bash
set -e

# Usage: killDotNetApp <AppDllFilename>
killDotNetApp() {
    local PID_LIST=$(ps aux | grep $1 | grep dotnet | awk '{print $2}')
    for pid in ${PID_LIST}; do {
        echo "kill process ${pid} ($1)"
        kill ${pid}
    } done
}

killDotNetApp "Frontend.dll"
killDotNetApp "Backend.dll"
killDotNetApp "TextRancCalc.dll"
killDotNetApp "TextListener.dll"
killDotNetApp "TextStatistics.dll"
killDotNetApp "VowelConsRater.dll"
killDotNetApp "VowelConsCounter.dll"
echo "done"
