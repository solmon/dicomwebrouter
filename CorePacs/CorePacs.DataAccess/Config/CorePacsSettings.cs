using CorePacs.DataAccess.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CorePacs.DataAccess.Config
{
    public class CorePacsSettings
    {
        public CorePacsSettings() { }
        public void Build(List<Settings> settings, List<AETitles> associations) {
            var bPath = settings.Where(x => x.Name.Equals("BASEPATH")).FirstOrDefault();
            if (bPath == null)
            {
                throw new Exception("No BasePath Found");
            }
            else
            {
                this.BasePath = bPath.Value;
            }

            var bPort = settings.Where(x => x.Name.Equals("PORT")).FirstOrDefault();
            if (bPort == null)
            {
                throw new Exception("No Port Set");
            }
            else
            {
                this.PacsPort = Int32.Parse(bPort.Value);
            }
            this.Associations = associations;
        }
        public string BasePath { get; set; }
        public int PacsPort { get; set; }
        public List<AETitles> Associations { get; set; }
        public bool IsValidAssociation(string aeTitle) {
            var association = this.Associations.Where(x => x.AETitle.Equals(aeTitle, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (association != null) return true;
            return false;
        }
    }
}
