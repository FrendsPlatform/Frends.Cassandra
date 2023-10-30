# Frends.CassandraDB.ExecuteQuery
Frends Task for CassandraDB Execute operation.

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)
[![Build](https://github.com/FrendsPlatform/Frends.CassandraDB/actions/workflows/ExecuteQuery_build_and_test_on_main.yml/badge.svg)](https://github.com/FrendsPlatform/Frends.CassandraDB/actions)
![Coverage](https://app-github-custom-badges.azurewebsites.net/Badge?key=FrendsPlatform/Frends.CassandraDB/Frends.CassandraDB.ExecuteQuery|main)

# Installing

You can install the task via Frends UI Task View.

## Building


Rebuild the project

`dotnet build`

Run tests

```
cd Frends.CassandraDB.ExecuteQuery.Tests`
docker-compose up -d
dotnet test
```


Create a NuGet package

`dotnet pack --configuration Release`