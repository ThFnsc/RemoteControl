@echo off
cd "%~dp0%"
set exename=ThFnsc.RemoteControl
sc.exe stop    %exename%
sc.exe delete  %exename%
pause