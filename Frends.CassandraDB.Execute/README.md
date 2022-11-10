# Frends.CassandraDB.Execute
Frends Task for CassandraDB Execute operation.

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)
[![Build](https://github.com/FrendsPlatform/Frends.CassandraDB/actions/workflows/Execute_build_and_test_on_main.yml/badge.svg)](https://github.com/FrendsPlatform/Frends.CassandraDB/actions)
![MyGet](https://img.shields.io/myget/frends-tasks/v/Frends.CassandraDB.Execute)
![Coverage](https://app-github-custom-badges.azurewebsites.net/Badge?key=FrendsPlatform/Frends.CassandraDB/Frends.CassandraDB.Execute|main)

# Installing

You can install the Task via Frends UI Task View or you can find the NuGet package from the following NuGet feed https://www.myget.org/F/frends-tasks/api/v2.

## Building


Rebuild the project

`dotnet build`

Run tests
 
`docker run --rm -d -p 9042:9042 cassandra:4`

`dotnet test`


Create a NuGet package

`dotnet pack --configuration Release`