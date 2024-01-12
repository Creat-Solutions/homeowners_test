<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GenerateStep2.ascx.cs" Inherits="UserControls_GenerateStep2" %>
<script type="text/javascript">
    function ddlWall_Validate(obj, args) {
        var ddlRoofType = $get('<%= ddlRoof.ClientID %>');
        var roofType = parseInt(ddlRoofType.options[ddlRoofType.selectedIndex].value);
        if (roofType == <%= ApplicationConstants.CONCRETE_ROOFTYPE %>) {
            args.IsValid = parseInt(args.Value) == <%= ApplicationConstants.CONCRETE_WALLTYPE %>;
        } else {
            args.IsValid = true;
        }
    }
    
    var isNav;
    function checkKeyInt(e) {
	    var keyChar;

	    if (isNav == 0)	{
		    if(e.which < 48 || e.which > 57) e.RETURNVALUE = false;
	    } else {
		    if(e.keyCode <48 || e.keyCode > 57) e.returnValue = false;
	    }
	}
    
    function txtStories_Validate(obj, args) {
        var ddlType = $get('<%= ddlType.ClientID %>');
        var cmpStoriesText = $get('<%= cmpStoriesMore.ClientID %>');
        var numberStories = parseInt(args.Value);
        var typeOfProperty = parseInt(ddlType.options[ddlType.selectedIndex].value);
        var cultureName = '<%= System.Globalization.CultureInfo.CurrentCulture.Name%>'; 
        var error1 = '';
        var error2 = '';
        if(cultureName == 'es-MX'){
            error1 = 'Dato Inv&aacute;lido. Debe ser mayor o igual a 1'
            error2 = 'Dato Inv&aacute;lido. Debe ser mayor o igual a 2'
        }
        else{
            error1 = 'Invalid data. Must be equal or greater than 1'
            error2 = 'Invalid data. Must be equal or greater than 2'
        }
        
        if (typeOfProperty == <%= ApplicationConstants.CONDO_TYPE %>) {
            args.IsValid = numberStories >= 2;
            cmpStoriesText.innerHTML = error2;
        } else {
            args.IsValid = numberStories >= 1;
            cmpStoriesText.innerHTML = error1;
        }
    }
    
    function ddlRoof_Change() {
        var valWallType = $get('<%= valWall.ClientID %>');
        ValidatorValidate(valWallType);
    }   
</script>

