using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

using Org.BouncyCastle.Bcpg.Sig;

using Protec.Authentication.Membership;
using Protec.Tools;
using Telepro.Conexion;

public partial class Application_EditPolicy_Agent : System.Web.UI.Page
{
   protected void Page_Load(object sender, EventArgs e)
   {
      if (Session["policyid"] == null)
      {
         Response.Redirect("~/Application/Load.aspx?option=editpolicy", true);
         return;
      }
      string policyno = Session["policyno"] as string;
      UserInformation user = (UserInformation) User.Identity;
      if (!AdminGateway.Gateway.CanEditAgentInformation(user.Agent, policyno, user.UserType))
      {
         Response.Redirect("~/Application/EditPolicy/", true);
         return;
      }
      if (!Page.IsPostBack)
      {
         LoadAgentInformation();
      }
   }

   private void LoadAgentInformation()
   {
      int id = (int) Session["policyid"];
      HouseInsuranceDataContext db = new HouseInsuranceDataContext();
      var policyInf = db.policies.Where(m => m.id == id).Single();
      var policyInfo = db.policies.Where(m => m.id == id).Select(m => new { m.issuedBy, m.company }).Single();
      UserInformationDB information = AdminGateway.Gateway.GetUserInformation(policyInfo.issuedBy);

      dbConexion con = new dbConexion(ApplicationConstants.ADMINDB);
      string sql = $"SELECT product, company, inscofee, MGAPolicyFee FROM admonPoliza.dbo.productCompany WHERE company = {ApplicationConstants.COMPANY} and product = {ApplicationConstants.APPLICATION}";
      DataTable dt = con.ExecuteDS(sql).Tables[0];

      decimal MGAPolicyFee = 0;
      decimal InsCoFee = 0;
      bool changeFee = false;

      decimal.TryParse(dt.Rows[0]["MGAPolicyFee"].ToString(), out MGAPolicyFee);
      decimal.TryParse(dt.Rows[0]["inscofee"].ToString(), out InsCoFee);

      decimal totalFee = MGAPolicyFee + InsCoFee;

      if (totalFee > policyInf.fee)
      {
         //string sqlChgFee = $"select bd.* from branchDetail bd inner join branch b on bd.branch = b.branch inner join branchUser bu on bu.branch = b.branch where product = {ApplicationConstants.APPLICATION} and company = {ApplicationConstants.COMPANY} and bu.branch = {information.BranchInformation.Branch}";

         //dbConexion con = new dbConexion(ApplicationConstants.ADMINDB);
         //DataTable dtBranchDetail = con.ExecuteDS(sqlChgFee).Tables[0];
         
         //if (dtBranchDetail.Rows.Count > 0)
         //{
         //   changeFee = bool.Parse(dtBranchDetail.Rows[0]["changePolicyFee"].ToString());
         //}
         changeFee = true;
      }




      ddlAgent.DataSource = changeFee ? AdminGateway.Gateway.GetListOfAgentsCanChangePolicyFee(policyInfo.company) : AdminGateway.Gateway.GetListOfAgents(policyInfo.company);
      ddlAgent.DataBind();
      ddlAgent.Items.FindByValue(information.AgentInformation.Agent.ToString()).Selected = true;

      OnSelectedAgent(information.AgentInformation.Agent, policyInfo.company);
      ddlBranch.Items.FindByValue(information.BranchInformation.Branch.ToString()).Selected = true;

      OnSelectedBranch(information.BranchInformation.Branch, policyInfo.company);

      var ddlU = ddlUser.Items.FindByValue(information.User.ToString());

      if (ddlU != null)
      {
         ddlU.Selected = true;
      }

      lblLicenseText.Text = information.UserLicense;
      lblPhoneText.Text = information.UserTelephone;

      lblAgentHidden.Text = information.AgentInformation.Agent.ToString();
      lblBranchHidden.Text = information.BranchInformation.Branch.ToString();
      lblUserHidden.Text = policyInfo.issuedBy.ToString();
   }

   private void OnSelectedAgent(int agent, int company)
   {
      ddlBranch.DataSource = AdminGateway.Gateway.GetListOfBranches(company, agent);
      ddlBranch.DataBind();
      ddlBranch.Items.Insert(0, new ListItem
      {
         Text = "---",
         Value = "0",
      });
   }

   private void OnSelectedBranch(int branch, int company)
   {
      ddlUser.DataSource = AdminGateway.Gateway.GetListOfBranchUsersFromProduct(branch, ApplicationConstants.APPLICATION, company);
      ddlUser.DataBind();
      ddlUser.Items.Insert(0, new ListItem
      {
         Text = "---",
         Value = "0",
      });
      BranchInformation info = AdminGateway.Gateway.GetBranchInformation(branch);
      lblCityState.Text = info.BranchCityState;
      lblCountry.Text = info.BranchCountry;
      lblZipCode.Text = info.BranchZipCode;
   }

   private void OnSelectedUser(int user)
   {
      UserInformationDB info = AdminGateway.Gateway.GetUserInformation(user);
      lblLicenseText.Text = info.UserLicense;
      lblPhoneText.Text = info.UserTelephone;
   }

