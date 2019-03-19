@echo off
setlocal enabledelayedexpansion

if [%1] == [] GOTO :USAGE
set version=%1
set fileversion=%1
set informationalversion=%1

if [%2] NEQ [] SET fileversion=%2
if [%3] NEQ [] SET informationalversion=%3

echo Setting assembly version information

for /F "delims=" %%f in ('dir /s /b Assemblyinfo.cs') do (
    echo ^> %%f

    pushd %%~dpf

    for /F "usebackq delims=" %%g in ("AssemblyInfo.cs") do (
        set ln=%%g
        set skip=0

        if "!ln:AssemblyVersion=!" NEQ "!ln!" set skip=1
        if "!ln:AssemblyFileVersion=!" NEQ "!ln!" set skip=1
        if "!ln:AssemblyInformationalVersion=!" NEQ "!ln!" set skip=1

        if !skip!==0 echo !ln! >> AssemblyInfo.cs.versioned
    )

    echo [assembly: AssemblyVersion^("%version%"^)] >> AssemblyInfo.cs.versioned
    echo [assembly: AssemblyFileVersion^("%fileversion%"^)] >> AssemblyInfo.cs.versioned
    echo [assembly: AssemblyInformationalVersion^("%informationalversion%"^)] >> AssemblyInfo.cs.versioned

    copy /y AssemblyInfo.cs AssemblyInfo.cs.orig
    move /y AssemblyInfo.cs.versioned AssemblyInfo.cs

    popd
)
echo Done^^!

GOTO:EOF

:USAGE
echo Usage:
echo.
echo SetVersion.bat Version [FileVersion] [InformationalVersion]
echo.