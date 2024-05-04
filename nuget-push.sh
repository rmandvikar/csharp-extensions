#!/bin/sh

#usage: nuget-push <version>

if [[ "$1" == "-h" ]]; then print-file-comments "$0"; exit; fi

version="$1"
if [[ -z "$version" ]]; then
	echo 1>&2 "fatal: version required"
	exec print-file-comments "$0"
	exit 1
fi

tag="v$version"

dotnet nuget push .nupkg/rmandvikar.Extensions."$version".nupkg \
	-k $(< ~/dump/.nuget.apikey) \
	-s https://api.nuget.org/v3/index.json \
	&& git push $(git remote) "$tag"
