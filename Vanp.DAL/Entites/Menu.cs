//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vanp.DAL.Entites
{
    using System;
    using System.Collections.Generic;
    
    public partial class Menu
    {
        public int Id { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedWhen { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedWhen { get; set; }
        public Nullable<int> Order { get; set; }
        public Nullable<bool> Enable { get; set; }
        public string MenuName { get; set; }
        public string MenuTitle { get; set; }
        public string MenuAlias { get; set; }
        public string MenuAliasPath { get; set; }
        public Nullable<int> MenuLevel { get; set; }
        public string MenuUrl { get; set; }
        public Nullable<int> MenuParentId { get; set; }
        public Nullable<bool> Show { get; set; }
    }
}
