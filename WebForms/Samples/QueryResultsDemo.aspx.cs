﻿using System;
using System.Web.UI;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Core.QueryTransformer;
using ActiveQueryBuilder.Web.Server;
using WebForms_Samples.Helpers;

namespace WebForms_Samples.Samples
{
    public partial class QueryResultsDemo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.Get("QueryResults");
            var qt = QueryTransformerStore.Get("QueryResults");

            if (qb == null)
                qb = CreateQueryBuilder();

            if (qt == null)
                qt = CreateQueryTransformer(qb.SQLQuery);

            QueryBuilderControl1.QueryBuilder = qb;
            ObjectTreeView1.QueryBuilder = qb;
            Canvas1.QueryBuilder = qb;
            Grid1.QueryBuilder = qb;
            SubQueryNavigationBar1.QueryBuilder = qb;
            SqlEditor1.QueryBuilder = qb;
            StatusBar1.QueryBuilder = qb;
            CriteriaBuilder1.QueryTransformer = qt;
        }


        /// <summary>
        /// Creates and initializes a new instance of the QueryBuilder object.
        /// </summary>
        /// <returns>Returns instance of the QueryBuilder object.</returns>
        private QueryBuilder CreateQueryBuilder()
        {
            // Create an instance of the QueryBuilder object
            var queryBuilder = QueryBuilderStore.Factory.SqLite("QueryResults");

            // Turn this property on to suppress parsing error messages when user types non-SELECT statements in the text editor.
            queryBuilder.BehaviorOptions.AllowSleepMode = false;
            
            // Bind Active Query Builder to a live database connection.
            queryBuilder.MetadataProvider = new SQLiteMetadataProvider
            {
                // Assign an instance of DBConnection object to the Connection property.
                Connection = DataBaseHelper.CreateSqLiteConnection("SqLiteDataBase")
            };

            // Assign the initial SQL query text the user sees on the _first_ page load
            queryBuilder.SQL = GetDefaultSql();

            return queryBuilder;
        }

        private string GetDefaultSql()
        {
            return @"Select customers.CustomerId,
                      customers.LastName,
                      customers.FirstName
                    From customers";
        }

        private QueryTransformer CreateQueryTransformer(SQLQuery query)
        {
            var qt = QueryTransformerStore.Create("QueryResults");

            qt.QueryProvider = query;
            qt.AlwaysExpandColumnsInQuery = true;

            return qt;
        }
    }
}