using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PEDevTracker.Models
{
    #region Developer class
    public class Developer
    {
        #region Attributes
        public virtual int Id { get; set; }
        public virtual string ProfileId { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string RealName { get; set; }
        #endregion
        #region Constructors
        public Developer()
        {

        }
        public Developer(string profileId, string displayName, string realName)
        {
            this.ProfileId = profileId;
            this.DisplayName = displayName;
            this.RealName = realName;
        }

        #endregion
        #region Methods
        public virtual Uri GetProfileUri()
        {
            return new Uri("http://forums.obsidian.net/user/" + ProfileId + "/");

        }
        #endregion
    }
    #endregion
    #region Fluent NHibernate Mappings
    public class AuthorMap : ClassMap<Developer>
    {
        public AuthorMap()
        {
            Table("pedt_Developer"); 
            Id(x => x.Id);
            Map(x => x.ProfileId);
            Map(x => x.DisplayName);
            Map(x => x.RealName);
        }
    }
    #endregion
}