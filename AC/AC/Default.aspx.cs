using AC.Model;
using System;
using System.Collections.Generic;
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
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IEnumerable<AC.Model.Contact> ListView1_GetData(int maximumRows, int startRowIndex, out int totalRowCount, string sortByExpression)
        {
            return Service.GetContactsPageWise(maximumRows, startRowIndex, out totalRowCount);
        }
        public void ListView1_InsertItem(Contact contact)
        {
            try
            {
                Service.SaveContact(contact);
                Response.Redirect("");
            }
            catch
            {
                ModelState.AddModelError("", "Ett oväntat fel inträffade.");
            }
            /*var item = new AC.Model.Contact();
            TryUpdateModel(item);
            if (ModelState.IsValid)
            {
                // Save changes here

            }*/
        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void ListView1_UpdateItem(int id)
        {
            try
            {
                Contact contact = Service.GetContact(id);

                if (contact == null)
                {
                    // The item wasn't found
                    ModelState.AddModelError("", String.Format("Item with id {0} was not found", id));
                    return;
                }

                if (TryUpdateModel(contact))
                {
                    // Save changes here, e.g. MyDataLayer.SaveChanges();
                    Service.SaveContact(contact);
                    Response.Redirect("");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Ett oväntat fel inträffade.");
            }
        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void ListView1_DeleteItem(int id)
        {
            try
            {
                Service.DeleteContact(id);
            }
            catch
            {
                ModelState.AddModelError("", "Ett oväntat fel inträffade.");
            }
        }
    }
}