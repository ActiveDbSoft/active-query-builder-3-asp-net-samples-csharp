<%@ Page Title="Client Event Handle Demo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientEventHandle.aspx.cs" Inherits="WebForms_Samples.Samples.ClientEventHandle" %>
<%@ Register TagPrefix="AQB" Namespace="ActiveQueryBuilder.Web.WebForms" Assembly="ActiveQueryBuilder.Web.WebForms" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
    <div class="col-md-12">
        <h1>Handle Query Building Events</h1>
        <p>Performing specific actions in the process of building a SQL query.</p>
    </div>
    <div class="col-md-12">
        <!--Turn the UseDefaultTheme to False for not using the default theme. You will have to load the JQueryUI library then. -->
        <AQB:QueryBuilderControl ID="QueryBuilderControl1" runat="server" UseDefaultTheme="false" />
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
            qb.CanvasComponent.on(AQB.Web.QueryBuilder.CanvasComponent.Events.CanvasOnAddTable, onAddTableToCanvas);

            AQB.Web.Core.on(AQB.Web.Core.Events.DataSending, beforeDataExchange);
            AQB.Web.Core.on(AQB.Web.Core.Events.DataReceived, afterDataExchange);
        });

        function onAddTableToCanvas() {
            $.jGrowl("Add table to canvas", { header: 'Canvas event' });
        }

        function beforeDataExchange() {
            $.jGrowl("Before data exchange", { header: 'Core event' });
        }

        function afterDataExchange() {
            $.jGrowl("After data exchange", { header: 'Core event' });
        }
    </script>

</asp:Content>
