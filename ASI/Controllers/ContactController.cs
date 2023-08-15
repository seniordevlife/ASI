using ASI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/contacts")]
public class ContactsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ContactsController> _logger; 
    public ContactsController(ApplicationDbContext dbContext, ILogger<ContactsController> logger)
    {
        _dbContext = dbContext;
        _logger = logger; 
    }

    // Create a new contact
    [HttpPost]
    public async Task<IActionResult> CreateContact(Contact contact)
    {
        _dbContext.Contacts.Add(contact);
        await _dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetContactById), new { id = contact.Id }, contact);
    }

    // Get all contacts
    [HttpGet]
    public async Task<IActionResult> GetAllContacts()
    {
        var contacts = await _dbContext.Contacts.ToListAsync();
        return Ok(contacts);
    }

    // Get a contact by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetContactById(long id)
    {

        try
        {

            var contact = await _dbContext.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }
        catch (Exception ex)
        {
            _logger.LogError(this.Request.Path.Value, ex.Message);
            return Ok(new { ResponseMessage = "An error occured while processing your request" });
        }
    

    }

    // Update a contact
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact(long id, Contact updatedContact)
    {
        try
        {
            if (id != updatedContact.Id)
            {
            return BadRequest();
            }

        _dbContext.Entry(updatedContact).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ContactExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }
    catch (Exception ex)
        {
            _logger.LogError(this.Request.Path.Value, ex.Message); 
            return Ok(new { ResponseMessage = "An error occured while processing your request" }); 
        }
    }

    // Delete a contact
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(long id)
    {
        try
        {
            var contact = await _dbContext.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            _dbContext.Contacts.Remove(contact);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(this.Request.Path.Value, ex.Message);
            return Ok(new { ResponseMessage = "An error occured while processing your request" });
        }
    }

    // Search contacts by name and birth date range
    [HttpGet("search")]
    public async Task<IActionResult> SearchContacts(string? name, DateTime? startDate, DateTime? endDate)
    {
        try
        {
            var query = _dbContext.Contacts.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.Name.Contains(name));
            }

            if (startDate != null && endDate != null)
            {
                query = query.Where(c => c.BirthDate >= startDate && c.BirthDate <= endDate);
            }

            var results = await query.ToListAsync();

            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(this.Request.Path.Value, ex.Message); 
            return Ok(new { ResponseMessage = "An error occured while processing your request" }); 
        }
    }

    private bool ContactExists(long id)
    {
        return _dbContext.Contacts.Any(c => c.Id == id);
    }
}
