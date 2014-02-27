using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AC.Model
{
    public class Contact
    {
        public int ContactId
        {
            get;
            set;
        }
        [StringLength(50, ErrorMessage="Emailaddressen kan inte vara längre än 50 tecken.")]
        [Required(ErrorMessage="Fyll i en emailaddress.")]
        [RegularExpression(@".+@.+", ErrorMessage="Skriv in en giltig emailadress.")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress
        {
            get;
            set;
        }
        [StringLength(50, ErrorMessage="Förnamnet kan inte vara längre än 50 tecken."),Required(ErrorMessage="Fyll i ett förnamn.")]
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
    }
    public static class ValidationExtensions
    {
        public static bool Validate<T>(this T instance, out ICollection<ValidationResult> validationResults)
        {
            var validationContext = new ValidationContext(instance);
            validationResults = new List<ValidationResult>();
            return Validator.TryValidateObject(instance, validationContext, validationResults, true);
        }
    }

}