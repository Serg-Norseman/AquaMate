del .\*.exe.*
del .\*.dll
del .\*.pdb
del .\*.mdb
del .\*.xml
del .\*.log

rmdir .\.vs /s /q

rmdir .\AquaLog\bin /s /q
rmdir .\AquaLog\obj /s /q

rmdir .\AquaLog.Tests\bin /s /q
rmdir .\AquaLog.Tests\obj /s /q
rmdir .\AquaLog.Tests\OpenCover /s /q

rmdir .\BSLib.Timeline\bin /s /q
rmdir .\BSLib.Timeline\obj /s /q

rmdir .\M3DViewerTest\bin /s /q
rmdir .\M3DViewerTest\obj /s /q

del msbuild.log

rmdir .\.sonarqube /s /q