<table width="100%">
    <tr>
        <td class="quoteStep borderStep" valign="top" style="width: 35%;">
            <table class="quoteAppData quoteAppDataAmple">
                <tr>
                    <td>
                        <div class="step2 row">
                            <asp:Label ID="lblType" AssociatedControlID="ddlType" Text="<%$ Resources:, strType %>" runat="server" />
                            <asp:DropDownList ID="ddlType" CssClass="step2" OnDataBound="ddlDataBound" DataSourceID="odsType" DataTextField="description" DataValueField="type" runat="server" />
                            <asp:RequiredFieldValidator ID="reqType" ControlToValidate="ddlType" Text="<%$ Resources:, strRequired %>" Display="Dynamic" InitialValue="0" runat="server" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="step2 row">
                            <asp:Label ID="lblRoof" AssociatedControlID="ddlRoof" Text="<%$ Resources:, strRoof %>" runat="server" />
                            <asp:DropDownList ID="ddlRoof" CssClass="step2" OnDataBound="ddlDataBound" DataSourceID="odsRoof" onchange="ddlRoof_Change();" DataTextField="description" DataValueField="type" runat="server" />
                            <asp:RequiredFieldValidator ID="reqRoof" ControlToValidate="ddlRoof" Text="<%$ Resources:, strRequired %>" Display="Dynamic" InitialValue="0" runat="server" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="step2 row">
                            <asp:Label ID="lblWall" AssociatedControlID="ddlWall" Text="<%$ Resources:, strWall %>" runat="server" />
                            <asp:DropDownList ID="ddlWall" CssClass="step2" OnDataBound="ddlDataBound" DataSourceID="odsWall" DataTextField="description" DataValueField="type" runat="server" />
                            <asp:RequiredFieldValidator ID="reqWall" ControlToValidate="ddlWall" Text="<%$ Resources:, strRequired %>" Display="Dynamic" InitialValue="0" runat="server" />
                            <asp:CustomValidator ID="valWall" ControlToValidate="ddlWall" Text="Invalid wall type" Display="Dynamic" ClientValidationFunction="ddlWall_Validate" OnServerValidate="ddlWall_Validate" runat="server" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="step2 row">
                            <asp:Label ID="lblStories" AssociatedControlID="txtStories" Text="<%$ Resources:, strStories %>" runat="server" />
                            <asp:TextBox ID="txtStories" CssClass="step2" runat="server" onkeypress="checkKeyInt(event)" />
                            <asp:RequiredFieldValidator ID="reqStories" ControlToValidate="txtStories" Text="<%$ Resources:, strRequired %>" Display="Dynamic" runat="server" />
                            <asp:CustomValidator ID="cmpStoriesMore" ControlToValidate="txtStories" Text="<%$ Resources:, strInvalidData %>" Display="Dynamic" ClientValidationFunction="txtStories_Validate" OnServerValidate="txtStories_Validate" runat="server" />
                        </div>
                        <div class="step2 row">
                            <asp:Label ID="lblUnderground" AssociatedControlID="txtUnderground" Text="<%$ Resources:, strUnderground %>" runat="server" />
                            <asp:TextBox ID="txtUnderground" CssClass="step2" runat="server" onkeypress="checkKeyInt(event)" />
                            <asp:CompareValidator ID="cmpUndergroundMore" ControlToValidate="txtUnderground" Text="<%$ Resources:, strInvalidData %>" Display="Dynamic" Operator="GreaterThanEqual" ValueToCompare="0" runat="server" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="step2nofloat row">
                            <asp:Label ID="lblShutters" AssociatedControlID="rblShutters" Text="<%$ Resources:, strShutters %>" runat="server" />
                            <asp:RequiredFieldValidator ID="reqShutters" Text="<%$ Resources:, strRequired %>" Display="Dynamic" ControlToValidate="rblShutters" runat="server" />
                            <asp:RadioButtonList CssClass="nofloat" ID="rblShutters" RepeatDirection="Horizontal" runat="server">
                                <asp:ListItem Text="<%$ Resources:, strYes %>" Value="1" />
                                <asp:ListItem Text="<%$ Resources:, strNo %>" Value="0" />
                            </asp:RadioButtonList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="step2nofloat row">
                            <asp:Label ID="lblAdjoining" AssociatedControlID="rblAdjoining" Text="<%$ Resources:, strAdjoining %>" runat="server" />
                            <asp:RequiredFieldValidator ID="reqAdjoining" Text="<%$ Resources:, strRequired %>" Display="Dynamic" ControlToValidate="rblAdjoining" runat="server" />
                            <asp:RadioButtonList CssClass="nofloat" ID="rblAdjoining" RepeatDirection="Horizontal" runat="server">
                                <asp:ListItem Text="<%$ Resources:, strYes %>" Value="1" />
                                <asp:ListItem Text="<%$ Resources:, strNo %>" Value="0" />
                            </asp:RadioButtonList>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
        <td class="quoteStep" valign="top">
            <table class="quoteAppData">
                <tr>
                    <td>
                        <div class="step2nofloat row">
                            <asp:Label ID="lblDistanceSea" AssociatedControlID="rblDistanceSea" Text="<%$ Resources:, strDistanceSea %>" runat="server" />
                            &nbsp;<asp:RequiredFieldValidator ID="reqDistance" ControlToValidate="rblDistanceSea" Text="<%$ Resources:, strRequired %>" Display="Dynamic" runat="server" />
                            <asp:RadioButtonList ID="rblDistanceSea" DataSourceID="odsDistanceSea" CssClass="nofloat"
                                DataValueField="distance" DataTextField="description" runat="server" 
                                 />
                        </div>
                    </td>
                </tr>
                <tr></tr>
                <tr>
                    <td>
                        <div class="step2nofloat row">
                            <asp:Label ID="lblBurglary" Text="<%$ Resources:, strBurglary %>" runat="server" /><br />
                            <table>
                            <tbody>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chk24Guards" CssClass="nofloat" Text="<%$ Resources:, str24Guards %>" runat="server" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkCentralAlarm" CssClass="nofloat" Text="<%$ Resources:, strCentralAlarm %>" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkTVCircuit" CssClass="nofloat" Text="<%$ Resources:, strTVCircuit %>" runat="server" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkTVGuard" CssClass="nofloat" Text="<%$ Resources:, strTVGuard %>" runat="server" />
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkLocalAlarm" CssClass="nofloat" Text="<%$ Resources:, strLocalAlarm %>" runat="server" />
                                    </td>
                                    <td></td>
                                </tr>
                            </tbody>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr></tr>
            </table>

        </td>
    </tr>
</table>

<asp:ObjectDataSource ID="odsType" TypeName="typeOfProperty" SelectMethod="SelectByLanguage" runat="server" />
<asp:ObjectDataSource ID="odsRoof" TypeName="roofType" SelectMethod="SelectByLanguage" runat="server" />
<asp:ObjectDataSource ID="odsWall" TypeName="wallType" SelectMethod="SelectByLanguage" runat="server" />
<asp:ObjectDataSource ID="odsDistanceSea" TypeName="distanceFromSea" SelectMethod="SelectForRBLByLanguage" runat="server" />