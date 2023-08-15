using System;
using System.Collections.Generic;
using System.Linq;
using ASI.Models;
using Microsoft.EntityFrameworkCore;

public static class SeedData
{
    public static void Initialize(ApplicationDbContext context)
    {
        if (!context.Contacts.Any())
        {
            var random = new Random();
            var sampleNames = new List<string> { "Alice", "Bob", "Charlie", "David", "Emma" };
            var sampleEmails = new List<string> { "alice@example.com", "bob@example.com", "charlie@example.com" };

            foreach (var name in sampleNames)
            {
                var contact = new Contact
                {
                    Name = name,
                    BirthDate = DateTime.Now.AddYears(-random.Next(20, 50)),
                    Emails = new List<Email>()
                };

                foreach (var email in sampleEmails)
                {
                    var isPrimary = random.NextDouble() < 0.3; 
                    contact.Emails.Add(new Email
                    {
                        Address = email,
                        IsPrimary = isPrimary
                    });
                }

                context.Contacts.Add(contact);
            }

            context.SaveChanges();
        }
    }
}
