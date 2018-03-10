<%@ Page Title="Virtual Object And Fields Demo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VirtualObjectsAndFields.aspx.cs" Inherits="WebForms_Samples.Samples.VirtualObjectsAndFields" %>
<%@ Register TagPrefix="AQB" Namespace="ActiveQueryBuilder.Web.WebForms" Assembly="ActiveQueryBuilder.Web.WebForms" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
    <div class="col-md-12">
        <h1>Virtual Objects And Fields Demo</h1>
        <div class="block-flat">
            This sample demonstrates the creation and usage of virtual database objects and fields.
            Switching the tabs at the bottom you can see the query text with virtual objects or the real query where virtual objects 
            are expanded to theirs expressions for execution against a database server.
        </div>
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
                            <label class="ui-widget-header qb-widget-header" for="tree-tab">Database</label>
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
    <div class="col-md-12">
        <div class="ui-widget">
            <div class="ui-widget-header real-header">
                <span>Real query text with virtual objects expanded to their expressions</span>
            </div>
            <div class="ui-dialog-content ui-widget-content real-sql">
                <textarea></textarea>
            </div>
        </div>
    </div>
</div>

<style>
    .real-sql textarea {
        max-width: 100%;
        width: 100%;
        height: 250px;
    }

    .real-header {
        padding: 5px 10px;
    }
</style>

    <script>
        $(function () {
            AQB.Web.onQueryBuilderReady(function () {
                AQB.Web.Core.on(AQB.Web.Core.Events.UserDataReceived, onUserDataReceived);
            });

            function onUserDataReceived(data) {
                $('.real-sql textarea').eq(0).val(data.SQL);
            };
        });
    </script>
</asp:Content>
