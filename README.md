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
last, I found it impossible to reuse the code. Since I have the opportunity to rebuild that part, I decide to build a
library to support my purpose.

2. Support test driven development & unit testings.
In my case, the part I need to rebuild is a calculation library. Result value is very important but it's also difficult
to test, because that application needs to get huge amount of data from database and use them to calculate. After
calculation, data will be updated and cannot be used in next run. If it's able to control the input and output process,
it will be easier to write unit test cases.

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

## Road map
Currently SqlManager is already used in enterprise applications. All planned functions are completed. There is no plan
to add more functions inside SqlManager. But there are several point may need updates in the future:

1. There might be small interface changes when I develop other components based on SqlManager;
2. Currently there is only one way to build sql script according to definition inside xml files, it's possible to expose
that part as interface for extensions in the future;
3. Performance is not observed yet, it should share the same performance with native ADO.Net code. If any performance
issue is found in the future, I will try to solve it. It's possible to change some of the interface.

## Copyright and license

## Version history