

# Usage: killDotNetApp <AppDllFilename>
killDotNetApp() {
    local PID_LIST=$(ps aux | grep $1 | head -n -1 | awk '{print $2}')
    for pid in ${PID_LIST}; do {
        echo "kill process ${pid} ($1)"
        kill ${pid}
    } done
}

killDotNetApp "Frontend.dll"
killDotNetApp "Backend.dll"
echo "done"
