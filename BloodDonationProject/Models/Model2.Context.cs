﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BloodDonationProject.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BloodDonationDBEntities9 : DbContext
    {
        public BloodDonationDBEntities9()
            : base("name=BloodDonationDBEntities9")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<bannedUser> bannedUsers { get; set; }
        public virtual DbSet<contactU> contactUs { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<report> reports { get; set; }
        public virtual DbSet<userInfo> userInfoes { get; set; }
    }
}
