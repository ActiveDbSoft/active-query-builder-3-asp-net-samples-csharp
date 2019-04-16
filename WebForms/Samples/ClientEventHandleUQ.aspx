<%@ Page Title="Client Event Handle Demo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientEventHandleUQ.aspx.cs" Inherits="WebForms_Samples.Samples.ClientEventHandleUQ" %>

<%@ Register TagPrefix="AQB" Namespace="ActiveQueryBuilder.Web.WebForms" Assembly="ActiveQueryBuilder.Web.WebForms" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    //CUT:STD{{
    <div class="row">
        <div class="col-md-12">
            <h1>Handle User-defined Queries Events Demo</h1>
            <p>Performing specific actions in the process of working with user-defined queries.</p>
        </div>
        <div class="col-md-12">
            <AQB:QueryBuilderControl ID="QueryBuilderControl1" runat="server" />
            <div class="qb-ui-layout">
                <div class="qb-ui-layout__top">
                    <div class="qb-ui-layout__left">
                        <div class="qb-ui-structure-tabs">
                            <div class="qb-ui-structure-tabs__tab">
                                <input type="radio" id="tree-tab" name="qb-tabs" checked />
                                <label for="tree-tab">Database</label>
                                <div class="qb-ui-structure-tabs__content">
                                    <AQB:ObjectTreeView runat="server" ID="ObjectTreeView1" />
                                </div>
                            </div>
                            <div class="qb-ui-structure-tabs__tab">
                                <input type="radio" id="queries-tab" name="qb-tabs" />
                                <label for="queries-tab">Queries</label>
                                <div class="qb-ui-structure-tabs__content">
                                    <AQB:UserQueries runat="server" ID="UserQueries1" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="qb-ui-layout__right">
                        <AQB:SubQueryNavigationBar runat="server" ID="SubQueryNavigationBar1" />
                        <AQB:Canvas runat="server" ID="Canvas1" />
                        <AQB:StatusBar runat="server" ID="StatusBar1" />
                        <AQB:Grid runat="server" ID="Grid1" />
                    </div>
                </div>
                <div class="qb-ui-layout__bottom">
                    <AQB:SqlEditor runat="server" ID="SqlEditor1" />
                </div>
            </div>
        </div>
    </div>

    <link href="https://cdnjs.cloudflare.com/ajax/libs/jquery-jgrowl/1.4.6/jquery.jgrowl.min.css" type="text/css" rel="stylesheet" />

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-jgrowl/1.4.6/jquery.jgrowl.min.js"></script>
    <script>
        AQB.Web.onQueryBuilderReady(function (qb) {
            var uq = qb.UserQueriesComponent;

            uq.on(uq.Events.OnCreateFolder, onCreateFolder);
            uq.on(uq.Events.OnCreateUserQuery, onCreateUserQuery);
            uq.on(uq.Events.OnSaveUserQuery, onSaveUserQuery);
            uq.on(uq.Events.OnRemove, onRemove);
            uq.on(uq.Events.OnRename, onRename);
        });

        function onCreateFolder() {
            $.jGrowl('Query Folder has been created', { header: 'UserQueries Event' });
        }

        function onCreateUserQuery() {
            $.jGrowl('UserQuery has been created', { header: 'UserQueries Event' });
        }

        function onSaveUserQuery() {
            $.jGrowl('UserQuery has been saved', { header: 'UserQueries Event' });
        }

        function onRemove() {
            $.jGrowl('Item has been removed', { header: 'UserQueries Event' });
        }

        function onRename() {
            $.jGrowl("Item has been renamed", { header: 'UserQueries Event' });
        }
    </script>
    //}}CUT:STD
</asp:Content>
