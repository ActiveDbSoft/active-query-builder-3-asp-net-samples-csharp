<%@ Page Title="Simple Offline Demo" Language="C#" AutoEventWireup="true" CodeBehind="MobileDemo.aspx.cs" Inherits="WebForms_Samples.Samples.MobileDemo" %>

<%@ Register TagPrefix="AQB" Namespace="ActiveQueryBuilder.Web.WebForms" Assembly="ActiveQueryBuilder.Web.WebForms" %>

<!DOCTYPE html>

<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title><%: Page.Title %></title>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />

    <link rel="stylesheet" type="text/css" href="https://code.jquery.com/ui/1.12.1/themes/black-tie/jquery-ui.css" />
    <script src="https://code.jquery.com/jquery-2.2.4.min.js"></script>

</head>
<body>
    <form runat="server">
        <AQB:QueryBuilderControl ID="QueryBuilderControl1" runat="server" UseDefaultTheme="False" />
        <div class="qb-ui-layout">
            <div class="qb-ui-layout__left">
                <div class="qb-ui-structure-tabs">
                    <div class="qb-ui-structure-tabs__tab">
                        <input type="radio" id="tree-tab" name="qb-tabs" checked />
                        <label class="ui-widget-header qb-widget-header" for="tree-tab">Database</label>
                        <div class="qb-ui-structure-tabs__content">
                            <AQB:ObjectTreeView ID="ObjectTreeView1" runat="server" DefaultExpandMetadataType="Server" ShowFields="True" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="qb-ui-layout__right">
                <div class="qb-ui-layout__right-top">
                    <AQB:SubQueryNavigationBar ID="SubQueryNavigationBar1" runat="server" />
                    <AQB:Canvas runat="server" ID="Canvas1" />
                    <AQB:StatusBar runat="server" ID="StatusBar1" />
                </div>
                <div class="qb-ui-layout__right-bottom">
                    <div class="qb-ui-structure-tabs">
                        <div class="qb-ui-structure-tabs__tab">
                            <input type="radio" id="qcl-tab" name="right-tabs" checked />
                            <label class="ui-widget-header qb-widget-header" for="qcl-tab">Column List</label>
                            <div class="qb-ui-structure-tabs__content">
                                <AQB:Grid runat="server" ID="Grid1" OrColumnCount="0" UseCustomExpressionBuilder="ExpressionColumn" />
                            </div>
                        </div>
                        <div class="qb-ui-structure-tabs__tab">
                            <input type="radio" id="sql-tab" name="right-tabs" />
                            <label class="ui-widget-header qb-widget-header" for="sql-tab">SQL Text</label>
                            <div class="qb-ui-structure-tabs__content">
                                <AQB:SqlEditor runat="server" ID="SqlEditor1" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <style>
        html,
        body,
        .qb-ui-layout {
            height: 100% !important;
            width: 100% !important;
            margin: 0;
        }

        .qb-ui-layout {
            opacity: 0;
            transition: opacity 2000ms linear;
        }

            .qb-ui-layout.fadeIn {
                opacity: 1;
            }

        .ui-qb-grid {
            bottom: auto;
        }

        .properties__label {
            vertical-align: middle;
        }

        @media screen and (max-height: 1024px) {
            form {
                position: fixed;
                height: 100%;
                width: 100%;
            }

            .editable-select-options {
                width: 300px !important;
            }
        }
    </style>

    <link rel="stylesheet" type="text/css" href="/Samples/layout.mobile.css" />
    <script src="/Samples/layout.mobile.js"></script>

    <script>
        document.addEventListener('DOMContentLoaded', function () {
            setTimeout(function () {
                document.getElementsByClassName('qb-ui-layout')[0].classList.add('fadeIn');
            });
        });
    </script>

</body>
</html>
