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
        [StringLength(50, ErrorMessage="Emailaddressen kan inte vara längre än 50 tecken."), Required(ErrorMessage="Fyll i en emailaddress."), DataType(DataType.EmailAddress,ErrorMessage="Fyll i en korrekt emailaddress.") ]
        public string EmailAddress
        {
            get;
            set;
        }
        [StringLength(50, ErrorMessage="Förnamnet kan inte vara längre än 50 tecken myes."),Required(ErrorMessage="Fyll i ett förnamn.")]
        public string FirstName
        {
            get;
            set;
        }
        [StringLength(50, ErrorMessage="Efternamnet kan inte vara längre än 50 tecken."),Required(ErrorMessage="Fyll i ett efternamn.")]
        public string LastName
        {
            get;
            set;
        }
        public List<ValidationResult> Validate(ref List<ValidationResult> vr,bool validateAll)
        {
            Validator.TryValidateObject(this, new ValidationContext(this), vr, validateAll);
            return vr;
        }
    }
}