#!/bin/bash
grep -Pob `sed 's/./&.?/g'<<<$1`<<<$2|cut -d: -f1
