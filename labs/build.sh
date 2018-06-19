#!/usr/bin/env bash
set -e

APPLICATION_NAME=rplabs
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
DISTRIBUTION_DIR="${SCRIPT_DIR}/${APPLICATION_NAME}-$1"

isValidVersion() {
    local re='^[[:digit:]]+\.[[:digit:]]+\.[[:digit:]]+$'
    [[ $1 =~ $re ]]
}

deleteFilesByMask() {
    # mkdir -p "$2"
    for file in $(find "$1" -name "$2"); do {
        rm "${file}"
    }; done
}

# Parameters: <project> <version>
publish() {
    local projectDir=$(dirname "$1")
    local projectName=$(basename ${projectDir})
    dotnet publish "$1" --version-suffix "$2" --configuration Release --output release
    deleteFilesByMask "${projectDir}/release" "*.pdb"
    mkdir -p "${DISTRIBUTION_DIR}"
    cp -r "${projectDir}/release/." "${DISTRIBUTION_DIR}/${projectName}"
}

if ! isValidVersion "$1"; then
    echo "requires one argument: semantic version, e.g. 1.1.1"
    exit 1
fi

if [ -d "${DISTRIBUTION_DIR}" ]; then
    if [ "$2" == "--force" ]; then
        echo Removing old "${DISTRIBUTION_DIR}"...
        rm -rf "${DISTRIBUTION_DIR}"
    else
        echo "output directory '${DISTRIBUTION_DIR}' already exists, add --force to overwrite"
        exit 1
    fi
fi

publish "${SCRIPT_DIR}/Frontend/Frontend.csproj" "$1"
publish "${SCRIPT_DIR}/Backend/Backend.csproj" "$1"
publish "${SCRIPT_DIR}/TextListener/TextListener.csproj" "$1"
publish "${SCRIPT_DIR}/TextRancCalc/TextRancCalc.csproj" "$1"
publish "${SCRIPT_DIR}/VowelConsCounter/VowelConsCounter.csproj" "$1"
publish "${SCRIPT_DIR}/VowelConsRater/VowelConsRater.csproj" "$1"
publish "${SCRIPT_DIR}/TextStatistics/TextStatistics.csproj" "$1"
cp -r "${SCRIPT_DIR}/share/." "${DISTRIBUTION_DIR}"
