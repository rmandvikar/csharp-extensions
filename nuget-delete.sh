#!/bin/sh

#usage: nuget-delete <version>

if [[ "$1" == "-h" ]]; then print-file-comments "$0"; exit; fi

version="$1"
if [[ -z "$version" ]]; then
	echo 1>&2 "fatal: version required"
	exec print-file-comments "$0"
	exit 1
fi

dotnet nuget delete rm.Extensions "$version" \
	-k $(< ~/dump/.nuget.apikey) \
	-s https://api.nuget.org/v3/index.json \
	--non-interactive
