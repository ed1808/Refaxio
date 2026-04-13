using Backend.Dtos.Customer;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CustomerResponseDto>> GetAllAsync()
    {
        return await _context.Customers
            .Include(c => c.docType)
            .Include(c => c.personType)
            .Where(c => c.active == true)
            .Select(c => ToResponse(c))
            .ToListAsync();
    }

    public async Task<CustomerResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Customers
            .Include(c => c.docType)
            .Include(c => c.personType)
            .FirstOrDefaultAsync(c => c.id == id && c.active == true);
        return entity is null ? null : ToResponse(entity);
    }

    public async Task<CustomerResponseDto> CreateAsync(CustomerCreateDto dto)
    {
        var entity = new Customer
        {
            firstName = dto.FirstName,
            middleName = dto.MiddleName,
            firstSurname = dto.FirstSurname,
            secondSurname = dto.SecondSurname,
            documentIdNumber = dto.DocumentIdNumber,
            docTypeId = dto.DocTypeId,
            personTypeId = dto.PersonTypeId,
            email = dto.Email,
            telephoneNumber = dto.TelephoneNumber
        };
        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(c => c.docType).LoadAsync();
        await _context.Entry(entity).Reference(c => c.personType).LoadAsync();
        return ToResponse(entity);
    }

    public async Task<CustomerResponseDto?> UpdateAsync(Guid id, CustomerUpdateDto dto)
    {
        var entity = await _context.Customers
            .Include(c => c.docType)
            .Include(c => c.personType)
            .FirstOrDefaultAsync(c => c.id == id && c.active == true);
        if (entity is null) return null;

        entity.firstName = dto.FirstName;
        entity.middleName = dto.MiddleName;
        entity.firstSurname = dto.FirstSurname;
        entity.secondSurname = dto.SecondSurname;
        entity.documentIdNumber = dto.DocumentIdNumber;
        entity.docTypeId = dto.DocTypeId;
        entity.personTypeId = dto.PersonTypeId;
        entity.email = dto.Email;
        entity.telephoneNumber = dto.TelephoneNumber;
        if (dto.Active.HasValue) entity.active = dto.Active.Value;

        await _context.SaveChangesAsync();
        await _context.Entry(entity).Reference(c => c.docType).LoadAsync();
        await _context.Entry(entity).Reference(c => c.personType).LoadAsync();
        return ToResponse(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Customers
            .FirstOrDefaultAsync(c => c.id == id && c.active == true);
        if (entity is null) return false;

        entity.active = false;
        await _context.SaveChangesAsync();
        return true;
    }

    private static CustomerResponseDto ToResponse(Customer c) => new()
    {
        Id = c.id,
        Active = c.active,
        CreatedAt = c.createdAt,
        FirstName = c.firstName,
        MiddleName = c.middleName,
        FirstSurname = c.firstSurname,
        SecondSurname = c.secondSurname,
        DocumentIdNumber = c.documentIdNumber,
        DocTypeId = c.docTypeId,
        DocumentIdName = c.docType.documentIdName,
        PersonTypeId = c.personTypeId,
        PersonTypeName = c.personType.personTypeName,
        Email = c.email,
        TelephoneNumber = c.telephoneNumber
    };
}
