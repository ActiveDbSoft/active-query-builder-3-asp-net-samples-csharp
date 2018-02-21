using System;
using System.Configuration;
using System.IO;
using System.Web.UI;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;

namespace WebForms_Samples.Samples
{
    public partial class MetadataStructure : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.Get("MetadataStructure");

            if (qb == null)
                qb = CreateQueryBuilder();

            QueryBuilderControl1.QueryBuilder = qb;
            ObjectTreeView1.QueryBuilder = qb;
            Canvas1.QueryBuilder = qb;
            Grid1.QueryBuilder = qb;
            SubQueryNavigationBar1.QueryBuilder = qb;
            SqlEditor1.QueryBuilder = qb;
            StatusBar1.QueryBuilder = qb;
        }

        private QueryBuilder CreateQueryBuilder()
        {
            // Get instance of QueryBuilder
            var queryBuilder = QueryBuilderStore.Create("MetadataStructure");
            // Turn this property on to suppress parsing error messages when user types non-SELECT statements in the text editor.
            queryBuilder.BehaviorOptions.AllowSleepMode = true;

            // Assign an instance of the syntax provider which defines SQL syntax and metadata retrieval rules.
            queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();

            queryBuilder.MetadataLoadingOptions.OfflineMode = true;

            // Load MetaData from XML document. File name stored in WEB.CONFIG file in [/configuration/appSettings/Db2XmlMetaData] key
            var path = ConfigurationManager.AppSettings["NorthwindXmlMetaData"];
            var xml = Path.Combine(Server.MapPath("~"), path);

            queryBuilder.MetadataContainer.ImportFromXML(xml);

            // Initialization of the Metadata Structure object that's
            // responsible for representation of metadata in a tree-like form
            // Disable the automatic metadata structure creation
            queryBuilder.MetadataStructure.AllowChildAutoItems = false;

            // queryBuilder.DatabaseSchemaTreeOptions.DefaultExpandLevel = 0;

            MetadataFilterItem filter;

            // Create a top-level folder containing all objects
            MetadataStructureItem allObjects = new MetadataStructureItem();
            allObjects.Caption = "All objects";
            filter = allObjects.MetadataFilter.Add();
            filter.ObjectTypes = MetadataType.All;
            queryBuilder.MetadataStructure.Items.Add(allObjects);

            // Create "Favorites" folder
            MetadataStructureItem favorites = new MetadataStructureItem();
            favorites.Caption = "Favorites";
            queryBuilder.MetadataStructure.Items.Add(favorites);

            MetadataItem metadataItem;
            MetadataStructureItem item;

            // Add some metadata objects to "Favorites" folder
            metadataItem = queryBuilder.MetadataContainer.FindItem<MetadataItem>("Orders");
            item = new MetadataStructureItem();
            item.MetadataItem = metadataItem;
            favorites.Items.Add(item);

            metadataItem = queryBuilder.MetadataContainer.FindItem<MetadataItem>("Order Details");
            item = new MetadataStructureItem();
            item.MetadataItem = metadataItem;
            favorites.Items.Add(item);

            // Create folder with filter
            MetadataStructureItem filteredFolder = new MetadataStructureItem(); // creates dynamic node
            filteredFolder.Caption = "Filtered by 'Prod%'";
            filter = filteredFolder.MetadataFilter.Add();
            filter.ObjectTypes = MetadataType.Table | MetadataType.View;
            filter.Object = "Prod%";
            queryBuilder.MetadataStructure.Items.Add(filteredFolder);

            queryBuilder.SQL = GetDefaultSql();

            return queryBuilder;
        }

        private string GetDefaultSql()
        {
            return @"SELECT Orders.OrderID, Orders.CustomerID, Orders.OrderDate, [Order Details].ProductID,
					[Order Details].UnitPrice, [Order Details].Quantity, [Order Details].Discount
					FROM Orders INNER JOIN [Order Details] ON Orders.OrderID = [Order Details].OrderID
					WHERE Orders.OrderID > 0 AND [Order Details].Discount > 0";
        }
    }
}