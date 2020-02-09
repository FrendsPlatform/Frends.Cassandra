 - [Frends.Sql](#frends.sql)
   - [Installing](#installing)
   - [Building](#building)
   - [Contributing](#contributing)
   - [Documentation](#documentation)
     - [Sql.ExecuteQuery](#sqlexecutequery) 
     - [Sql.ExecuteProcedure](#sqlexecuteprocedure) 
     - [Sql.BulkInsert](#sqlbulkinsert)
     - [Sql.BatchOperation](#sqlbatchoperation) 
   - [License](#license)

# Frends.Cassandra
FRENDS Cassandra Tasks.

## Installing
You can install the task via FRENDS UI Task view, by searching for packages. You can also download the latest NuGet package from https://www.myget.org/feed/frends/package/nuget/Frends.Sql and import it manually via the Task view.

## Building
Clone a copy of the repo

`git clone https://github.com/FrendsPlatform/Frends.Cassandra.git`

Restore dependencies

`dotnet restore`

Rebuild the project

`dotnet build`

Run Tests
To run the tests you will need an SQL server. You can set the database connection string in test project [appsettings.json](Frends.Sql.Tests/appsettings.json) file

`dotnet test Frends.Sql.Tests`

Create a nuget package

`dotnet pack Frends.Sql`

## Contributing
When contributing to this repository, please first discuss the change you wish to make via issue, email, or any other method with the owners of this repository before making a change.

1. Fork the repo on GitHub
2. Clone the project to your own machine
3. Commit changes to your own branch
4. Push your work back up to your fork
5. Submit a Pull request so that we can review your changes

NOTE: Be sure to merge the latest from "upstream" before making a pull request!

## Documentation

### Cql.ExecuteQuery
#### Input 
| Property          | Type                              | Description                                             | Example                                   |
|-------------------|-----------------------------------|---------------------------------------------------------|-------------------------------------------|
| Query             | string                            | The query that will be executed to the database.        | `Insert into KeyspaceName.TableName(ColumnName1, ColumnName2, ColumnName3 . . . .) values (Column1Value, Column2Value, Column3Value . . . .)` 
| Parameters        | Array{Name: string, Value: string} | A array of parameters to be appended to the query.     | `Name = Age, Value = 42`
| Connection        | Array{Username: string, Password: string, Port: int, Nodes: string} | Connection parameters to be used to connect to the database.|


#### Options
| Property               | Type                 | Description                                                |
|------------------------|----------------------|------------------------------------------------------------|
| Command Timeout        | int                  | Timeout in seconds to be used for the query. 60 seconds by default. |

#### Result
JToken. JArray[]

Example result
```
[ 
 {
  "Name": "Foo",
  "Age": 42
 },
 {
  "Name": "Adam",
  "Age": 42
 }
]
```
```
The second name 'Adam' can be now be accessed by #result[1].Name in the process parameter editor.

```

#### Result
Integer - Number of copied rows

## License

This project is licensed under the MIT License - see the LICENSE file for details