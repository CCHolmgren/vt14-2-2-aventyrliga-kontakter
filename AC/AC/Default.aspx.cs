﻿using AC.Model;
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
        /// <summary>
        /// Represents a Service object
        /// If there isn't already one, create a new one
        /// </summary>
        private Service _service;
        private Service Service { get { return _service ?? (_service = new Service()); } }

        /// <summary>
        /// Returns the DataPager that is associated with ListView1
        /// </summary>
        private DataPager _datapager;
        private DataPager DataPager { get { return _datapager ?? (_datapager = (DataPager)ListView1.FindControl("DataPager")); } }

        /// <summary>
        /// Represents the Session["SuccessMessage"]
        /// But in an easier format
        /// </summary>
        private string SuccessMessage 
        {
            get { return Session["SuccessMessage"] as string; }
            set { Session["SuccessMessage"] = value; } 
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //ListView1.InsertItemPosition = InsertItemPosition.None;
            if (SuccessMessage != null)
            {
                SuccessPanel.Visible = true;
                SuccessLabel.Text = SuccessMessage;
                SuccessMessage = null;
            }
        }
        /// <summary>
        /// Adds a ModelError with the given message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caller"></param>
        private void setModelState(string message, string caller = null)
        {
            ModelState.AddModelError(String.Empty, message + caller);
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
                setModelState(sx.Message);
                totalRowCount = 0;
                return null;
            }
            catch (ConnectionException cx)
            {
                setModelState(cx.Message);
                totalRowCount = 0;
                return null;
            }
        }
        public void ListView1_InsertItem(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Service.SaveContact(contact);
                    SuccessMessage = String.Format("Kontakten har lagts till.");
                    //Inserted items get put at the last page, so Redirect to that page which is TotalRowCount/PageSizse+1
                    Response.Redirect(String.Format("?page={0}",(DataPager.TotalRowCount/DataPager.PageSize)+1));
                }
                catch (ValidationException vx)
                {
                    var validationResult = vx.Data["validationResult"] as List<ValidationResult>;
                    validationResult.ForEach(r => ModelState.AddModelError(String.Empty, r.ErrorMessage));
                }
                catch(System.Data.SqlClient.SqlException sx)
                {
                    setModelState("Ett oväntat fel inträffade vid skapandet av kontakten.");
                }
                catch (ConnectionException cx)
                {
                    setModelState(cx.Message);
                }
            }
        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void ListView1_UpdateItem(int contactId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Contact contact = Service.GetContact(contactId);

                    if (contact == null)
                    {
                        // The item wasn't found
                        setModelState(String.Format("Item with id {0} was not found", contactId));
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
                        catch (System.Data.SqlClient.SqlException ex)
                        {
                            setModelState("Ett oväntat fel inträffade vid uppdateringen av kontakten.");
                        }
                        catch (ValidationException vx)
                        {
                            var validationResult = vx.Data["validationResult"] as List<ValidationResult>;
                            validationResult.ForEach(r => ModelState.AddModelError(String.Empty, r.ErrorMessage));
                        }
                    }
                }
                catch(ArgumentException ax)
                {
                    setModelState(ax.Message);
                }
                catch (ConnectionException cx)
                {
                    setModelState(cx.Message);
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
                Response.Redirect(String.Format("?page={0}", DataPager.StartRowIndex / DataPager.PageSize + 1));
            }
            catch (ArgumentException ax)
            {
                setModelState(ax.Message);
            }
            catch(Exception ex)
            {
                setModelState("Ett oväntat fel inträffade vid borttagningen av kontakten.");
            }
        }
    }
}