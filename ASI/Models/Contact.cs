using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASI.Models;

public class Contact
{
    public long Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; } = null!;

    public ICollection<Email> Emails { get; set; } = new List<Email>();
}

public class Email
{
    public long Id { get; set; }

    public bool IsPrimary { get; set; }

    [Required]
    [EmailAddress]
    public string Address { get; set; } = default!;
}
