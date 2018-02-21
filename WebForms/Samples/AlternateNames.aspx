<%@ Page Title="Alternate Names Demo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AlternateNames.aspx.cs" Inherits="WebForms_Samples.Samples.AlternateNames" %>
<%@ Register TagPrefix="AQB" Namespace="ActiveQueryBuilder.Web.WebForms" Assembly="ActiveQueryBuilder.Web.WebForms" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <h1>Alternate Names Demo</h1>
            <p>Active Query Builder lets substitute unreadable names for user-friendly aliases.</p>
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
                                    <AQB:ObjectTreeView ID="ObjectTreeView1" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="qb-ui-layout__right">
                        <AQB:SubQueryNavigationBar ID="SubQueryNavigationBar1" runat="server" />
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
                <div class="ui-widget-header alternate-header">
                    <span>Query text with alternate object names</span>
                </div>
                <div class="ui-dialog-content ui-widget-content alternate-sql">
                    <textarea></textarea>
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <div class="ui-widget">
                <div class="ui-widget-header alternate-header">
                    <span>Query text without alternate object names</span>
                </div>
                <div class="ui-dialog-content ui-widget-content alternate-sql">
                    <textarea></textarea>
                </div>
            </div>
        </div>
    </div>

    <style>
        .alternate-sql textarea {
            max-width: 100%;
            width: 100%;
            height: 250px;
        }

        .alternate-header {
            padding: 5px 10px;
        }
    </style>

    <script>
        $(function () {
            AQB.Web.onApplicationReady(function () {
                AQB.Web.Core.on(AQB.Web.Core.Events.UserDataReceived, onUserDataReceived);
            });

            function onUserDataReceived(data) {
                $('.alternate-sql textarea').eq(0).val(data.AlternateSQL);
                $('.alternate-sql textarea').eq(1).val(data.SQL);
            };
        });
    </script>

</asp:Content>
