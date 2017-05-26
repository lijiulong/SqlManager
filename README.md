# SqlManager
A light weight, mock enabled, easy to extend, and also fully documented sql script manager for dot net 2.0 and above
applications. SqlManager is focused on eliminating sql text inside dot net code. It provides various mock and diagnostic
tools at the same time.

## Introduction
SqlManager is not an ORM (object-relational mapping) solution. It's built as a library to manage all kinds of sql
scripts inside dot net applications.

There are two purposes for creating SqlManager:

1. Seperate sql scripts from dot net code.  
    It's not a good idea to mix them together. Before I build this library, I got a task to add some function in an
    enterprise application. The source code mixed everything together. It's very difficult to understand the logic. At
    last, I found it impossible to reuse the code. Since I have the opportunity to rebuild that part, I decide to build
    a library to support my purpose.

2. Support test driven development & unit testings.  
    In my case, the part I need to rebuild is a calculation library. Result value is very important but it's also
    difficult to test, because that application needs to get huge amount of data from database and use them to
    calculate. After calculation, data will be updated and cannot be used in next run. If it's able to control the input
    and output process, it will be easier to write unit test cases.

## Why SqlManager?
There are similar solutions especially those light weight ORM solutions which can solve the first or second problem.
(Seperate sql scripts with dot net code and support test driven development & unit testings) But few can solve them at
the same time. I also find some of the solutions inactive now. I need to spend a lot of effort before I can use them.

SqlManager is built to focus on single purpose. Different function locates in different libraries. The basic library is
simple but also powerful. You can store sql scripts in xml files and get them by key in code. You can also store only
parts of sql clauses and build the query dynamically without writing any sql text in code.

SqlManager supports all database which provides support to ADO.Net. SqlManager provides interface and base class for
extension.

SqlManager.Mock library enables applications using SqlManager to create unit tests by controlling query results. You can
provide different query results for the same sql script by executed times. SqlManager.Mock supports both csv files and
all kinds of databases as input of mocked query result. For insert, update and delete statements, SqlManager.Mock 
supports to execute the query in a different database, most likely a SQLite database, which can be copied easily.

SqlManager.Diagnostic library enables applications using SqlManager to build diagnostic tools. You can modify queries
and parameters before execute or cancel the query. You can get time elapsed during execution or provide a different
value as the result. With these features, it's easy to build powerful diagnostic tools without touching any code in
original code.

SqlManager is the best solution for small or medium sized applications which do not have too many kinds of entities or
the business is not strictly about entities. It's necessary to build sql queries dynamically according to previous query
results. And there is a strong need to write unit testing cases for the application. It's not suitable to use only 
SqlManager in an ERP system, but it can be used in some part of such large system.

## How to use SqlManager?
SqlManager can be installed from [Nuget](https://docs.nuget.org/docs/start-here/installing-nuget) Package Manager UI in
Visual Studio or via Package Manager Console with the following command:

```
PM> Install-Package SqlManager
```

To work with SqlManager, you need to store your sql scripts inside an xml file. You can organize those files by folders
or put them in different places. For content and format of query xml files, please refer to
[this sample file](SqlManager/StandaloneQueries_Sample.xml).

In your program, you need to add `Franksoft.SqlManager.DbProviders` to namespace usings and create a new instance of
any type of DbProvider. DbProvider is a wrap class of native ADO.Net class. You can create a new instance like this:

```C#
OleDbProvider provider = new OleDbProvider(connectionString);
```

After create this provider, assign it to SqlManager, so that SqlManager will be able to use this provider.

```C#
SqlManager.Instance.DbProvider = provider;
```

Then you can access all methods in `SqlManager.Instance` such as `DbDataReader GetStandaloneQueryReader(string key)`,
`int ExecuteStandaloneNonQuery(string key)` or `object ExecuteStandaloneScalar(string key)`. You can get results
directly with the key you configured inside xml file. SqlManager is designed as singleton pattern, there will be only
one instance in one application domain.

For detailed instructions, please refer to [HOWTO.md](HOWTO.md).

## Road map
Currently SqlManager is already used in enterprise applications. All planned functions are completed. There is no plan
to add more functions inside SqlManager. But there are several points may need updates in the future:

1. There might be small changes about interface when I develop other components based on SqlManager;
2. Currently there is only one way to build sql script according to definition inside xml files, it's possible to expose
that part as interface for extensions in the future;
3. Performance is not observed yet, it should share the same performance with native ADO.Net code. If any performance
issue is found in the future, I will try to solve it. It's possible to change some of the interface.

## Copyright and license
SqlManager is licensed under GNU Lesser General Public License v3.0 (LGPLv3). The purpose is to make it useful and
available for most people, but also able to recieve enhancements to this library. For complete license detail, please
refer to [LICENSE](LICENSE).

## Version history
### Latest release
* SqlManager: 1.0.1.2  
    Source code XML comments for SqlManager library finished, update package to deliver with it.
* SqlManager.SQLiteProvider: 1.0.0.1  
    Publish again to include dependency to SqlManager.
* SqlManager.Mock: 1.0.0.2  
    Publish again to include dependency to SqlManager.
* SqlManager.Diagnostic: 1.0.1.3  
    Fix a code error, correct version number, publish again to include dependency to SqlManager.

### 2017-05-26
* SqlManager: 1.0.1.2  
    Source code XML comments for SqlManager library finished, update package to deliver with it.

### 2017-04-17
* SqlManager.SQLiteProvider: 1.0.0.1  
    Publish again to include dependency to SqlManager.
* SqlManager.Mock: 1.0.0.2  
    Publish again to include dependency to SqlManager.
* SqlManager.Diagnostic: 1.0.1.3  
    Fix a code error, correct version number, publish again to include dependency to SqlManager.

### 2017-04-14
* SqlManager: 1.0.1.1  
    SqlManager now supports location of assembly as default base path of relative path. App domain path will be enabled
    only by configuration.
* SqlManager.SQLiteProvider: 1.0.0.0  
* SqlManager.Mock: 1.0.0.1  
    Update code according to SqlManager changes.
* SqlManager.Diagnostic: 1.0.0.1  
    Add SqlDiagnosticWrapper to SqlManager.Diagnostic.