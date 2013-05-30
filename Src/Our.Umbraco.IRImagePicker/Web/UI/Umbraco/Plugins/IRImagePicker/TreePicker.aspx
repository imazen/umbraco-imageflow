<%@ Page Language="c#" MasterPageFile="/umbraco/masterpages/umbracoDialog.Master"
    AutoEventWireup="True" Inherits="umbraco.dialogs.treePicker" %>

<%@ Register TagPrefix="umb" Namespace="ClientDependency.Core.Controls" Assembly="ClientDependency.Core" %>
<%@ Register TagPrefix="umb" Namespace="umbraco.uicontrols" Assembly="controls" %>
<%@ Register TagPrefix="umb" Namespace="umbraco.controls.Tree" Assembly="umbraco" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    
    <style type="text/css">
        .uc-treenode-noclick > a > div { color: #CC8888; }
        .tree.tree-umbraco li.uc-treenode-noclick > a:hover { text-decoration: none; }
    </style>

    <script type="text/javascript" language="javascript">

        function doHighlight(node) {
            var div = node.find('a').find("div:first");
            div.css("border", "1px solid #FBC2C4").css("background-color", "#FBE3E4").css("color", "#8a1f11");
            setTimeout(function () { div.attr("style", ""); }, 500);
        }

        function dialogHandler(id) {

            var $node = $("li[id='" + id + "']");

            if ($node.hasClass("uc-treenode-noclick")) {
                doHighlight($node);
                return "";
            }

            UmbClientMgr.closeModalWindow(id);
        }

    </script>

    <umb:CustomTreeControl runat="server" ID="DialogTree" App='<%#TreeParams.App %>' TreeType='<%#TreeParams.TreeType %>'
        IsDialog="true" ShowContextMenu="false" DialogMode="id" FunctionToCall="dialogHandler" NodeKey='<%#TreeParams.NodeKey %>' />
        
</asp:Content>
