<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="bank_add_details.ascx.cs"
    Inherits="CMS.Atlas.Web.UI.asp.global.controls.bank_add_details" %>
<%@ Register TagPrefix="cms1" Namespace="CMS.Atlas.Web.Controls.Validation" Assembly="CMS.Atlas.Web.Controls.Validation" %>
<%@ Register TagPrefix="cms" Namespace="CMS.Atlas.Web.Controls.WebForm" Assembly="CMS.Atlas.Web.Controls.WebForm" %>
<%@ Register TagPrefix="BankControl" TagName="PayThroughBankControl" Src="add_paythrough_bank.ascx" %>

<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
<link href="../../stylesheets/mainstyle.css" type="text/css" rel="stylesheet">
<script language="javascript1.2" src="/includes/mainscript.js" type="text/javascript"></script>
<script language="javascript1.2" src="/includes/atlas_global_functions.js" type="text/javascript"></script>

<script language ="javascript" type="text/javascript">
    function ValidateAddressLine(oSrc, args) {
        var LastName = document.getElementById("<%=bankNewAddrLine1.ClientID %>").value;

        if (LastName != "") {
            if (parseInt(LastName.length) < 3) {
                document.getElementById("<%=bankNewAddrLine1.ClientID %>").focus();
                args.IsValid = false
            }
        }
        else { args.IsValid = true }
    }

   
    function ValidateFederalID(oSrc, args) {
        var accountclass = document.getElementById("<%=accountClassDDL.ClientID %>");
        var federalTaxID = document.getElementById("<%=federalTaxNumTxt.ClientID %>").value;
        var customval = document.getElementById("<%=FederalTaxIdCustomVal.ClientID %>");
        if (federalTaxID != "" && accountclass.selectedIndex != 0) {

            if ((accountclass.selectedIndex == 2 && parseInt(federalTaxID.length) != 11) || (accountclass.selectedIndex == 1 && parseInt(federalTaxID.length) != 14)) {
                document.getElementById("<%=federalTaxNumTxt.ClientID %>").focus();
                if (accountclass.selectedIndex == 2) {
                    customval.errormessage = "CPF Number must be 11 digits.";
                }
                else if (accountclass.selectedIndex == 1) {
                    customval.errormessage = "CNPJ Number must be 14 digits.";
                }
                args.IsValid = false
            }
        }
        else { args.IsValid = true }

    }

    function EnableCountryValidator(countryType) {
        if (countryType == 1) {
            EnableValidators("<%= bankNewAddressCountryTxtCountryValidator.ClientID %>");
        }
        else {
            EnableValidators("<%= bankNewAddressCountryTxtCountryValidator_Supplier.ClientID %>");

            if (document.getElementById("<%= bankNewAddressCountryTxt.ClientID %>")) {
                document.getElementById("<%=bankNewAddressCountryTxt.ClientID %>").value = document.getElementById("<%=bankNewAddressCountryTxt_Supplier.ClientID %>").value
            }
        }
    }
    function CityStateCountryFinder(countryType) {

        var locationFinderURL = "/asp/locationfinder/locationfinder.aspx?locFindType=LocationFinder";
        if (countryType == 1)
        {
            locationFinderURL += "&countryName=<%=bankNewAddressCountryTxt.ClientID %>";
            locationFinderURL += "&countryValue=" + document.getElementById("<%=bankNewAddressCountryTxt.ClientID %>").value;
        }
        else {
            locationFinderURL += "&countryName=<%=bankNewAddressCountryTxt_Supplier.ClientID %>";
            locationFinderURL += "&countryValue=" + document.getElementById("<%=bankNewAddressCountryTxt_Supplier.ClientID %>").value;
        }

        locationFinderURL += "&stateName=<%=bankNewAddresssStateProvTxt.ClientID %>";
        locationFinderURL += "&stateValue=" + document.getElementById("<%=bankNewAddresssStateProvTxt.ClientID %>").value;

        if (countryType == 1) {
            locationFinderURL += "&cityName=<%=bankNewAddressCityTxt.ClientID %>";
            locationFinderURL += "&cityValue=" + document.getElementById("<%=bankNewAddressCityTxt.ClientID %>").value;
        }
        else {
            locationFinderURL += "&cityName=<%=bankNewAddressCityTxt_Supplier.ClientID %>";
            locationFinderURL += "&cityValue=" + document.getElementById("<%=bankNewAddressCityTxt_Supplier.ClientID %>").value;
        }

        locationFinderURL += "&postalCode=<%=bankNewAddressPostalCodeTxt.ClientID %>";
        locationFinderURL += "&postalValue=" + document.getElementById("<%=bankNewAddressPostalCodeTxt.ClientID %>").value;
        showWindow(locationFinderURL, "LOCATION FINDER", 630, 200, 150, 150, 0, 0, '', '', '', '', 'status', 'scrollBars', 0);

        if (countryType == 1)
            document.getElementById("<%=bankNewAddressCountryTxt.ClientID %>").focus();
        else
            document.getElementById("<%=bankNewAddressCountryTxt_Supplier.ClientID %>").focus();

    }

    function CallPayRollCountryFinder() {


        var locationFinderURL = "/asp/locationfinder/locationfinder.aspx?locFindType=CountryCodeFinder";
        locationFinderURL += "&countryName=" + "<%=payrollNameTxt.ClientID %>";
        locationFinderURL += "&countryValue=" + document.getElementById("<%=payrollNameTxt.ClientID %>").value;
        showWindow(locationFinderURL, "LOCATION FINDER", 630, 200, 150, 150, 0, 0, '', '', '', '', 'status', 'scrollBars', 0);
        document.getElementById("<%=payrollNameTxt.ClientID %>").focus();
    }

    function ClientStateValidate(oSrc, args) {
        var CountryString = new String();
        var countryReqCustomVal = document.getElementById("<%=bankNewAddresssStateProvTxtCustomValidator.ClientID %>");

        CountryString = document.getElementById("<%=bankNewAddressCountryTxt.ClientID %>").value;
        if (CountryString.length == "" || CountryString.length == 0) {
            if (countryReqCustomVal != null) {
                countryReqCustomVal.errorMessage = "Country is Required";

            }
        }
        if (CountryString.toUpperCase() == "UNITED STATES OF AMERICA" || CountryString.toUpperCase() == "USA" || CountryString.toUpperCase() == "CANADA") {
            if (TruncateValues(document.getElementById("<%=bankNewAddresssStateProvTxt.ClientID %>").value, [" "] ) != "") {
                args.IsValid = true;
            }
            else {
                document.getElementById("<%=bankNewAddrLine1.ClientID %>").focus();
                args.IsValid = false;
            }
        }

    }

    function ClientPostalCodeValidate(oSrc, args) {
        var CountryString = new String();
        var USPostalFormatCustomvalidator = document.getElementById("<%=USPostalFormatCustomvalidator.ClientID %>");
        var CanadianPostalFormatCustomvalidator = document.getElementById("<%=CanadianPostalFormatCustomvalidator.ClientID %>");

        CountryString = document.getElementById("<%=bankNewAddressCountryTxt.ClientID %>").value
        if (CountryString.toUpperCase() == "UNITED STATES OF AMERICA" || CountryString.toUpperCase() == "USA" || CountryString.toUpperCase() == "CANADA") {
            if (TruncateValues(document.getElementById("<%=bankNewAddressPostalCodeTxt.ClientID %>").value,  [" "]) != "") {
                args.IsValid = true;
                USPostalFormatCustomvalidator.enabled = true;
                CanadianPostalFormatCustomvalidator.enabled = true;
            }
            else {
                args.IsValid = false;
                USPostalFormatCustomvalidator.enabled = false;
                CanadianPostalFormatCustomvalidator.enabled = false;
            }
        }
    }

   

    function ClientUSFormatPostalCodeValidate(oSrc, args) {
        var CtrlTxt = document.getElementById("<%=bankNewAddressPostalCodeTxt.ClientID %>").value
        var CountryString = new String();
        CountryString = document.getElementById("<%=bankNewAddressCountryTxt.ClientID %>").value
        if (CountryString.toUpperCase() == "UNITED STATES OF AMERICA" || CountryString.toUpperCase() == "USA") {
            if (CtrlTxt.length == 5 || CtrlTxt.length == 10) {
                if (CtrlTxt.length == 5) {
                    if (isNaN(document.getElementById("<%=bankNewAddressPostalCodeTxt.ClientID %>").value)) {
                        args.IsValid = false;
                    }
                    else { args.IsValid = true; }
                }
                if (CtrlTxt.length == 10) {
                    var regExp = /^\d{5}-\d{4}$/;
                    if (!regExp.test(document.getElementById("<%=bankNewAddressPostalCodeTxt.ClientID %>").value)) {
                        args.IsValid = false;
                    }
                    else { args.IsValid = true; }
                }
            }
            else { args.IsValid = false; }

        }
        else
        { args.IsValid = true; }

    }

    function InsertHypenForText(textboxId) {
        var TextCtrl = document.getElementById(textboxId)
        var TextBoxLen = TextCtrl.value.length;
        var CountryString = new String();

        CountryString = document.getElementById("<%=bankNewAddressCountryTxt.ClientID %>").value
        if (CountryString.toUpperCase() == "UNITED STATES OF AMERICA" || CountryString.toUpperCase() == "USA") {
            if (isNaN(TextCtrl)) {
                if (parseInt(TextBoxLen) == 9) {
                    var TextVal = TextCtrl.value;
                    TextVal = TextVal.substring(0, 5) + "-" + TextVal.substring(5, 9);
                    TextCtrl.value = TextVal;
                }
            }
        }
    }

    function ValidateAccountTypeList(oSrc, args) {
        if (document.getElementById("<%=paymentMethodDDL.ClientID %>").selectedIndex == 1 || document.getElementById("<%=paymentMethodDDL.ClientID %>").selectedIndex == 3) {
            if (document.getElementById("<%=accountTypeDDL.ClientID %>").selectedIndex == 0)
            { args.IsValid = false; }
            else { args.IsValid = true; }
        }
    }

    function ValidatePayrollName(oSrc, args) {
        if (document.getElementById("<%=globalPayrollCheck.ClientID %>").checked == true) {
            if (document.getElementById("<%=payrollNameTxt.ClientID %>").value.length == 0)
            { args.IsValid = false; }
        }
        else { args.IsValid = true; }
    }

    function TruncateValues(originalText, values) {
        for (var i = 0; i < values.length; i++) {
            while (originalText.indexOf(values[i]) != -1) {
                originalText = originalText.replace(values[i], "");
            }
        }

        return originalText;
    }

    function ValidateBankPhone(oSrc, args) {
        var phoneVal = document.getElementById("<%=bankPhoneTxt.ClientID %>").value;
        var phoneVal = TruncateValues(phoneVal, ["-", " ", ".", ":", "(", ")", "ex", "ext", "extn", "x", "X", "/", "*"]);

        if (isNaN(phoneVal))
        { args.IsValid = false; }
        else { args.IsValid = true; }
    }

    function CompareBankCountry(oSrc, args) {
        var countryDDLObj = document.getElementById("<%=bankCountryNameDDL.ClientID %>");
        var countryTxtObj;

        if ("<%= this.BankType %>" == "<%= BankTypeList.Supplier %>")
            countryTxtObj = document.getElementById("<%=bankNewAddressCountryTxt_Supplier.ClientID %>");
        else
            countryTxtObj = document.getElementById("<%=bankNewAddressCountryTxt.ClientID %>");

        if (countryDDLObj && countryTxtObj) {
            if (countryTxtObj.value != "" && countryDDLObj.options[countryDDLObj.selectedIndex].text != countryTxtObj.value)
                args.IsValid = false;
            else
                args.IsValid = true;
        }
        else
            args.IsValid = true;
    }

    function ClearAddressValidationFlag() {
        var validationHiddenObj = document.getElementById("bankAddressInvalidHidden");
        if (validationHiddenObj)
            validationHiddenObj.value = "";
    }

    function CheckPastePAN(id, evt) {
        var source = document.getElementById(id);
        var data = window.clipboardData.getData("Text");
        if ((source.value.length + data.length) > 10) {
            window.clipboardData.setData("Text", data.substring(0, 10 - source.value.length));
        }
    }

    function RestrictTextAreaLength(control, length) {
        if (control != null && control.value.length > length) {
            control.value = control.value.substring(0, length);
        }
    }
