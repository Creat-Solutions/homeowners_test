using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ME.Admon;
using System.Collections.Generic;

public partial class LoadPage : System.Web.UI.Page
{
    private Hashtable m_commands = new Hashtable();
 
    private void InitCommands()
    {
        HttpContext currentContext = HttpContext.Current;
        AbstractLoadCommand quoteLoadCommand = new QuoteLoadCommand(currentContext, lblError);
        m_commands.Add("default", quoteLoadCommand);
        m_commands.Add("newquote", new NewQuoteLoadCommand(currentContext));
        m_commands.Add("quickquote", new QuickQuoteLoadCommand(currentContext));
        m_commands.Add("generatequote", new GenerateQuoteCommand(currentContext));
        m_commands.Add("clear", new ClearLoadCommand(currentContext));
        m_commands.Add("quote", quoteLoadCommand);
        m_commands.Add("editpolicy", new EditPolicyLoadCommand(currentContext, lblError, lblLoad));
        m_commands.Add("buyquote", new BuyQuoteLoadCommand(currentContext, lblError));
    }

    private AbstractLoadCommand GetLoadCommand()
    {
        string option = string.Empty;
        if (string.IsNullOrEmpty(Request.QueryString["option"])) {
            option = "default";
        } else {
            option = Request.QueryString["option"];
        }
        try {
            AbstractLoadCommand command = m_commands[option] as AbstractLoadCommand;
            return command;
        } catch {
            return null;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        InitCommands();
        AbstractLoadCommand command = GetLoadCommand();
        if (command == null) {
            lblError.Text = "There is no such option to load.";
        }
        command.Init();
    }

    protected void btnLoad_click(object sender, EventArgs e)
    {
        AbstractLoadCommand command = GetLoadCommand();
        if (command == null) {
            lblError.Text = "There is no such option to load.";
        }
        command.Load(txtLoad.Text);
        //clear data
        txtLoad.Text = string.Empty;
        
    }
   
}
