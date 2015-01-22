@echo OFF
pushd %~dp0build
scriptcs -I
scriptcs baufile.csx -- %*
popd