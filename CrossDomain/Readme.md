# Active Query Builder Cross-domain request demo

## Setup the project

The source of information about database schema must be configured via the `Web.config` file.

Find the `aspQueryBuilder` section and define the XML with pre-loaded metadata information or setup live connection to the database.

Sample of the XML file metadata specification: 

	  <aspQueryBuilder>
	    <syntaxProvider type="ActiveQueryBuilder.Core.MySQLSyntaxProvider, ActiveQueryBuilder.Core" />
	    <metadataSource xml="C:\Path\To\YourMetadataFile"></metadataSource>
	  </aspQueryBuilder>

*Note* that the path to the XML file must be **absolute**.

Sample of the live database connection specification:

	<aspQueryBuilder>
	    <syntaxProvider type="ActiveQueryBuilder.Core.MSSQLSyntaxProvider, ActiveQueryBuilder.Core" />
		<metadataProvider type="ActiveQueryBuilder.Core.MSSQLMetadataProvider, ActiveQueryBuilder.MSSQLMetadataProvider"/> 
		<metadataSource> 
		    <dbConnection 
		        type="System.Data.SqlClient.SqlConnection, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" 
		        connectionString="Data Source=SERVER\SQLEXPRESS;Initial Catalog=my_db_name;Integrated Security=True" 
		    />
	    </metadataSource>
	</aspQueryBuilder>

## Running the project normally

To run this demo, run two web servers:

- Server A: ASP.NET server running this demo project.
- Server B: Simple HTTP server returning the `./FrontEnd/index.html` page.
   This might be any static HTTP server, for example https://www.npmjs.com/package/static-server

    In the `index.html` file, specify the *host address of the server A* via the **AQB.Web.host** property.

Run the browser and open the `/index.html` page from server B.


## Running ASP.NET server in a Docker container

To run the ASP.NET server in a docker container, do the following.

1. In the case of loading metadata from XML file, add this file to the root of the demo project.
2. Specify path to the XML file in the `aspQueryBuilder` section of `Web.config` file as follows:

       <metadataSource xml="/inetpub/wwwroot/YourXmlMetadata.xml"></metadataSource> 
    
3. Publish project to the `./bin/Release/PublishOutput/` directory.
4. Run the `CreateDocker.bat` file. It will create a docker image and run the container with the  `aqbcrossdomain` name.
5. You will get the container IP-address. Specify it via the **AQB.Web.host** property in the `./FrontEnd/index.html` file hosted on a static HTTP server.

Run the browser and open the `/index.html` page from the static HTTP server.
