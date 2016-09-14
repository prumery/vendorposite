<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VendorSite.Default" MasterPageFile="~/POMaster.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="server">

    <telerik:RadScriptManager runat="server" ID="rsm"></telerik:RadScriptManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default">
</telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" DefaultLoadingPanelID="RadAjaxLoadingPanel1" runat="server">
    </telerik:RadAjaxManager>
 
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="200px" Width="300px"  LoadingPanelID="RadAjaxLoadingPanel1">
        
         <telerik:RadNotification ID="RadNotification1" runat="server" EnableRoundedCorners="True" EnableShadow="True" Height="100px" Position="Center"  Width="300px">
                    </telerik:RadNotification>

          <telerik:RadGrid ID="RadGrid2" runat="server" AutoGenerateColumns="false" Width="191%" CellSpacing="0" GridLines="None" OnNeedDataSource="RadGrid1_NeedDataSource"
              Skin="WebBlue" Height="90px"  >
            <MasterTableView>
                <NoRecordsTemplate>
                    <br />
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text=""></asp:Label>
                    <br />
                    <br />
                </NoRecordsTemplate>
                <Columns>
                    <telerik:GridBoundColumn AllowFiltering="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="X-Large" DataField="Vendor_ID" FilterControlAltText="Filter Vendor_ID column" HeaderText="Vendor ID" SortExpression="Vendor_ID" UniqueName="Vendor_ID">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                          <ItemStyle Font-Size="X-Large" HorizontalAlign="Left" VerticalAlign="Top" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn AllowFiltering="false" HeaderStyle-Font-Size="X-Large" ItemStyle-HorizontalAlign="Left" DataField="Vendor_Name" FilterControlAltText="Filter Vendor_Name column" HeaderText="Vendor Name" SortExpression="Vendor_Name" UniqueName="Vendor_Name">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                          <ItemStyle Font-Size="X-Large" HorizontalAlign="Left" VerticalAlign="Top" />
                    </telerik:GridBoundColumn>                    
                </Columns>
            </MasterTableView>
</telerik:RadGrid>
         <br />

        <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="False" Width="1431px" OnNeedDataSource="RadGrid1_NeedDataSource"
            OnItemCommand="RadGrid1_ItemCommand" OnItemDataBound="RadGrid1_ItemDataBound" Skin="WebBlue" OnItemCreated="RadGrid1_ItemCreated" Height="129px" >
            <MasterTableView>
                <NoRecordsTemplate>
                    <br />
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="There are no open PO's for you at this time."></asp:Label>
                    <br />
                    <br />
                </NoRecordsTemplate>
                <Columns>
                    <telerik:GridBoundColumn AllowFiltering="false" DataField="PO_Num" FilterControlAltText="Filter PO_Num column" HeaderStyle-Font-Size="X-Large" HeaderText="PO #" SortExpression="PO_Num" HeaderStyle-Width="10%" UniqueName="PO_Num">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                        <ItemStyle Font-Size="X-Large" HorizontalAlign="left" VerticalAlign="Top" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn AllowFiltering="false" DataField="PO_SentDT" FilterControlAltText="Filter PO_SentDT column" HeaderText="Sent Date" HeaderStyle-Font-Size="X-Large" HeaderStyle-Width="20%"  SortExpression="PO_SentDT" UniqueName="PO_SentDT">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                          <ItemStyle Font-Size="X-Large" HorizontalAlign="left" VerticalAlign="Top" />
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="Deliver To" HeaderStyle-Font-Size="X-Large" HeaderStyle-Width="400px">
                        <ItemTemplate>
                            <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("Address")  %>' ></asp:Label> 
                            <br> </br>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Address2")  %>' ></asp:Label>
                            <br></br>
                            <asp:Label ID="Label3" runat="server" Text='<%# string.Format("{0},",Eval("City") ) %>' ></asp:Label> 
                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("State")  %>' ></asp:Label>
                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("Zipcode")  %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle Font-Size="x-Large" HorizontalAlign="left" VerticalAlign="Top" />
                    </telerik:GridTemplateColumn>                                  
                    <telerik:GridBoundColumn AllowFiltering="false" DataField="Vendor_ID" Display="false" FilterControlAltText="Filter Vendor_ID column" HeaderText="vendor id" SortExpression="Vendor_ID" UniqueName="Vendor_ID">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                          <ItemStyle Font-Size="X-Large" HorizontalAlign="Center" VerticalAlign="Top" />
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="ETA Date" UniqueName="EDD" HeaderStyle-Font-Size="X-Large" HeaderStyle-Width="150px">                        
                        <ItemTemplate>
                            <telerik:RadDatePicker ID="radVenConfirmDT" runat="server"   ToolTip="Enter your estimated delivery date"   DateInput-EmptyMessage="Enter ETA Date">                                
                            </telerik:RadDatePicker>
                        </ItemTemplate>
                          <ItemStyle Font-Size="X-Large" HorizontalAlign="Center" VerticalAlign="Top" />
                    </telerik:GridTemplateColumn>
                    <telerik:GridButtonColumn ButtonType="PushButton"  CommandName="ConfirmPO" ConfirmDialogType="RadWindow"  ConfirmText="Confirm PO?" DataTextField="PO_Num" HeaderStyle-Width="10%" Text="Confirm PO Receipt" UniqueName="ConfirmPO">
                        <HeaderStyle Width="110px" />
                          <ItemStyle Font-Size="X-Large" HorizontalAlign="Center" VerticalAlign="Top" />                        
                    </telerik:GridButtonColumn>
                     <telerik:GridBoundColumn AllowFiltering="false"  DataField="po_status" Display="false"   FilterControlAltText="Filter po_status column" HeaderStyle-Font-Size="X-Large" HeaderText="Status" SortExpression="po_status" UniqueName="po_status">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                        <ItemStyle Font-Size="X-Large" HorizontalAlign="left" VerticalAlign="Top" />
                    </telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>


</telerik:RadGrid>

    </telerik:RadAjaxPanel>
    
</asp:Content>