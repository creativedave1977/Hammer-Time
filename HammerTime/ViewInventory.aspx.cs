using AjaxControlToolkit;
using HammerTime.Classes;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HammerTime
{
    public partial class ViewInventory : System.Web.UI.Page
    {
        #region "assets"
        string displayMessage;
        Hammer hammer;
        DataTable dtHammers;
        #endregion

        #region "events"
        protected void btnAddHammer_Click(object sender, EventArgs e)
        {
            //validate
            if (ValidateAddHammer())
            {
                //if valid, call the add hammer method
                AddHammer();
            }
            else
            {
                //otherwise, display error message
                DisplayMessage(displayMessage, false, updateHammers);
                mpeAddHammer.Show();
            }
        }

        protected void gvHammers_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Delete":
                    DeleteItem(Convert.ToInt32(e.CommandArgument.ToString()));
                    break;
                case "DoDecrement":
                    AdjustInventory(App.AdjustmentType.Decrement, Convert.ToInt32(e.CommandArgument.ToString()));
                    break;
                case "DoIncrement":
                    AdjustInventory(App.AdjustmentType.Increment, Convert.ToInt32(e.CommandArgument.ToString()));
                    break;
                case "DoUpdate":
                    if (ValidateUpdateHammer(Convert.ToInt32(e.CommandArgument.ToString())))
                    {
                        //if valid, do update
                        UpdateHammer(Convert.ToInt32(e.CommandArgument.ToString()));
                    }
                    else
                    {
                        //if not, show error
                        DisplayMessage(displayMessage, false, updateHammers);

                        //keep the dialog open
                        ModalPopupExtender mpeUpdateHammer = (ModalPopupExtender)gvHammers.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("mpeUpdateHammer");
                        mpeUpdateHammer.Show();
                    }
                    break;
            }
        }

        protected void gvHammers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //cast controls
                Button btnDecrement = (Button)e.Row.FindControl("btnDecrement");
                Button btnDeleteItem = (Button)e.Row.FindControl("btnDeleteItem");
                Button btnIncrement = (Button)e.Row.FindControl("btnIncrement");
                Button btnUpdateHammerModal = (Button)e.Row.FindControl("btnUpdateHammerModal");

                //retreive and assign row index as command argument
                int arg = e.Row.RowIndex;
                btnDecrement.CommandArgument = arg.ToString();
                btnDeleteItem.CommandArgument = arg.ToString();
                btnIncrement.CommandArgument = arg.ToString();
                btnUpdateHammerModal.CommandArgument = arg.ToString();
            }
        }

        protected void gvHammers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //no action at this time
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindInventory();
            }
        }
        #endregion

        #region "methods"
        /// <summary>
        /// Add a new hammer record to the database and re-bind inventory
        /// </summary>
        private void AddHammer()
        {
            string hammerName = txtHammerName.Text.Trim();
            string hammerSKU = txtHammerDesc.Text.Trim();
            int hammerQty = txtHammerQty.Text.Trim() == "" ? 0 : Convert.ToInt32(txtHammerQty.Text.Trim());

            //instantiate new hammer object and add hammer
            hammer = new Hammer();
            int hammerID = hammer.Add(hammerName, hammerSKU, hammerQty);
            if (hammerID > 0)
            {
                //clear form
                txtHammerName.Text = "";
                txtHammerDesc.Text = "";
                txtHammerQty.Text = "";
                mpeAddHammer.Hide();

                //bind inventory
                BindInventory();

                //show success message
                displayMessage = "Successfully added new hammer!";
                DisplayMessage(displayMessage, true, updateHammers);
            }
            else
            {
                //show error message
                displayMessage = "Error adding new hammer!";
                DisplayMessage(displayMessage, false, updateHammers);
            }
        }

        /// <summary>
        /// Increment or decrement inventory
        /// </summary>
        /// <param name="aType"></param>
        /// <param name="atIndex"></param>
        private void AdjustInventory(App.AdjustmentType aType, int atIndex)
        {
            TextBox txtAdj;
            int adj;

            //instantiate hammer object
            hammer = new Hammer();

            //cast grid controls to retrieve item id, name, and adjustment that was entered
            HiddenField hidHammerID = (HiddenField)gvHammers.Rows[atIndex].FindControl("hidHammerID");
            Label lblHammerName = (Label)gvHammers.Rows[atIndex].FindControl("lblHammerName");
            if (aType == App.AdjustmentType.Decrement)
            {
                txtAdj = (TextBox)gvHammers.Rows[atIndex].FindControl("txtDecrementQty");
            }
            else
            {
                txtAdj = (TextBox)gvHammers.Rows[atIndex].FindControl("txtIncrementQty");
            }
            adj = Convert.ToInt32(txtAdj.Text.Trim());

            //submit adjustment and display result
            if (hammer.AdjustHammerQty(Convert.ToInt32(hidHammerID.Value), adj, aType))
            {
                //show success message
                displayMessage = aType == App.AdjustmentType.Decrement ? "Successfully reduced " + lblHammerName.Text + " by " + adj : "Successfully increased " + lblHammerName.Text + " by " + adj;
                DisplayMessage(displayMessage, true, updateHammers);

                //clear adjustment
                txtAdj.Text = "";

                //bind inventory
                BindInventory();
            }
            else
            {
                displayMessage = "Error adjusting inventory";
                DisplayMessage(displayMessage, false, updateHammers);
            }
        }

        /// <summary>
        /// Bind Hammer Inventory
        /// </summary>
        private void BindInventory()
        {
            //get hammers
            dtHammers = Hammer.GetAllHammers();
            if (dtHammers.Rows.Count > 0)
            {
                //bind hammers if results are returned
                gvHammers.DataSource = dtHammers;
                gvHammers.DataBind();
                divHammers.Visible = true;
                divNoHammers.Visible = false;
            }
            else
            {
                //display message if no hammers are returned
                divHammers.Visible = false;
                divNoHammers.Visible = true;
            }
        }

        /// <summary>
        /// Delete selected item
        /// </summary>
        /// <param name="atIndex"></param>
        private void DeleteItem(int atIndex)
        {
            //instantiate hammer object
            hammer = new Hammer();

            //cast grid controls to retrieve item id and name
            HiddenField hidHammerID = (HiddenField)gvHammers.Rows[atIndex].FindControl("hidHammerID");
            Label lblHammerName = (Label)gvHammers.Rows[atIndex].FindControl("lblHammerName");

            //request deletion and display result
            if (hammer.DeleteHammer(Convert.ToInt32(hidHammerID.Value)))
            {
                //bind inventory
                BindInventory();

                //show success message
                displayMessage = "Successfully deleted item: " + lblHammerName.Text;
                DisplayMessage(displayMessage, true, updateHammers);
            }
            else
            {
                displayMessage = "Error deleting item: " + lblHammerName.Text;
                DisplayMessage(displayMessage, false, updateHammers);
            }
        }

        /// <summary>
        /// Displays success or error message on client side
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isSuccess"></param>
        /// <param name="uPanel"></param>
        private void DisplayMessage(string message, bool isSuccess, UpdatePanel uPanel = null)
        {
            //take out bad characters
            message = message.Replace("'", "&quot;");
            message = App.GetFirstLine(message);

            if (isSuccess)
            {
                if (uPanel == null)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "updateInfo", "ShowMessage('" + message + "', true);", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(uPanel, uPanel.GetType(), "updateInfo", "ShowMessage('" + message + "', true);", true);
                }
            }
            else
            {
                if (uPanel == null)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "updateInfo", "ShowMessage('" + message + "', false);", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(uPanel, uPanel.GetType(), "updateInfo", "ShowMessage('" + message + "', false);", true);
                }
            }
        }

        /// <summary>
        /// Update selected hammer name or SKU
        /// </summary>
        /// <param name="atIndex"></param>
        private void UpdateHammer(int atIndex)
        {
            //cast objects and set update variables
            HiddenField hidHammerID = (HiddenField)gvHammers.Rows[atIndex].FindControl("hidHammerID");
            TextBox txtUpdateHammerName = (TextBox)gvHammers.Rows[atIndex].FindControl("txtUpdateHammerName");
            TextBox txtUpdateHammerDesc = (TextBox)gvHammers.Rows[atIndex].FindControl("txtUpdateHammerDesc");
            Label lblQty = (Label)gvHammers.Rows[atIndex].FindControl("lblQty");

            string hammerName = txtUpdateHammerName.Text.Trim();
            string hammerSKU = txtUpdateHammerDesc.Text.Trim();
            int hammerQty = Convert.ToInt32(lblQty.Text);

            //instantiate new hammer object and update hammer details
            hammer = new Hammer();
            if (hammer.Update(Convert.ToInt32(hidHammerID.Value), hammerName, hammerSKU, hammerQty))
            {
                //clear form
                txtUpdateHammerName.Text = "";
                txtUpdateHammerDesc.Text = "";

                //bind inventory
                BindInventory();

                //show success message
                displayMessage = "Successfully updated " + txtHammerName.Text;
                DisplayMessage(displayMessage, true, updateHammers);
            }
            else
            {
                //show error message
                displayMessage = "Error updating " + txtHammerName.Text;
                DisplayMessage(displayMessage, false, updateHammers);
            }
        }

        /// <summary>
        /// Validate whether or not fields are filled and hammer name/sku is not already in DB (for adding new hammers)
        /// </summary>
        /// <returns>Whether or not the hammer can be added to the database</returns>
        private bool ValidateAddHammer()
        {
            string query = string.Empty;
            bool ynValid = true;
            DataRow[] dr;

            //get hammers
            dtHammers = Hammer.GetAllHammers();

            //validate hammer name
            if (txtHammerName.Text.Trim() == "")
            {
                ynValid = false;
                displayMessage += "Hammer name is required<br />";
            }
            else
            {
                //check to see if name already exists
                query = "HammerName = '" + txtHammerName.Text.Trim() + "'";
                dr = dtHammers.Select(query);
                if (dr.Length > 0)
                {
                    ynValid = false;
                    displayMessage += "Hammer name already exists<br />";
                }
            }

            //validate hammer SKU
            if (txtHammerDesc.Text.Trim() == "")
            {
                ynValid = false;
                displayMessage += "Hammer SKU is required<br />";
            }
            else
            {
                //check to see if sku (Desc) already exists
                query = "HammerDesc = '" + txtHammerDesc.Text.Trim() + "'";
                dr = dtHammers.Select(query);
                if (dr.Length > 0)
                {
                    ynValid = false;
                    displayMessage += "Hammer SKU already exists<br />";
                }
            }

            //return result
            return ynValid;
        }

        /// <summary>
        /// Validate whether or not fields are filled and hammer name/sku is not already in DB (for updating hammers)
        /// </summary>
        /// <returns>Whether or not the hammer can be added to the database</returns>
        private bool ValidateUpdateHammer(int atIndex)
        {
            string query = string.Empty;
            bool ynValid = true;
            DataRow[] dr;

            //get existing hammers
            dtHammers = Hammer.GetAllHammers();

            //cast embedded controls
            HiddenField hidHammerID = (HiddenField)gvHammers.Rows[atIndex].FindControl("hidHammerID");
            int thisHammerID = Convert.ToInt32(hidHammerID.Value);
            TextBox txtUpdateHammerName = (TextBox)gvHammers.Rows[atIndex].FindControl("txtUpdateHammerName");
            TextBox txtUpdateHammerDesc = (TextBox)gvHammers.Rows[atIndex].FindControl("txtUpdateHammerDesc");

            //validate hammer name
            if (txtUpdateHammerName.Text.Trim() == "")
            {
                ynValid = false;
                displayMessage += "Hammer name is required<br />";
            }
            else
            {
                //check to see if name already exists that is not associate with this hammer
                query = "HammerName = '" + txtUpdateHammerName.Text.Trim() + "' AND HammerID <> " + thisHammerID;
                dr = dtHammers.Select(query);
                if (dr.Length > 0)
                {
                    ynValid = false;
                    displayMessage += "Hammer name already exists for another item<br />";
                }
            }

            //validate hammer SKU
            if (txtUpdateHammerDesc.Text.Trim() == "")
            {
                ynValid = false;
                displayMessage += "Hammer SKU is required<br />";
            }
            else
            {
                //check to see if sku (Desc) already exists
                query = "HammerDesc = '" + txtUpdateHammerDesc.Text.Trim() + "' AND HammerID <> " + thisHammerID;
                dr = dtHammers.Select(query);
                if (dr.Length > 0)
                {
                    ynValid = false;
                    displayMessage += "Hammer SKU already exists for another item<br />";
                }
            }

            //return result
            return ynValid;
        }
        #endregion
    }
}