   protected void ddlAgent_SelectedIndexChanged(object sender, EventArgs e)
   {
      HouseInsuranceDataContext db = new HouseInsuranceDataContext();
      int policyid = (int) Session["policyid"];
      int company = db.policies.Where(m => m.id == policyid).Select(m => m.company).Single();
      OnSelectedAgent(int.Parse(ddlAgent.SelectedValue), company);
      OnSelectedBranch(int.Parse(ddlBranch.Items[0].Value), company);
      OnSelectedUser(int.Parse(ddlUser.Items[0].Value));
      ddlBranch.SelectedIndex = 0;
      ddlUser.SelectedIndex = 0;
   }

   protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
   {
      HouseInsuranceDataContext db = new HouseInsuranceDataContext();
      int policyid = (int) Session["policyid"];
      int company = db.policies.Where(m => m.id == policyid).Select(m => m.company).Single();
      OnSelectedBranch(int.Parse(ddlBranch.SelectedValue), company);
      OnSelectedUser(int.Parse(ddlUser.Items[0].Value));
      ddlUser.SelectedIndex = 0;
   }

   protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
   {
      OnSelectedUser(int.Parse(ddlUser.SelectedValue));
   }

   protected void ddlUser_Validate(object sender, ServerValidateEventArgs e)
   {
      try
      {
         int data = int.Parse(e.Value);
         e.IsValid = AdminGateway.Gateway.IsUserAuthorizedInProduct(data, ApplicationConstants.APPLICATION);
      } catch
      {
         e.IsValid = false;
      }
   }

   private void SaveAgentInformation()
   {
      HouseInsuranceDataContext db = new HouseInsuranceDataContext();
      string policyno = Session["policyno"] as string;
      int policyid = (int) Session["policyid"];
      int newUser = int.Parse(ddlUser.SelectedValue);
      string agentName = ddlAgent.SelectedItem.Text;
      string agentAddress = ddlBranch.SelectedItem.Text;
      string userName = ddlUser.SelectedItem.Text;
      string notes = string.Empty;

      if (lblAgentHidden.Text != ddlAgent.SelectedValue)
      {
         notes += "Change Agent to:" + ddlAgent.SelectedValue.Replace("'", "") + ";";
      }
      if (lblBranchHidden.Text != ddlBranch.SelectedItem.Value)
      {
         notes += "Change Branch to:" + ddlBranch.SelectedValue.Replace("'", "") + ";";
      }
      if (lblUserHidden.Text != ddlUser.SelectedItem.Value)
      {
         notes += "Change User to:" + ddlUser.SelectedValue.Replace("'", "");
      }

      if (!string.IsNullOrEmpty(notes))
      {
         //save in local database
         //db.spUpd_V1_policyAgent(policyid, newUser, agentName, agentAddress, userName);

         int userid = ((UserInformation) User.Identity).UserID;
         //save in the admin database
         //AdminGateway.Gateway.ChangeAgent(policyno, newUser, userid, notes);

         using (SqlConnection conn = GetMapfreConnection())
         {
            var trans = BeginTransaction(conn, "change_Agent");

            try
            {
               var command = new SqlCommand("spUpd_V2_policyAgent", conn, trans);
               command.CommandType = System.Data.CommandType.StoredProcedure;
               command.Parameters.AddWithValue("@id", policyid);
               command.Parameters.AddWithValue("@userid", newUser);
               command.Parameters.AddWithValue("@agentName", agentName);
               command.Parameters.AddWithValue("@agentAddress", agentAddress);
               command.Parameters.AddWithValue("@username", userName);
               command.Parameters.AddWithValue("@policyno", policyno);
               command.Parameters.AddWithValue("@product", ApplicationConstants.APPLICATION);
               command.Parameters.AddWithValue("@madeby", userid.ToString());
               command.Parameters.AddWithValue("@description", notes);
               command.ExecuteNonQuery();
            } catch
            {
               RollbackTransaction(trans, "change_Agent");
               trans = null;
            } finally
            {
               if (trans != null)
               {
                  CommitTransaction(trans);
                  trans = null;
               }
            }
         }
      }
   }

   protected void btnSave_Click(object sender, EventArgs e)
   {
      SaveAgentInformation();
      Response.Redirect("~/Application/EditPolicy/", true);
   }

   protected void btnCancel_Click(object sender, EventArgs e)
   {
      Response.Redirect("~/Application/EditPolicy/", true);
   }

   private SqlConnection GetConnection(int connIndex)
   {
      var connection = new SqlConnection(new ConexionToCS().GetConnectionString(connIndex));
      connection.Open();
      return connection;
   }

   private SqlConnection GetMapfreConnection()
   {
      return GetConnection(ApplicationConstants.DB);
   }

   private SqlTransaction BeginTransaction(SqlConnection conn, string name)
   {
      return conn.BeginTransaction(name);
   }

   private void RollbackTransaction(SqlTransaction conn, string name)
   {
      conn.Rollback(name);
   }

   private void CommitTransaction(SqlTransaction conn)
   {
      conn.Commit();
   }
}
