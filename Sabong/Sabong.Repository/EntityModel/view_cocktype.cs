//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sabong.Repository.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class view_cocktype
    {
        public int cock_id { get; set; }
        public int fightagainst_cock_id { get; set; }
        public int breeder_id { get; set; }
        public int fightagainst_breeder_id { get; set; }
        public string cock_type { get; set; }
        public string against_type { get; set; }
        public int fslno { get; set; }
        public string fdate { get; set; }
        public int match_no { get; set; }
        public int status { get; set; }
        public int block { get; set; }
        public int cancelmatch { get; set; }
        public int arena { get; set; }
        public int winner_cockid { get; set; }
        public int match_order { get; set; }
        public string winner { get; set; }
    }
}
