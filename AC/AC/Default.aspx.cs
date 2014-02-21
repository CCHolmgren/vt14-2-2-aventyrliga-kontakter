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

        protected void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ListView1_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }

        public void ListView1_InsertItem()
        {
            var item = new AC.Model.Contact();
            TryUpdateModel(item);
            if (ModelState.IsValid)
            {
                // Save changes here

            }
        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void ListView1_UpdateItem(int id)
        {
            AC.Model.Contact item = null;
            // Load the item here, e.g. item = MyDataLayer.Find(id);
            if (item == null)
            {
                // The item wasn't found
                ModelState.AddModelError("", String.Format("Item with id {0} was not found", id));
                return;
            }
            TryUpdateModel(item);
            if (ModelState.IsValid)
            {
                // Save changes here, e.g. MyDataLayer.SaveChanges();

            }
        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void ListView1_DeleteItem(int id)
        {

        }
    }
}