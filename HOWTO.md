# SqlManager Documentation

## SqlManager
### Quick start
The part for SqlManager is already included inside [README.md file](README.md#how-to-use-sqlmanager).

### Application configurations
Although SqlManager can work without any configuration. It's good to have options to change its behavior when necessary.
Here is [App.config.sample](SqlManager/App.config.sample) file showing how to do this.

There are three application setting items available now:

* SqlManager.ModelDirectory  
A string type value. It indicates the directory to search for sql query files. The default value is the root directory
of this assembly. You can change it to relative path or absolute path. SqlManager will search xml files in all
subdirectories under that path.

* SqlManager.IgnoreDuplicateKeys  
A boolean type value. It indicates whether SqlManager should throw exception when duplicated keys found during
deserialization process. The default value is `false`. If it's set to `true` and duplication occurred, a
`Franksoft.SqlManager.KeyDuplicateException` will be thrown during deserialization process.

* SqlManager.UseAppDomainForRelativePath  
A boolean type value. It indicates whether SqlManager should use application domain path as root directory for relative
pathes. The default value is `false`. If it's set to `true`, all relative pathes will be resolved based on the value of
`AppDomain.CurrentDomain.BaseDirectory`.

In some cases, you may not want to put query files together with executables. The customized configuration section
`ModelRegistrationSection` will be able to accomplish this job. To use this section, you need to import it from
SqlManager assembly like this:

```XML
<configuration>
    <configSections>
        <section type="Franksoft.SqlManager.ModelRegistrationSection,SqlManager" name="ModelRegistrations"/>
    </configSections>
    ...
</configuration>
```

Then you can register query xml files like this:

```XML
<configuration>
    ...
    <ModelRegistrations>
        <pathes>
            <add path="models\01.xml"/>
            <add path="models\02.xml"/>
            <add path="models\03.xml"/>
        </pathes>
    </ModelRegistrations>
</configuration>
```

**CAUTION**  
When you put files in registered pathes and also in ModelDirectory (like the default root folder) at the same time, all
of them will be loaded inside SqlManager.

### Sql query file
Sql query file for SqlManager can be very simple or very complex.
Take [StandaloneQueries_Sample.xml](SqlManager/StandaloneQueries_Sample.xml) as reference.

You can put sql commands with a key inside the file like this:

```XML
<Sql Key="key" Command="select * from dual" />
```

You can also put sql commands with structures, so that you can modify any part of it in code:

```XML
<Sql Key="a">
    <ChildItems>
        <SqlClause Keyword="Select">
            <ChildItems>
                <SqlClause Expression="A AS 'A1'" />
                <SqlClause Expression="B AS 'B1'" />
            </ChildItems>
        </SqlClause>
        <SqlClause Keyword="From" Expression="TableA"/>
        <SqlClause Keyword="Where">
            <ChildItems>
                <SqlClause Expression="A='1'" />
                <SqlClause LogicalOperator="And">
                    <ChildItems>
                        <SqlClause Expression="B='2'" />
                        <SqlClause LogicalOperator="Or" Expression="C='3'" />
                    </ChildItems>
                </SqlClause>
            </ChildItems>
        </SqlClause>
        <SqlClause Keyword="OrderBy">
            <ChildItems>
                <SqlClause Expression="C" />
                <SqlClause Expression="D DESC" />        
            </ChildItems>
        </SqlClause>
    </ChildItems>
</Sql>
```

The result of the sql command in this xml is something like this:

```SQL
SELECT A AS 'A1', B AS 'B1' FROM TableA WHERE (A='1' AND (B='2' OR (C='3'))) ORDER BY C, D DESC
```

The current supported keywords are as follows:  
None, Select, InsertInto, DeleteFrom, Update, Where, From, GroupBy, OrderBy, Fields, Values, Set, SetFields,
EqualValues, Exists, Begin, End, Create, Drop, Alter, Grant

The current supported logical operators are as follows:  
None, And, Or, Not

### Class documentations
(To be updated later.)

## SqlManager.Mock
### Quick start
SqlManager.Mock can be installed from [Nuget](https://docs.nuget.org/docs/start-here/installing-nuget) Package Manager
UI in Visual Studio or via Package Manager Console with the following command:

```
PM> Install-Package SqlManager.Mock
```

If SqlManager is not installed yet, it will also be installed by nuget as a dependency.

Similar to SqlManager, SqlManager.Mock also read mock definitions from xml files. The purpose for building
SqlManager.Mock is to provide alternative and controllable results for each sql command. Thus, the mock xml files
contains mapping among results and queries. And very similar like SqlManager, you can organize those files by folders or
put them in different places. For content and format of mock xml files, please refer to
[this sample file](SqlManager.Mock/StandaloneQueriesMock_Sample.xml).

In your program, you need to add `Franksoft.SqlManager.Mock` to namespace usings and create a new instance of
`MockProvider`:

```C#
MockProvider provider = new MockProvider();
```

After create this provider, assign it to SqlManager, so that SqlManager will be able to use mock provider.

```C#
SqlManager.Instance.DbProvider = provider;
```

If you need to execute some queries in another database instead of only using csv files. You need to create a new
instance of `MockProvider` with the other constructor like this:

```C#
MockProvider provider = new MockProvider(providerForOtherDb);
```

If you need to test nonquery commands, it's the best way.

After that, you will be able to run application with mock provider.

### Application configurations
SqlManager.Mock can also work without any configuration. And of course there is another
[App.config.sample](SqlManager.Mock/App.config.sample) file to show how to configure it when you need to.

There is one extra application setting item available now:

* SqlManager.MockDirectory 
A string type value. It indicates the directory to search for mock files. The default value is also the root directory
of SqlManager assembly. You can change it to relative path or absolute path. SqlManager will search xml files in all
subdirectories under that path.

Besides, SqlManager.Mock shares the same following application setting items:

* SqlManager.IgnoreDuplicateKeys
* SqlManager.UseAppDomainForRelativePath

Like SqlManager, you can also put mock files to somewhere else. The customized configuration section
`MockRegistrationSection` will be able to accomplish this job. To use this section, you need to import it from assembly
like this:

```XML
<configuration>
    <configSections>
        <section type="Franksoft.SqlManager.Mock.MockRegistrationSection,SqlManager.Mock" name="MockRegistrationSection"/>
    </configSections>
    ...
</configuration>
```

Then you can register query xml files like this:

```XML
<configuration>
    ...
    <MockRegistrationSection>
        <pathes>
            <add path="mocks\01.xml"/>
            <add path="mocks\02.xml"/>
            <add path="mocks\03.xml"/>
        </pathes>
    </MockRegistrationSection>
</configuration>
```

### Mock file
Mock file for SqlManager.Mock is more complex than query files.
Please take [StandaloneQueriesMock_Sample.xml](SqlManager.Mock/StandaloneQueriesMock_Sample.xml) for reference.

`SqlMock` represents the mock config mapped to the `Sql` with the same key. Each `SqlMock` includes one or more
`MockConfig` items. Currently, there are two types of `MockConfig` items:

MockConfig referring to a database

```XML
<MockConfig Sequence="0" Repeat="1" ConnectionString="Data Source=mocks\test.db" />
```

MockConfig referring to a csv file

```XML
<MockConfig Sequence="1" Repeat="1" CsvFilePath="mocks\02.csv" Delimiter="," IsIncludeHeader="1" IsIncludeType="1"/>
```

`Sequence` attribute determines which one is executed first. `Repeat` attribute determines how many times this result
is used. If you set `Repeat` to 0, it will continue to repeat all the time and the rest of mock config will be ignored.

For MockConfig items referring to a database, a valid connection string is needed, and for MockConfig items referring to
a csv file, the path of csv file is required. You can change the delimiter with `Delimiter` attribute. If the csv file
includes header, set `IsIncludeHeader` to 1. If the csv file includes type description, set `IsIncludeType` to 1.

Type description should be the assembly-qualified name of the type. You can check `System.Type.AssemblyQualifiedName`
for more information.

If you meet any problem related to file encoding, you can set `EncodingName` attribute manually.
According to dot net documentation, it is "the code page name of the preferred encoding. Any value returned by
`System.Text.Encoding.WebName` is a valid input".

### Class documentations
(To be updated later.)

## SqlManager.Diagnostic
### Quick start
SqlManager.Diagnostic is designed to enable developers to collect as much information as they want from applications
built with SqlManager and change application behaviours when necessary without change any code inside the application.

SqlManager.Diagnostic can be installed from [Nuget](https://docs.nuget.org/docs/start-here/installing-nuget) Package
Manager UI in Visual Studio or via Package Manager Console with the following command:

```
PM> Install-Package SqlManager.Diagnostic
```

If SqlManager is not installed yet, it will also be installed by nuget as a dependency.

Currently there are two ways to create diagnositc functions. One way is using IDbProvider wrapper class:
`DiagnosticProvider`, another way is via Sql wrapper class: `SqlDiagnosticWrapper`.

With events inside `DiagnosticProvider`, you will be able to get messages of every method call to provider.

For events with "Before" prefix, such as `BeforeExecuteNonQuery` and `BeforeDispose`, you will be able to get all
parameters sent to the method, you can change the parameter if necessary and you can cancel the method from execution.

For events with "After" prefix, such as `AfterExecuteNonQuery` and `AfterDispose`, you will be able to get parameters
sent to the method, output values, and executed time duration of the method. You can change output values if necessary.

To use `DiagnosticProvider` in your program, you need to add `Franksoft.SqlManager.Diagnostic` to namespace usings and
create a new instance of `DiagnosticProvider`:

```C#
OleDbProvider dbProvider = new OleDbProvider(connectionString);
DiagnosticProvider provider = new DiagnosticProvider(dbProvider);
```

After create this provider, assign it to SqlManager, so that SqlManager will be able to use diagnostic provider.

```C#
SqlManager.Instance.DbProvider = provider;
```

With events inside `SqlDiagnosticWrapper`, you will be able to get messages of every method call to the specific `Sql`
object. The way to use `SqlDiagnosticWrapper` is much similar with `DiagnosticProvider`.

You can create a `SqlDiagnosticWrapper` object with static method `WrapSql(Sql sql)` inside `SqlDiagnosticWrapper`
class. Or you can convert entire sql query collection inside SqlManager with static method `ConvertEntireCollection()`,
which is also a static method in `SqlDiagnosticWrapper` class.

### Class documentations
(To be updated later.)