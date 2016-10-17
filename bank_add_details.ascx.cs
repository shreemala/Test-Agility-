#region System NameSpaces
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

#endregion

#region CMS NameSpaces
using CMS.ATLAS.MW.Address.Entity.Datasets;
using CMS.ATLAS.MW.EdgeUser;
using CMS.Atlas.Web.Controls.WebForm;
using CMS.Atlas.Web.BasePages;

using CMS.ATLAS.MW.FinanceDetails.IFacade;
using CMS.Codes;

#if DEBUG
using CMS.ATLAS.MW.FinanceDetails.Facade;
#endif

using CMS.ATLAS.MW.Address.Meta.Datasets;
using CMS.ATLAS.MW.UserMessages;
using CMS.ATLAS.MW.FinanceDetails.Entity.DataSets;
using CMSWebForm = CMS.Atlas.Web.Controls.WebForm;
using CMS.ATLAS.MW.Employee.Facade;
using CMS.ATLAS.MW.Employee.IFacade;
using CMS.ATLAS.MW.Employee.Entity.Datasets;

#endregion

namespace CMS.Atlas.Web.UI.asp.global.controls
{
    public partial class bank_add_details : System.Web.UI.UserControl
    {
        #region Private Variables 
        private const int Zero = 0;
        #endregion

        #region Public Properties

        public int RequestedOracleStatus
        {
            get
            {   
                if (ViewState["RequestedOracleStatusLocal"] != null)
                    return int.Parse(ViewState["RequestedOracleStatusLocal"].ToString());
                return 0;
            }
            set
            {
                ViewState["RequestedOracleStatusLocal"] = value;
            }
        }

        #region Enum List
        public enum BankTypeList
        {
            Supplier = 0,
            Customer,
            Division,
            Geo
        };

        public enum EditModeList
        {
            Add = 0,
            Edit
        };
        #endregion

        #region Current Page
        private BasePages.BasePage CurrentPage
        {
            get { return (BasePages.BasePage)Page; }
        }
        #endregion Current Page

        #region AccountHolderName
        public string AccountHolderName
        {
            get
            {
                if (ViewState["AccountHolderName"] != null)
                    return ViewState["AccountHolderName"].ToString();
                return "";
            }
            set
            {
                ViewState["AccountHolderName"] = value;
            }
        }
        #endregion

        #region Bank Account Id
        public int BankAccountId
        {
            get
            {
                if (Request.QueryString["BankAcctId"] != null)
                    return int.Parse("0" + Request.QueryString["BankAcctId"].ToString());
                return 0;
            }
        }
        #endregion

        #region Paythrough Bank Id
        public int PayThroughBankAcctId
        {
            get
            {
                if (Request.QueryString["PayThroughBankAcctId"] != null)
                    return int.Parse("0" + Request.QueryString["PayThroughBankAcctId"].ToString());
                return 0;
            }
        }
        #endregion

        #region ColorNumber
        public int ColorNumber
        {
            get
            {
                if (ViewState["ColorNumberLocal"] != null)
                    return int.Parse(ViewState["ColorNumberLocal"].ToString());
                else if (Request.QueryString["Color"] != null)
                    return int.Parse(Request.QueryString["Color"].ToString());
                return 1;
            }
            set
            {
                ViewState["ColorNumberLocal"] = value;
            }
        }
        #endregion

        #region Bank Type
        public BankTypeList BankType
        {
            get
            {
                if (ViewState["BankTypeLocal"] != null)
                    return (BankTypeList)(ViewState["BankTypeLocal"]);
                return BankTypeList.Supplier;
            }
            set
            {
                ViewState["BankTypeLocal"] = value;
            }
        }
        #endregion

        #region GeoOrigingCd
        public int GeoOrigingCd
        {
            get
            {
                if (ViewState["GeoOriginCdLocal"] != null)
                    return int.Parse("0" + ViewState["GeoOriginCdLocal"].ToString());
                return 0;
            }
            set
            {
                ViewState["GeoOriginCdLocal"] = value;
            }

        }
        #endregion

        #region EditMode
        public EditModeList EditMode
        {
            get
            {
                if (ViewState["EditModeLocal"] != null)
                    return (EditModeList)(ViewState["EditModeLocal"]);
                return EditModeList.Add;
            }
            set
            {
                ViewState["EditModeLocal"] = value;
            }
        }
        #endregion

        #region FileId
        public int FileId
        {
            get
            {
                if (Request.QueryString["FileId"] != null)
                    return int.Parse(Request.QueryString["FileId"].ToString());
                return 0;
            }
        }
        #endregion

        #region BankSIRemoteObject
        IBankSI _bankSI;
        private IBankSI BankSIRemoteObject
        {
            get
            {
                if (_bankSI == null)
                {
#if DEBUG
                    _bankSI = new BankSI();
#else
                    string remoteURL = System.Configuration.ConfigurationManager.AppSettings["RemotingServer"];
                    _bankSI = (IBankSI)Activator.GetObject(typeof(IBankSI), remoteURL + "CMS.ATLAS.MW.FinanceDetails.Facade.BankSI.rem");
#endif
                }
                return _bankSI;
            }
        }

        #endregion

        #region BankDS
        public BankDS BankDataSet
        {
            get
            {
                if (ViewState["BankDS_Local"] != null)
                    return (BankDS)ViewState["BankDS_Local"];
                return new BankDS();
            }
            set
            {
                ViewState["BankDS_Local"] = value;
            }
        }
        #endregion

        #region BusnPartId
        public int BusnPartId
        {
            get
            {
                if (ViewState["BusnPartId_Local"] != null)
                    return int.Parse("0" + ViewState["BusnPartId_Local"].ToString());
                return 0;
            }
            set
            {
                ViewState["BusnPartId_Local"] = value;
            }
        }
        #endregion

        #region BusnPartEmpId
        public int BusnPartEmpId
        {
            get
            {
                if (ViewState["BusnPartEmpId_Local"] != null)
                    return int.Parse("0" + ViewState["BusnPartEmpId_Local"].ToString());
                return 0;
            }
            set
            {
                ViewState["BusnPartEmpId_Local"] = value;
            }
        }
        #endregion

        #region CountryName
        public string CountryName
        {
            get
            {
                if (bankCountryNameDDL != null)
                {
                    if (bankCountryNameDDL.SelectedItem != null)
                        return bankCountryNameDDL.SelectedItem.Text;
                    return "";
                }
                return "";
            }
        }
        #endregion

        #region PageLoadedAlready
        public bool PageLoadedAlready
        {
            get
            {
                if (ViewState["PageLoadedAlready_Local"] != null)
                    return (bool)ViewState["PageLoadedAlready_Local"];
                return true;
            }
            set
            {
                ViewState["PageLoadedAlready_Local"] = value;
            }
        }
        #endregion

        #region PayThroughBankDisplay
        public bool PayThroughBankDisplay
        {
            get
            {
                if (ViewState["PayThroughBankDisplay"] != null)
                    return (bool)(ViewState["PayThroughBankDisplay"]);
                return false;
            }
            set
            {
                ViewState["PayThroughBankDisplay"] = value;
            }
        }
        #endregion

        #region DisplaySupplierActionPanel
        public bool DisplaySupplierActionPanel
        {
            get
            {
                if (ViewState["DisplaySupplierActionPanel_Local"] != null)
                    return bool.Parse(ViewState["DisplaySupplierActionPanel_Local"].ToString());

                return false;
            }
            set
            {
                ViewState["DisplaySupplierActionPanel_Local"] = value;
            }
        }
        #endregion

        #region InvalidAddressDisplay
        public bool InvalidAddressDisplay
        {
            get
            {
                if (ViewState["InvalidAddressDisplay_Local"] != null)
                    return (bool)ViewState["InvalidAddressDisplay_Local"];
                return false;
            }
            set
            {
                ViewState["InvalidAddressDisplay_Local"] = value;
            }
        }
        #endregion

        #region BaseTabIndex
        public short BaseTabIndex
        {
            get
            {
                if (ViewState["BaseTabIndex"] != null)
                    return short.Parse("0" + ViewState["BaseTabIndex"].ToString());
                return 0;
            }
            set
            {
                ViewState["BaseTabIndex"] = value;
            }
        }
        #endregion

        #region VendorMasterFlag

        public int VendorMasterFlag
        {
            get
            {
                if (Request.QueryString["VendorMasterFlag"] != null)
                    return int.Parse("0" + Request.QueryString["VendorMasterFlag"].ToString());
                return 0;
            }
        }

        #endregion

        #region Bank Name
        public string BankName
        {
            get
            {
                if (ViewState["BankName_Local"] != null)
                    return ViewState["BankName_Local"].ToString();
                return "";
            }
            set
            {
                ViewState["BankName_Local"] = value;
            }
        }
        #endregion

        #region InvalidBank
        public bool InvalidBank
        {
            get
            {
                if (ViewState["InvalidBank_Local"] != null)
                    return bool.Parse(ViewState["InvalidBank_Local"].ToString());
                return false;
            }
            set
            {
                ViewState["InvalidBank_Local"] = value;
            }
        }
        #endregion

        #region SelectedCountryCd
        public int SelectedCountryCd
        {
            get
            {
                if (ViewState["SelectedCountryCd_Local"] != null)
                    return int.Parse(ViewState["SelectedCountryCd_Local"].ToString());
                return 0;
            }
            set
            {
                ViewState["SelectedCountryCd_Local"] = value;
            }
        }
        #endregion 

        #region CurrCd
        public int CurrCd
        {
            get
            {
                if (ViewState["CurrCd"] != null)
                    return int.Parse(ViewState["CurrCd"].ToString());
                return 0;
            }
            set
            {
                ViewState["CurrCd"] = value;
            }
        }
        #endregion

        #region EmployeeObject
        IEmployeeSI _employeeSI;
        private IEmployeeSI EmployeeObject
        {
            get
            {
                if (_employeeSI == null)
                {
#if DEBUG
                    _employeeSI = new EmployeeSI();
#else
                    string remoteURL = System.Configuration.ConfigurationManager.AppSettings["RemotingServer"];
                    _employeeSI = (IEmployeeSI)Activator.GetObject(typeof(IEmployeeSI), remoteURL + "CMS.ATLAS.MW.Employee.Facade.EmployeeSI.rem");
#endif
                }
                return _employeeSI;
            }
        }

        #endregion

     // private bool manualEntry = false;
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageLoadedAlready = !this.PageLoadedAlready;

                if (!this.PageLoadedAlready)
                {
                    LoadDropDown();
                    SetControlsTabIndex();
                }
                HideBankNotFoundMessage(false);
            }

