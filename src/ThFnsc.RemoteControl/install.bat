@echo off
cd "%~dp0%"
set exename=ThFnsc.RemoteControl
sc.exe stop    %exename%
sc.exe delete  %exename%
sc.exe create  %exename% "binPath=%~dp0\%exename%.exe" start=auto
sc.exe failure %exename% reset= 86400 actions= restart/60000/restart/60000//1000
sc.exe start   %exename%
pause