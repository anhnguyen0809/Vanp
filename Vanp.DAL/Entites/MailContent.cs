
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
    
public partial class MailContent
{

    public int Id { get; set; }

    public Nullable<int> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreatedWhen { get; set; }

    public Nullable<int> ModifiedBy { get; set; }

    public Nullable<System.DateTime> ModifiedWhen { get; set; }

    public Nullable<int> Order { get; set; }

    public Nullable<bool> Enable { get; set; }

    public string MailContent1 { get; set; }

    public string MailContentHTML { get; set; }

    public string MailSubject { get; set; }

    public Nullable<int> MailTypeId { get; set; }



    public virtual MailType MailType { get; set; }

}

}
