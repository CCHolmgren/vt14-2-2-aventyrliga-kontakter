using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;

namespace AC
{
    [MetadataType(typeof(ContactMetadata))]
    public partial class ContactPartial
    {
    }
    public class ContactMetadata
    {
    }
}