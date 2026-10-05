call .\clean.cmd
call .\build_mswin_x86.cmd

set BUILD_STATUS=%ERRORLEVEL% 
if %BUILD_STATUS%==0 goto installer
if not %BUILD_STATUS%==0 goto fail 
 
:fail 
pause 
exit /b 1 
 
:installer 
cd .\deploy
call aquamate_win_portable.cmd
cd ..
pause 
exit /b 0
