#!/bin/bash
pushd `dirname $0`/build
scriptcs -script baufile.csx -I -- $@