using AutoMapper;
using BSG.Common.DTO;
using BSG.Common.Model;
using BSG.Common.Sorts;
using BSG.Database;
using BSG.Entities;
using BSG.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Repository;

public interface IUserRepository: IRepositoryExtended<User, UserDto>
{
    Task<UserDto?> GetByUsername(string username);
    Task<UserDto?> GetByUsernameAndEmailToken(string username, string emailToken);
    Task<bool> VerifyEmailExistence(long? userId, string email);
    Task<bool> SetEmailToken(long userId, string emailToken, DateTime? expires);
    Task<bool> ValidateEmailToken(long userId, string emailToken);
}

public class UserRepository(IMapper mapper, BsgDbContext db)
    : RepositoryBase<User, UserDto>(mapper, db), IUserRepository
{
    private readonly IMapper _mapper = mapper;
    private readonly BsgDbContext _db = db;
    
    public async Task<PagedResponse<UserDto>> GetPageAsync( QueryParams parameters )
    {
        var qry = GetQuery();

        var sort = parameters.Sort ?? UserSort.Name;

        var filter = string.IsNullOrEmpty( parameters.Filter )
            ? ""
            : parameters.Filter.ToLower();

        if ( filter != string.Empty )
            qry = qry
                .Where( r =>
                    r.Username!.ToLower().Contains( filter ) ||
                    r.Email!.ToLower().Contains( filter ) );

        qry = sort switch
        {
            UserSort.Name => parameters.Descending
                ? qry.OrderByDescending( o => o.Username )
                : qry.OrderBy( o => o.Username ),
            UserSort.Email => parameters.Descending
                ? qry.OrderByDescending( o => o.Email )
                : qry.OrderBy( o => o.Email ),
            _ => parameters.Descending
                ? qry.OrderByDescending( o => o.Username )
                : qry.OrderBy( o => o.Username )
        };

        return await GetAsync( parameters, qry );
    }

    public async Task<UserDto?> GetByUsername( string username )
    {
        var user = await GetQuery()
            .FirstOrDefaultAsync( r => r.Email!.ToLower() == username.ToLower() );

        return user == null
            ? null
            : _mapper.Map<UserDto>( user );
    }

    public async Task<UserDto?> GetByUsernameAndEmailToken( string username, string emailToken )
    {
        var user = await GetQuery()
            .FirstOrDefaultAsync( r =>
                r.Username == username &&
                r.EmailToken == emailToken );

        if ( user != null && !(user.EmailTokenExpiration < DateTime.UtcNow) )
            return _mapper.Map<UserDto>( user );

        if ( user != null )
            await SetEmailToken( user.Id, "", null );

        return null;
    }

    public async Task<bool> VerifyEmailExistence( long? userId, string email )
    {
        var exist = userId == null
            ? await GetQuery()
                .AnyAsync( r => r.Email == email )
            : await GetQuery()
                .AnyAsync( r =>
                    r.Id != userId &&
                    r.Email == email );

        return exist;
    }

    public async Task<bool> SetEmailToken( long userId, string emailToken, DateTime? expires )
    {
        await using var t = await _db.Database.BeginTransactionAsync();

        try
        {
            var user = await _db.Users.FirstOrDefaultAsync( r => r.Id == userId );
            if ( user == null )
                return false;

            user.EmailToken = emailToken;
            user.EmailTokenExpiration = expires;

            _db.Update( user );
            await _db.SaveChangesAsync();
            await t.CommitAsync();

            return true;
        }
        catch ( Exception )
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> ValidateEmailToken( long userId, string emailToken )
    {
        var valid = await GetQuery()
            .AnyAsync( r =>
                r.Id == userId &&
                r.EmailToken == emailToken &&
                r.EmailTokenExpiration > DateTime.UtcNow );

        return valid;
    }
}