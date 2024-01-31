using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarWash.Entity.Entities;

namespace CarWash.Repository.EntityConfigurations
{
    public class EmployeeWashProcessConfig : IEntityTypeConfiguration<EmployeeWashProcess>
    {
        public void Configure(EntityTypeBuilder<EmployeeWashProcess> builder)
        {
            // Anahtarlar ve ilişki tanımlamaları
            builder.HasKey(e => new { e.EmployeeId, e.WashProcessId });

            // EmployeeWashProcess tablosu, Employee ve WashProcess tabloları arasında bir bağlantı tablosudur.
            // Employee tablosu ile ilişki
            builder.HasOne(ewp => ewp.Employee)
                .WithMany(e => e.WashProcesses)
                .HasForeignKey(ewp => ewp.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction); 

            // WashProcess tablosu ile ilişki
            builder.HasOne(ewp => ewp.WashProcess)
                .WithMany(wp => wp.Employees)
                .HasForeignKey(ewp => ewp.WashProcessId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
