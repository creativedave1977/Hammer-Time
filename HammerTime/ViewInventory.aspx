<%@ Page Title="Inventory" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewInventory.aspx.cs" Inherits="HammerTime.ViewInventory" %>
<%@ Register Assembly="AjaxControlToolKit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadPlaceholder" runat="server">
    <script type="text/javascript">
        //ensure numbers are entered as input
        function IsNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        //cancel add hammer cleanup
        function CancelAddHammer() {
            //clear values
            document.getElementById('<%= txtHammerName.ClientID%>').value = "";
            document.getElementById('<%= txtHammerDesc.ClientID%>').value = "";
            document.getElementById('<%= txtHammerQty.ClientID%>').value = "";

            //hide message
            HideMessage();
        }
    </script>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="updateHammers">
        <ContentTemplate>
            <div class="jumbotron">
                <h1>Hammer Time Inventory</h1>
            <div id="divActions">
                <asp:Button ID="btnAddHammerDialog" runat="server" Text="Add New Hammer to Inventory List" CssClass="btn btn-primary btn-lg" />
            </div>
        </div>
        <div id="divHammers" runat="server">
            <p class="lead">
                <asp:GridView ID="gvHammers" runat="server" AutoGenerateColumns="False" CssClass="Grid" 
                    Width="80%" DataKeyNames="HammerID" OnRowCommand="gvHammers_RowCommand" 
                    OnRowDataBound="gvHammers_RowDataBound" OnRowDeleting="gvHammers_RowDeleting">
                    <Columns>
                        <asp:TemplateField HeaderText="Item Name" ItemStyle-Width="250px">
                            <ItemTemplate>
                                <asp:HiddenField ID="hidHammerID" runat="server" Value='<%# Eval("HammerID")%>' />
                                <asp:Label ID="lblHammerName" runat="server" Text='<%# Eval("HammerName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Item SKU">
                            <ItemTemplate>
                                <asp:Label ID="lblHammerDesc" runat="server" Text='<%# Eval("HammerDesc")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="On Hand">
                            <ItemTemplate>
                                <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Qty")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Button ID="btnIncrementInv" runat="server" Text="Add Inventory"  CommandName="Increment" />&nbsp;
                                <asp:Button ID="btnDecrementInv" runat="server" Text="Remove Inventory" CommandName="Decrement" />&nbsp;
                                <asp:Button ID="btnUpdateItem" runat="server" Text="Update Item" CommandName="Update" />&nbsp;
                                <asp:Button ID="btnDeleteItem" runat="server" Text="Delete Item" CommandName="Delete" 
                                    OnClientClick="return confirm('Are you sure you want to delete this item?\nThis cannot be undone.');" />
                                <%--Nested popups for actions--%>
                                <%--Decrement Hammer Inventory--%>
                                <act:ModalPopupExtender ID="mpeDecrement" runat="server"
                                    CancelControlID="ibtnCloseDecrement" TargetControlID="btnDecrementInv" PopupControlID="pnlDecrement" BackgroundCssClass="modal-background">
                                </act:ModalPopupExtender>
                                <asp:Panel ID="pnlDecrement" runat="server" style="display:none;width:400px;" CssClass="modal-popup">
                                    <div style="width:100%;text-align:right;">
                                        <asp:ImageButton ID="ibtnCloseDecrement" runat="server" ImageUrl="~/Content/Images/close-button.png" style="position:absolute;top:-20px;right:-20px;display:none;" ToolTip="Close Window" />
                                    </div>
                                    <div style="text-align:center;" class="dialog-title">
                                        <asp:Label ID="lblDecrementTitle" runat="server" Text='<%# "Decrement Quantity of : " + Eval("HammerName")%>'></asp:Label>
                                    </div>
                                    <div>&nbsp;</div>
                                    <div>&nbsp;</div>
                                    <div class="modal-div">
                                        <span>Enter Decrement Quantity:</span>&nbsp;
                                        <asp:TextBox ID="txtDecrementQty" runat="server" MaxLength="50" Width="50px" onkeypress="return IsNumberKey(event);"></asp:TextBox>
                                    </div>
                                    <div>&nbsp;</div>
                                    <div class="modal-div">
                                        <asp:Button ID="btnDecrement" runat="server" Text="Update" CommandName="DoDecrement" />&nbsp;
                                        <asp:Button ID="btnCancelDecrement" runat="server" Text="Cancel" CommandName="DoCancelDecrement" />
                                    </div> 
                                </asp:Panel>
                                <%--End Decrement Hammer Inventory--%>
                                <%--Increment Hammer Inventory--%>
                                <act:ModalPopupExtender ID="mpeIncrement" runat="server"
                                    CancelControlID="ibtnCloseIncrement" TargetControlID="btnIncrementInv" PopupControlID="pnlIncrement" BackgroundCssClass="modal-background">
                                </act:ModalPopupExtender>
                                <asp:Panel ID="pnlIncrement" runat="server" style="display:none;width:400px;" CssClass="modal-popup">
                                    <div style="width:100%;text-align:right;">
                                        <asp:ImageButton ID="ibtnCloseIncrement" runat="server" ImageUrl="~/Content/Images/close-button.png" style="position:absolute;top:-20px;right:-20px;display:none;" ToolTip="Close Window" />
                                    </div>
                                    <div style="text-align:center;" class="dialog-title">
                                        <asp:Label ID="lblIncrementTitle" runat="server" Text='<%# "Increment Quantity of : " + Eval("HammerName")%>'></asp:Label>
                                    </div>
                                    <div>&nbsp;</div>
                                    <div>&nbsp;</div>
                                    <div class="modal-div">
                                        <span>Enter Increment Quantity:</span>&nbsp;
                                        <asp:TextBox ID="txtIncrementQty" runat="server" MaxLength="50" Width="50px" onkeypress="return IsNumberKey(event);"></asp:TextBox>
                                    </div>
                                    <div>&nbsp;</div>
                                    <div class="modal-div">
                                        <asp:Button ID="btnIncrement" runat="server" Text="Update" CommandName="DoIncrement" />&nbsp;
                                        <asp:Button ID="btnCancelIncrement" runat="server" Text="Cancel" CommandName="DoCancelIncrement" />
                                    </div> 
                                </asp:Panel>
                                <%--End Increment Hammer Inventory--%>
                                <%--Update Hammer--%>
                                <act:ModalPopupExtender ID="mpeUpdateHammer" runat="server"
                                    CancelControlID="ibtnCloseUpdateHammer" TargetControlID="btnUpdateItem" PopupControlID="pnlUpdateHammer" BackgroundCssClass="modal-background">
                                </act:ModalPopupExtender>
                                <asp:Panel ID="pnlUpdateHammer" runat="server" style="display:none;width:400px;" CssClass="modal-popup">
                                    <div style="width:100%;text-align:right;">
                                        <asp:ImageButton ID="ibtnCloseUpdateHammer" runat="server" ImageUrl="~/Content/Images/close-button.png" style="position:absolute;top:-20px;right:-20px;display:none;" ToolTip="Close Window" />
                                    </div>
                                    <div style="text-align:center;" class="dialog-title">
                                        Update Item
                                    </div>
                                    <div id="divUpdateHammerError" runat="server" class="modal-error-message" style="text-align:center;">&nbsp;</div>
                                    <div>&nbsp;</div>
                                    <div>&nbsp;</div>
                                    <div class="modal-div">
                                        <span>Hammer Name:</span>&nbsp;
                                        <asp:TextBox ID="txtUpdateHammerName" runat="server" MaxLength="50" Width="175px" Text='<%# Eval("HammerName")%>'></asp:TextBox>
                                    </div>
                                     <div class="modal-div">
                                        <span>SKU:</span>&nbsp;
                                        <asp:TextBox ID="txtUpdateHammerDesc" runat="server" MaxLength="50" Width="125px" Text='<%# Eval("HammerDesc")%>'></asp:TextBox>
                                    </div>
                                    <div>&nbsp;</div>
                                    <div class="modal-div">
                                        <asp:Button ID="btnUpdateHammerModal" runat="server" Text="Update Item" CommandName="DoUpdate" />&nbsp;
                                        <asp:Button ID="btnCancelUpdateHammerModal" runat="server" Text="Cancel" CommandName="DoCancelUpdate" />
                                    </div> 
                                </asp:Panel>
                                <%--End Update Hammer--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </p>
        </div>
        <div id="divNoHammers" runat="server" class="no-inventory">
            <p>There are no hammers in the Hammer Time inventory system at this time.</p>
        </div>
        <%--Popups for actions--%>
        <%--Add Hammer--%>
        <act:ModalPopupExtender ID="mpeAddHammer" runat="server"
            CancelControlID="ibtnCloseAddHammer" TargetControlID="btnAddHammerDialog" PopupControlID="pnlAddHammer" BackgroundCssClass="modal-background">
        </act:ModalPopupExtender>
        <asp:Panel ID="pnlAddHammer" runat="server" style="display:none;width:400px;" CssClass="modal-popup">
            <div style="width:100%;text-align:right;">
                <asp:ImageButton ID="ibtnCloseAddHammer" runat="server" ImageUrl="~/Content/Images/close-button.png" style="position:absolute;top:-20px;right:-20px;display:none;" ToolTip="Close Window" />
            </div>
            <div style="text-align:center;" class="dialog-title">
                Add New Hammer to Inventory List
            </div>
            <div id="divAddHammerError" runat="server" class="modal-error-message" style="text-align:center;">&nbsp;</div>
            <div>&nbsp;</div>
            <div>&nbsp;</div>
            <div class="modal-div">
                <span>Hammer Name:</span>&nbsp;
                <asp:TextBox ID="txtHammerName" runat="server" MaxLength="50" Width="175px"></asp:TextBox>
            </div>
             <div class="modal-div">
                <span>SKU:</span>&nbsp;
                <asp:TextBox ID="txtHammerDesc" runat="server" MaxLength="50" Width="125px"></asp:TextBox>
            </div>
             <div class="modal-div">
                <span>Quantity on hand:</span>&nbsp;
                <asp:TextBox ID="txtHammerQty" runat="server" MaxLength="50" Width="50px" onkeypress="return IsNumberKey(event);"></asp:TextBox>
                <br /><span>*if left blank, zero will be enterered</span>
            </div>
            <div>&nbsp;</div>
            <div class="modal-div">
                <asp:Button ID="btnAddHammer" runat="server" Text="Add Item" OnClick="btnAddHammer_Click" />&nbsp;
                <asp:Button ID="btnCancelAddHammer" runat="server" Text="Cancel" onclientclick="CancelAddHammer();" />
            </div> 
        </asp:Panel>
        <%--End Add Hammer--%>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
