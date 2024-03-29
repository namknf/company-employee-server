﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Company
    {
        [Column("CompanyId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Company name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")] 
        public string Name { get; set; }

        [ForeignKey(nameof(Address))]
        public short AddressId { get; set; }

        public Address Address { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
