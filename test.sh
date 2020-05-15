#/bin/bash

dotnet restore
dotnet test src/route-simulator.test/route-simulator.test.csproj 

