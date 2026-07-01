#!/bin/bash

VERSION=2.2.1
BOLD_GREEN='\e[1;32m'
BOLD_YELLOW='\e[1;33m'
RESET='\e[0m'

echo -e "${BOLD_GREEN}RogerHubEngine app builder${RESET}"
echo -e "=== ${BOLD_GREEN}build .NET${RESET} ==="

dotnet restore

dotnet publish Yocto_Roger.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
dotnet publish Yocto_Roger.csproj -c Release -r win-arm64 --self-contained true -p:PublishSingleFile=true
dotnet publish Yocto_Roger.csproj -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true
dotnet publish Yocto_Roger.csproj -c Release -r linux-arm64 --self-contained true -p:PublishSingleFile=true
dotnet publish Yocto_Roger.csproj -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true

echo -e "=== ${BOLD_GREEN}Installing VPK-tool${RESET} ==="

dotnet tool install -g vpk

echo -e "=== ${BOLD_GREEN}build VPK${RESET} ==="

echo -e "${BOLD_GREEN}Building win-x64"
vpk pack --runtime win-x64 --packId Yocto_Roger-win-x64 --packVersion ${VERSION} --packDir bin/Release/net9.0/win-x64/publish --mainExe Yocto_Roger.exe --yes

echo -e "${BOLD_GREEN}Building win-arm64"
vpk pack --runtime win-arm64 --packId Yocto_Roger-win-arm64 --packVersion ${VERSION} --packDir bin/Release/net9.0/win-arm64/publish --mainExe Yocto_Roger.exe --yes

echo -e "${BOLD_GREEN}Building osx-x64 (${BOLD_YELLOW}mac${RESET})"
echo -e "${BOLD_YELLOW}If you're compiling on Windows, it won't work because it requires tools that are only available on Mac. Therefore, if you want to compile a binary for Mac, please compile it on a Mac"
vpk [osx] pack --runtime osx-x64 --packId Yocto_Roger-osx-x64 --packVersion ${VERSION} --packDir bin/Release/net9.0/osx-x64/publish/Yocto_Roger.app --mainExe Yocto_Roger --yes

echo -e "${BOLD_GREEN}Building linux-x64"
vpk [linux] pack --runtime linux-x64 --packId Yocto_Roger-linux-x64 --packVersion ${VERSION} --packDir bin/Release/net9.0/linux-x64/publish --mainExe Yocto_Roger --yes

echo -e "${BOLD_GREEN}Building linux-arm64"
vpk [linux] pack --runtime linux-arm64 --packId Yocto_Roger-win-arm64 --packVersion ${VERSION} --packDir bin/Release/net9.0/linux-arm64/publish --mainExe Yocto_Roger --yes

echo "\e[5;92mdone."
pause