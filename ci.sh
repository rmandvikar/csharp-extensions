#!/bin/sh

dotnet restore
dotnet clean -c Release
# netcoreapp3.1 only as test csproj also targets others
dotnet build rm.Extensions.sln \
	-c Release \
	-f netcoreapp3.1
dotnet test tests/rm.ExtensionsTest/rm.ExtensionsTest.csproj \
	-c Release --no-build --no-restore \
	-f netcoreapp3.1
