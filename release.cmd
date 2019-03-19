@echo off
IF %1.==. GOTO NoCommitMessage
echo %~1
pause

for /f "tokens=1 delims=." %%G IN (.semver.txt) DO set MAJOR=%%G 
for /f "tokens=2 delims=." %%G IN (.semver.txt) DO set MINOR=%%G 
for /f "tokens=3 delims=." %%G IN (.semver.txt) DO set PATCH=%%G
set VERSION_CURRENT=%MAJOR:~0,1%.%MINOR:~0,1%.%PATCH%
echo Current: %VERSION_CURRENT%

SET /a PATCH1 = %PATCH%+1
set VERSION=%MAJOR:~0,1%.%MINOR:~0,1%.%PATCH1%
echo %VERSION%>.semver.txt
echo Updating to %VERSION%

call tools\SetVersion.cmd %VERSION%

git add .
git commit -m "%~1"

git tag -m "Version v%VERSION%: %~1" -a v%VERSION%
git push origin v%VERSION%

git push
GOTO End
pause

:NoCommitMessage
echo Please specify commit message: %0 "commit message"
GOTO End

:End