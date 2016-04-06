# PowerBIEmbeddedSimplified
Power BI Embedded Simple Application

Simple ASP.NET Application that uses Embedded Power BI feature.

What you need is to update 3 keys in the web.config:

<add key="powerbi:AccessKey" value="" />
<add key="powerbi:WorkspaceCollection" value="" />
<add key="powerbi:WorkspaceId" value="" />

Then, Run the app and you will be able to embed first report you have imported in Power BI workspace collection in Azure.

You can change this by checking the HomeController.

Detailed blog post is here: 