</script>


<script language="javascript" type="text/javascript">

    function SetUserMessage(errorType) {
        var messageTab = document.getElementById("messageTable");
        var userMessageDiv = document.getElementById("userMessageOracleSite");

        if (messageTab) {
            if ("<%= this.BankType %>" == "<%= BankTypeList.Customer %>") {
                messageTab.setAttribute("class", "display_true color_2_1");
                messageTab.attributes["class"].value = "display_true color_2_1"; //compatibility mode
            }
            else {
                messageTab.setAttribute("class", "display_true color_1_1");
                messageTab.attributes["class"].value = "display_true color_1_1"; //compatibility mode
            }
        }


        if (errorType == "1") // Other than Error
        {
            userMessageDiv.setAttribute("class", "top_message confirmation");
            userMessageDiv.attributes["class"].value = "top_message confirmation"; //compatibility mode
        }
        else if (errorType == "2")// BR Error
        {
            userMessageDiv.setAttribute("class", "top_message validation");
            userMessageDiv.attributes["class"].value = "top_message validation"; //compatibility mode
        }
        else // Page Error
        {
            userMessageDiv.setAttribute("class", "top_message hard_error");
            userMessageDiv.attributes["class"].value = "top_message hard_error"; //compatibility mode
        }

        userMessageDiv.innerHTML = (document.getElementById("<%= errorTextHidden.ClientID%>").value);
    }



    function ShowHideBelowFindBank(hide) {
        var belowBankControls = document.getElementById("belowBankbuttonControlsTR");
        var buttonObj = document.getElementById("submitSiteInfoButtonTable");
        var actionTable = document.getElementById("actionTable");
        var w8table = document.getElementById("w8InformationTable"); 

        if (belowBankControls != null) {
            if (hide == 1) {
                belowBankControls.style.display = "none";
                if (buttonObj) // To Hide or Show Parent Page Submit button
                    buttonObj.style.display = "none";

                if ("<%= this.DisplaySupplierActionPanel %>" == "True") { // To Hide or Show the Supplier Action Table
                    if (actionTable)
                        actionTable.style.display = "none";
                }

                if (w8table) // hide Vendor Master W8/W9 information
                {
                    w8table.style.display = "none";
                }
            }

            else {
                belowBankControls.style.display = "";
                if (buttonObj) // To Hide or Show Parent Page Submit button
                    buttonObj.style.display = "";

                if ("<%= this.DisplaySupplierActionPanel %>" == "True") { // To Hide or Show the Supplier Action Table
                    if (actionTable)
                        actionTable.style.display = "";
                }

                if (w8table) // show Vendor Master W8/W9 information
                {
                    w8table.style.display = "";
                    AutoAdjustFrameHeight();
                }
            }
        }
    }

    function ShowHideBelowContinueBank(hide) {
        var routingNoTr = document.getElementById("routingNumberTR");
        var findBankButton = document.getElementById("findBankTR");

        if (hide == 1) {
            if (routingNoTr != null)
                routingNoTr.style.display = "none";

            if (findBankButton != null)
                findBankButton.style.display = "none";

            ShowHideBelowFindBank(hide);
        }
        else {
            if (routingNoTr != null)
                routingNoTr.style.display = "";

            if (findBankButton != null)
                findBankButton.style.display = "";
        }
    }

    function HandleBankDivDisplayOnClick(controlName, hide) {
       
        if (controlName == "Continue") {
            ShowHideBelowContinueBank(hide);
            AutoAdjustFrameHeight();
            ShowHideBelowFindBank(1); // To hide the Bank Address Div
        }
        else if (controlName == "Find") {

            ShowHideBelowFindBank(hide);
            AutoAdjustFrameHeight();
        }
    }

    function EnableValidators(validatorToEnable) {
        for (var j = 0; j < validatorToEnable.length; j++) {
            if (document.getElementById(validatorToEnable[j]))
                document.getElementById(validatorToEnable[j]).enabled = true;
        }
    }

    function DisableValidators(validatorToDisable) {
        if (typeof (Page_Validators) != "undefined") {
            for (var j = 0; j < validatorToDisable.length; j++) {
                if (document.getElementById(validatorToDisable[j]))
                    document.getElementById(validatorToDisable[j]).enabled = false;
            }
        }
    }

    function GetPageValid() {
        var validator;
        var isValid = true;

        for (var i = 0; i < Page_Validators.length; i++) {
            validator = Page_Validators[i];
            ValidatorValidate(validator);

            // validation fails if at least one validator fails
            if (!validator.isvalid)
                isValid = false;
        }
        return isValid;
    }

    function EnableDisableCountryValidation(disable) {
        var validators = ["<%=bankCountryNameReqValidator.ClientID %>", "<%=disbursementCurrencyReqValidator.ClientID %>"];
        if (disable == 1) {
            DisableValidators(validators);
            EnableDisableRoutingNoValidation(disable);
        }
        else {
            EnableValidators(validators);
            if (GetPageValid()) {
                HandleBankDivDisplayOnClick('Find', 2);
                return true;
            }
            else
                return false;
        }

    }

    function EnableDisableRoutingNoValidation(disable) {
        var validators = ["<%= routingNumberReqValidator.ClientID %>"];
        if (disable == 1) {
            DisableValidators(validators);
            EnableDisableBankValidation(disable);
        }
        else {
            if (document.getElementById("bankNotFoundMessageDiv"))
                document.getElementById("bankNotFoundMessageDiv").style.display = "none";

            EnableValidators(validators);
            if (GetPageValid()) {
                //HandleBankDivDisplayOnClick('Find', 2) // This has been moved to Server side since the frame needs to be displayed after the postback.
                return true;
            }
            else
                return false;
        }
    }
    function DisableRoutingNumberValidators() {
        var validators = ["<%= routingNumberReqValidator.ClientID %>"];
        DisableValidators(validators);
    }
    function EnableDisableBankValidation(disable) {
        var validators = ["<%= bankStatusDDLReqValidator.ClientID %>", "<%= paymentMethodDDLReqValidator.ClientID %>",
                          "<%= bankPhoneTxtCustomerValidator.ClientID %>",
                          "<%= IBANAccountNumberTxtReqValidator.ClientID %>",
                          "<%= IBANAccountNumberTxtRegExpValidator.ClientID %>", "<%= accountHolderTxtReqValidator.ClientID %>", "<%= accountHolderTxtRegExpValidator.ClientID %>",
                          "<%= accountTypeDDLReqvalidator.ClientID %>", "<%= accountClassDDLReqvalidator.ClientID %>",
                          "<%= paymentcategoryCBLRequiredFieldValidator.ClientID %>", "<%= paymentcatdescrTxtRegExpValidator.ClientID %>", "<%= paymentcatdescrTxtMaxLengthValidator.ClientID %>",
                          "<%= globalPayrollCheckCustomValidator.ClientID %>", "<%= payrollNameTxtCountryValidator.ClientID %>", "<%= specialInstructionsRegExpValidator.ClientID %>", "<%= specialInstructionsTxtMaxValidator.ClientID %>",
                          "<%= FederalTaxNumRegexVal.ClientID %>", "<%= CityTaxIDRegexVal.ClientID %>", "<%= StateTaxIDRegExVal.ClientID %>", "<%=FederalTaxNumCustomVal.ClientID %>", "<%=FederalTaxIDMaxLengthVal.ClientID %>", "<%=StateTaxIdMaxLenghtVal.ClientID %>", "<%=CityTaxIdMaxLenghtVal.ClientID %>", "<%= reTypeIBANAccountNumberCustomValidator.ClientID %>", "<%=FederalTaxIdCustomVal.ClientID %>",
                             "<%= PANRegExprValidator.ClientID %>", "<%= TANRegExpValidator.ClientID %>"];
        var newAddressValidators = ["<%= payeeBankNameTxtReqValidator.ClientID %>", "<%= payeeBankNameTxtRegularExpValidator.ClientID %>",
                          "<%= bankNewAddrLine1ReqValidator.ClientID %>", "<%= bankNewAddrLine1CustomValidator.ClientID %>",
                          "<%= bankNewAddrLine1Regexpvalidator.ClientID %>", "<%= bankNewAddressCityTxtReqValidator.ClientID %>", "<%= CityRegexpvalidator.ClientID %>",
                          "<%= bankNewAddrLine2TxtRegExpValidator.ClientID %>", "<%= bankNewAddresssStateProvTxtRegExpValidator.ClientID %>", "<%= bankNewAddrLine3TxtRegExpValidator.ClientID %>",
                          "<%= bankNewAddresssStateProvTxtCustomValidator.ClientID %>", "<%= bankNewAddressPostalCodeTxtCustomvalidator.ClientID %>",
                          "<%= USPostalFormatCustomvalidator.ClientID %>", "<%= bankNewAddressPostalCodeTxtRegExpvalidator.ClientID %>",
                          "<%= bankNewAddressCityTxtReqValidator_Supplier.ClientID %>", "<%= CityRegexpvalidator_Supplier.ClientID %>",
                          "<%= bankNewAddressCountryTxtCountryValidator_Supplier.ClientID %>", "<%= bankNewAddressCountryTxtCountryValidator.ClientID %>",
                          "<%= bankNewAddressCountryTxtReqValidator.ClientID %>", "<%= bankNewAddressCountryTxtReqValidator_Supplier.ClientID %>",                          
                          "<%= bankAdressCountryCompareValidator.ClientID %>", "<%=CanadianPostalFormatCustomvalidator.ClientID %>"
                          ];
        var supplierActionValidators = ["requestedActionRequiredFieldValidator", "requestedActionCustomerValidator", "reasonForRejectCustomValidator", "CommentsTextCustomvalidator"];

        var w8W9InformationValidators = ["noOfW8FormsReqdValidator",
                                          "W9ReqdListValidator",
                                          "reasonW8NtReqdListCustomValidator",
                                          "taxIdentNoVerfdDtCustomValidator",
                                          "otherDescrCustomValidator",
                                          "DescriptionIfOtherValidator"];

        if (disable == 1) {
            DisableValidators(validators);
            DisableValidators(newAddressValidators);
            DisableValidators(supplierActionValidators);
            DisableValidators(w8W9InformationValidators);
        }
        else {
            EnableValidators(validators);
            if (document.getElementById("<%=bankAddressFoundHidden.ClientID%>")) {
                if (document.getElementById("<%=bankAddressFoundHidden.ClientID%>").value == "2") {
                    EnableValidators(newAddressValidators);
                }
            }

            if ("<%= this.DisplaySupplierActionPanel %>" == "True") {
                EnableValidators(supplierActionValidators);
            }

            EnableValidators(w8W9InformationValidators);
        }

     
    }

    function ValidateRoutingAndSwiftCode(oSrc, args) {
        var routingtype;
        var customvalidator = document.getElementById("<%=routingNumberReqValidator.ClientID %>");
        var routingNumberLabelObj = document.getElementById("<%=routingNumberLabel.ClientID %>");
        var routingNumberTxtObj = document.getElementById("<%=routingNumberTxt.ClientID %>");
        var SWIFTBICCodeTxtObj = document.getElementById("<%=SWIFTBICCodeTxt.ClientID %>")
        var routingNoReqIndObj = document.getElementById("<%=routingNoRequiredIndHidden.ClientID %>");

        if (routingNumberLabelObj.innerText != null && routingNumberLabelObj.innerText != "") {
            routingtype = routingNumberLabelObj.innerText;
        }
        else {
            routingtype = "Local Routing Number";
        }

        if (routingNoReqIndObj.value == "1") // both of them are false, so default validation
        {
            if ((trimString(routingNumberTxtObj.value).length == 0 && trimString(SWIFTBICCodeTxtObj.value).length == 0)) {
                args.IsValid = false;
                customvalidator.errormessage = "Either " + routingtype.replace("*", "") + " or a SWIFT/BIC Code is required.";
            }
            else { args.IsValid = true; }
        }
        else if (routingNoReqIndObj.value == "2") // Routing Number is required.
        {
            if ((trimString(routingNumberTxtObj.value).length == 0)) {
                args.IsValid = false;
                customvalidator.errormessage = routingtype.replace("*", "") + " is required.";
            }
            else { args.IsValid = true; }
        }
        else if (routingNoReqIndObj.value == "3") // SWIFT/BIC Code is required.
        {
            if ((trimString(SWIFTBICCodeTxtObj.value).length == 0)) {
                args.IsValid = false;
                customvalidator.errormessage = "SWIFT/BIC Code is required.";
            }
            else { args.IsValid = true; }
        }
        else if (routingNoReqIndObj.value == "4") // SWIFT/BIC Code and Routing No are required.
        {
            if ((trimString(routingNumberTxtObj.value).length == 0)) {
                args.IsValid = false;
                customvalidator.errormessage = routingtype.replace("*", "") + " is required.";
            }
            else { args.IsValid = true; }
        }
    }

    function SetPageFocus(buttonName) {
        if (buttonName == "Continue") {
            if (document.getElementById("<%=routingNumberTxt.ClientID %>" )) {
                document.getElementById("<%=routingNumberTxt.ClientID %>").focus();
            }
            else if (document.getElementById("<%=SWIFTBICCodeTxt.ClientID %>")) {
                document.getElementById("<%=SWIFTBICCodeTxt.ClientID %>").focus();
            }
        }
        else if (buttonName == "Find") {
            if (document.getElementById("<%=bankStatusDDL.ClientID %>") && !document.getElementById("<%=bankStatusDDL.ClientID %>").disabled    ) {
                document.getElementById("<%=bankStatusDDL.ClientID %>").focus();
            }
        }
        else if (buttonName == "AccountClass") {
            if (document.getElementById("<%=accountClassDDL.ClientID %>") && !document.getElementById("<%=accountClassDDL.ClientID %>").disabled) {
                document.getElementById("<%=accountClassDDL.ClientID %>").focus();
            }
        }
    }

    function ShowHideBankAddressDetailForRoutingNo() {
        var BankValidObj = document.getElementById("<%= bankAddressFoundHidden.ClientID %>");
        if (BankValidObj) {
            if (BankValidObj.value == "1") {
                document.getElementById("bankAddressDetailDiv").style.display = "";
                document.getElementById("newBankAddressTR").style.display = "none";

            }
            else if (BankValidObj.value == "2") {
                document.getElementById("bankAddressDetailDiv").style.display = "none";
                document.getElementById("newBankAddressTR").style.display = "";
            }
            else {
                document.getElementById("bankAddressDetailDiv").style.display = "none";
                document.getElementById("newBankAddressTR").style.display = "none";
            }
        }
    }

    function DisplayControlOnPageLoad() {
        var routingInfo = document.getElementById("<%= displayRoutingNoDivHidden.ClientID %>");
        var bankInfo = document.getElementById("<%= displayBankInfoDivHidden.ClientID %>");

        if (routingInfo) {
            if (routingInfo.value == "2") {
                if (document.getElementById("routingNumberTR")) {
                    document.getElementById("routingNumberTR").style.display = "";
                    document.getElementById("findBankTR").style.display = "";
                }
            }
        }

        if (bankInfo) { //1
            if (bankInfo.value == "2") { //2
                if (document.getElementById("belowBankbuttonControlsTR")) { //3
                    document.getElementById("belowBankbuttonControlsTR").style.display = "";
                    ShowHideBankAddressDetailForRoutingNo();
                    var buttonObj = document.getElementById("submitSiteInfoButtonTable");
                    if (buttonObj) // To Hide or Show Parent Page Submit button
                        buttonObj.style.display = "";

                    var actionTable = document.getElementById("actionTable");
                    if ("<%= this.DisplaySupplierActionPanel %>" == "True") { // To Hide or Show the Supplier Action Table
                        if (actionTable)
                            actionTable.style.display = "";
                    }

                }//3
            }//2
        } //1

        if ("<%= this.BankType %>" == "<%= BankTypeList.Supplier %>") // Supplier
        {
            HideControl("<%= bankNewAddressCountryTxt.ClientID %>", true);
            HideControl("bankNewAddressCountryTxt", true);

            HideControl("<%= bankNewAddressCountryTxt_Supplier.ClientID %>", false);
            HideControl("bankNewAddressCountryTxt_Supplier", false);
        }
        else {
            HideControl("<%= bankNewAddressCountryTxt.ClientID %>", false);
            HideControl("bankNewAddressCountryTxt", false);

            HideControl("<%= bankNewAddressCountryTxt_Supplier.ClientID %>", true);
            HideControl("bankNewAddressCountryTxt_Supplier", true);
        }

    }
    function federalTaxVal(oSrc, args) {
        if ("<%=this.GeoOrigingCd %>" != 0 && document.getElementById("<%=FederalTaxNumCustomVal.ClientID %>") != null) {
            if (("<%= this.GeoOrigingCd%>" == "13" && ("<%= this.BankType%>" == "<%=BankTypeList.Customer%>" || "<%=this.BankType%>" == "<%=BankTypeList.Supplier%>"))
                    || "<%=this.BankType%>" == "<%= BankTypeList.Division%>") {
                if ("<%=this.BankType %>" == "<%= BankTypeList.Division%>" && (TruncateValues(document.getElementById("<%=federalTaxNumTxt.ClientID %>").value, [" "]) == "")) {
                    args.IsValid = true;
                    document.getElementById("<%=FederalTaxNumCustomVal.ClientID %>").Enabled = false;
                   
                }
                else if (("<%=this.BankType %>" == "<%= BankTypeList.Supplier%>" || "<%=this.BankType %>" == "<%= BankTypeList.Customer%>") 
                && (TruncateValues(document.getElementById("<%=federalTaxNumTxt.ClientID %>").value, [" "]) == "")) {
                    args.IsValid = false;
                    document.getElementById("<%=FederalTaxNumCustomVal.ClientID%>").Enabled = true;
                   
                }

            }

        }
        return;
    }

    function HideControl(controlId, hide) {
        if (document.getElementById(controlId)) {
            if (hide)
                document.getElementById(controlId).style.display = "none";
            else
                document.getElementById(controlId).style.display = "";
        }
    }

    function HideUserMessageDiv() {
        if (document.getElementById("messageTable")) {
            document.getElementById("messageTable").style.display = "none";
        }

    }

    function DisableControlOnEditMode() {
        var controlsList = ["<%=bankCountryNameDDL.ClientID %>", "<%=disbursementCurrencyDDL.ClientID %>", "<%=routingNumberTxt.ClientID %>"
                           , "<%=SWIFTBICCodeTxt.ClientID %>", "<%=paymentMethodDDL.ClientID %>", "<%=payeeBankNameTxt.ClientID %>", "<%=bankPhoneTxt.ClientID %>"
                           ];
        var newAddrControlList = ["<%=bankNewAddrLine1.ClientID %>", "<%=bankNewAddrLine2Txt.ClientID %>", "<%=bankNewAddrLine3Txt.ClientID %>",
                                , "<%=bankNewAddressCityTxt.ClientID %>", "<%=bankNewAddresssStateProvTxt.ClientID %>", "<%=bankNewAddressPostalCodeTxt.ClientID %>"
                                , "<%=bankNewAddressCountryTxt_Supplier.ClientID %>", "<%=bankNewAddressCountryTxt.ClientID %>"
                                , "addCountryWidgetLink_Supplier", "addCountryWidgetLink"
                                ]

        for (var i = 0; i < controlsList.length; i++) {
            if (document.getElementById(controlsList[i])) {
                document.getElementById(controlsList[i]).disabled = true;
            }
        }

        if (document.getElementById("<%= bankAddressFoundHidden.ClientID %>")) {
            if (document.getElementById("<%= bankAddressFoundHidden.ClientID %>").value == "2") {
                for (var i = 0; i < newAddrControlList.length; i++) {
                    if (document.getElementById(newAddrControlList[i])) {
                        document.getElementById(newAddrControlList[i]).disabled = true;
                    }
                }
            }
        }

    }

    function SetControlFocus() {
       
            AutoAdjustFrameHeight();
       
    }
    function setfocustoContinuebutton() {
        if ("<%= this.BankType %>" == "<%= BankTypeList.Supplier %>" || "<%= this.VendorMasterFlag %>" == 1) {
            var elementid = document.getElementById("<%= countryContinueBtn.ClientID %>");
            elementid.focus();
        }
    }
    function setfocustoFindbutton() {
        if ("<%= this.BankType %>" == "<%= BankTypeList.Supplier %>" || "<%= this.VendorMasterFlag %>" == 1) {
            var elementid = document.getElementById("<%= findBankBtn.ClientID %>");
            elementid.focus();
        }
    }
    function ValidatereTypeIBANAccountNumber(oSrc, args) {
        var reTypeIBANAccountNumberCustomValidator = document.getElementById("<%=reTypeIBANAccountNumberCustomValidator.ClientID %>");
        var IBANAccountNumberReqdValidator = document.getElementById("<%=IBANAccountNumberTxtReqValidator.ClientID %>");
        var reTypeIBANAccountNumberTxtObj = document.getElementById("<%=reTypeIBANAccountNumberTxt.ClientID %>");
        var IBANAccountNumberTxtObj = document.getElementById("<%=IBANAccountNumberTxt.ClientID %>");
        if (reTypeIBANAccountNumberCustomValidator && IBANAccountNumberTxtObj && reTypeIBANAccountNumberTxtObj)
            if ((reTypeIBANAccountNumberTxtObj.value != IBANAccountNumberTxtObj.value)
            || (reTypeIBANAccountNumberTxtObj.value == "" && IBANAccountNumberTxtObj.value == "")) {
                args.IsValid = false;
                reTypeIBANAccountNumberTxtObj.value = "";
                IBANAccountNumberTxtObj.value = "";
                IBANAccountNumberReqdValidator.enabled = false;
                reTypeIBANAccountNumberCustomValidator.enabled = true;
            }
            else {
                args.IsValid = true;
            }
        }          
          
