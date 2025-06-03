using AutoMapper;
using BSG.BackEnd.Common.Model;
using BSG.BackEnd.Services.Encryption;
using BSG.Common.DTO;
using BSG.Database;
using BSG.Entities;
using BSG.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Repository;

public interface IUserPasswordRepository: IRepositoryBase<UserPassword, UserPasswordDto>
{
    Task<(byte[]? Key, byte[]? Password)?> GetUserKeyPassword( long userId );
    Task<bool> VerifyIfPasswordWasUsed( long userId, byte[] password );
    Task<bool> ValidatePassword(long userId, string password);
}

public class UserPasswordRepository(IMapper mapper, BsgDbContext db, IEncryptionService encryptionService, PasswordSettings passwordSettings)
    : RepositoryBase<UserPassword, UserPasswordDto>(mapper, db), IUserPasswordRepository
{
    public async Task<(byte[]? Key, byte[]? Password)?> GetUserKeyPassword( long userId )
    {
        var userPass = await GetQuery()
            .OrderBy( o => o.UserId )
            .ThenByDescending( t => t.StartDate )
            .FirstOrDefaultAsync( r => r.UserId == userId );

        if ( userPass == null || userPass.EndDate < DateTime.Now )
            return null;

        return (userPass.Key, userPass.Password);
    }

    public async Task<bool> VerifyIfPasswordWasUsed(long userId, byte[] password)
    {
        // Returns true if the password has been previously used

        var qry = await GetQuery()
            .OrderBy( o => o.UserId )
            .ThenByDescending( t => t.StartDate )
            .Where( r => r.UserId == userId )
            .Take( passwordSettings.PasswordHistory )
            .ToListAsync();

        return qry.Any( a => a.Password == password );
    }

    public async Task<bool> ValidatePassword(long userId, string password)
    {
        var lastPassword = await GetQuery()
            .OrderBy(o => o.UserId)
            .ThenByDescending(t => t.StartDate)
            .FirstOrDefaultAsync(f => f.UserId == userId);

        if (lastPassword == null)
            return false;
        
        var isValid = lastPassword is {Password: not null, Key: not null} && 
                      encryptionService.VerifyHashed(password, lastPassword.Password, lastPassword.Key);

        return isValid;
    }
}