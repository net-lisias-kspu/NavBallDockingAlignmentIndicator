@echo off

copy /Y "bin\Release\NavBallDockingAlignmentIndicatorCE.dll" "GameData\NavBallDockingAlignmentIndicatorCE\Plugins"
copy /Y NavBallDockingAlignmentIndicatorCE.version GameData\NavBallDockingAlignmentIndicatorCE
copy /Y ..\MiniAVC.dll GameData\NavBallDockingAlignmentIndicatorCE

set DEFHOMEDRIVE=d:
set DEFHOMEDIR=%DEFHOMEDRIVE%%HOMEPATH%
set HOMEDIR=
set HOMEDRIVE=%CD:~0,2%

set RELEASEDIR=d:\Users\jbb\release
set ZIP="c:\Program Files\7-zip\7z.exe"
echo Default homedir: %DEFHOMEDIR%

rem set /p HOMEDIR= "Enter Home directory, or <CR> for default: "

if "%HOMEDIR%" == "" (
set HOMEDIR=%DEFHOMEDIR%
) 
echo %HOMEDIR%

SET _test=%HOMEDIR:~1,1%
if "%_test%" == ":" (
set HOMEDRIVE=%HOMEDIR:~0,2%
)


type NavBallDockingAlignmentIndicatorCE.version
set /p VERSION= "Enter version: "


copy /y /s license.md GameData\NavBallDockingAlignmentIndicatorCE


set FILE="%RELEASEDIR%\NavBallDockingAlignmentIndicatorCE-%VERSION%.zip"
IF EXIST %FILE% del /F %FILE%
%ZIP% a -tzip %FILE% GameData
