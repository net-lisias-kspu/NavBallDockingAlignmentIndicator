
set H=R:\KSP_1.1.4_dev
echo %H%


copy /Y "bin\Debug\NavBallDockingAlignmentIndicatorCE.dll" "GameData\NavBallDockingAlignmentIndicatorCE\Plugins"
copy /Y NavBallDockingAlignmentIndicatorCE.version GameData\NavBallDockingAlignmentIndicatorCE

cd GameData
mkdir "%H%\GameData\NavBallDockingAlignmentIndicatorCE"
xcopy /y /s NavBallDockingAlignmentIndicatorCE "%H%\GameData\NavBallDockingAlignmentIndicatorCE"

pause