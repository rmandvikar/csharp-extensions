#!/bin/sh

#usage: nuget-push <version>

version="$1"
if [[ -z "$version" ]]; then
	echo 1>&2 "fatal: version required"
	exec print-file-comments "$0"
	exit 1
fi

dotnet nuget push rm.Extensions/bin/Release/rm.Extensions."$version".nupkg \
	-k $(< ~/.nuget.apikey) \
	-s https://api.nuget.org/v3/index.json
