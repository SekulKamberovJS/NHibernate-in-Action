using System;

using NHibernateInAction.CaveatEmptor;
using NHibernateInAction.CaveatEmptor.Model;
using NHibernateInAction.CaveatEmptor.Dao;
using NHibernateInAction.CaveatEmptor.Dao.NHibernate;

public partial class ApproveItem : System.Web.UI.Page {
    User loggedUser = new User();
    ItemDAO itemDAO = new NHibernateDAOFactory().GetItemDAO();
    //System.Web.UI.WebControls.TextBox editItemName = new System.Web.UI.WebControls.TextBox();
    //System.Web.UI.WebControls.Button btnApprove = new System.Web.UI.WebControls.Button();
    // ---
    const string ItemKey = "NHibernateInAction.CaveatEmptor.Web.ViewItem.ItemKey";
    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            long itemId = long.Parse(Context.Request.QueryString["Id"]);
            // First request implicitly start the conversation
            Item item = itemDAO.FindById(itemId);
            Session[ItemKey] = item;
            editItemName.Text = item.Description; // TODO: Just for testing
            // ... Show the item
            //btnApprove.Click += new EventHandler(btnApprove_Click);
        }
    }
    protected void btnApprove_Click(object sender, EventArgs e) {
        // Second request uses the running conversation and ends it
        Item item = (Item) Session[ItemKey];
        item.Description = editItemName.Text; // TODO: Just for testing
        item.Approve(loggedUser);
        NHibernateConversationWebModule.EndConversationAtTheEndOfThisRequest();
        Context.Response.Redirect("Default.aspx");
    }
}