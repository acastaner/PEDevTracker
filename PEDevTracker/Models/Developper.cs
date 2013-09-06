using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PEDevTracker.Models
{
    public class Developper
    {
        public virtual int Id { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string ProfileId { get; set; }
    }
}