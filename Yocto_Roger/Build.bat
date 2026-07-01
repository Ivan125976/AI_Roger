@echo off

set VERSION=2.2.1

echo RogerHubEngine app builder
echo === build .NET ===

dotnet restore

dotnet publish Yocto_Roger.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
dotnet publish Yocto_Roger.csproj -c Release -r win-arm64 --self-contained true -p:PublishSingleFile=true
dotnet publish Yocto_Roger.csproj -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true
dotnet publish Yocto_Roger.csproj -c Release -r linux-arm64 --self-contained true -p:PublishSingleFile=true
dotnet publish Yocto_Roger.csproj -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true

echo "=== Installing VPK-tool ==="

dotnet tool install -g vpk

echo === build VPK ===

echo Building win-x64
vpk pack --runtime win-x64 --packId Yocto_Roger-win-x64 --packVersion %VERSION% --packDir bin\Release\net9.0\win-x64\publish --mainExe Yocto_Roger.exe -y

echo Building win-arm64

vpk pack --runtime win-arm64 --packId Yocto_Roger-win-arm64 --packVersion %VERSION% --packDir bin\Release\net9.0\win-arm64\publish --mainExe Yocto_Roger.exe -y

echo Building osx-x64 (mac)
echo If you're compiling on Windows, it won't work because it requires tools that are only available on Mac. Therefore, if you want to compile a binary for Mac, please compile it on a Mac
vpk [osx] pack --runtime osx-x64 --packId Yocto_Roger-osx-x64 --packVersion %VERSION% --packDir bin\Release\net9.0\osx-x64\publish\Yocto_Roger.app --mainExe Yocto_Roger -y

echo Building linux-x64
vpk [linux] pack --runtime linux-x64 --packId Yocto_Roger-linux-x64 --packVersion %VERSION% --packDir bin\Release\net9.0\linux-x64\publish --mainExe Yocto_Roger -y

echo Building linux-arm64
vpk [linux] pack --runtime linux-arm64 --packId Yocto_Roger-win-arm64 --packVersion %VERSION% --packDir bin\Release\net9.0\linux-arm64\publish --mainExe Yocto_Roger -y 

echo done.
pause