

# Usage: killDotNetApp <AppDllFilename>
killDotNetApp() {
    local PROCESS_LIST=$(ps aux | grep $1)
    if [ $(echo "${PROCESS_LIST}" | wc -l) > 1 ]; then
        local PID_LIST=$(echo ${PROCESS_LIST} | head -n -1 | awk '{print $2}')
        for pid in ${PID_LIST}; do {
            echo "kill process ${pid} ($1)"
            kill ${pid}
        } done
    fi
}

killDotNetApp "Frontend.dll"
killDotNetApp "Backend.dll"
echo "done"
