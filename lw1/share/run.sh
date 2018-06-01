#!/usr/bin/env bash

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
LOG_DIR="${SCRIPT_DIR}/logs"

mkdir -p "${LOG_DIR}"
nohup dotnet "${SCRIPT_DIR}/Frontend/Frontend.dll" 1>"${LOG_DIR}/frontend.log" 2>"${LOG_DIR}/frontend_errors.log" &
nohup dotnet "${SCRIPT_DIR}/Backend/Backend.dll" 1>"${LOG_DIR}/backend.log" 2>"${LOG_DIR}/backend_errors.log" &
