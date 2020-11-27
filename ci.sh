#!/bin/sh

dotnet restore
dotnet clean -c Release
# netcoreapp3.1 only as test csproj also targets others
dotnet build rm.Extensions.sln \
	-c Release \
	-f net5.0
dotnet test tests/rm.ExtensionsTest/rm.ExtensionsTest.csproj \
	-c Release --no-build --no-restore \
	-f net5.0 \
	-v normal \
	--filter "TestCategory!=very.slow"
