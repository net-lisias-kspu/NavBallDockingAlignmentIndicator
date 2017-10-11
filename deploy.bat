
set H=R:\KSP_1.3.1_dev
echo %H%


copy /Y "bin\Debug\NavBallDockingAlignmentIndicatorCE.dll" "GameData\NavBallDockingAlignmentIndicatorCE\Plugins"
copy /Y NavBallDockingAlignmentIndicatorCE.version GameData\NavBallDockingAlignmentIndicatorCE

cd GameData

xcopy /y /s /i NavBallDockingAlignmentIndicatorCE "%H%\GameData\NavBallDockingAlignmentIndicatorCE"
