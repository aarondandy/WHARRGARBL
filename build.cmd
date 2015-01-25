@echo OFF
setlocal
pushd %~dp0build
scriptcs -script baufile.csx -I -- %*