</script>

<div id="mainDiv" class="color_<%=ColorNumber%>_1" style="width: 100%" runat="server">
    <table class="color_<%=ColorNumber%>_1" id="mainTable" cellspacing="0" cellpadding="0" width="100%"
        border="0">
        <colgroup>
            <col width="24%" />
            <col width="26%" />
            <col width="24%" />
            <col width="26%" />
        </colgroup>
        <tr>
            <td>
                <span class="heading2 color_<%=ColorNumber%>">Bank Country & Currency </span>&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr class="border_bottom">
            <td class="heading4right" nowrap="nowrap">
                <cms:Label ID="bankCountryCaptionLabel" Display="Editable" runat="server">Country Of Bank <span class="required_red" title="Country of Bank is Required.">*</span></cms:Label>
            </td>
            <td>
                <cms:DropDownList ID="bankCountryNameDDL" runat="server" Display="editable" cssclass="select"  Width="150" onchange="HandleBankDivDisplayOnClick('Continue', 1);SetControlFocus();setfocustoContinuebutton();">
                </cms:DropDownList>
                <cms1:RequiredFieldValidator ID="bankCountryNameReqValidator" runat="server" Display="None"
                    Enabled="false" ErrorMessage="Country of Bank is Required." ControlToValidate="bankCountryNameDDL"
                    LabelControl="bankCountryCaptionLabel">
                </cms1:RequiredFieldValidator>
            </td>
            <td class="heading4right" nowrap="nowrap">
                <cms:Label ID="disbursementCurrencyLabel" Display="Editable" cssclass="select" Width="170" runat="server">Disbursement Currency <span class="required_red" title="Disbursement Currency is Required.">*</span></cms:Label>
            </td>
            <td>
                <cms:DropDownList ID="disbursementCurrencyDDL" runat="server" Display="editable" Width="140" onchange="HandleBankDivDisplayOnClick('Continue', 1);SetControlFocus();setfocustoContinuebutton();"
                    CssClass="inputText form_margin">
                </cms:DropDownList>
                <cms1:RequiredFieldValidator ID="disbursementCurrencyReqValidator" runat="server"
                    Display="None" ErrorMessage="Disbursement Currency is Required."
                    ControlToValidate="disbursementCurrencyDDL" LabelControl="disbursementCurrencyLabel">
                </cms1:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="right">
                <cms:Button ID="countryContinueBtn" runat="server" Display="Editable" Text="Continue" OnClick="countryContinueBtn_Click"
                    ButtonImageType="SubmitDown" align="right" OnClientClick="HideUserMessageDiv();EnableDisableRoutingNoValidation(1);EnableDisableCountryValidation(2);">
                </cms:Button>
            </td>
        </tr>
        <tr id="routingNumberTR">
            <td colspan="4" class="border_bottom">
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <colgroup>
                        <col width="24%" />
                        <col width="26%" />
                        <col width="24%" />
                        <col width="26%" />
                    </colgroup>
                    <tr>
                        <td>
                            <span class="heading2 color_<%=ColorNumber%>">Routing Number/Code </span>&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="heading4right">
                            <cms:Label ID="routingNumberLabel" Display="Editable" runat="server" Text="Local Routing Number" />
                        </td>
                        <td>
                            <cms:TextBox ID="routingNumberTxt" Display="Editable" runat="server" MaxLength="50"
                                CssClass="inputTextLarge form_margin" onchange="HandleBankDivDisplayOnClick('Find', 1);SetControlFocus();setfocustoFindbutton();">
                            </cms:TextBox>
                        </td>
                        <td class="heading4right" nowrap="nowrap">
                        <%--<span class="required_blue" title="SWIFT/BIC Code is Required.">*</span>--%>
                            <cms:Label ID="SWIFTBICCodelabel" Display="Editable" runat="server" Text ="SWIFT/BIC Code"></cms:Label> 
                        </td>
                        <td>
                            <cms:TextBox ID="SWIFTBICCodeTxt" Display="Editable" runat="server" MaxLength="50"
                                CssClass="inputTextLarge form_margin" onchange="HandleBankDivDisplayOnClick('Find', 1);SetControlFocus();setfocustoFindbutton();">
                            </cms:TextBox>
                            <cms1:CustomValidator ID="routingNumberReqValidator" runat="server" Display="None"
                                Enabled="false" LabelControl="routingNumberLabel,SWIFTBICCodelabel" ControlToValidate="routingNumberTxt"
                                ErrorMessage="Either Bank Routing number Or the Swift/BIC Code has to be entered."
                                ClientValidationFunction="ValidateRoutingAndSwiftCode" ValidateEmptyText="true"></cms1:CustomValidator>
                            <%--<cms1:CustomValidator ID="BankRoutingTypeCustomvalidator"
                                runat="server" Display="None" LabelControl="routingNumberLabel,SWIFTBICCodelabel"
                                ErrorMessage="Canadian and Mexican Bank requires Swift Code and Routing Number should not be 9 characters."
                                Enabled="false" ClientValidationFunction="ValidateBankRoutingType">
                            </cms1:CustomValidator>--%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="findBankTR">
            <td colspan="4" align="right">
                <cms:Button ID="findBankBtn" runat="server" Display="Editable" Text="Find Bank" ButtonImageType="SubmitDown"
                    align="right" OnClientClick="HideUserMessageDiv();EnableDisableBankValidation(1);EnableDisableRoutingNoValidation(2);" OnClick="findBankBtn_Click">
                </cms:Button>
            </td>
        </tr>
        <tr id="belowBankbuttonControlsTR" style="display:none;">
            <td colspan="4">
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <colgroup>
                        <col width="24%" />
                        <col width="26%" />
                        <col width="24%" />
                        <col width="26%" />
                    </colgroup>
                    <tr>
                        <td colspan="4">
                            <div id="bankNotFoundMessageDiv" runat="server" > Bank Not Found
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <span class="heading2 color_<%=ColorNumber%>">Bank Details</span>&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr id="bankAddressHeaderLabel">
                        <td colspan="4">
                            <cms:Label ID="addressHeaderLabel" runat="server" Display="Editable" CssClass="heading2 level1indent  color_<%=ColorNumber%>"><b>Note</b> : Modifying the Bank Instructions will inactivate the current Site ID and create new one.
                            </cms:Label>
                        </td>
                    </tr>
                    <tr id="bankAddressSupplierLabel">
                        <td colspan="4">
                            <cms:Label ID="addressSupplierLabel" runat="server" Display="Editable" CssClass="heading2 level1indent  color_<%=ColorNumber%>">The Bank Address displayed may be the main branch address not the address of your specific bank branch.
                            </cms:Label>
                        </td>
                    </tr>

                    <tr id="createdByRow" runat="server" visible="false">
                        <td class="heading4right" nowrap>
                            <cms:Label ID="createdBylbl" Display="Editable" runat="server">Created By</cms:Label>
                        </td>
                        <td>&nbsp;
                            <cms:Label ID="createdBylbltxt" Display="Editable" runat="server" >
                            </cms:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="heading4right" nowrap>
                            <cms:Label ID="statusLabel" Display="Editable" runat="server">Status <span class="required_red" title="Status is required.">*</span></cms:Label>
                        </td>
                        <td>
                            <cms:DropDownList ID="bankStatusDDL" runat="server" Width="140" Display="editable"
                                CssClass="inputText form_margin">
                            </cms:DropDownList>
                            <cms1:RequiredFieldValidator ID="bankStatusDDLReqValidator" runat="server" Display="None"
                                Enabled="false" ErrorMessage="Status is required." ControlToValidate="bankStatusDDL"
                                LabelControl="statusLabel">
                            </cms1:RequiredFieldValidator>
                        </td>
                        <td class="heading4right">
                            <cms:Label ID="paymentMethdLabel" Display="Editable" runat="server">Payment Method <span class="required_red" title="Payment Method is required.">*</span></cms:Label>
                        </td>
                        <td>
                            <cms:DropDownList ID="paymentMethodDDL" runat="server" Width="140" Display="editable"
                                CssClass="inputText form_margin">
                            </cms:DropDownList>
                            <cms1:RequiredFieldValidator ID="paymentMethodDDLReqValidator" runat="server" Display="None"
                                Enabled="false" ErrorMessage="Payment Method is required." ControlToValidate="paymentMethodDDL"
                                LabelControl="paymentMethdLabel">
                            </cms1:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="heading4right" nowrap>
                            <cms:Label ID="payeeBankNameLabel" Display="Editable" runat="server">Payee Bank Name <span class="required_red" title="Payee Bank Name is required.">*</span></cms:Label>
                        </td>
                        <td>
                            <cms:TextBox ID="payeeBankNameTxt" Display="Editable" runat="server" MaxLength="50"
                                CssClass="inputTextLarge form_margin">
                            </cms:TextBox>&nbsp;&nbsp;&nbsp;
                            <cms1:RequiredFieldValidator ID="payeeBankNameTxtReqValidator" runat="server" Display="None"
                                Enabled="false" ErrorMessage="Payee Bank Name is required." ControlToValidate="payeeBankNameTxt"
                                LabelControl="payeeBankNameLabel">
                            </cms1:RequiredFieldValidator>
                            <cms1:RegularExpressionValidator ID="payeeBankNameTxtRegularExpValidator" runat="Server"
                                ValidationExpression='([a-zA-Z0-9À-ÿŠšŽžŸ\\/\,* "\@\.\-_()&amp;&#039;]*[a-zA-Z0-9À-ÿŠšŽžŸ]+[a-zA-Z0-9À-ÿŠšŽžŸ\\/\,* "\@\.\-_()&amp;&#039;]*)'
                                LabelControl="payeeBankNameLabel" ControlToValidate="payeeBankNameTxt" Display="None"
                                Enabled="false" ErrorMessage="Entered Characters are not allowed for Payee Bank Name."
                                ErrorCssClass="red">
                            </cms1:RegularExpressionValidator>
                        </td>
                        <td class="heading4right">
                            <cms:Label ID="bankPhoneLabel" Display="Editable" runat="server">Bank Phone&nbsp;&nbsp;&nbsp;</cms:Label>
                        </td>
                        <td>
                            <cms:TextBox ID="bankPhoneTxt" Display="Editable" runat="server" MaxLength="15" CssClass="inputTextLarge form_margin">
                            </cms:TextBox>
                            <cms1:CustomValidator ID="bankPhoneTxtCustomerValidator" LabelControl="bankPhoneLabel"
                                runat="server" Display="None" ControlToValidate="bankPhoneTxt" ErrorMessage="Invalid Bank Phone."
                                ClientValidationFunction="ValidateBankPhone" Enabled="false">
                            </cms1:CustomValidator>
                            <cms1:RegularExpressionValidator ID="bankPhoneTxtRegexpvalidator" runat="Server"
                                ValidationExpression="[a-z,A-Z,0-9/\n\s.\-?:()'+']*" LabelControl="bankPhoneLabel"
                                ControlToValidate="bankPhoneTxt" Display="None" Enabled="false" ErrorMessage="Entered Characters are not allowed for Bank Phone."
                                ErrorCssClass="red">
                            </cms1:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr id="bankAddressDetailDiv">
                        <td colspan="2" align="left" width="100%">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <colgroup>
                                    <col width="24%" />
                                    <col width="26%" />
                                    <col width="24%" />
                                    <col width="26%" />
                                </colgroup>
                                <tr>
                                <td> </td>
                                    <td  class="heading4right" nowrap rowspan="3" valign="middle">
                                        <cms:Label ID="bankAddrCaptionLabel" Display="Editable" runat="server">Bank Address</cms:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td colspan="3">
                                        <cms:Label ID="bankAddressLabel" Display="Editable" runat="server">
                                        </cms:Label>
                                    </td>
                                    <td>
                                        </td>
                                </tr>
                                <tr>
                                <td> </td>
                                    <td colspan="3">
                                       <cms:Label ID="bankCityLabel" Display="Editable" runat="server">
                                       </cms:Label>
                                       <cms:Label ID="bankStateLabel" Display="Editable" runat="server">
                                       </cms:Label>
                                       <cms:Label ID="bankPostalCodeLabel" Display="Editable" runat="server">
                                       </cms:Label>
                                    </td>
                                     <td colspan="2">
                                        </td>
                                </tr>
                                <tr>
                                <td>
                                        </td>
                                    <td colspan="3">
                                       <cms:Label ID="bankCountryLabel" Display="Editable" runat="server"></cms:Label>
                                    </td>
                                    <td colspan="2">
                                        </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="newBankAddressTR">
                        <td colspan="4" width="100%" align="left">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <colgroup>
                                    <col width="24%" />
                                    <col width="26%" />
                                    <col width="24%" />
                                    <col width="26%" />
                                </colgroup>
                                <tr id="invalidAddressTR">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <cms:Label ID="invalidAddressLabel" runat="server" Display="Editable" ForeColor="red">&nbsp;&nbsp; *** INVALID ADDRESS ***</cms:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="heading4right">
                                        <cms:Label ID="bankNewaddrLine1Label" Display="Editable" runat="server">Address Line 1 <span class="required_red" title="Address Line 1 is required.">*</span></cms:Label>
                                    </td>
                                    <td>
                                        <cms:TextBox ID="bankNewAddrLine1" Display="Editable" runat="server" MaxLength="72"
                                            CssClass="inputTextLarge form_margin">
                                        </cms:TextBox>
                                        <cms1:RequiredFieldValidator ID="bankNewAddrLine1ReqValidator" runat="server" Display="None"
                                            LabelControl="bankNewaddrLine1Label" ControlToValidate="bankNewAddrLine1" ErrorMessage="Address Line 1 is required."
                                            Enabled="false">
                                        </cms1:RequiredFieldValidator>
                                        <cms1:CustomValidator ID="bankNewAddrLine1CustomValidator" runat="server" Display="None"
                                            Enabled="False" LabelControl="bankNewaddrLine1Label" ClientValidationFunction="ValidateAddressLine" 
                                            ErrorMessage="Address Line 1 is required with minimum 3 characters." ControlToValidate="bankNewAddrLine1">
                                        </cms1:CustomValidator>
                                        <cms1:RegularExpressionValidator ID="bankNewAddrLine1Regexpvalidator" runat="Server"
                                            ValidationExpression="[a-z,A-Z,0-9/\n\s.\-?:()'+'À-ÿŠšŽžŸ]*" LabelControl="bankNewaddrLine1Label"
                                            ControlToValidate="bankNewAddrLine1" Display="None" Enabled="false" ErrorMessage="Entered Characters are not allowed for AddrLine1."
                                            ErrorCssClass="red">
                                        </cms1:RegularExpressionValidator>
                                    </td>
                                    <td class="heading4right" align="left">
                                        <cms:Label ID="bankNewAddressCityLabel" Display="Editable" runat="server">City <span class="required_red" title="City is required.">*</span></cms:Label>
                                        <cms:Label ID="bankNewAddrCountryLabel_Supplier" Display="Editable" runat="server">Country<span class="required_red" title="Country is required.">*</span></cms:Label>
                                    </td>
                                    <td>
                                        <cms:TextBox ID="bankNewAddressCityTxt" Display="Editable" runat="server" MaxLength="50"
                                            CssClass="inputTextLarge form_margin" onchange="ClearAddressValidationFlag()">
                                        </cms:TextBox>
                                        <cms:TextBox ID="bankNewAddressCountryTxt_Supplier" Display="Editable" runat="server" MaxLength="50" onchange="EnableCountryValidator(2);ClearAddressValidationFlag();"
                                            CssClass="inputTextLarge form_margin">
                                        </cms:TextBox> <cms:Widget ID="addCountryWidget_Supplier" runat="server" Display="editable">
                                                            <a onclick="CityStateCountryFinder(2);return false;" href="#" id="addCountryWidgetLink_Supplier">
                                                                <img height="14" alt="Location Finder" src="/images/global/icons/location_finder.gif"
                                                                        width="14" border="0"></a>
                                                        </cms:Widget>
                                        <cms1:CountryValidator ID="bankNewAddressCountryTxtCountryValidator_Supplier" runat="server"
                                            Display="none" Enabled="false" ControlToValidate="bankNewAddressCountryTxt_Supplier" LabelControl="bankNewAddrCountryLabel_Supplier"
                                            ErrorMessage="Invalid Country.">
                                        </cms1:CountryValidator>
                                        <cms1:RequiredFieldValidator ID="bankNewAddressCountryTxtReqValidator_Supplier" runat="server"
                                            Display="None" Enabled="false" ErrorMessage="Country is required." ControlToValidate="bankNewAddressCountryTxt_Supplier"
                                            LabelControl="bankNewAddrCountryLabel_Supplier">
                                        </cms1:RequiredFieldValidator>
                                        <cms1:RequiredFieldValidator ID="bankNewAddressCityTxtReqValidator" runat="server"
                                            Display="None" Enabled="false" ErrorMessage="City is required." ControlToValidate="bankNewAddressCityTxt"
                                            LabelControl="bankNewAddressCityLabel">
                                        </cms1:RequiredFieldValidator>
                                        <cms1:RegularExpressionValidator ID="CityRegexpvalidator" runat="Server" ValidationExpression="[a-z,A-Z,0-9/\n\s.\-?:()'+'À-ÿŠšŽžŸ]*"
                                            LabelControl="bankNewAddressCityLabel" ControlToValidate="bankNewAddressCityTxt"
                                            Display="None" Enabled="false" ErrorMessage="Entered Characters are not allowed for City."
                                            ErrorCssClass="red">
                                        </cms1:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="heading4right" nowrap>
                                        <cms:Label ID="bankNewAddrLine2Label" Display="Editable" runat="server">Address Line 2</cms:Label>
                                    </td>
                                    <td>
                                        <cms:TextBox ID="bankNewAddrLine2Txt" Display="Editable" runat="server" MaxLength="72"
                                            CssClass="inputTextLarge form_margin">
                                        </cms:TextBox>
                                        <cms1:RegularExpressionValidator ID="bankNewAddrLine2TxtRegExpValidator" runat="Server"
                                            ValidationExpression="[a-z,A-Z,0-9/\n\s.\-?:()'+'À-ÿŠšŽžŸ]*" LabelControl="bankNewAddrLine2Label"
                                            ControlToValidate="bankNewAddrLine2Txt" Display="None" Enabled="false" ErrorMessage="Entered Characters are not allowed for Address Line2."
                                            ErrorCssClass="red">
                                        </cms1:RegularExpressionValidator>
                                    </td>
                                    <td class="heading4" nowrap align="right">
                                        <cms:Label ID="bankNewAddressStateProvLabel" Display="Editable" runat="server">State/Province <span class="required_blue" title="State/Province is required, if Country is USA or Canada.">*</span></cms:Label>
                                    </td>
                                    <td>
                                        <cms:TextBox ID="bankNewAddresssStateProvTxt" Display="Editable" runat="server" CssClass="inputTextLarge form_margin" onchange="ClearAddressValidationFlag();">
                                        </cms:TextBox>
                                        <cms1:RegularExpressionValidator ID="bankNewAddresssStateProvTxtRegExpValidator"
                                            runat="Server" ValidationExpression="[a-z,A-Z,0-9/\n\s.\-?:()'+']*" LabelControl="bankNewAddressStateProvLabel"
                                            ControlToValidate="bankNewAddresssStateProvTxt" Display="None" Enabled="false"
                                            ErrorMessage="Entered Characters are not allowed for State." ErrorCssClass="red">
                                        </cms1:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="heading4right" nowrap align="right">
                                        <cms:Label ID="bankNewAddrLine3Label" Display="Editable" runat="server">Address Line 3</cms:Label>
                                    </td>
                                    <td nowrap>
                                        <cms:TextBox ID="bankNewAddrLine3Txt" Display="Editable" runat="server" MaxLength="72"
                                            CssClass="inputTextLarge form_margin">
                                        </cms:TextBox>
                                        <cms1:RegularExpressionValidator ID="bankNewAddrLine3TxtRegExpValidator" runat="Server"
                                            ValidationExpression="[a-z,A-Z,0-9/\n\s.\-?:()'+'À-ÿŠšŽžŸ]*" LabelControl="bankNewAddrLine3Label"
                                            ControlToValidate="bankNewAddrLine3Txt" Display="None" Enabled="false" ErrorMessage="Entered Characters are not allowed for AddressLine3"
                                            ErrorCssClass="red">
                                        </cms1:RegularExpressionValidator>
                                    </td>
                                    <td class="heading4" align="right">
                                        <cms:Label ID="bankNewAddrCountryLabel" Display="Editable" runat="server">Country<span class="required_red" title="Country is required.">*</span></cms:Label>
                                        <cms:Label ID="bankNewAddressCityLabel_Supplier" Display="Editable" runat="server">City <span class="required_red" title="City is required.">*</span></cms:Label>
                                    </td>
                                    <td>
                                        <cms:TextBox ID="bankNewAddressCountryTxt" Display="Editable" runat="server" MaxLength="50" onchange="EnableCountryValidator(1);ClearAddressValidationFlag();"
                                            CssClass="inputTextLarge form_margin">
                                        </cms:TextBox> <cms:Widget ID="addCountryWidget" runat="server" Display="editable">
                                                            <a onclick="CityStateCountryFinder(1);return false;" href="#" id="addCountryWidgetLink">
                                                                <img height="14" alt="Location Finder" src="/images/global/icons/location_finder.gif"
                                                                        width="14" border="0"></a>
                                                        </cms:Widget>
                                        <cms1:CountryValidator ID="bankNewAddressCountryTxtCountryValidator" runat="server"
                                            Display="none" Enabled="false" ControlToValidate="bankNewAddressCountryTxt" LabelControl="bankNewAddrCountryLabel"
                                            ErrorMessage="Invalid Country.">
                                        </cms1:CountryValidator>
                                        <cms1:RequiredFieldValidator ID="bankNewAddressCountryTxtReqValidator" runat="server"
                                            Display="None" Enabled="false" ErrorMessage="Country is required." ControlToValidate="bankNewAddressCountryTxt"
                                            LabelControl="bankNewAddrCountryLabel">
                                        </cms1:RequiredFieldValidator>
                                        <cms:TextBox ID="bankNewAddressCityTxt_Supplier" Display="Editable" runat="server" MaxLength="50" onchange="ClearAddressValidationFlag();"
                                            CssClass="inputTextLarge form_margin"></cms:TextBox>
                                        <cms1:CustomValidator ID="bankNewAddresssStateProvTxtCustomValidator" runat="server"
                                            Display="None" Enabled="False" LabelControl="bankNewAddressStateProvLabel" ClientValidationFunction="ClientStateValidate"
                                            ErrorMessage="State/Province is required, if Country is USA or Canada." ControlToValidate="bankNewAddressCountryTxt"
                                            ErrorCssClass="blue">
                                        </cms1:CustomValidator>

                                         
                                        <cms1:RequiredFieldValidator ID="bankNewAddressCityTxtReqValidator_Supplier" runat="server"
                                            Display="None" Enabled="false" ErrorMessage="City is required." ControlToValidate="bankNewAddressCityTxt_Supplier"
                                            LabelControl="bankNewAddressCityLabel_Supplier">
                                        </cms1:RequiredFieldValidator>
                                        <cms1:RegularExpressionValidator ID="CityRegexpvalidator_Supplier" runat="Server" ValidationExpression="[a-z,A-Z,0-9/\n\s.\-?:()'+'À-ÿŠšŽžŸ]*"
                                            LabelControl="bankNewAddressCityLabel_Supplier" ControlToValidate="bankNewAddressCityTxt_Supplier"
                                            Display="None" Enabled="false" ErrorMessage="Entered Characters are not allowed for City."
                                            ErrorCssClass="red">
                                        </cms1:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                    <td class="heading4right" align="right">
                                        <cms:Label ID="bankNewAddrPostalCodeLabel" Display="Editable" runat="server">Postal Code <span class="required_blue" title="Postal Code is required, if Country is USA or Canada.">*</span></cms:Label>
                                    </td>
                                    <td nowrap>
                                        <cms:TextBox ID="bankNewAddressPostalCodeTxt" Display="Editable" runat="server" MaxLength="20" onchange="ClearAddressValidationFlag();"
                                            CssClass="inputTextLarge form_margin">
                                        </cms:TextBox>
                                        <cms1:CustomValidator ID="bankNewAddressPostalCodeTxtCustomvalidator" runat="server"
                                            Display="None" Enabled="False" LabelControl="bankNewAddrPostalCodeLabel" ClientValidationFunction="ClientPostalCodeValidate"
                                            ErrorMessage="Postal Code is required, if Country is USA or Canada." ControlToValidate="bankNewAddressCountryTxt"
                                            ErrorCssClass="blue">
                                        </cms1:CustomValidator>
                                        <cms1:CustomValidator ID="USPostalFormatCustomvalidator" runat="server" Display="None"
                                            Enabled="False" LabelControl="bankNewAddrPostalCodeLabel" ClientValidationFunction="ClientUSFormatPostalCodeValidate"
                                            ErrorMessage="Invalid Postal Code." ControlToValidate="bankNewAddressCountryTxt">
                                        </cms1:CustomValidator>
                                      
                                        <cms1:RegularExpressionValidator ID="bankNewAddressPostalCodeTxtRegExpvalidator"
                                            runat="Server" ValidationExpression="[a-z,A-Z,0-9/\n\s.\-?:()'+']*" LabelControl="bankNewAddrPostalCodeLabel"
                                            ControlToValidate="bankNewAddressPostalCodeTxt" Display="None" Enabled="false"
                                            ErrorMessage="Entered Characters are not allowed for Postal code." ErrorCssClass="red">
                                        </cms1:RegularExpressionValidator>
                                        <cms1:CustomValidator ID="bankAdressCountryCompareValidator" runat="server" Display="None"
                                            Enabled="False" LabelControl="bankNewAddrCountryLabel,bankNewAddrCountryLabel_Supplier,bankCountryCaptionLabel" ClientValidationFunction="CompareBankCountry"
                                            ErrorMessage="Country of Bank and entered Country should be same." ControlToValidate="bankCountryNameDDL">
                                        </cms1:CustomValidator>
                                        <cms1:customvalidator id="CanadianPostalFormatCustomvalidator" ResourceName="19695.RemitToAddress" labelcontrol="PostalCodeLabel"
                                        runat="server" Display="None" ErrorMessage="Invalid Postal Code." ClientValidationFunction="ValidateCanadianPostalCode"                                   
					Enabled="True" ErrorCssClass="red"></cms1:customvalidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="heading4right" nowrap="nowrap">
                            <cms:Label ID="IBANAccNoLabel" Display="Editable" runat="server">Account Number/IBAN <span class="required_red" title="Account Number/IBAN is required.">*</span></cms:Label>
                        </td>
                        <td>
                            <cms:TextBox ID="IBANAccountNumberTxt" Display="Editable" runat="server" MaxLength="40"
                                CssClass="inputTextLarge form_margin">
                            </cms:TextBox>
                            <cms1:RequiredFieldValidator ID="IBANAccountNumberTxtReqValidator" runat="server"
                                Display="None" Enabled="false" ErrorMessage="Account Number/IBAN is required."
                                ControlToValidate="IBANAccountNumberTxt" LabelControl="IBANAccNoLabel">
                            </cms1:RequiredFieldValidator>
                            <cms1:RegularExpressionValidator ID="IBANAccountNumberTxtRegExpValidator" runat="Server"
                                ValidationExpression="[a-z,A-Z,0-9/\n\s.\-?:()'+']*" LabelControl="IBANAccNoLabel"
                                ControlToValidate="IBANAccountNumberTxt" Display="None" Enabled="false" ErrorMessage="Entered Characters are not allowed for Account Number/IBAN."
                                ErrorCssClass="red">
                            </cms1:RegularExpressionValidator>
                        </td>
                        <td class="heading4right">
                            <cms:Label ID="reTypeIBANAccNoLabel" Display="Editable" runat="server">Retype Account Number/IBAN <span class="required_red" title="ReType Account Class is required.">*</span></cms:Label>
                        </td>
                        <td>
                            <cms:TextBox ID="reTypeIBANAccountNumberTxt" Display="Editable" runat="server" MaxLength="40"
                                CssClass="inputTextLarge form_margin" oncopy="return false" onpaste="return false" oncut="return false">
                            </cms:TextBox>                         
                            <cms1:RegularExpressionValidator ID="reTypeIBANAccountNumberTxtRegExpValidator" runat="Server"
                                ValidationExpression="[a-z,A-Z,0-9/\n\s.\-?:()'+']*" LabelControl="reTypeIBANAccNoLabel"
                                ControlToValidate="reTypeIBANAccountNumberTxt" Display="None" Enabled="false"
                                ErrorMessage="Entered Characters are not allowed for ReType Account Number/IBAN."
                                ErrorCssClass="red">
                            </cms1:RegularExpressionValidator>
                            <cms1:CustomValidator ID="reTypeIBANAccountNumberCustomValidator" runat="server"
                                Display="None" Enabled="false" ErrorMessage="Account Numbers entered do not match, please re-enter the information."
                                ClientValidationFunction="ValidatereTypeIBANAccountNumber" ValidateEmptyText="true"
                                ErrorCssClass="blue">
                            </cms1:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="heading4right" nowrap colspan="1">
                            <cms:Label ID="accountHolderLabel" Display="Editable" runat="server">Account Holder Name(s) <span class="required_red" title="Account Holder Name(s) is required">*</span></cms:Label>
                        </td>
                        <td id="accountHolderrowtxt" runat="server" colspan="3">
                            <cms:TextBox ID="accountHolderTxt" Display="Editable" runat="server" MaxLength="70"
                                CssClass="inputTextLarge form_margin">
                            </cms:TextBox>
                            <cms1:RequiredFieldValidator ID="accountHolderTxtReqValidator" runat="server" Display="None"
                                Enabled="false" ErrorMessage="Account Holder Name(s) is required." ControlToValidate="accountHolderTxt"
                                LabelControl="AccountHolderLabel"></cms1:RequiredFieldValidator>
                            <cms1:RegularExpressionValidator ID="accountHolderTxtRegExpValidator" runat="Server"
                                ValidationExpression="([a-z,A-Z,0-9/\n\s.\-?:()'+']*[^\\x20-\\x7E].*)" LabelControl="accountHolderLabel"
                                ControlToValidate="accountHolderTxt" Display="None" Enabled="false" ErrorMessage="Entered Characters are not allowed for Account Holder Name(s)."
                                ErrorCssClass="red">
                            </cms1:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr id="bankAddlInfoDiv" runat="server">
                        <td class="heading4right" colspan="2">
                            &nbsp;&nbsp;
                        </td>
                        <td class="heading4right" colspan="2" align="right">
                            <cms:Label ID="accountHolderStaticLabel" Display="Editable" runat="server">(Enter Name(s) exactly as they appear on Bank Account)</cms:Label>
                        </td>
                    </tr>
                    <tr id="federalTaxNumRow" runat="server" visible="false">
                    <td></td>
                    <td></td>
                        <td id="federalTaxNum" class="heading4right" nowrap>
                            <cms:Label ID="federalTaxNumLabel" Display="Editable" runat="server">CPF Number 
                                <span class="required_red" title="Federal Tax Number is required">*</span>
                            </cms:Label>
                        </td>
                       <td id="FederalTaxNUmValue" runat="server" >
                            <cms:TextBox ID="federalTaxNumTxt" Display="Editable" runat="server"
                                CssClass="inputTextLarge form_margin">
                            </cms:TextBox>
                           
                        <cms1:CustomValidator ID="FederalTaxNumCustomVal" runat="server"
                                Display="None" Enabled="False" LabelControl="federalTaxNumLabel" ClientValidationFunction="federalTaxVal"
                                ErrorMessage="Federal Tax Number is required. " ControlToValidate="federalTaxNumTxt"
                                ErrorCssClass="red" ValidateEmptyText="true">
                        </cms1:CustomValidator>
                        <cms1:CustomValidator ID="FederalTaxIdCustomVal" runat="server" Display="None"
                                Enabled="false" LabelControl="federalTaxNumLabel" ClientValidationFunction="ValidateFederalID" 
                                ErrorMessage="CPF Number must be 11 digits. " ControlToValidate="federalTaxNumTxt">
                        </cms1:CustomValidator>

                            <cms1:RegularExpressionValidator ID="FederalTaxNumRegexVal" runat="Server"
                                ValidationExpression="[a-z,A-Z,0-9/\n\s.\-?:()'+']*" LabelControl="federalTaxNumLabel"
                                ControlToValidate="federalTaxNumTxt" Display="None" Enabled="false" ErrorMessage="Entered Characters are not allowed for Federal Tax Number."
                                ErrorCssClass="red">
                            </cms1:RegularExpressionValidator>
                            <cms1:RegularExpressionValidator id="FederalTaxIDMaxLengthVal" runat="server"
                                display="None" labelcontrol="federalTaxNumLabel" controltovalidate="federalTaxNumTxt" Enabled="false"
                                validationexpression="^(.|\n){0,11}$" errormessage="CPF Number cannot be more than 11 characters.">
                            </cms1:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr id="PANTANrow" runat="server" visible="false">
                        <td class="heading4right">
                            <cms:Label ID="PANLabel" runat="server" Display="Editable">PAN
                                <span id="PANspan" runat="server" class="required_blue" title="PAN is required.">*</span></cms:Label>
                        </td>
                        <td>
                            <cms:TextBox ID="PANTextBox" runat="server" Display="Editable" CssClass="inputTextLarge" 
                                onkeypress="RestrictTextAreaLength( this,10);" onblur="RestrictTextAreaLength( this,10);"
                                onpaste="CheckPastePAN(this.id,event);" MaxLength="10"></cms:TextBox>
                             <cms1:RegularExpressionValidator ID="PANRegExprValidator" runat="server" ControlToValidate="PANTextBox" Enabled="false" 
                                Display="None" LabelControl="PANLabel" ValidationExpression="[a-zA-Z]{5}\d{4}[a-zA-Z]{1}" ErrorMessage="Please enter a valid PAN."></cms1:RegularExpressionValidator>
 
                        </td>
                        <td class="heading4right">
                            <cms:Label ID="TANLabel" runat="server" Display="Editable">TAN
                                <span id="TANspan" runat="server" class="required_blue" title="TAN is required.">*</span></cms:Label>
                        </td>
                        <td>
                            <cms:TextBox ID="TANTextBox" runat="server" Display="Editable" CssClass="inputTextLarge" 
                                onkeypress="RestrictTextAreaLength( this,10);" onblur="RestrictTextAreaLength( this,10);"
                                onpaste="CheckPastePAN(this.id,event);" MaxLength="10"></cms:TextBox>
                             <cms1:RegularExpressionValidator ID="TANRegExpValidator" runat="server" ControlToValidate="TANTextBox" Enabled="false" 
                                Display="None" LabelControl="TANLabel" ValidationExpression="[a-zA-Z]{4}\d{5}[a-zA-Z]{1}" ErrorMessage="Please enter a valid TAN."></cms1:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr id="stateAndCityTaxRow" runat="server" visible="false">
                        <td class="heading4right" nowrap>
                            <cms:Label ID="stateTaxIdLabel" Display="Editable" runat="server">State Tax ID</cms:Label>
                        </td>
                       <td id="stateTaxIdValue" runat="server" >
                            <cms:TextBox ID="stateTaxIDTxt" Display="Editable" runat="server" MaxLength="30"
                                CssClass="inputTextLarge form_margin">
                            </cms:TextBox>
                            <cms1:RegularExpressionValidator ID="StateTaxIDRegExVal" runat="Server"
                                ValidationExpression="[a-z,A-Z,0-9/\n\s.\-?:()'+']*" LabelControl="stateTaxIdLabel"
                                ControlToValidate="stateTaxIDTxt" Display="None" Enabled="false" ErrorMessage="Entered Characters are not allowed for State Tax ID."
                                ErrorCssClass="red">
                            </cms1:RegularExpressionValidator>
                            <cms1:RegularExpressionValidator id="StateTaxIdMaxLenghtVal" runat="server"
                                display="None" labelcontrol="stateTaxIdLabel" controltovalidate="stateTaxIDTxt" Enabled="false"
                                validationexpression="^(.|\n){0,30}$" errormessage="State Tax ID cannot be more than 30 characters">
                            </cms1:RegularExpressionValidator>
                        </td>
                         <td class="heading4right" nowrap>
                            <cms:Label ID="CityTaxIDLabel" Display="Editable" runat="server">City Tax ID</cms:Label>
                        </td>
                       <td id="CityTaxIdValue" runat="server" >
                            <cms:TextBox ID="CityTaxIDTxt" Display="Editable" runat="server" MaxLength="12"
                                CssClass="inputTextLarge form_margin">
                            </cms:TextBox>
                            <cms1:RegularExpressionValidator ID="CityTaxIDRegexVal" runat="Server"
                                ValidationExpression="[a-z,A-Z,0-9/\n\s.\-?:()'+']*" LabelControl="CityTaxIDLabel"
                                ControlToValidate="CityTaxIDTxt" Display="None" Enabled="false" ErrorMessage="Entered Characters are not allowed for City Tax ID."
                                ErrorCssClass="red">
                            </cms1:RegularExpressionValidator>
                            <cms1:RegularExpressionValidator id="CityTaxIdMaxLenghtVal" runat="server"
                                display="None" labelcontrol="CityTaxIDLabel" controltovalidate="CityTaxIDTxt" Enabled="false"
                                validationexpression="^(.|\n){0,12}$" errormessage="City Tax ID cannot be more than 12 characters">
                            </cms1:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="heading4right">
                            <cms:Label ID="accountTypeLabel" Display="Editable" runat="server">Account Type <span class="required_red" title="Account Type is required.">*</span></cms:Label>
                        </td>
                        <td>
                            <cms:DropDownList ID="accountTypeDDL" runat="server" Width="155" Display="editable"
                                CssClass="inputText form_margin">
                            </cms:DropDownList>
                            <cms1:RequiredFieldValidator ID="accountTypeDDLReqvalidator" runat="server" Display="None"
                                Enabled="false" ErrorMessage="Account Type is required." ControlToValidate="accountTypeDDL"
                                LabelControl="accountTypeLabel"></cms1:RequiredFieldValidator>
                        </td>
                        <td class="heading4right">
                            <cms:Label ID="accountClassLabel" Display="Editable" runat="server">Account Class <span class="required_red" title="Account Class is required.">*</span></cms:Label>
                        </td>
                        <td>
                            <cms:DropDownList ID="accountClassDDL" runat="server" Width="140" Display="editable"
                                CssClass="inputText form_margin" AutoPostBack="true" OnSelectedIndexChanged="AccountClassDDL_OnSelectedIndexChanged">
                            </cms:DropDownList>
                            <cms1:RequiredFieldValidator ID="accountClassDDLReqvalidator" runat="server" Display="None"
                                Enabled="false" ErrorMessage="Account Class is required." ControlToValidate="accountClassDDL"
                                LabelControl="accountClassLabel">
                            </cms1:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="paymentCategoryTR">
                        <td class="heading4right" nowrap="nowrap">
                            <cms:Label Display="Editable" runat="server" ID="paymentcatLabel" CssClass="heading4">Payment Category <span class="required_red" title="Payment Category is Required">*</span></cms:Label>
                        </td>
                        <td style="padding-left:9px;">
                            <div class="listBox" style="width: 140px; height: 100px" id="paymentCategoryDiv" runat="server">
                                <cms:CheckBoxList Display="Editable" runat="server" ID="paymentcategoryCBL" VisibleRows="5"
                                    OutputLabelIfReadonly="true">
                                </cms:CheckBoxList>
                                <cms1:RequiredFieldValidator ID="paymentcategoryCBLRequiredFieldValidator"
                                    runat="server" Display="None" ControlToValidate="paymentcategoryCBL"
                                    ErrorCssClass="red" ErrorMessage="Payment Category required." LabelControl="paymentcatLabel"
                                    Enabled="false" ></cms1:RequiredFieldValidator>
                            </div>
                        </td>
                        <td class="heading4right">
                            <cms:Label Display="Editable" runat="server" ID="paymentcatdescrLabel" CssClass="heading4">Payment Category Description</cms:Label>
                        </td>
                        <td>
                            <cms:TextBox ID="paymentcatdescrTxt" class="textArea" Display="Editable" runat="server"
                                Width="150" MaxLength="250" TextMode="MultiLine" Columns="116" Rows="6">
                            </cms:TextBox>
                            <cms1:RegularExpressionValidator ID="paymentcatdescrTxtRegExpValidator" runat="Server"
                                ValidationExpression="[a-z,A-Z,0-9/\n\s.\-?:()'+']*" LabelControl="paymentcatdescrLabel"
                                ControlToValidate="paymentcatdescrTxt" Display="None" Enabled="false" ErrorMessage="Entered Characters are not allowed for Payment Category Description."
                                ErrorCssClass="red">
                            </cms1:RegularExpressionValidator>
                            <cms1:regularexpressionvalidator id="paymentcatdescrTxtMaxLengthValidator" runat="server"
                                display="None" labelcontrol="paymentcatdescrLabel" controltovalidate="paymentcatdescrTxt" Enabled="false"
                                validationexpression="^(.|\n){0,255}$" errormessage="Payment Category Description cannot be more than 255 characters">
                            </cms1:regularexpressionvalidator>
                        </td>
                    </tr>
                    <tr id="customerPayrollTR">
                        <td class="heading4right" nowrap>
                            <cms:Label ID="globalPayrollCheckLabel" runat="server" Display="Editable">Global Payroll&nbsp;&nbsp;&nbsp;</cms:Label>
                        </td>
                        <td align="left">
                            <cms:CheckBox ID="globalPayrollCheck" runat="server" Display="Editable"></cms:CheckBox>
                            <cms1:CustomValidator ID="globalPayrollCheckCustomValidator" runat="server" Display="None"
                                LabelControl="globalPayrollCheckLabel" Enabled="false" ErrorMessage="Payroll Name is required if Global Payroll is checked."
                                ControlToValidate="globalPayrollCheck" ClientValidationFunction="ValidatePayrollName"
                                ErrorCssClass="blue">
                            </cms1:CustomValidator>
                        </td>
                        <td class="heading4right" nowrap>
                            <cms:Label ID="payrollNameLabel" Display="Editable" runat="server">Payroll Name <span class="required_blue" title="Payroll Name is required if Global Payroll is checked">*</span></cms:Label>
                        </td>
                        <td>
                            <cms:TextBox ID="payrollNameTxt" runat="server" CssClass="inputTextLarge form_margin"
                                Display="Editable">
                            </cms:TextBox><cms:Widget ID="countryWidget" runat="server" Display="Readonly">
                                <a id="countryLink" onclick="CallPayRollCountryFinder();return false;" href="#">
                                    <img height="14" alt="Location Finder" src="/images/global/icons/Location_finder.gif"
                                        width="14" border="0"></a></cms:Widget>
                            <cms1:CountryValidator ID="payrollNameTxtCountryValidator" runat="server" Display="None"
                                Enabled="false" ErrorMessage="Invalid Payroll Name." ControlToValidate="payrollNameTxt"
                                LabelControl="payrollNameLabel">
                            </cms1:CountryValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="heading4right">
                            <cms:Label ID="additionalBankInformationLabel" Display="Editable" runat="server">Additional Information for the Bank&nbsp;&nbsp;&nbsp;</cms:Label>
                        </td>
                        <td colspan="3">
                            <cms:TextBox ID="specialInstructionsTxt" class="textArea" Display="Editable" runat="server"
                                Width="500" MaxLength="250" TextMode="MultiLine" Columns="116" Rows="4">
                            </cms:TextBox>
                            <cms1:RegularExpressionValidator ID="specialInstructionsRegExpValidator" runat="Server"
                                ValidationExpression="[a-z,A-Z,0-9/\n\s.\-?:()'+']*" LabelControl="additionalBankInformationLabel"
                                ControlToValidate="specialInstructionsTxt" Display="None" Enabled="false" ErrorMessage="Entered Characters are not allowed for Additional Information for the Bank."
                                ErrorCssClass="red">
                            </cms1:RegularExpressionValidator>
                            <cms1:regularexpressionvalidator id="specialInstructionsTxtMaxValidator" runat="server"
                                display="None" labelcontrol="additionalBankInformationLabel" controltovalidate="specialInstructionsTxt" Enabled="false"
                                validationexpression="^(.|\n){0,255}$" errormessage="Additional Information for the Bank cannot be more than 255 characters">
                            </cms1:regularexpressionvalidator>

                        </td>
                    </tr>
                    <tr class="border_bottom">
                        <td class="heading4right" colspan="3">
                            <cms:Label ID="paythroughbankneededlabel" runat="server" Display="Editable">Pay Through Bank is Needed</cms:Label>
                        </td>
                        <td>
                            <cms:CheckBox ID="paythroughBankNeededCheck" runat="server" Display="Editable" Checked="true">
                            </cms:CheckBox>
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
        <tr id="paythroughBankTR" runat="server">
            <td colspan="4" align="left">
                <BankControl:PayThroughBankControl ID="payThroughBankUC" runat="server" />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="displayRoutingNoDivHidden" runat="server" />
    <asp:HiddenField ID="displayBankInfoDivHidden" runat="server" />
    <asp:HiddenField ID="bankRoutingTypCdHidden" runat="server" />
    <asp:HiddenField ID="bankAddressFoundHidden" runat="server" />
    <asp:HiddenField ID="errorTextHidden" runat="server" />
    <asp:HiddenField ID="paythroughbankReqIndHidden" runat="server" />
    <asp:HiddenField ID="routingNoRequiredIndHidden" runat="server" />
    <asp:HiddenField ID="isABANumbrvalidHidden" runat="server" />
    <asp:HiddenField ID="isABANumberAlphaHidden" runat="server" />
    
</div>
