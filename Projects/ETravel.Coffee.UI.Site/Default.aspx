<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
	CodeBehind="Default.aspx.cs" Inherits="ETravel.Coffee.UI.Site._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
	<div class="col-md-12">
		<h2>Orders</h2>
		<div class="orders-list">
			<div class="loader"><i class="fa fa-cog fa-spin large"></i> Loading...</div>
		</div>
	</div>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="scripts">
	<script type="text/javascript" src="Scripts/orders.js"></script>
</asp:Content>