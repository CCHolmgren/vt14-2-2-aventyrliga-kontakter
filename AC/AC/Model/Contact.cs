using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AC.Model
{
    [MetadataType(typeof(Contact))]
    public partial class ContactPartial { }
    public class Contact
    {
        public int ContactId
        {
            get;
            set;
        }
        [StringLength(50), Required(), DataType(DataType.EmailAddress) ]
        public string EmailAddress
        {
            get;
            set;
        }
        [StringLength(50),Required()]
        public string FirstName
        {
            get;
            set;
        }
        [StringLength(50),Required()]
        public string LastName
        {
            get;
            set;
        }
    }
}