#!/bin/bash

ln -s $1 `echo "$2.svg" | tr '[:upper:]' '[:lower:]'`

