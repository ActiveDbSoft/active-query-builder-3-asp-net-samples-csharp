using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.UI;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;

namespace WebForms_Samples.Samples
{
    public partial class QueryModificationDemo : BasePage
    {
        private SQLQualifiedName _joinFieldName;
        private SQLQualifiedName _companyNameFieldName;
        private SQLQualifiedName _orderDateFieldName;

        private DataSource _customers;
        private DataSource _orders;
        private QueryColumnListItem _companyName;
        private QueryColumnListItem _orderDate;

        private const string CustomersName = "Northwind.dbo.Customers";
        private const string OrdersName = "Northwind.dbo.Orders";
        private const string CustomersAlias = "c";
        private const string OrdersAlias = "o";
        private const string CustomersCompanyName = "CompanyName";
        private const string CusomerId = "CustomerId";
        private const string OrderDate = "OrderDate";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.Get("QueryModification");

            if (qb == null)
                qb = CreateQueryBuilder();

            QueryBuilderControl1.QueryBuilder = qb;
            ObjectTreeView1.QueryBuilder = qb;
            Canvas1.QueryBuilder = qb;
            Grid1.QueryBuilder = qb;
            SubQueryNavigationBar1.QueryBuilder = qb;
            SqlEditor1.QueryBuilder = qb;
            StatusBar1.QueryBuilder = qb;

            cbCompanyName.Enabled = cbCustomers.Checked;
            tbCompanyName.Enabled = cbCompanyName.Checked;

            cbOrderDate.Enabled = cbOrders.Checked;
            tbOrderDate.Enabled = cbOrderDate.Checked;

            //prepare parsed names
            _joinFieldName = qb.SQLContext.ParseQualifiedName(CusomerId);
            _companyNameFieldName = qb.SQLContext.ParseQualifiedName(CustomersCompanyName);
            _orderDateFieldName = qb.SQLContext.ParseQualifiedName(OrderDate);
        }

        private QueryBuilder CreateQueryBuilder()
        {
            // Create an instance of the QueryBuilder object
            var queryBuilder = QueryBuilderStore.Factory.MsSql("QueryModification");
            
            // Denies metadata loading requests from the metadata provider
            queryBuilder.MetadataLoadingOptions.OfflineMode = true;

            // Load MetaData from XML document. File name stored in WEB.CONFIG file in [/configuration/appSettings/NorthwindXmlMetaData] key
            var path = ConfigurationManager.AppSettings["NorthwindXmlMetaData"];
            var xml = Path.Combine(Server.MapPath("~"), path);

            queryBuilder.MetadataContainer.ImportFromXML(xml);

            return queryBuilder;
        }

        private bool IsTablePresentInQuery(UnionSubQuery unionSubQuery, DataSource table)
        {
            // collect the list of datasources used in FROM
            var dataSources = unionSubQuery.GetChildrenRecursive<DataSource>(false);

            // check given table in list of all datasources
            return dataSources.IndexOf(table) != -1;
        }

        private bool IsQueryColumnListItemPresentInQuery(UnionSubQuery unionSubQuery, QueryColumnListItem item)
        {
            return unionSubQuery.QueryColumnList.IndexOf(item) != -1 && !String.IsNullOrEmpty(item.ExpressionString);
        }

        private void ClearConditionCells(UnionSubQuery unionSubQuery, QueryColumnListItem item)
        {
            for (int i = 0; i < unionSubQuery.QueryColumnList.GetMaxConditionCount(); i++)
            {
                item.ConditionStrings[i] = "";
            }
        }

        private DataSource AddTable(UnionSubQuery unionSubQuery, string name, string alias)
        {
            var queryBuilder1 = QueryBuilderStore.Get("QueryModification");

            using (var parsedName = queryBuilder1.SQLContext.ParseQualifiedName(name))
            using (var parsedAlias = queryBuilder1.SQLContext.ParseIdentifier(alias))
            {
                return queryBuilder1.SQLQuery.AddObject(unionSubQuery, parsedName, parsedAlias);
            }
        }

