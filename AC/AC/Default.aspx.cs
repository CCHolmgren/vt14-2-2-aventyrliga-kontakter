using AC.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AC
{
    public partial class Default : System.Web.UI.Page
    {
        private Service _service;
        private Service Service
        {
            get { return _service ?? (_service = new Service()); }
        }
        private DataPager _datapager;
        private DataPager DataPager
        {
            get
            {
                return _datapager ?? (_datapager = (DataPager)ListView1.FindControl("DataPager"));
            }
        }
        private string SuccessMessage
        {
            get { return Session["SuccessMessage"] as string; }
            set { Session["SuccessMessage"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ModelState.Remove("");
            ListView1.InsertItemPosition = InsertItemPosition.None;
            if (SuccessMessage != null)
            {
                SuccessPanel.Visible = true;
                SuccessLabel.Text = SuccessMessage;
                //Session.Remove("SuccessMessage");
                SuccessMessage = null;
            }
        }
        private void setModelState(string message, string caller)
        {
            //ModelState.AddModelError(String.Empty, message + caller);
        }
        protected void NewContactButton_Click(object sender, EventArgs e)
        {
            ListView1.InsertItemPosition = InsertItemPosition.FirstItem;
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IEnumerable<AC.Model.Contact> ListView1_GetData(int maximumRows, int startRowIndex, out int totalRowCount, string sortByExpression)
        {
            try
            {
                return Service.GetContactsPageWise(maximumRows, startRowIndex, out totalRowCount);
            }
            catch (System.Data.SqlClient.SqlException sx)
            {
                setModelState(sx.Message, "GetData");
                //ModelState.AddModelError("", sx.Message+"GetData");
                totalRowCount = 0;
                return null;
            }
            catch (ConnectionException cx)
            {
                setModelState(cx.Message, "GetData");
                //ModelState.AddModelError("", cx.Message+"GetData");
                totalRowCount = 0;
                return null;
            }
        }
        public void ListView1_InsertItem(Contact contact)
        {
            if (Page.IsValid)
            {
                try
                {
                    Service.SaveContact(contact);
                    SuccessMessage = String.Format("Kontakten har lagts till.");
                    Response.Redirect(String.Format("?page={0}",(DataPager.TotalRowCount/DataPager.PageSize)+1));
                }
                catch (ArgumentException ax)
                {
                    ModelState.Remove("");
                    List<ValidationResult> vr = (List<ValidationResult>)ax.Data["ValidationResult"];
                    vr.ForEach(x => setModelState(x.ErrorMessage, "InsertItem"));
                }
                catch(System.Data.SqlClient.SqlException sx)
                {
                    setModelState("Ett oväntat fel inträffade vid skapandet av kontakten.", "InsertItem");
                    //ModelState.AddModelError("", "Ett oväntat fel inträffade vid skapandet av kontakten." + "InsertItem");
                }
                catch (ConnectionException cx)
                {
                    setModelState(cx.Message, "InsertItem");
                    //ModelState.AddModelError("", cx.Message+"InsertItem");
                }
            }
        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void ListView1_UpdateItem(int contactId)
        {
            if (Page.IsValid)
            {
                try
                {
                    Contact contact = Service.GetContact(contactId);

                    if (contact == null)
                    {
                        // The item wasn't found
                        setModelState("Item with id was not found", "UpdateItem");
                        //ModelState.AddModelError("", String.Format("Item with id {0} was not found", contactId)+"UpdateItem");
                        return;
                    }

                    if (TryUpdateModel(contact))
                    {
                        // Save changes here, e.g. MyDataLayer.SaveChanges();
                        try
                        {
                            Service.SaveContact(contact);
                            SuccessMessage = String.Format("Kontakten uppdaterades.");
                            Response.Redirect(String.Format("?page={0}", DataPager.StartRowIndex / DataPager.PageSize + 1), true);
                        }
                        catch(ArgumentException ax)
                        {
                            List<ValidationResult> vr = (List<ValidationResult>)ax.Data["ValidationResult"];
                            vr.ForEach(x => setModelState(x.ErrorMessage, "UpdateItem"));
                        }
                        catch (System.Data.SqlClient.SqlException ex)
                        {
                            setModelState("Ett oväntat fel inträffade vid uppdateringen av kontakten.", "InsertItem");
                            //ModelState.AddModelError("", String.Format("Ett oväntat fel inträffade vid uppdateringen av kontakten."+"UpdateItem"));
                        }
                    }
                }
                catch(ArgumentException ax)
                {
                    setModelState(ax.Message, "UpdateItemOuter");
                    //ModelState.AddModelError("", String.Format("{0}",ax.Message)+"UpdateItemOuter");
                }
                catch (ConnectionException cx)
                {
                    setModelState(cx.Message, "UpdateItem");
                    //ModelState.AddModelError("", cx.Message+"UpdateItem");
                }
            }
        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void ListView1_DeleteItem(int contactId)
        {
            try
            {
                Service.DeleteContact(contactId);
                SuccessMessage = String.Format("Kontakten togs bort.");
            }
            catch (ArgumentException ax)
            {
                setModelState(ax.Message, "DeleteItem");
                //ModelState.AddModelError("", String.Format("{0}", ax.Message)+"DeleteItem");
            }
            catch(Exception ex)
            {
                setModelState("Ett oväntat fel inträffade vid borttagningen av kontakten.", "DeleteItem");
                //ModelState.AddModelError("", String.Format("Ett oväntat fel inträffade vid borttagningen av kontakten. {0}",ex.Message)+"DeleteItem");
            }
        }
    }
}