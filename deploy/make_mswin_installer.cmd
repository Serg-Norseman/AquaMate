call .\clean.cmd
call .\build_mswin_x86.cmd

set BUILD_STATUS=%ERRORLEVEL% 
if %BUILD_STATUS%==0 goto installer
if not %BUILD_STATUS%==0 goto fail 
 
:fail 
pause 
exit /b 1 
 
:installer 
"C:\Program Files (x86)\NSIS\makensis.exe" .\deploy\aquamate_win_setup.nsi
"c:\Program Files\7-zip\7z.exe" a -tzip -mx5 -scsWIN .\deploy\aquamate_1.4.0_win.zip .\deploy\aquamate_1.4.0_winsetup.exe
pause 
exit /b 0