          if (CanadianPostalFormatCustomvalidator.Attributes["CountryToValidate"] == null)
                CanadianPostalFormatCustomvalidator.Attributes.Add("CountryToValidate", bankNewAddressCountryTxt.ClientID);
            if (CanadianPostalFormatCustomvalidator.Attributes["ProvinceToValidate"] == null)
                CanadianPostalFormatCustomvalidator.Attributes.Add("ProvinceToValidate", bankNewAddresssStateProvTxt.ClientID);
            if (CanadianPostalFormatCustomvalidator.Attributes["ControlToValidate"] == null)
                CanadianPostalFormatCustomvalidator.Attributes.Add("ControlToValidate", bankNewAddressPostalCodeTxt.ClientID);      
            if (reTypeIBANAccountNumberCustomValidator.Attributes["ControlToValidate"] == null)
                reTypeIBANAccountNumberCustomValidator.Attributes.Add("ControlToValidate", reTypeIBANAccountNumberTxt.ClientID);     

            invalidAddressLabel.Visible = this.InvalidAddressDisplay;

            if (this.Visible)
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "DisplayDivOnPageLoad", "<script> DisplayControlOnPageLoad();</script>", false);

            if (this.BankType == BankTypeList.Customer && this.BankAccountId > 0)
                addressHeaderLabel.Visible = true;
                //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "DisplayEditMsgOnPageLoad", "<script> DisplayBankEditInstruction();</script>", false);
            else
                addressHeaderLabel.Visible = false;

            if (this.BankAccountId > 0 && this.BankType != BankTypeList.Supplier)
                DisableControlOnEditMode();

            addressSupplierLabel.Visible = (this.BankType == BankTypeList.Supplier && this.BankAccountId > 0);

            paythroughBankTR.Visible = PayThroughBankDisplay;
            if (this.PayThroughBankDisplay)
                payThroughBankUC.RenderControlAsReadOnly();

            bankAddlInfoDiv.Visible = (this.BankType == BankTypeList.Customer);
            //Fix for ICRAS00101023 & ICRAS00101025-starts

            if (this.BankType == BankTypeList.Supplier || this.VendorMasterFlag == 1|| this.BankAccountId > 0)
            {
                bankStatusDDL.Enabled = false;
            }

            //Fix for ICRAS00101023 & ICRAS00101025-ends

            //Fix for ICRAS00101190 & ICRAS00101191 -starts

            if (CurrentPage.IsReadOnly == true && (this.BankType != BankTypeList.Division && this.BankType != BankTypeList.Geo))
            {

                if (this.BankDataSet.tblBankAcct.Rows.Count > 0)
                {
                    bool noBankRole = true;
                    bool noGenpactRole = true;
                    bool maskBankInstruction = true;

                    EmployeeObject.GetUserSecRoles(CurrentPage.User, Convert.ToInt32(CurrentPage.User.UserId), out noBankRole, out noGenpactRole, out maskBankInstruction, (this.BankType == BankTypeList.Supplier));

                    int strLen = 0;
                    string maskchars = "";

                    if ((noBankRole && this.BankType == BankTypeList.Customer) || (noBankRole && noGenpactRole && this.BankType == BankTypeList.Supplier))
                    {
                        #region specialInstructionsTxt
                        if (specialInstructionsTxt.Text.Length > 0 && maskBankInstruction)
                        {
                            strLen = specialInstructionsTxt.Text.Length;
                            for (int i = 0; i <= strLen; i++)
                            {
                                maskchars = maskchars + "*";
                            }
                            specialInstructionsTxt.Text = maskchars;
                        }
                        #endregion

                        #region IBANAccountNumberTxt
                        if (IBANAccountNumberTxt.Text.ToString().Trim().Length > 4)
                        {
                            IBANAccountNumberTxt.Text = "********" + IBANAccountNumberTxt.Text.ToString().Trim().Substring(IBANAccountNumberTxt.Text.ToString().Trim().Length - 4, 4);
                        }
                        else
                        {
                            IBANAccountNumberTxt.Text = Convert.ToString(IBANAccountNumberTxt.Text).ToString();
                        } 
                        #endregion

                        #region reTypeIBANAccountNumberTxt
                        if (reTypeIBANAccountNumberTxt.Text.ToString().Trim().Length > 4)
                        {
                            reTypeIBANAccountNumberTxt.Text = "********" + reTypeIBANAccountNumberTxt.Text.ToString().Trim().Substring(reTypeIBANAccountNumberTxt.Text.ToString().Trim().Length - 4, 4);
                        }
                        else
                        {
                            reTypeIBANAccountNumberTxt.Text = Convert.ToString(reTypeIBANAccountNumberTxt.Text).ToString();
                        } 
                        #endregion

                    }
                }
            }
            //Fix for ICRAS00101190 & ICRAS00101191 -Ends
            stateAndCityTaxRow.Visible = false;
            federalTaxNumRow.Visible = false;
            if (this.GeoOrigingCd != 0)
            {
                if ((this.GeoOrigingCd == CMS.Codes.EDGE.GeoOriginTypCd.Brazil && (this.BankType == BankTypeList.Customer || this.BankType == BankTypeList.Supplier)) || this.BankType == BankTypeList.Division)
                {
                    federalTaxNumRow.Visible = true;
                    if (accountClassDDL.SelectedValue == CMS.Codes.EDGE.USACHCd.ACHP.ToString())
                    {
                        if (this.BankType != BankTypeList.Division)
                        {
                            federalTaxNumLabel.Text = "CPF Number" + " <span class='required_red' title='CPF Number is required.'>*</span>";
                        }
                        else
                        {
                            federalTaxNumLabel.Text = "CPF Number";
                        }
                        federalTaxNumTxt.MaxLength = 11;
                        FederalTaxNumCustomVal.ErrorMessage = "CPF Number is required.";
                        FederalTaxNumRegexVal.ErrorMessage = "Entered Characters are not allowed for CPF Number.";
                        FederalTaxIDMaxLengthVal.ValidationExpression = "^(.|\n){0,11}$";
                        FederalTaxIDMaxLengthVal.ErrorMessage = "CPF Number cannot be more than 11 characters.";
                    }
                    else if (accountClassDDL.SelectedValue == CMS.Codes.EDGE.USACHCd.ACHC.ToString())
                    {
                        if (this.BankType != BankTypeList.Division)
                        {
                            federalTaxNumLabel.Text = "CNPJ Number" + "<span class='required_red' title='CNPJ Number is required.'>*</span>";
                        }
                        else
                        {
                            federalTaxNumLabel.Text = "CNPJ Number";
                        }
                        federalTaxNumTxt.MaxLength = 14;
                        FederalTaxNumCustomVal.ErrorMessage = "CNPJ Number is required.";
                        FederalTaxNumRegexVal.ErrorMessage = "Entered Characters are not allowed for CNPJ Number.";
                        FederalTaxIDMaxLengthVal.ValidationExpression = "^(.|\n){0,14}$";
                        FederalTaxIDMaxLengthVal.ErrorMessage = "CNPJ Number cannot be more than 14 characters.";
                    }
                    if (this.BankType == BankTypeList.Division || this.BankType == BankTypeList.Supplier)
                    {
                        stateAndCityTaxRow.Visible=true;
                    }
                }
                if (this.GeoOrigingCd == CMS.Codes.EDGE.GeoOriginTypCd.India || this.BankType == BankTypeList.Division)
                {
                    PANTANrow.Visible = true;
                    if (this.BankType == BankTypeList.Division)
                    {
                        PANLabel.Text = "PAN";
                        TANLabel.Text = "TAN";
                    }
                }
                //if (FederalTaxNumReqVal != null && federalTaxNumRow.Visible == true && this.federalTaxNumTxt.ClientID!=null)
                //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "federalTaxVal", "<script>federalTaxVal();</script>", false); 
            }
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "AdjustPageHeight", "<script> SetControlFocus();</script>", false);

        }
        #endregion

        #region Create Roles List
        private List<int> CreateRolecdsList()
        {
            List<int> roleCDs = new List<int>();

            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.SVPRelo);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.AdmAstRelo);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.SrSysTech);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.SrQCAnlstRloAcct);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.MgrRelo);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.ActAsstRLO);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.AcctAsstOA);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.TeamLeadOA);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.SrQCAnlst);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.QCAnalstOA);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.VPOpsAcct);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.SrAcctOA);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.MgrOpsAct);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.DirOpsAct);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.FinanceAccountMngr);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.SrDepartRASAnalyst);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.DirectorReloAccting);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.IntlAccountsPayable);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.DepartureRASAnalyst);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.RloYrEndSummTxPkgAdm);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.CPCMgrOA);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.FinControlTeamQC);

            return roleCDs;
        }

        private List<int> CreateGenPactRolecdsList()
        {
            List<int> roleCDs = new List<int>();

            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.SrCSIAnlst);
            roleCDs.Add(CMS.Codes.EDGE.PartyRoleCd.CSIAnlst);


            return roleCDs;
        }
        #endregion

        #region Load Drop Down
        private void LoadDropDown()
        {
            try
            {
                UtilityUI.BindCodeValues(bankCountryNameDDL, Codes.EDGE.CodeDomains.CountryCd, "Descr", "Cd");
            }
            catch
            {
                bankCountryNameDDL.SelectedValue = "0";
            }

            if (this.BankAccountId != 0 )
            {
                if (this.SelectedCountryCd != 0)
                {
                    bankCountryNameDDL.SelectedValue = this.SelectedCountryCd.ToString();
                }
            }

            UtilityUI.BindCodeValues(bankStatusDDL, Codes.EDGE.CodeDomains.BankAcctStatusCd, "Descr", "Cd");
            UtilityUI.BindCodeValues(paymentMethodDDL, Codes.EDGE.CodeDomains.BankPaymentMethods, "Descr", "Cd", false);
            UtilityUI.BindCodeValues(accountClassDDL, Codes.EDGE.CodeDomains.USACHCd, "Descr", "Cd");

            if (this.BankAccountId == 0)
            {
                if (this.BankType == BankTypeList.Customer)
                    accountClassDDL.SelectedValue = CMS.Codes.EDGE.USACHCd.ACHP.ToString();
                else
                    accountClassDDL.SelectedValue = CMS.Codes.EDGE.USACHCd.ACHC.ToString();
            }
            BindCodeValuesAccntTypeList(accountTypeDDL, Codes.EDGE.CodeDomains.BankAcctTypCd, "Descr", "Cd", false); // Checking should not be displayed, so need an customization
            LoadDisbursementCurrencies();
            LoadPaymentCategory();
        }

        #region LoadDisbursementCurrencies
        public void LoadDisbursementCurrencies()
        {
            BankInfoDS currencyList = new BankInfoDS();
                disbursementCurrencyDDL.Items.Clear();

                switch (this.BankType)
                {
                    case BankTypeList.Supplier:
                        if (this.GeoOrigingCd != 0)
                        {
                            currencyList = BankSIRemoteObject.GetBankDisbursementCurrency(CurrentPage.User, this.GeoOrigingCd);
                        }
                        break;

                    case BankTypeList.Division: // Display the Currencies from all the GO's
                        currencyList = BankSIRemoteObject.GetBankDisbursementCurrency(CurrentPage.User, CMS.Codes.EDGE.GeoOriginTypCd.None);
                        break;

                    default:
                        currencyList = BankSIRemoteObject.GetBankDisbursementCurrencyByFileId(CurrentPage.User, this.FileId);
                        break;
                }
                if (BankType != BankTypeList.Division)
                {
                    if (currencyList.tblGeoDisbmntCurrency.Rows.Count > 0)
                        this.GeoOrigingCd = currencyList.tblGeoDisbmntCurrency[0].GeoOriginCd;
                }
                else
                {
                    if (currencyList.tblGeoDisbmntCurrency.Rows.Count > 0)
                        this.GeoOrigingCd = CMS.Codes.EDGE.GeoOriginTypCd.UnitedStates;
                }
            try
            {
                disbursementCurrencyDDL.DataTextField = currencyList.tblGeoDisbmntCurrency.CurrDescColumn.ColumnName;
                disbursementCurrencyDDL.DataValueField = currencyList.tblGeoDisbmntCurrency.CurrCdColumn.ColumnName;
                disbursementCurrencyDDL.DataSource = currencyList.tblGeoDisbmntCurrency;
                disbursementCurrencyDDL.DataBind();
                disbursementCurrencyDDL.Items.Insert(0, new ListItem(" --select one-- ", ""));
            }
            catch
            {
                disbursementCurrencyDDL.SelectedValue = "0";
            }
            if (this.BankAccountId != 0 && disbursementCurrencyDDL.Items.Count > 0 && this.CurrCd != 0)
            {
                if (disbursementCurrencyDDL.Items.FindByValue(this.CurrCd.ToString()) != null)
                {
                    disbursementCurrencyDDL.SelectedValue = this.CurrCd.ToString();
                }
                
            }
        }
        #endregion

        #region BindCodeValuesAccntTypeList
        /// <summary>
        /// Binds codes values to DropDownList
        /// </summary>
        /// <param name="comboList">DropDownList Object</param>
        /// <param name="typeCode">TypeCode Value</param>
        /// <param name="textField">Textfield</param>
        /// <param name="valueField">ValueField</param>
        /// <returns><c>void</c></returns>
        public static void BindCodeValuesAccntTypeList(CMS.Atlas.Web.Controls.WebForm.DropDownList comboList, int typeCode, string textField, string valueField, bool isOrderReqd)
        {
            DataSet staticCodeDS = new DataSet();
            staticCodeDS = CMS.Codes.Cache.EDGE.Get(typeCode, true).DS;
            DataView dataView = new DataView(staticCodeDS.Tables[0]);
            comboList.DataSource = dataView;
            dataView.RowFilter = "Cd <> 1";
            if (isOrderReqd)
                dataView.Sort = "Descr ASC";
            comboList.DataTextField = textField;
            comboList.DataValueField = valueField;
            comboList.DataBind();
            comboList.Items.Insert(0, new ListItem(" --select one-- ", ""));
        }
        #endregion

        #region LoadPaymentCategory
        private void LoadPaymentCategory()
        {
            DataSet codeDS = (DataSet)CMS.Codes.Cache.EDGE.Get(CMS.Codes.EDGE.CodeDomains.BankPaymentCategory).DS;
            paymentcategoryCBL.DataSource = codeDS;
            paymentcategoryCBL.DataTextField = "Descr";
            paymentcategoryCBL.DataValueField = "Cd";
            paymentcategoryCBL.DataBind();

            for (int i = 0; i < paymentcategoryCBL.Items.Count; i++)
            {
                var result = from list in this.BankDataSet.tblBankAcctPayCategory
                             where list.BankAcctPayCatCd == int.Parse("0" + paymentcategoryCBL.Items[i].Value)
                             select list;
                if (result.Count() > 0)
                {
                    paymentcategoryCBL.Items[i].Selected = true;
                }
            }
        }
        #endregion

        #endregion

        #region SetControlVisibilityByBankType
        public void SetControlVisibilityByBankType()
        {
            paymentcatLabel.Visible = false;
            paymentCategoryDiv.Visible = false;
            paymentcatdescrLabel.Visible = false;
            paymentcatdescrTxt.Visible = false;
            paymentcatdescrTxtRegExpValidator.Visible = false;
            paymentcatdescrTxtMaxLengthValidator.Visible = false;

            globalPayrollCheckLabel.Visible = false;
            globalPayrollCheck.Visible = false;
            payrollNameLabel.Visible = false;
            payrollNameTxt.Visible = false;
            countryWidget.Visible = false;
            globalPayrollCheckCustomValidator.Visible = false;
            payrollNameTxtCountryValidator.Visible = false;


            switch (this.BankType)
            {
                case BankTypeList.Customer:
                    globalPayrollCheckLabel.Visible = true;
                    globalPayrollCheck.Visible = true;
                    payrollNameLabel.Visible = true;
                    payrollNameTxt.Visible = true;
                    countryWidget.Visible = true;
                    payrollNameTxtCountryValidator.Visible = true;
                    break;
                case BankTypeList.Division:
                    paymentcatLabel.Visible = true;
                    paymentCategoryDiv.Visible = true;
                    paymentcatdescrLabel.Visible = true;
                    paymentcatdescrTxt.Visible = true;
                    paymentcatdescrTxtRegExpValidator.Visible = true;
                    paymentcatdescrTxtMaxLengthValidator.Visible = true;
                    break;
            }
        }
        #endregion

        #region countryContinueBtn_Click
        protected void countryContinueBtn_Click(object sender, EventArgs e)
        {
            if (this.VendorMasterFlag == 1)
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "autoadjustheight", "<script>AutoAdjustFrameHeight();</script>");
            }
            try
            {
                routingNumberTxt.Text = string.Empty;
                SWIFTBICCodeTxt.Text = string.Empty;
                displayRoutingNoDivHidden.Value = "2";
                displayBankInfoDivHidden.Value = "1";
                isABANumbrvalidHidden.Value = "0";
                isABANumberAlphaHidden.Value = "0";

                FindAndAssignRoutingNoCaption(int.Parse("0" + bankCountryNameDDL.SelectedItem.Value), int.Parse("0" + disbursementCurrencyDDL.SelectedItem.Value));

                //ICRAS00109568
                if ((this.BankType == BankTypeList.Supplier || this.BankType == BankTypeList.Customer) && (int.Parse("0" + bankCountryNameDDL.SelectedItem.Value) == CMS.Codes.EDGE.CountryCd.USA && int.Parse("0" + disbursementCurrencyDDL.SelectedItem.Value) == CMS.Codes.EDGE.SWIFTCd.USD && this.GeoOrigingCd == CMS.Codes.EDGE.GeoOriginTypCd.UnitedStates))
                {
                    SWIFTBICCodelabel.Visible = false;
                    SWIFTBICCodeTxt.Visible = false;
                }

                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "SetPageFocusOnClick", "<script> SetPageFocus('Continue');</script>", false);
                if (this.BankType == BankTypeList.Supplier)
                {
                    FocusControlOnPageLoad(this.routingNumberTxt.ClientID, this.Page);
                }
            }
            catch (UserMessageException ume)
            {
                errorTextHidden.Value = ume.UserMessage;
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "SetPageError", "<script>SetUserMessage('2');</script>", false);
            }
            catch (Exception ex)
            {
                errorTextHidden.Value = ex.Message;
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "SetPageError", "<script>SetUserMessage('3');</script>", false);
            }
        }

        private void FindAndAssignRoutingNoCaption(int countryCd, int currCd)
        {
            bankRoutingTypCdHidden.Value = "0";
            BankInfoDS bankInfoDS = BankSIRemoteObject.GetCountryRoutingInfo(CurrentPage.User, countryCd, this.GeoOrigingCd, currCd);

            if (bankInfoDS.tblCountryRoutingInfo.Rows.Count > 0)
                SetRoutingNoLabelCaption(bankInfoDS.tblCountryRoutingInfo[0].RoutingNoDescCd, bankInfoDS.tblCountryRoutingInfo[0].RoutingNoReq, bankInfoDS.tblCountryRoutingInfo[0].SWIFTBICCodeReq);
            else// If the country does not find any Mapping Routing Info then we are defaulting to Local Routing Number
                SetRoutingNoLabelCaption(CMS.Codes.EDGE.GPFRoutingNoType.LocalRoutingNo, false, false);
        }

        #endregion

        #region findBankBtn_Click
        protected void findBankBtn_Click(object sender, EventArgs e)
        {
            if (this.VendorMasterFlag == 1)
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "autoadjustheight", "<script>AutoAdjustFrameHeight();</script>");
            }
            try
            {
                bool isRoutingNoValid = true;
                bool isRoutingNoSWIFTValid = true;

                ClearControlsValue(); // To Clear below button control values;
                displayBankInfoDivHidden.Value = "2";
                EnableDisableInvalidAddress(false);
                paythroughbankReqIndHidden.Value = "0";
                paythroughBankNeededCheck.Checked = false;
                addressSupplierLabel.Visible = false;
                this.BankName = string.Empty;

                bankStatusDDL.SelectedValue = CMS.Codes.EDGE.BankAcctStatusCd.Active.ToString();
                accountHolderTxt.Text = this.AccountHolderName.Trim();

                bankAddressFoundHidden.Value = "0";
                this.InvalidBank = false;
                bool isABANoValid;
                bool isABAAlphaNumeric;

                BankAddressDS bankAddressDS = new BankAddressDS();
                if (isABANumbrvalidHidden.Value == "1")
                {
                    isABANoValid = false;
                }
                else
                {
                    isABANoValid = true;
                }
                if (isABANumberAlphaHidden.Value == "2")
                {
                    isABAAlphaNumeric = false;
                }
                else
                {
                    isABAAlphaNumeric = true;
                }
                if (BankType == BankTypeList.Supplier || BankType == BankTypeList.Customer)
                {
                     bankAddressDS = BankSIRemoteObject.GetBankAddressByRoutingCode(CurrentPage.User, int.Parse("0" + bankCountryNameDDL.SelectedItem.Value), routingNumberTxt.Text.Trim(), SWIFTBICCodeTxt.Text.Trim(), int.Parse("0" + disbursementCurrencyDDL.SelectedItem.Value), this.GeoOrigingCd, out isRoutingNoSWIFTValid, isABANoValid, isABAAlphaNumeric,0,1);
                }
                else
                {
                     bankAddressDS = BankSIRemoteObject.GetBankAddressByRoutingCode(CurrentPage.User, int.Parse("0" + bankCountryNameDDL.SelectedItem.Value), routingNumberTxt.Text.Trim(), SWIFTBICCodeTxt.Text.Trim(), int.Parse("0" + disbursementCurrencyDDL.SelectedItem.Value), this.GeoOrigingCd, out isRoutingNoSWIFTValid, isABANoValid, isABAAlphaNumeric,0,0);
                }
                if (!isRoutingNoSWIFTValid ||
                    (bankAddressDS.tblBnkRoutingNumber.Rows.Count == 0 && routingNumberTxt.Text.Trim() != string.Empty 
                        && SWIFTBICCodeTxt.Text.Trim() != string.Empty )
                   )
                {
                    //string errorMessage = GetDescriptionForCode(int.Parse("0" + bankRoutingTypCdHidden.Value), CMS.Codes.EDGE.CodeDomains.GPFRoutingNoType) + " + SWIFT/BIC code entered do not belong to the same bank. Please reenter the data and retry the bank search";
                    //throw new UserMessageException(errorMessage, "", errorMessage, CurrentPage.User.UserId.ToString());

                    errorTextHidden.Value = GetDescriptionForCode(int.Parse("0" + bankRoutingTypCdHidden.Value), CMS.Codes.EDGE.CodeDomains.GPFRoutingNoType) + " + SWIFT/BIC code entered do not belong to the same bank.";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "SetPageError", "<script>SetUserMessage('2');</script>", false);

                    if (bankAddressDS.tblBnkRoutingNumber.Rows.Count > 0) // To continue with the Bank Not found flow this is required.
                    {
                        bankAddressDS.tblBnkRoutingNumber[0].Delete();
                        bankAddressDS.tblBnkRoutingNumber.AcceptChanges();
                    }
                }

                if (SWIFTBICCodeTxt.Text.Trim().Length == 0 && bankAddressDS.tblBnkRoutingNumber.Rows.Count > 0)
                {
                    SWIFTBICCodeTxt.Text = bankAddressDS.tblBnkRoutingNumber[0].SWIFTBICCode;
                }
                if (BankType == BankTypeList.Supplier || BankType == BankTypeList.Customer)
                {
                    if (routingNoRequiredIndHidden.Value == "4" && (routingNumberTxt.Text.Trim().Length != 6 && routingNumberTxt.Text.Trim().Length != 8) && (Convert.ToInt32(bankCountryNameDDL.SelectedValue) == CMS.Codes.EDGE.CountryCd.UnitedKingdom))
                    {
                        bool flag = false;
                        if (ViewState["manualEntry"] != null)
                        {
                            flag = (bool)ViewState["manualEntry"];
                        }
                        if (flag == false)
                        {
                            string errorMessage = "UK SORT CODE is typically 6 or 8 characters in length. If the data entered is correct please continue with manual entry of the bank.";
                            ViewState["manualEntry"] = true;
                            throw new UserMessageException(errorMessage, "BankError", errorMessage, CurrentPage.User.UserId.ToString());
                        }

                    }
                }
                 if (routingNoRequiredIndHidden.Value == "4" && (routingNumberTxt.Text.Trim() == string.Empty
                                    || SWIFTBICCodeTxt.Text.Trim() == string.Empty)
                               ) // Both the Local Routing Number and SWIFTBIC code are requried.
                {
                    //string errorMessage = "Both " + GetDescriptionForCode(int.Parse("0" + bankRoutingTypCdHidden.Value), CMS.Codes.EDGE.CodeDomains.GPFRoutingNoType)
                    //        + " and SWIFT/BIC Code are required.";
                    
                        string errorMessage = "Unable to find an associated SWIFT/BIC Code for this bank. Please continue to manually enter a SWIFT/BIC Code.";
                        throw new UserMessageException(errorMessage, "BankError", errorMessage, CurrentPage.User.UserId.ToString());
                    
                }

                if (bankAddressDS.tblPaythroughBank.Rows.Count > 0)
                {
                    paythroughbankReqIndHidden.Value = (bankAddressDS.tblPaythroughBank[0].PaythroughBankFlg ? "1" : "0");
                    paythroughBankNeededCheck.Checked = bankAddressDS.tblPaythroughBank[0].PaythroughBankFlg;

                    paymentMethodDDL.SelectedValue = bankAddressDS.tblPaythroughBank[0].BankAcceptMethCd.ToString();
                }

                if (bankAddressDS.tblBnkRoutingNumber.Rows.Count > 0)
                {
                    //if (!bankAddressDS.tblBnkRoutingNumber[0].IsPaymentMethodCdNull() && bankAddressDS.tblBnkRoutingNumber[0].PaymentMethodCd != 0)
                    //    paymentMethodDDL.SelectedValue = bankAddressDS.tblBnkRoutingNumber[0].PaymentMethodCd.ToString();

                    payeeBankNameTxt.Text = bankAddressDS.tblBnkRoutingNumber[0].BnkNm;
                    this.BankName = payeeBankNameTxt.Text.Trim();

                    if (!bankAddressDS.tblBnkRoutingNumber[0].IsBnkPhoneNbrNull())
                        bankPhoneTxt.Text = bankAddressDS.tblBnkRoutingNumber[0].BnkPhoneNbr.Trim();
                    else
                        bankPhoneTxt.Text = "";

                    HideBankNotFoundMessage(false);
                    this.InvalidBank = false;

                    if (this.BankType != BankTypeList.Supplier)
                    {
                        payeeBankNameTxt.Display = ControlDisplay.Readonly;
                        bankAddressLabel.Text = bankAddressDS.tblBnkRoutingNumber[0].BnkAddress;
                        bankCityLabel.Text = bankAddressDS.tblBnkRoutingNumber[0].BnkCityNm;
                        bankStateLabel.Text = bankAddressDS.tblBnkRoutingNumber[0].BnkStateNm;
                        bankPostalCodeLabel.Text = bankAddressDS.tblBnkRoutingNumber[0].BnkPostalCode.Trim();
                        bankCountryLabel.Text = bankAddressDS.tblBnkRoutingNumber[0].BnkCountryNm;
                        bankAddressFoundHidden.Value = "1";

                        bankNewAddrLine1.Text = "";
                        bankNewAddressCityTxt.Text = "";
                        bankNewAddressCityTxt_Supplier.Text = bankNewAddressCityTxt.Text;
                        bankNewAddrLine2Txt.Text = "";
                        bankNewAddresssStateProvTxt.Text = "";
                        bankNewAddrLine3Txt.Text = "";
                        bankNewAddressCountryTxt.Text = "";
                        bankNewAddressCountryTxt_Supplier.Text = "";
                        //bankNewAddressCountryAltLabel.Text = "";
                        //bankNewAddressCountryAltLabel_Supplier.Text = "";
                        bankNewAddressPostalCodeTxt.Text = "";
                    }
                    else
                    {
                        addressSupplierLabel.Visible = true;
                        bankNewAddrLine1.Text = bankAddressDS.tblBnkRoutingNumber[0].BnkAddress;
                        bankNewAddressCityTxt.Text = bankAddressDS.tblBnkRoutingNumber[0].BnkCityNm;
                        bankNewAddressCityTxt_Supplier.Text = bankNewAddressCityTxt.Text;
                        bankNewAddrLine2Txt.Text = "";
                        bankNewAddresssStateProvTxt.Text = bankAddressDS.tblBnkRoutingNumber[0].BnkStateNm.Trim();
                        bankNewAddrLine3Txt.Text = "";
                        bankNewAddressCountryTxt.Text = bankAddressDS.tblBnkRoutingNumber[0].BnkCountryNm;
                        bankNewAddressCountryTxt_Supplier.Text = bankNewAddressCountryTxt.Text;
                        //bankNewAddressCountryAltLabel.Text = bankAddressDS.tblBnkRoutingNumber[0].BnkCountryNm;
                        //bankNewAddressCountryAltLabel_Supplier.Text = bankAddressDS.tblBnkRoutingNumber[0].BnkCountryNm; 
                        bankNewAddressPostalCodeTxt.Text = bankAddressDS.tblBnkRoutingNumber[0].BnkPostalCode.Trim();
                        bankAddressFoundHidden.Value = "2";
                    }
                }
                else
                {
                    /*if (this.BankType == BankTypeList.Supplier)
                    {
                        isRoutingNoValid = false;
                        errorTextHidden.Value = "Invalid Routing Number";
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "SetPageError", "<script>SetUserMessage('2');</script>", false);
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "DisplayBankDetailForRoutNo", "<script>ShowHideBelowFindBank(1);</script>", false);
                    }
                    else
                    {*/
                    payeeBankNameTxt.Text = "";
                    payeeBankNameTxt.Display = ControlDisplay.Editable;
                    bankPhoneTxt.Text = "";
                    bankAddressLabel.Text = "";
                    bankCityLabel.Text = "";
                    bankStateLabel.Text = "";
                    bankPostalCodeLabel.Text = "";
                    bankCountryLabel.Text = "";
                    //paythroughBankNeededCheck.Checked = false;
                    bankAddressFoundHidden.Value = "2";
                    this.InvalidBank = true;
                   
                    bankNewAddressCountryTxt.Text = bankCountryNameDDL.SelectedItem.Text;
                    bankNewAddressCountryTxt_Supplier.Text = bankNewAddressCountryTxt.Text;
                    //bankNewAddressCountryAltLabel.Text = bankCountryNameDDL.SelectedItem.Text;
                    //bankNewAddressCountryAltLabel_Supplier.Text = bankNewAddressCountryTxt.Text;

                    HideBankNotFoundMessage(true);

                    //}
                }
                isABANumbrvalidHidden.Value = "0";
                isABANumberAlphaHidden.Value = "0";
                if (isRoutingNoValid)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "DisplayBankDetailForRoutNo", "<script>ShowHideBelowFindBank(2);ShowHideBankAddressDetailForRoutingNo();SetPageFocus('Find');</script>", false);
                    SetAddressCountryPosition();
                    if (this.BankAccountId == 0)
                    {
                        if (this.BankType == BankTypeList.Customer)
                            accountClassDDL.SelectedValue = CMS.Codes.EDGE.USACHCd.ACHP.ToString();
                        else
                            accountClassDDL.SelectedValue = CMS.Codes.EDGE.USACHCd.ACHC.ToString();
                    }

                }
                if (this.BankType == BankTypeList.Supplier || this.VendorMasterFlag == 1)
                {
                   FocusControlOnPageLoad(this.bankStatusDDL.ClientID, this.Page);
                   this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "autoadjustheight", "<script>AutoAdjustFrameHeight();</script>");
                }
            }
            catch (UserMessageException ume)
            {
                if (ume.ErrorNumber == "FinancialDetails.BankDetails.AccountNumber.006")
                {
                    isABANumbrvalidHidden.Value = "1";
                }
                else if (ume.ErrorNumber == "FinancialDetails.BankDetails.AccountNumber.003" && isABANumberAlphaHidden.Value!="2")
                {
                    isABANumberAlphaHidden.Value = "2";
                }
                errorTextHidden.Value = ume.UserMessage;
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "SetPageError", "<script>SetUserMessage('2');</script>", false);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "DisplayBankDetailForRoutNo", "<script>ShowHideBelowFindBank(1);</script>", false);
            }
            catch (Exception ex)
            {
                errorTextHidden.Value = ex.Message;
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "SetPageError", "<script>SetUserMessage('3');</script>", false);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "DisplayBankDetailForRoutNo", "<script>ShowHideBelowFindBank(1);</script>", false);
            }
        }
        #endregion

        #region SetRoutingNoLabelCaption
        private void SetRoutingNoLabelCaption(int routingTypCd, bool routingNoRequried, bool SWIFTBICCodeRequired)
        {
            bankRoutingTypCdHidden.Value = routingTypCd.ToString();
            routingNumberLabel.Text = GetDescriptionForCode(routingTypCd, CMS.Codes.EDGE.CodeDomains.GPFRoutingNoType);
            SWIFTBICCodelabel.Text = "SWIFT/BIC Code";

            routingNoRequiredIndHidden.Value = "";

            if (!routingNoRequried && !SWIFTBICCodeRequired)
            {
                SWIFTBICCodelabel.Text += " <span class='required_blue' title='Either " + routingNumberLabel.Text + " or SWIFT/BIC Code is required.'>*</span>";
                routingNumberLabel.Text += " <span class='required_blue' title='Either " + routingNumberLabel.Text + " or SWIFT/BIC Code is required.'>*</span>";
                routingNoRequiredIndHidden.Value = "1";
                routingNumberReqValidator.LabelControl = "routingNumberLabel,SWIFTBICCodelabel";
                routingNumberReqValidator.ErrorCssClass = Web.Controls.Validation.ErrorClass.blue;
            }
            else if (routingNoRequried && !SWIFTBICCodeRequired)
            {
                routingNumberLabel.Text += " <span class='required_red' title='" + routingNumberLabel.Text + " is required.'>*</span>";
                routingNoRequiredIndHidden.Value = "2";
                routingNumberReqValidator.LabelControl = "routingNumberLabel";
                routingNumberReqValidator.ErrorCssClass = Web.Controls.Validation.ErrorClass.red;
            }
            else if (!routingNoRequried && SWIFTBICCodeRequired)
            {
                SWIFTBICCodelabel.Text += " <span class='required_red' title='SWIFT/BIC Code is required.'>*</span>";
                routingNoRequiredIndHidden.Value = "3";
                routingNumberReqValidator.LabelControl = "SWIFTBICCodelabel";
                routingNumberReqValidator.ErrorCssClass = Web.Controls.Validation.ErrorClass.red;
            }
            else if (routingNoRequried && SWIFTBICCodeRequired)
            {
                routingNoRequiredIndHidden.Value = "4";
                routingNumberLabel.Text += " <span class='required_red' title='" + routingNumberLabel.Text + " is required.'>*</span>";
                routingNumberReqValidator.LabelControl = "routingNumberLabel";
                routingNumberReqValidator.ErrorCssClass = Web.Controls.Validation.ErrorClass.red;
            }
                
        }
        #endregion

        #region Clear Controls
        private void ClearControlsValue()
        {
            bankStatusDDL.SelectedIndex = 0;
            paymentMethodDDL.SelectedIndex = 0;
            bankAddressLabel.Text = "";
            bankCityLabel.Text = "";
            bankStateLabel.Text = "";
            bankCountryLabel.Text = "";

            bankNewAddrLine1.Text = "";
            bankNewAddressCityTxt.Text = "";
            bankNewAddressCityTxt_Supplier.Text = bankNewAddressCityTxt.Text;
            bankNewAddrLine2Txt.Text = "";
            bankNewAddresssStateProvTxt.Text = "";
            bankNewAddrLine3Txt.Text = "";
            bankNewAddressCountryTxt.Text = "";
            bankNewAddressCountryTxt_Supplier.Text = "";
            //bankNewAddressCountryAltLabel.Text = "";
            //bankNewAddressCountryAltLabel_Supplier.Text = "";
            bankNewAddressPostalCodeTxt.Text = "";
            paythroughBankNeededCheck.Checked = false;

            //Code change for ICRAS00097819 start
            IBANAccountNumberTxt.Text = "";
            reTypeIBANAccountNumberTxt.Text = ""; 
            accountTypeDDL.SelectedIndex = 0;
            globalPayrollCheck.Checked = false;
            payrollNameTxt.Text = "";
            specialInstructionsTxt.Text = "";
            //Code change for ICRAS00097819 end
        }
        #endregion

        #region GetControls
        /// <summary>
        /// GetControls
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private Control[] GetControls(Control control)
        {
            List<Control> controlList = new List<Control>();
            controlList.Add(control);

            if (control.HasControls())
            {
                foreach (Control controlItem in control.Controls)
                {
                    controlList.AddRange(GetControls(controlItem));
                }
            }
            return controlList.ToArray();
        }
        #endregion GetControls

        #region RenderControlValidate
        public void RenderControlValidate(bool readOnly)
        {
            if (readOnly)
            {
                Control[] controlList = GetControls(this);
                foreach (Control control in controlList)
                {
                    if (control.GetType().Equals(typeof(CMSWebForm.DropDownList)))
                    {
                        CMSWebForm.DropDownList ddControl = (CMSWebForm.DropDownList)control;
                        ddControl.Display = ControlDisplay.Readonly;
                    }
                    else if (control.GetType().Equals(typeof(CMSWebForm.TextBox)))
                    {
                        CMSWebForm.TextBox textControl = (CMSWebForm.TextBox)control;
                        textControl.Display = ControlDisplay.Readonly;
                    }
                    else if (control.GetType().Equals(typeof(CMSWebForm.ValueCheckBoxList)))
                    {
                        CMSWebForm.ValueCheckBoxList checkBoxListControl = (CMSWebForm.ValueCheckBoxList)control;
                        checkBoxListControl.Display = ControlDisplay.Readonly;
                    }
                    else if (control.GetType().Equals(typeof(CMSWebForm.CheckBox)))
                    {
                        CMSWebForm.CheckBox checkBoxControl = (CMSWebForm.CheckBox)control;
                        checkBoxControl.Display = ControlDisplay.Readonly;
                    }
                    else if (control.GetType().Equals(typeof(CMSWebForm.Button)))
                    {
                        ((CMSWebForm.Button)control).Display = ControlDisplay.Readonly;
                    }
                    else if (control.GetType().Equals(typeof(CMSWebForm.Widget)))
                    {
                        ((CMSWebForm.Widget)control).Display = ControlDisplay.Readonly;
                    }
                    else if (control.GetType().Equals(typeof(CMSWebForm.CheckBoxList)))
                    {
                        ((CMSWebForm.CheckBoxList)control).Display = ControlDisplay.Readonly;
                    }
                }

                HideBankNotFoundMessage(false);
            }
        }
        #endregion

        #region BindData
        public void BindData()
        {
            BankDS bankDS = this.BankDataSet;
            int userTypeCd = 0;
            int countryCd = 0;
            

            if (bankDS.tblBankAcct.Rows.Count > 0)
            {
                #region Bank Details
                displayRoutingNoDivHidden.Value = "2";
                displayBankInfoDivHidden.Value = "2";
                this.SelectedCountryCd = 0;

                if (bankDS.tblPhysicalAddr.Rows.Count > 0)
                {
                    //bankCountryNameDDL.SelectedValue = bankDS.tblPhysicalAddr[0].CountryCd.ToString();
                    this.SelectedCountryCd = bankDS.tblPhysicalAddr[0].CountryCd;
                    countryCd = bankDS.tblPhysicalAddr[0].CountryCd;
                }

                if (bankDS.tblBankAcct[0].BankAcctCurrCd == 0)
                    disbursementCurrencyDDL.SelectedIndex = -1;
                //else
                //    disbursementCurrencyDDL.SelectedValue = bankDS.tblBankAcct[0].BankAcctCurrCd.ToString();
                else
                this.CurrCd = bankDS.tblBankAcct[0].BankAcctCurrCd;

                userTypeCd = bankDS.tblBankAcct[0].IsEdgeUserTypeCdNull() ? 0 : bankDS.tblBankAcct[0].EdgeUserTypeCd;
                if (CurrentPage.FileId > 0)
                {
                    createdByRow.Visible = true;
                    if (userTypeCd != CMS.Codes.EDGE.EdgeUserType.Customer)
                        createdBylbltxt.Text = "Cartus";
                    else
                        createdBylbltxt.Text = GetDescriptionForCode(CMS.Codes.EDGE.EdgeUserType.Customer, CMS.Codes.EDGE.CodeDomains.EdgeUserType);
                }

                SWIFTBICCodeTxt.Text = (bankDS.tblBankAcct[0].IsSwiftBICCodeNull()? string.Empty:bankDS.tblBankAcct[0].SwiftBICCode);
                routingNumberTxt.Text = (bankDS.tblBankAcct[0].IsBankAcctRoutingNoNull()? string.Empty:bankDS.tblBankAcct[0].BankAcctRoutingNo);

                if (bankDS.tblBankAcct[0].BankAcctStatusCd == 0)
                    bankStatusDDL.SelectedIndex = -1;
                else
                    bankStatusDDL.SelectedValue = bankDS.tblBankAcct[0].BankAcctStatusCd.ToString();

                if (bankDS.tblBankAcct[0].BankAcceptMethCd != 0)
                    paymentMethodDDL.SelectedValue = bankDS.tblBankAcct[0].BankAcceptMethCd.ToString();
                else
                    paymentMethodDDL.SelectedIndex = -1;

                payeeBankNameTxt.Text = bankDS.tblBankAcct[0].BankNm;
                this.BankName = bankDS.tblBankAcct[0].BankNm.Trim();
                this.InvalidBank = bankDS.tblBankAcct[0].InvalidBankFlg;

                if (bankDS.tblBankAcct[0].InvalidRoutingNumber || this.BankType == BankTypeList.Supplier)
                {
                    bankNewAddrLine1.Text = bankDS.tblPhysicalAddr[0].AddrLine1;
                    bankNewAddrLine2Txt.Text = bankDS.tblPhysicalAddr[0].IsAddrLine2Null() ? "" : bankDS.tblPhysicalAddr[0].AddrLine2;
                    bankNewAddrLine3Txt.Text = bankDS.tblPhysicalAddr[0].IsAddrLine3Null() ? "" : bankDS.tblPhysicalAddr[0].AddrLine3;
                    bankNewAddressCityTxt.Text = bankDS.tblPhysicalAddr[0].IsCityNmNull() ? "" : bankDS.tblPhysicalAddr[0].CityNm;
                    bankNewAddressCityTxt_Supplier.Text = bankNewAddressCityTxt.Text;
                    bankNewAddresssStateProvTxt.Text = bankDS.tblPhysicalAddr[0].IsStProvNmNull() ? "" : bankDS.tblPhysicalAddr[0].StProvNm;
                    bankNewAddressCountryTxt.Text = bankDS.tblPhysicalAddr[0].IsCountryNmNull() ? "" : bankDS.tblPhysicalAddr[0].CountryNm;
                    bankNewAddressCountryTxt_Supplier.Text = bankNewAddressCountryTxt.Text;
                    //bankNewAddressCountryAltLabel.Text = bankDS.tblPhysicalAddr[0].IsCountryNmNull() ? "" : bankDS.tblPhysicalAddr[0].CountryNm;
                    //bankNewAddressCountryAltLabel_Supplier.Text = bankNewAddressCountryTxt.Text;
                    bankNewAddressPostalCodeTxt.Text = bankDS.tblPhysicalAddr[0].IsPostalCodeNull() ? "" : bankDS.tblPhysicalAddr[0].PostalCode.Trim();
                    bankAddressFoundHidden.Value = "2";
                }
                else
                {
                    if (bankDS.tblPhysicalAddr.Rows.Count > 0)
                    {
                        bankAddressLabel.Text = bankDS.tblPhysicalAddr[0].AddrLine1;
                        bankCityLabel.Text = bankDS.tblPhysicalAddr[0].IsCityNmNull() ? "" : bankDS.tblPhysicalAddr[0].CityNm;
                        bankStateLabel.Text = bankDS.tblPhysicalAddr[0].IsStProvNmNull() ? "" : bankDS.tblPhysicalAddr[0].StProvNm;
                        bankPostalCodeLabel.Text = bankDS.tblPhysicalAddr[0].IsPostalCodeNull() ? "" : bankDS.tblPhysicalAddr[0].PostalCode.Trim();
                        bankCountryLabel.Text = bankDS.tblPhysicalAddr[0].IsCountryNmNull() ? "" : bankDS.tblPhysicalAddr[0].CountryNm;
                    }
                    else
                    {
                        bankAddressLabel.Text = "";
                        bankCityLabel.Text = "";
                        bankStateLabel.Text = "";
                        bankPostalCodeLabel.Text = "";
                        bankCountryLabel.Text = "";
                    }
                    bankAddressFoundHidden.Value = "1";
                }

                IBANAccountNumberTxt.Text = bankDS.tblBankAcct[0].BankAcctNo.ToString();
                reTypeIBANAccountNumberTxt.Text = bankDS.tblBankAcct[0].BankAcctNo.ToString(); 
                accountHolderTxt.Text = bankDS.tblBankAcct[0].BankAcctNm.Trim() + " ";
                if (CurrentPage.IsReadOnly)
                {
                    if (payeeBankNameTxt.Text.Trim().Length > 28)
                    {
                        //Fix for ICRAS00098822-starts
                        //payeeBankNameTxt.ToolTip = payeeBankNameTxt.Text;
                        //payeeBankNameTxt.Text = payeeBankNameTxt.Text.Substring(0, 25) + "...";
                        payeeBankNameTxt.Text = GetMouseOverText(payeeBankNameTxt.Text.Trim(), 0, 25);
                        //Fix for ICRAS00098822-ends
                    }

                    if (accountHolderTxt.Text.Trim().Length > 28)
                    {
                        //Fix for ICRAS00098822-starts
                        //accountHolderTxt.ToolTip = accountHolderTxt.Text;
                        //accountHolderTxt.Text = accountHolderTxt.Text.Substring(0, 25) + "...";
                        accountHolderTxt.Text = GetMouseOverText(accountHolderTxt.Text.Trim(), 0, 25);
                        //Fix for ICRAS00098822-ends
                    }
                }

                if (bankDS.tblBankAcct[0].BankAcctTypCd == CMS.Codes.EDGE.BankAcctTypCd.None)
                    accountTypeDDL.SelectedIndex = -1;
                else
                    accountTypeDDL.SelectedValue = bankDS.tblBankAcct[0].BankAcctTypCd.ToString();

                if (bankDS.tblBankAcct[0].BankAcctClassTypCd == CMS.Codes.EDGE.BankAcctTypCd.None)
                    accountClassDDL.SelectedIndex = -1;
                else
                    accountClassDDL.SelectedValue = bankDS.tblBankAcct[0].BankAcctClassTypCd.ToString();

                paymentcatdescrTxt.Text = (bankDS.tblBankAcct[0].IsBankPaymentCmntNull() ? "" : bankDS.tblBankAcct[0].BankPaymentCmnt);
                globalPayrollCheck.Checked = bankDS.tblBankAcct[0].GlobalBankFlg;
                payrollNameTxt.Text = bankDS.tblBankAcct[0].IsPaymentCountryNmNull() ? "" : bankDS.tblBankAcct[0].PaymentCountryNm;
                specialInstructionsTxt.Text = bankDS.tblBankAcct[0].IsSpclInstructionCmntNull() ? "" : bankDS.tblBankAcct[0].SpclInstructionCmnt;

                paythroughBankNeededCheck.Checked = (bankDS.tblBankAcct[0].IsPayThroughBankReqFlgNull() ? false : bankDS.tblBankAcct[0].PayThroughBankReqFlg);
                paythroughbankReqIndHidden.Value = (paythroughBankNeededCheck.Checked ? "1" : "0");

                //if (paythroughBankNeededCheck.Checked && this.BankType == BankTypeList.Supplier)
                //    paythroughBankNeededCheck.Enabled = false;
                //else
                //    paythroughBankNeededCheck.Enabled = true;

                FindAndAssignRoutingNoCaption(countryCd, this.CurrCd); // One more remoting Call to find the Routing No Label Caption

                //bankRoutingTypCdHidden.Value = bankDS.tblBankAcct[0].BankAcctRoutingTypCd.ToString();
                //SetRoutingNoLabelCaption(bankDS.tblBankAcct[0].BankAcctRoutingTypCd);

                if (bankDS.tblPhoneNumber.Rows.Count > 0)
                    bankPhoneTxt.Text = bankDS.tblPhoneNumber[0].EnteredPhoneNbr.Trim();

                // To Display the Invalid Address in the Top
                if (bankDS.tblPhysicalAddr.Rows.Count > 0)
                {
                    if (bankDS.tblPhysicalAddr[0].AddrValidateIndCd == CMS.Codes.EDGE.AddrValidateIndCd.NotValidated)
                    {
                        EnableDisableInvalidAddress(true);
                        this.InvalidAddressDisplay = true;
                    }
                }
                if ((this.GeoOrigingCd == CMS.Codes.EDGE.GeoOriginTypCd.Brazil && (this.BankType == BankTypeList.Customer || this.BankType == BankTypeList.Supplier)) || this.BankType == BankTypeList.Division)
                {
                    federalTaxNumRow.Visible = true;
                    if (bankDS.tblBankAcct[0].BankAcctClassTypCd == CMS.Codes.EDGE.USACHCd.ACHP)
                    {
                        if (this.BankType != BankTypeList.Division)
                        {
                            federalTaxNumLabel.Text = "CPF Number" + " <span class='required_red' title='CPF Number is required.'>*</span>";
                        }
                        else
                        {
                            federalTaxNumLabel.Text = "CPF Number";
                        }
                        federalTaxNumTxt.MaxLength = 11;
                        federalTaxNumTxt.Text = (bankDS.tblBankAcct[0].IsFederalTaxNumberNull() ? "" : bankDS.tblBankAcct[0].FederalTaxNumber.Trim());
                        FederalTaxNumCustomVal.ErrorMessage = "CPF Number is required.";
                        FederalTaxNumRegexVal.ErrorMessage = "Entered Characters are not allowed for CPF Number.";
                        FederalTaxIDMaxLengthVal.ValidationExpression = "^(.|\n){0,11}$";
                        FederalTaxIDMaxLengthVal.ErrorMessage = "CPF Number cannot be more than 11 characters.";
                    }
                    else if (bankDS.tblBankAcct[0].BankAcctClassTypCd == CMS.Codes.EDGE.USACHCd.ACHC)
                    {
                        if (this.BankType != BankTypeList.Division)
                        {
                            federalTaxNumLabel.Text = "CNPJ Number" + " <span class='required_red' title='CNPJ Number is required.'>*</span>";
                        }
                        else
                        {
                            federalTaxNumLabel.Text = "CNPJ Number";
                        }
                        federalTaxNumTxt.MaxLength = 14;
                        federalTaxNumTxt.Text = (bankDS.tblBankAcct[0].IsFederalTaxNumberNull() ? "" : bankDS.tblBankAcct[0].FederalTaxNumber.Trim());
                        FederalTaxNumCustomVal.ErrorMessage = "CNPJ Number is required.";
                        FederalTaxNumRegexVal.ErrorMessage = "Entered Characters are not allowed for CNPJ Number.";
                        FederalTaxIDMaxLengthVal.ValidationExpression = "^(.|\n){0,14}$";
                        FederalTaxIDMaxLengthVal.ErrorMessage = "CNPJ Number cannot be more than 14 characters.";
                    }
                    if (this.BankType == BankTypeList.Division || this.BankType == BankTypeList.Supplier)
                    {
                        stateAndCityTaxRow.Visible = true;
                        stateTaxIDTxt.Text = (bankDS.tblBankAcct[0].IsStateTaxNumberNull() ? "" : bankDS.tblBankAcct[0].StateTaxNumber.Trim());
                        CityTaxIDTxt.Text = (bankDS.tblBankAcct[0].IsMunicipalTaxNumberNull() ? "" : bankDS.tblBankAcct[0].MunicipalTaxNumber.Trim());
                    }
                }
                else
                {
                    federalTaxNumRow.Visible = false;
                    stateAndCityTaxRow.Visible = false;
                }

                if (this.GeoOrigingCd == CMS.Codes.EDGE.GeoOriginTypCd.India || this.BankType == BankTypeList.Division)
                {
                    PANTANrow.Visible = true;
                    if (this.BankType == BankTypeList.Division)
                    {
                        PANLabel.Text = "PAN";
                        TANLabel.Text = "TAN";
                    }
                    PANTextBox.Text = (bankDS.tblBankAcct[0].IsPANNbrNull() ? "" : bankDS.tblBankAcct[0].PANNbr.Trim());
                    TANTextBox.Text = (bankDS.tblBankAcct[0].IsTANNbrNull() ? "" : bankDS.tblBankAcct[0].TANNbr.Trim());
                }
                #endregion

                #region Payment Category Binding
                LoadPaymentCategory();
                
                #endregion

                if (this.BankType != BankTypeList.Supplier)
                    DisableControlOnEditMode();

                if (this.PayThroughBankAcctId > 0)
                    this.PayThroughBankDisplay = true;

                SetAddressCountryPosition();
            }
        }
        #endregion

        #region DisableControlOnEditMode
        public void DisableControlOnEditMode()
        {

            //bankCountryNameDDL.Display = ControlDisplay.Readonly;
            //disbursementCurrencyDDL.Display = ControlDisplay.Readonly;
            //routingNumberTxt.Display = ControlDisplay.Readonly;
            //SWIFTBICCodeTxt.Display = ControlDisplay.Readonly;
            countryContinueBtn.Display = ControlDisplay.Readonly;
            findBankBtn.Display = ControlDisplay.Readonly;
            //paymentMethodDDL.Display = ControlDisplay.Readonly;
            //payeeBankNameTxt.Display = ControlDisplay.Readonly;
            //bankPhoneTxt.Display = ControlDisplay.Readonly;

            /*if (bankAddressFoundHidden.Value == "2") // Invalid Routing Number
            {
                bankNewAddrLine1.Display = ControlDisplay.Readonly;
                bankNewAddrLine2Txt.Display = ControlDisplay.Readonly;
                bankNewAddrLine3Txt.Display = ControlDisplay.Readonly;
                bankNewAddressCityTxt.Display = ControlDisplay.Readonly;
                bankNewAddresssStateProvTxt.Display = ControlDisplay.Readonly;
                //bankNewAddressCountryTxt.Display = ControlDisplay.Readonly;
                bankNewAddressPostalCodeTxt.Display = ControlDisplay.Readonly;
                paythroughBankNeededCheck.Display = ControlDisplay.Readonly;
            }*/
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "DisableControlOnEditMode_JS", "<script>DisableControlOnEditMode();</script>");
        }
        #endregion

        #region GetBankData
        public BankDS GetBankData()
        {
            BankDS bankDS = new BankDS();
            //string federalTaxId,stateTaxId,cityTaxId = string.Empty;

            if (routingNoRequiredIndHidden.Value == "4" && (routingNumberTxt.Text.Trim() == string.Empty
                    || SWIFTBICCodeTxt.Text.Trim() == string.Empty)
               ) // Both the Local Routing Number and SWIFTBIC code are requried.
            {
                if ((routingNumberTxt.Text.Trim().Length == 6 || routingNumberTxt.Text.Trim().Length == 8) 
                    && !(BankType==BankTypeList.Supplier && RequestedOracleStatus==CMS.Codes.EDGE.OracleVndrStatCd.Inactive))
                {
                    string errorMessage = "Both " + GetDescriptionForCode(int.Parse("0" + bankRoutingTypCdHidden.Value), CMS.Codes.EDGE.CodeDomains.GPFRoutingNoType)
                            + " and SWIFT/BIC Code are required.";
                    throw new UserMessageException(errorMessage, "BankError", errorMessage, CurrentPage.User.UserId.ToString());
                }
            }


            #region Bank Info

            BankDS.tblBankAcctRow bankAcctRow = bankDS.tblBankAcct.NewtblBankAcctRow();
            if (this.BankDataSet.tblBankAcct.Rows.Count > 0)
                bankAcctRow.UpdateDt = this.BankDataSet.tblBankAcct[0].UpdateDt;
            else
                bankAcctRow.UpdateDt = DateTime.Now;
            bankAcctRow.BankAcctId = this.BankAccountId;
            bankAcctRow.BusnPartID = this.BusnPartId;
            bankAcctRow.BusnPartEmpId = this.BusnPartEmpId;
            bankAcctRow.DeptrInfoId = 0;
            bankAcctRow.ContractId = 0;
            bankAcctRow.LeaseDetailId = 0;
            bankAcctRow.ParentBankAccttId = 0;
            bankAcctRow.BankNm = payeeBankNameTxt.Text.Trim();

            bankAcctRow.BankAcctNm = accountHolderTxt.Text.Trim().ToUpper();
            bankAcctRow.BankAcctCurrCd = int.Parse("0" + disbursementCurrencyDDL.SelectedValue);

            bankAcctRow.BankAcctNo = IBANAccountNumberTxt.Text.Trim().ToUpper();
            bankAcctRow.SwiftBICCode = SWIFTBICCodeTxt.Text.Trim();
            bankAcctRow.BankAcctRoutingNo = routingNumberTxt.Text.Trim().ToUpper();

            if (SWIFTBICCodeTxt.Text.Trim().Length > 0 && routingNumberTxt.Text.Trim().Length == 0)
                bankAcctRow.BankAcctRoutingTypCd = CMS.Codes.EDGE.GPFRoutingNoType.Swift;
            else
                bankAcctRow.BankAcctRoutingTypCd = Convert.ToInt32("0" + bankRoutingTypCdHidden.Value);

            bankAcctRow.BankAcctClassTypCd = Convert.ToInt32(accountClassDDL.SelectedValue);
            bankAcctRow.SpclInstructionCmnt = specialInstructionsTxt.Text.Trim().ToUpper();
            bankAcctRow.PayThroughBankNm = "";
            bankAcctRow.PayThroughBankAcctNo = "";

            bankAcctRow.BankAcctTypCd = Convert.ToInt32("0" + accountTypeDDL.SelectedValue);
            bankAcctRow.BankABABranchNo = String.Empty;
            bankAcctRow.BankContactNm = String.Empty;
            bankAcctRow.BankContactTitle = String.Empty;
            bankAcctRow.BankContactDesc = String.Empty;
            bankAcctRow.PrimaryBankFlg = true;
            bankAcctRow.SetSwiftABABSBNoNull();
            bankAcctRow.SwiftABABSBCd = 0;
            bankAcctRow.BankSwiftABACdNm = String.Empty;
            bankAcctRow.ReimbmntMethCd = 0;
            bankAcctRow.BankAcctDesc = String.Empty;
            bankAcctRow.BankPaymentCmnt = paymentcatdescrTxt.Text;
            bankAcctRow.SetPayThroughBankSeqNbrNull();  // check
            bankAcctRow.BankAcceptMethCd = Convert.ToInt32("0" + paymentMethodDDL.SelectedValue);

            if (this.BankType == BankTypeList.Customer)
                bankAcctRow.BankAcctOwnerCd = CMS.Codes.EDGE.BnkAccOwnrCd.Customer;
            else if (this.BankType == BankTypeList.Division)
                bankAcctRow.BankAcctOwnerCd = CMS.Codes.EDGE.BnkAccOwnrCd.Client;
            else
                bankAcctRow.BankAcctOwnerCd = CMS.Codes.EDGE.BnkAccOwnrCd.None;

            bankAcctRow.DivBranchId = 0; // check
            bankAcctRow.FundFacId = 0; // check
            bankAcctRow.BankAcctStatusCd = int.Parse(bankStatusDDL.SelectedValue);
            bankAcctRow.BankAcctStatusDt = DateTime.Now;
            bankAcctRow.GlobalBankFlg = globalPayrollCheck.Checked;
            bankAcctRow.PaymentCountryNm = payrollNameTxt.Text.Trim().Equals(string.Empty) ? null : payrollNameTxt.Text.Trim();

            if (bankAddressFoundHidden.Value == "1" || !this.InvalidBank)
            {
                bankAcctRow.InvalidBankFlg = false;
                bankAcctRow.InvalidRoutingNumber = false;
            }
            else
            {
                bankAcctRow.InvalidBankFlg = true;
                bankAcctRow.InvalidRoutingNumber = true;
            }
            if (!bankAcctRow.InvalidBankFlg)
            {
                if (paythroughbankReqIndHidden.Value != (paythroughBankNeededCheck.Checked ? "1" : "0"))
                    bankAcctRow.InvalidBankFlg = true;
            }

            if (!bankAcctRow.InvalidBankFlg)
            { 
                if (this.BankName != bankAcctRow.BankNm .Trim())
                    bankAcctRow.InvalidBankFlg = true;
            }

            bankAcctRow.PayThroughBankReqFlg = paythroughBankNeededCheck.Checked;

            if ((this.GeoOrigingCd == CMS.Codes.EDGE.GeoOriginTypCd.Brazil && (this.BankType == BankTypeList.Customer || this.BankType == BankTypeList.Supplier)) || this.BankType == BankTypeList.Division)
            {
                if (this.BankType == BankTypeList.Customer)
                {
                    bankAcctRow.FederalTaxNumber = federalTaxNumTxt.Text.Trim().Equals(string.Empty) ? null : federalTaxNumTxt.Text.Trim();
                }
                if (this.BankType == BankTypeList.Division || this.BankType==BankTypeList.Supplier )
                {
                    bankAcctRow.FederalTaxNumber = federalTaxNumTxt.Text.Trim().Equals(string.Empty) ? null : federalTaxNumTxt.Text.Trim();
                    bankAcctRow.StateTaxNumber = stateTaxIDTxt.Text.Trim().Equals(string.Empty) ? null : stateTaxIDTxt.Text.Trim();
                    bankAcctRow.MunicipalTaxNumber = CityTaxIDTxt.Text.Trim().Equals(string.Empty) ? null : CityTaxIDTxt.Text.Trim();
                }
            }
            if (this.GeoOrigingCd == CMS.Codes.EDGE.GeoOriginTypCd.India || this.BankType == BankTypeList.Division)
            {
                bankAcctRow.PANNbr = PANTextBox.Text.Trim().Equals(string.Empty) ? null : PANTextBox.Text.Trim();
                bankAcctRow.TANNbr = TANTextBox.Text.Trim().Equals(string.Empty) ? null : TANTextBox.Text.Trim();
            }
            bankDS.tblBankAcct.AddtblBankAcctRow(bankAcctRow);
             

            #endregion

            #region Payment Category
            for (int i = 0; i < paymentcategoryCBL.Items.Count; i++)
            {
                if (paymentcategoryCBL.Items[i].Selected)
                {
                    BankDS.tblBankAcctPayCategoryRow bankAcctCatRow = bankDS.tblBankAcctPayCategory.NewtblBankAcctPayCategoryRow();

                    bankAcctCatRow.BankAcctId = this.BankAccountId;
                    bankAcctCatRow.UpdateDt = DateTime.Now;
                    bankAcctCatRow.BankAcctPayCatCd = int.Parse("0" + paymentcategoryCBL.Items[i].Value);
                    bankDS.tblBankAcctPayCategory.AddtblBankAcctPayCategoryRow(bankAcctCatRow);
                }
            }
            #endregion

            #region Bank Address

            BankDS.tblPhysicalAddrRow physicalBankAddressRow = bankDS.tblPhysicalAddr.NewtblPhysicalAddrRow();

            if (BankDataSet.tblPhysicalAddr.Rows.Count > 0)
            {
                physicalBankAddressRow.PhysicalAddrId = this.BankDataSet.tblPhysicalAddr[0].PhysicalAddrId;
                physicalBankAddressRow.LocId = this.BankDataSet.tblPhysicalAddr[0].LocId;
                physicalBankAddressRow.UpdateDt = this.BankDataSet.tblPhysicalAddr[0].UpdateDt;
            }
            else
            {
                physicalBankAddressRow.PhysicalAddrId = CMS.Codes.EDGE.SuppPrimaryId.OneValue;	//Random Output Parameter
                physicalBankAddressRow.LocId = 0;
                physicalBankAddressRow.UpdateDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }

            physicalBankAddressRow.AddrValidateIndCd = CMS.Codes.EDGE.AddrValidateIndCd.Validated; // Let it be validated every time
            if (bankAddressFoundHidden.Value == "1")
            {
                physicalBankAddressRow.AddrLine1 = bankAddressLabel.Text;
                physicalBankAddressRow.AddrLine2 = string.Empty;
                physicalBankAddressRow.AddrLine3 = string.Empty;
                physicalBankAddressRow.CityNm = bankCityLabel.Text;
                physicalBankAddressRow.StProvNm = bankStateLabel.Text;

                physicalBankAddressRow.CountryNm = bankCountryLabel.Text;
                physicalBankAddressRow.PostalCode = bankPostalCodeLabel.Text;
            }
            else
            {
                physicalBankAddressRow.AddrLine1 = bankNewAddrLine1.Text;
                physicalBankAddressRow.AddrLine2 = bankNewAddrLine2Txt.Text;
                physicalBankAddressRow.AddrLine3 = bankNewAddrLine3Txt.Text;
                if (this.BankType == BankTypeList.Supplier)
                    physicalBankAddressRow.CityNm = bankNewAddressCityTxt_Supplier.Text;
                else
                    physicalBankAddressRow.CityNm = bankNewAddressCityTxt.Text;

                physicalBankAddressRow.StProvNm = bankNewAddresssStateProvTxt.Text;

                physicalBankAddressRow.CountryNm = bankNewAddressCountryTxt.Text;
                physicalBankAddressRow.PostalCode = bankNewAddressPostalCodeTxt.Text;
            }

            physicalBankAddressRow.AddrLine4 = String.Empty;
            physicalBankAddressRow.AddrLine5 = String.Empty;
            physicalBankAddressRow.AddrLineAttn = String.Empty;
            physicalBankAddressRow.StProvAbbr = String.Empty;
            physicalBankAddressRow.CountyNm = String.Empty;
            physicalBankAddressRow.AddrOffLocNm = String.Empty;

            bankDS.tblPhysicalAddr.AddtblPhysicalAddrRow(physicalBankAddressRow);
            #endregion

            #region Bank Phone

            if (bankPhoneTxt.Text.Trim().Length > 0)
            {
                BankDS.tblPhoneNumberRow bankPhoneNumberRow = bankDS.tblPhoneNumber.NewtblPhoneNumberRow();

                if (BankDataSet.tblPhoneNumber.Rows.Count > 0)
                {
                    bankPhoneNumberRow.PhoneNbrId = BankDataSet.tblPhoneNumber[0].PhoneNbrId;
                    bankPhoneNumberRow.UpdateDt = BankDataSet.tblPhoneNumber[0].UpdateDt;
                }
                else
                {
                    bankPhoneNumberRow.UpdateDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    
                }

                bankPhoneNumberRow.EnteredPhoneNbr   = bankPhoneTxt.Text.Trim();
                bankPhoneNumberRow.SearchPhoneNbr = bankPhoneTxt.Text.Trim();

                bankDS.tblPhoneNumber.AddtblPhoneNumberRow(bankPhoneNumberRow);
            }

            #endregion

            return bankDS;
        }
        #endregion

        #region GetDescriptionForCode
        /// <summary>
        /// This Method Returns Description for the code and CdType supplied
        /// </summary>
        /// <param name="code"><c>Code</c></param>
        /// <param name="CdType"><c>Code Type</c></param>
        /// <returns><c>Code Description</c></returns>
        public static string GetDescriptionForCode(int code, int CdType)
        {
            string codeDescr = string.Empty;
            if (code != Zero && CdType != Zero)
            {
                Hashtable codeDescrHT = CMS.Codes.Cache.EDGE.Get(CdType).HT;
                IDictionaryEnumerator iDicEnum = codeDescrHT.GetEnumerator();
                while (iDicEnum.MoveNext())
                {
                    if (Convert.ToInt16(iDicEnum.Key) == code)
                    {
                        codeDescr = iDicEnum.Value.ToString();
                        return codeDescr;
                    }
                }
            }
            else
            {
                codeDescr = string.Empty;
            }
            return codeDescr;
        }
        #endregion

        #region EnableInvalidAddress
        public void EnableDisableInvalidAddress(bool enabled)
        {
            invalidAddressLabel.Visible = enabled;
        }
        #endregion

        #region SetAddressCountryPosition
        public void SetAddressCountryPosition()
        {
            if (this.BankType == BankTypeList.Supplier)
            {
                bankNewAddressCityLabel.Visible = false;
                bankNewAddressCityTxt.Visible = false;
                bankNewAddressCityTxtReqValidator.Visible = false;
                CityRegexpvalidator.Visible = false;

                bankNewAddrCountryLabel.Visible = false;
                //bankNewAddressCountryTxt.Visible = false; // requires for State and city validatior
                bankNewAddressCountryTxtCountryValidator.Visible = false;
                bankNewAddressCountryTxtReqValidator.Visible =false;
                addCountryWidget.Visible = false;

                //bankNewAddressCountryTxt_Supplier.Visible = true;

                bankNewAddressCountryTxtCountryValidator_Supplier.Visible = true;
                bankNewAddressCountryTxtReqValidator_Supplier.Visible = true;
                bankNewAddrCountryLabel_Supplier.Visible = true;
                addCountryWidget_Supplier.Visible = true;

                bankNewAddressCityLabel_Supplier.Visible = true;
                bankNewAddressCityTxt_Supplier.Visible = true;
                bankNewAddressCityTxtReqValidator_Supplier.Visible = true;
                CityRegexpvalidator_Supplier.Visible = true;

            }
            else
            {
                bankNewAddressCityLabel.Visible = true;
                bankNewAddressCityTxt.Visible = true;
                bankNewAddressCityTxtReqValidator.Visible = true;
                CityRegexpvalidator.Visible = true;

                //bankNewAddressCountryTxt.Visible = true;
                bankNewAddressCountryTxtCountryValidator.Visible = true;
                bankNewAddrCountryLabel.Visible = true;
                addCountryWidget.Visible = true;
                bankNewAddressCountryTxtReqValidator.Visible = true;

                //bankNewAddressCountryTxt_Supplier.Visible = false;
                bankNewAddressCountryTxtCountryValidator_Supplier.Visible = false;
                bankNewAddrCountryLabel_Supplier.Visible = false;
                addCountryWidget_Supplier.Visible = false;
                bankNewAddressCountryTxtReqValidator_Supplier.Visible = false;

                bankNewAddressCityLabel_Supplier.Visible = false;
                bankNewAddressCityTxt_Supplier.Visible = false;
                bankNewAddressCityTxtReqValidator_Supplier.Visible = false;
                CityRegexpvalidator_Supplier.Visible = false;
            }
        }
        #endregion

        #region SetControlsTabIndex
        public void SetControlsTabIndex()
        {
            bankCountryNameDDL.TabIndex = (short)(this.BaseTabIndex + 1);
            disbursementCurrencyDDL.TabIndex = (short)(this.BaseTabIndex + 2);
            countryContinueBtn.TabIndex = (short)(this.BaseTabIndex + 3);
            routingNumberTxt.TabIndex = (short)(this.BaseTabIndex + 4);
            SWIFTBICCodeTxt.TabIndex = (short)(this.BaseTabIndex + 5);
            findBankBtn.TabIndex = (short)(this.BaseTabIndex + 6);
            bankStatusDDL.TabIndex = (short)(this.BaseTabIndex + 7);
            paymentMethodDDL.TabIndex = (short)(this.BaseTabIndex + 8);
            payeeBankNameTxt.TabIndex = (short)(this.BaseTabIndex + 9);
            bankPhoneTxt.TabIndex = (short)(this.BaseTabIndex + 10);
            bankNewAddrLine1.TabIndex = (short)(this.BaseTabIndex + 11);
            bankNewAddrLine2Txt.TabIndex = (short)(this.BaseTabIndex + 12);
            bankNewAddrLine3Txt.TabIndex = (short)(this.BaseTabIndex + 13);
            bankNewAddressCityTxt.TabIndex = (short)(this.BaseTabIndex + 14);
            bankNewAddressCountryTxt_Supplier.TabIndex = (short)(this.BaseTabIndex + 14);
            bankNewAddresssStateProvTxt.TabIndex = (short)(this.BaseTabIndex + 15);
            bankNewAddressCountryTxt.TabIndex = (short)(this.BaseTabIndex + 16);
            bankNewAddressCityTxt_Supplier.TabIndex = bankNewAddressCountryTxt.TabIndex;
            bankNewAddressPostalCodeTxt.TabIndex = (short)(this.BaseTabIndex + 17);

            IBANAccountNumberTxt.TabIndex = (short)(this.BaseTabIndex + 18);
            accountHolderTxt.TabIndex = (short)(this.BaseTabIndex + 19);
            federalTaxNumTxt.TabIndex = (short)(this.BaseTabIndex + 20);
            stateTaxIDTxt.TabIndex = (short)(this.BaseTabIndex + 21);
            CityTaxIDTxt.TabIndex = (short)(this.BaseTabIndex + 22);
            accountTypeDDL.TabIndex = (short)(this.BaseTabIndex + 23);
            accountClassDDL.TabIndex = (short)(this.BaseTabIndex + 24);
            paymentcategoryCBL.TabIndex = (short)(this.BaseTabIndex + 25);
            paymentcatdescrTxt.TabIndex = (short)(this.BaseTabIndex + 26);
            globalPayrollCheck.TabIndex = (short)(this.BaseTabIndex + 27);
            payrollNameTxt.TabIndex = (short)(this.BaseTabIndex + 28);
            specialInstructionsTxt.TabIndex = (short)(this.BaseTabIndex + 29);
            paythroughBankNeededCheck.TabIndex = (short)(this.BaseTabIndex + 30);
        }
        #endregion

        #region HideBankNotFoundMessage
        public void HideBankNotFoundMessage(bool visible)
        {
            bankNotFoundMessageDiv.Visible = visible;
            if (visible)
            {
                bankNotFoundMessageDiv.Attributes.Add("class", "top_message validation");
                bankNotFoundMessageDiv.InnerText = "Bank Not Found";
                bankNotFoundMessageDiv.Style.Add("color", "red");
            }
        }
        #endregion

        #region GetMouseOverText
        public static string GetMouseOverText(string stringValue, int startIndex, int length)
        {
            string resultString = String.Empty;
            string tempString = String.Empty;
            string mouseOverText = "<span style = 'border:1;' title='{0}' nowrap>{1}</span>";
            string stringCleanValue = stringValue.Replace("'", "&#39;");
            resultString = mouseOverText.Replace("{0}", stringCleanValue);

            if (stringValue.Trim().Length > (length + 1))
            {
                tempString = stringValue.Substring(startIndex, length) + "...";
                string tempstr = resultString.Replace("{1}", tempString);
                return resultString.Replace("{1}", tempString);
            }
            else
            {
                return resultString.Replace("{1}", stringValue);
            }
        }
        #endregion GetMouseOverText

        protected  void FocusControlOnPageLoad(string ClientID,
                                       System.Web.UI.Page page)
        {

            this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(),"CtrlFocus",

            @"<script> 

      function ScrollView()
      {
         var el = document.getElementById('" + ClientID + @"')
         if (el != null && !el.disabled)
         {        
            el.scrollIntoView();
            el.focus();
         }
    
      }

    function window.onload{ScrollView();}

      </script>");

        }

        protected void AccountClassDDL_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "SetPageFocusOnClick", "<script> SetPageFocus('AccountClass');</script>", false); 
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "DisplayBankDetailForRoutNo", "<script>ShowHideBelowFindBank(2);ShowHideBankAddressDetailForRoutingNo();SetPageFocus('AccountClass');</script>", false);
        }
        
          

    }
}