        private DataSource FindTableInQueryByName(UnionSubQuery unionSubQuery, string name)
        {
            var queryBuilder1 = QueryBuilderStore.Get("QueryModification");

            List<DataSourceObject> foundDatasources;
            using (var qualifiedName = queryBuilder1.SQLContext.ParseQualifiedName(name))
            {
                foundDatasources = new List<DataSourceObject>();
                unionSubQuery.FromClause.FindTablesByDbName(qualifiedName, foundDatasources);
            }

            // if found more than one tables with given name in the query, use the first one
            return foundDatasources.Count > 0 ? foundDatasources[0] : null;
        }

        private void AddWhereCondition(QueryColumnList columnList, QueryColumnListItem whereListItem, string condition)
        {
            bool whereFound = false;

            // GetMaxConditionCount: returns the number of non-empty criteria columns in the grid.
            for (int i = 0; i < columnList.GetMaxConditionCount(); i++)
            {
                // CollectCriteriaItemsWithWhereCondition:
                // This function returns the list of conditions that were found in
                // the i-th criteria column, applied to specific clause (WHERE or HAVING).
                // Thus, this function collects all conditions joined with AND
                // within one OR group (one grid column).
                List<QueryColumnListItem> foundColumnItems = new List<QueryColumnListItem>();
                CollectCriteriaItemsWithWhereCondition(columnList, i, foundColumnItems);

                // if found some conditions in i-th column, append condition to i-th column
                if (foundColumnItems.Count > 0)
                {
                    whereListItem.ConditionStrings[i] = condition;
                    whereFound = true;
                }
            }

            // if there are no cells with "where" conditions, add condition to new column
            if (!whereFound)
            {
                whereListItem.ConditionStrings[columnList.GetMaxConditionCount()] = condition;
            }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            var queryBuilder1 = QueryBuilderStore.Get("QueryModification");

            // get the active SELECT

            var usq = queryBuilder1.ActiveUnionSubQuery;

            #region actualize stored references (if query is modified in GUI)
            #region actualize datasource references
            // if user removed previously added datasources then clear their references
            if (_customers != null && !IsTablePresentInQuery(usq, _customers))
            {
                // user removed this table in GUI
                _customers = null;
            }

            if (_orders != null && !IsTablePresentInQuery(usq, _orders))
            {
                // user removed this table in GUI
                _orders = null;
            }
            #endregion

            // clear CompanyName conditions
            if (_companyName != null)
            {
                // if user removed entire row OR cleared expression cell in GUI, clear the stored reference
                if (!IsQueryColumnListItemPresentInQuery(usq, _companyName))
                    _companyName = null;
            }

            // clear all condition cells for CompanyName row
            if (_companyName != null)
            {
                ClearConditionCells(usq, _companyName);
            }

            // clear OrderDate conditions
            if (_orderDate != null)
            {
                // if user removed entire row OR cleared expression cell in GUI, clear the stored reference
                if (!IsQueryColumnListItemPresentInQuery(usq, _orderDate))
                    _orderDate = null;
            }

            // clear all condition cells for OrderDate row
            if (_orderDate != null)
            {
                ClearConditionCells(usq, _orderDate);
            }
            #endregion

            #region process Customers table
            if (cbCustomers.Checked)
            {
                // if we have no previously added Customers table, try to find one already added by the user
                if (_customers == null)
                {
                    _customers = FindTableInQueryByName(usq, CustomersName);
                }

                // there is no Customers table in query, add it
                if (_customers == null)
                {
                    _customers = AddTable(usq, CustomersName, CustomersAlias);
                }

                #region process CompanyName condition
                if (cbCompanyName.Enabled && cbCompanyName.Checked && !String.IsNullOrEmpty(tbCompanyName.Text))
                {
                    // if we have no previously added grid row for this condition, add it
                    if (_companyName == null || _companyName.IsDisposing)
                    {
                        _companyName = usq.QueryColumnList.AddField(_customers, _companyNameFieldName.QualifiedName);
                        // do not append it to the select list, use this row for conditions only
                        _companyName.Selected = false;
                    }

                    // write condition from edit box to all needed grid cells
                    AddWhereCondition(usq.QueryColumnList, _companyName, tbCompanyName.Text);
                }
                else
                {
                    // remove previously added grid row
                    if (_companyName != null)
                    {
                        _companyName.Dispose();
                    }

                    _companyName = null;
                }
                #endregion
            }
            else
            {
                // remove previously added datasource
                if (_customers != null)
                {
                    _customers.Dispose();
                }

                _customers = null;
            }
            #endregion

            #region process Orders table
            if (cbOrders.Checked)
            {
                // if we have no previosly added Orders table, try to find one already added by the user
                if (_orders == null)
                {
                    _orders = FindTableInQueryByName(usq, OrdersName);
                }

                // there are no Orders table in query, add one
                if (_orders == null)
                {
                    _orders = AddTable(usq, OrdersName, OrdersAlias);
                }

                #region link between Orders and Customers
                // we added Orders table,
                // check if we have Customers table too,
                // and if there are no joins between them, create such join
                string joinFieldNameStr = _joinFieldName.QualifiedName;
                if (_customers != null &&
                    usq.FromClause.FindLink(_orders, joinFieldNameStr, _customers, joinFieldNameStr) == null &&
                    usq.FromClause.FindLink(_customers, joinFieldNameStr, _orders, joinFieldNameStr) == null)
                {
                    queryBuilder1.SQLQuery.AddLink(_customers, _joinFieldName, _orders, _joinFieldName);
                }
                #endregion

                #region process OrderDate condition
                if (cbOrderDate.Enabled && cbOrderDate.Checked && !String.IsNullOrEmpty(tbOrderDate.Text))
                {
                    // if we have no previously added grid row for this condition, add it
                    if (_orderDate == null)
                    {
                        _orderDate = usq.QueryColumnList.AddField(_orders, _orderDateFieldName.QualifiedName);
                        // do not append it to the select list, use this row for conditions only
                        _orderDate.Selected = false;
                    }

                    // write condition from edit box to all needed grid cells
                    AddWhereCondition(usq.QueryColumnList, _orderDate, tbOrderDate.Text);
                }
                else
                {
                    // remove prviously added grid row
                    if (_orderDate != null)
                    {
                        _orderDate.Dispose();
                    }

                    _orderDate = null;
                }
                #endregion
            }
            else
            {
                if (_orders != null)
                {
                    _orders.Dispose();
                    _orders = null;
                }
            }
            #endregion
        }

        private void CollectCriteriaItemsWithWhereCondition(QueryColumnList criteriaList, int columnIndex, IList<QueryColumnListItem> result)
        {
            result.Clear();

            foreach (var item in criteriaList)
            {
                if (item.ConditionType == ConditionType.Where &&
                    item.ConditionCount > columnIndex &&
                    item.GetASTCondition(columnIndex) != null)
                {
                    result.Add(item);
                }
            }
        }

        protected void btnQueryCustomers_Click(object sender, EventArgs e)
        {
            var queryBuilder1 = QueryBuilderStore.Get("QueryModification");
            queryBuilder1.SQL = "select * from Northwind.dbo.Customers c";
        }

        protected void btnQueryOrders_Click(object sender, EventArgs e)
        {
            var queryBuilder1 = QueryBuilderStore.Get("QueryModification");
            queryBuilder1.SQL = "select * from Northwind.dbo.Orders o";
        }

        protected void btnQueryCustomersOrders_Click(object sender, EventArgs e)
        {
            var queryBuilder1 = QueryBuilderStore.Get("QueryModification");
            queryBuilder1.SQL = "select * from Northwind.dbo.Customers c, Northwind.dbo.Orders o";
        }
    }
}