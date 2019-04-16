<%@ Page Title="Load Metadata Demo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LoadMetadataDemo.aspx.cs" Inherits="WebForms_Samples.Samples.LoadMetadataDemo" %>

<%@ Register TagPrefix="AQB" Namespace="ActiveQueryBuilder.Web.WebForms" Assembly="ActiveQueryBuilder.Web.WebForms" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <h1>Load Metadata Demo</h1>
            <p>Four ways to fill the Metadata Container programatically.</p>
        </div>
        <div class="col-md-12">
            <div id="main-tabs" class="block-flat">
                <ul>
                    <li><a href="#way1">Direct filling of MetadataContainer</a></li>
                    <li><a href="#way2">On-demand filling using ItemMetadataLoading event</a></li>
                    <li><a href="#way3">Using the ExecSQL event</a></li>
                    <li><a href="#way4">Fill from DataSet</a></li>
                </ul>
                <div id="way1">
                    <div>This method demonstrates the direct access to the internal metadata objects collection (MetadataContainer).</div>
                    <ASP:Button runat="server" OnClick="Way1" Text="Load Metadata" />
                </div>
                <div id="way2">
                    <div>This method demonstrates manual filling of metadata structure using MetadataContainer.ItemMetadataLoading event.</div>
                    <ASP:Button runat="server" OnClick="Way2" Text="Load Metadata" />
                </div>
                <div id="way3">
                    <div>
                        This method demonstrates loading of metadata through .NET data providers unsupported by our QueryBuilder component. If such data provider is able to execute SQL queries, you can use our EventMetadataProvider with handling it's ExecSQL event. In this event the EventMetadataProvider will provide you SQL queries it use for the metadata retrieval. You have to execute a query and return resulting data reader object.
                    Note: In this sample we are using GenericSyntaxProvider that tries to detect the the server type automatically. In some conditions it's unable to detect the server type and it also has limited syntax parsing abilities. For this reason you have to use specific syntax providers in your application, e.g. MySQLSyntaxProver, OracleSyntaxProvider, etc.
                    </div>
                    <ASP:Button runat="server" OnClick="Way3" Text="Load Metadata" />
                </div>
                <div id="way4">
                    <div>This method demonstrates manual filling of metadata structure from stored DataSet.</div>
                    <ASP:Button runat="server" OnClick="Way4" Text="Load Metadata" />
                </div>
            </div>
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
    </div>

    <script src="https://code.jquery.com/ui/1.12.0/jquery-ui.min.js"></script>

    <script>
        $(function () {
            $('#main-tabs').tabs({ active: <%= ActiveTabs %> });
        });
    </script>

</asp:Content>
