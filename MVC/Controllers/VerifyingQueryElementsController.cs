using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;
using ActiveQueryBuilder.Web.Server.Events;

namespace MVC_Samples.Controllers
{
    public class VerifyingQueryElementsController : Controller
    {
        private const string InstanceId = "VerifyingQueryElements";

        private const string RequiredTable = "Orders";
        private const string RequiredField = "OrderID";
        private const string RequiredFieldExpression = "o.OrderID";

        public ActionResult Index()
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.Get(InstanceId);

            if (qb == null)
                qb = CreateQueryBuilder();

            return View(qb);
        }

        private QueryBuilder CreateQueryBuilder()
        {
            // Create an instance of the QueryBuilder object
            var queryBuilder = QueryBuilderStore.Factory.MsSql(InstanceId);

            // Denies metadata loading requests from the metadata provider
            queryBuilder.MetadataLoadingOptions.OfflineMode = true;

            // Load MetaData from XML document. File name stored in WEB.CONFIG file in [/configuration/appSettings/NorthwindXmlMetaData] key
            var path = ConfigurationManager.AppSettings["NorthwindXmlMetaData"];
            var xml = Path.Combine(Server.MapPath("~"), path);

            queryBuilder.DataSourceFieldRemoving += QueryBuilderOnDataSourceFieldRemoving;
            queryBuilder.DataSourceRemoving += QueryBuilderOnDataSourceRemoving;
            queryBuilder.SQLQuery.SQLUpdated += SqlQueryOnSqlUpdated;

            queryBuilder.MetadataContainer.ImportFromXML(xml);

            //Set default query
            queryBuilder.SQL = GetDefaultSql();

            return queryBuilder;
        }

        private void SqlQueryOnSqlUpdated(object sender, EventArgs e)
        {
            var qb = QueryBuilderStore.Get(InstanceId);
            if (qb == null)
                return;

            var isRequiredPresent = qb.SQLQuery.QueryRoot.Items.OfType<UnionSubQuery>().Any(IsRequiredItemsPresent);
            if (!isRequiredPresent)
                qb.Message.Warning("Required table and field is not present in the query.");
            else
                qb.Message.Clear();
        }

        private bool IsRequiredItemsPresent(UnionSubQuery query)
        {
            var isTablePresent = query.FromClause.Items.OfType<DataSourceObject>()
                .FirstOrDefault(x => x.MetadataObject.Name == RequiredTable) != null;

            var isFieldPresent = query.QueryColumnList.Items
                .FirstOrDefault(x => x.ExpressionString == RequiredFieldExpression) != null;

            return isTablePresent && isFieldPresent;
        }

        private void QueryBuilderOnDataSourceRemoving(DataSourceRemovingEventArgs e)
        {
            if (IsMainSubQuery() && IsRequiredDataSource(e.DataSource))
                e.Abort = true;
        }

        private void QueryBuilderOnDataSourceFieldRemoving(CheckDataSourceFieldEventArgs e)
        {
            if (IsMainSubQuery() && IsRequiredDataSource(e.DataSource) && e.Field == RequiredField)
                e.Abort = true;
        }

        private bool IsMainSubQuery()
        {
            var qb = QueryBuilderStore.Get(InstanceId);
            if (qb == null)
                return false;

            return qb.ActiveSubQuery == qb.SQLQuery.QueryRoot;
        }

        private bool IsRequiredDataSource(DataSource dataSource)
        {
            var dsObject = dataSource as DataSourceObject;
            if (dsObject == null)
                return false;

            return dsObject.MetadataObject.Name == RequiredTable;
        }

        private string GetDefaultSql()
        {
            return @"Select o.OrderID,
                        c.CustomerID,
                        s.ShipperID,
                        o.ShipCity
                    From Orders o
                        Inner Join Customers c On o.CustomerID = c.CustomerID
                        Inner Join Shippers s On s.ShipperID = o.OrderID
                    Where o.ShipCity = 'A'";
        }
    }
}