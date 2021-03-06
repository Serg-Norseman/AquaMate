@echo off
cls
rem "AquaMate", Home aquariums manager.
rem Copyright (C) 2019-2020 by Sergey V. Zhdanovskih.

set lstfile=".\listfile.txt"
set out_fn="aquamate_1.4.0_win_portable"
set zip_fn=".\%out_fn%.zip"
set log_fn=".\%out_fn%.log"

echo Processing portable installation start

del .\*.zip /q
rem del ..\appdata\*.* /q

echo "..\AquaMate.exe" > %lstfile%
echo "..\AquaMate.exe.config" >> %lstfile%

echo "..\AquaMate.Core.dll" >> %lstfile%

echo "..\BSLib.dll" >> %lstfile%
echo "..\BSLib.Controls.dll" >> %lstfile%
echo "..\BSLib.Timeline.dll" >> %lstfile%
echo "..\csgl.dll" >> %lstfile%
echo "..\csgl.native.dll" >> %lstfile%
echo "..\DotNetRtfWriter.dll" >> %lstfile%
echo "..\ExcelLibrary.dll" >> %lstfile%
echo "..\log4net.dll" >> %lstfile%
echo "..\sqlite3.dll" >> %lstfile%
echo "..\ZedGraph.dll" >> %lstfile%

echo "..\LICENSE" >> %lstfile%

echo "..\locales\" >> %lstfile%

rem "c:\Program Files\7-zip\7z.exe" a -tzip -mx5 -scsWIN %zip_fn% @%lstfile% > %log_fn%
"c:\Program Files\7-zip\7z.exe" a -tzip -mx9 -scsWIN %zip_fn% @%lstfile%
del %lstfile%

echo Processing portable installation complete
