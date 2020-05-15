#/bin/bash

if [ "$1" = "" ]
then echo "missing operations file path"
  exit
fi

dotnet restore
dotnet run -p src/route-simulator.consoleapp/route-simulator.consoleapp.csproj $1

