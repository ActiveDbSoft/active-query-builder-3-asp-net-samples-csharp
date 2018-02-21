<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebForms_Samples._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
        <div class="jumbotron">
            <h1>Active Query Builder</h1>
            <p class="lead">Active Query Builder is a visual SQL query builder component with powerful API to parse, analyze, and modify SQL queries.</p>
            <p><a href="https://www.activequerybuilder.com/" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
        </div>

        <div class="row">
            <div class="col-md-12">
            <h2 class="section-title">Basic Demos</h2>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <h3>OLE DB Connection</h3>
                <p>Loading metadata from live database.</p>
                <p>
                    <a class="btn btn-default" href="Samples/SimpleOLEDBDemo.aspx">Learn more &raquo;</a>
                </p>
            </div>
            <div class="col-md-4">
                <h3>Offline Demo</h3>
                <p>Loading metadata from the pre-generated XML file.</p>
                <p>
                    <a class="btn btn-default" href="Samples/SimpleOfflineDemo.aspx">Learn more &raquo;</a>
                </p>
            </div>
            <div class="col-md-4">
                <h3>Display Query Results</h3>
                <p>Displaying SQL query results and modifying SQL queries while browsing the data.</p>
                <p>
                    <a class="btn btn-default" href="Samples/QueryResultsDemo.aspx">Learn more &raquo;</a>
                </p>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <h2 class="section-title">Designing a user-friendly query building environment</h2>
                <p>Defining friendly names for database objects and fields, adding meaningful objects, customizing database schema tree.</p>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <h3>Alternate Names</h3>
                <p>Active Query Builder lets substitute unreadable names for user-friendly aliases.</p>
                <p>
                    <a class="btn btn-default" href="Samples/AlternateNames.aspx">Learn more &raquo;</a>
                </p>
            </div>
            <div class="col-md-4">
                <h3>Virtual Objects And Fields</h3>
                <p>Add meaningful objects acting like views and friendly calculated or lookup fields to your database schema view.</p>
                <p>
                    <a class="btn btn-default" href="Samples/VirtualObjectsAndFields.aspx">Learn more &raquo;</a>
                </p>
            </div>
            <div class="col-md-4">
                <h3>Custom Metadata Structure</h3>
                <p>Customize your Database Schema View the way you like: group objects by subject area, define folders with favorite objects, etc.</p>
                <p>
                    <a class="btn btn-default" href="Samples/MetadataStructure.aspx">Learn more &raquo;</a>
                </p>
            </div>
            <div class="col-md-4">
                <h3>Toggling Usage of Alternate Names</h3>
                <p>User can see and edit SQL text with both alternate and real object names.</p>
                <p>
                    <a class="btn btn-default" href="Samples/ToggleUseAltNames.aspx">Learn more &raquo;</a>
                </p>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <h2 class="section-title">User-defined Queries and Fields</h2>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <h3>User-defined Queries</h3>
                <p>Users can save their queries and use them as data sources in subsequent queries.</p>
                <p>
                    <a class="btn btn-default" href="Samples/SaveAndLoadUserQueries.aspx">Learn more &raquo;</a>
                </p>
            </div>
            <div class="col-md-4">
                <h3>User-defined Fields</h3>
                <p>Let advanced users create own calculated fields.</p>
                <p>
                    <a class="btn btn-default" href="Samples/UserDefinedFields.aspx">Learn more &raquo;</a>
                </p>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <h2 class="section-title">User Interface Customizations</h2>
            </div>
        </div>

        <div class="row">
            <div class="col-md-4">
                <h3>Custom Expression Editor</h3>
                <p>Define own editor to deal with complex SQL expressions.</p>
                <p>
                    <a class="btn btn-default" href="Samples/CustomExpressionEditor.aspx">Learn more &raquo;</a>
                </p>
            </div>
            <div class="col-md-4">
                <h3>SQL Syntax Highlighting</h3>
                <p>Highlight SQL syntax using a third-party SQL text editor.</p>
                <p>
                    <a class="btn btn-default" href="Samples/SQLSyntaxHighlighting.aspx">Learn more &raquo;</a>
                </p>
            </div>
            <div class="col-md-4">
                <h3>jQueryUI Theming</h3>
                <p>Apply any jQueryUI skin to Active Query Builder UI.</p>
                <p>
                    <a class="btn btn-default" href="Samples/JQueryUITheming.aspx">Learn more &raquo;</a>
                </p>
            </div>
            <div class="col-md-4">
                <h3>Work without the Design Pane</h3>
                <p>Build queries without the design pane by dragging fields right to the Query Columns List.</p>
                <p>
                    <a class="btn btn-default" href="Samples/NoDesignArea.aspx">Learn more &raquo;</a>
                </p>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <h2 class="section-title">Metadata Loading</h2>
            </div>
        </div>

        <div class="row">
            <div class="col-md-4">
                <h3>Load Metadata Demo</h3>
                <p>Four ways to fill the Metadata Container programatically.</p>
                <p>
                    <a class="btn btn-default" href="Samples/LoadMetadataDemo.aspx">Learn more &raquo;</a>
                </p>
            </div>
            <div class="col-md-4">
                <h3>Switch Database Connections</h3>
                <p>Switching between different database connections at run time.</p>
                <p>
                    <a class="btn btn-default" href="Samples/ChangeConnection.aspx">Learn more &raquo;</a>
                </p>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <h2 class="section-title">SQL Query Analysis and Modification</h2>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <h3>Query Analysis</h3>
                <p>Explore the internal query object model, get summary information about the query.</p>
                <p>
                    <a class="btn btn-default" href="Samples/QueryAnalysis.aspx">Learn more &raquo;</a>
                </p>
            </div>
            <div class="col-md-4">
                <h3>Query Modification</h3>
                <p>Modify SQL queries programmatically.</p>
                <p>
                    <a class="btn btn-default" href="Samples/QueryModificationDemo.aspx">Learn more &raquo;</a>
                </p>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <h2 class="section-title">Advanced Programming Tasks</h2>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <h3>Handle Query Building Events</h3>
                <p>Performing specific actions in the process of building a SQL query.</p>
                <p>
                    <a class="btn btn-default" href="Samples/ClientEventHandle.aspx">Learn more &raquo;</a>
                </p>
            </div>
            <div class="col-md-4">
                <h3>Handle User-defined Queries Events</h3>
                <p>Performing specific actions in the process of working with user-defined queries.</p>
                <p>
                    <a class="btn btn-default" href="Samples/ClientEventHandleUQ.aspx">Learn more &raquo;</a>
                </p>
            </div>
        </div>
    </div>
</asp:Content>
