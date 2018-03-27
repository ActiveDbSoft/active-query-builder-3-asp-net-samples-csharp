# Active Query Builder Cross-domain request demo

To run this demo, run two web servers:

- Server A: ASP.NET server running this demo project.
- Server B: Simple HTTP server returning the `./FrontEnd/index.html` page.

1. Run the server A.

    In the `Global.asax` file indicate the *host address of the server B* via the **Application_EndRequest** method (Access-Control-Allow-Origin).

2. Run the server B.
   This might be any static HTTP server, for example https://www.npmjs.com/package/static-server

    In the `index.html` file, specify the *host address of the server A* via the **AQB.Web.host** property.

3. Run the browser and open the `/index.html` page from server B.