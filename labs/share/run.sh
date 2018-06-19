#!/usr/bin/env bash

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
LOG_DIR="${SCRIPT_DIR}/logs"

# Read config file
IFS="="
while read -r name value
do
if ! [[ -z "$name" ]]
then
    export $name=$value
fi
done < "${SCRIPT_DIR}/services.conf"

# Usage: runDotNetApp <ProjectName>
runDotNetApp() {
    nohup dotnet "${SCRIPT_DIR}/$1/$1.dll" 1>"${LOG_DIR}/$1.out.log" 2>"${LOG_DIR}/$1.err.log" &
}

# Usage: runDotNetApp <Count> <ProjectName>
# <Count> should be in range 1..32
runDotNetAppMulti() {
    if [[ $1 -lt 1 ]] || [[ $1 -gt 32 ]] ; then
        echo "wrong count for runDotNetAppMulti"
        exit
    fi
    for ((i=0; i<$1; i++)); do
        runDotNetApp "$2"
    done
}

echo "killing existing instances..."
bash "${SCRIPT_DIR}/stop.sh"
echo "starting up..."
mkdir -p "${LOG_DIR}"
runDotNetApp "Frontend"
runDotNetApp "Backend"
runDotNetApp "TextListener"
runDotNetApp "TextRancCalc"
runDotNetAppMulti $NumVowelConsRater "VowelConsRater"
runDotNetAppMulti $NumVowelConsCounter "VowelConsCounter"
echo "done"
