#!/usr/bin/env bash
set -e

# Usage: killDotNetApp <AppDllFilename>
killDotNetApp() {
    local PID_LIST=$(ps aux | grep $1.dll | grep dotnet | awk '{print $2}')
    for pid in ${PID_LIST}; do {
        echo "kill process ${pid} ($1)"
        kill ${pid}
    } done
}

killDotNetApp "Frontend"
killDotNetApp "Backend"
killDotNetApp "TextRancCalc"
killDotNetApp "TextListener"
killDotNetApp "TextStatistics"
killDotNetApp "VowelConsRater"
killDotNetApp "VowelConsCounter"
killDotNetApp "TextSuccessMarker"
killDotNetApp "TextProcessingLimiter"
echo "done"
