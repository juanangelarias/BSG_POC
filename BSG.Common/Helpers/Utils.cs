using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace BSG.Common.Helpers;

public static class Utils
{
    public static IEnumerable<Claim> ParseClaimsFromJwt( string jwt )
    {
        var payload = jwt.Split( '.' )[1];
        var jsonBytes = ParseBase64WithoutPadding( payload );
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>( jsonBytes );

        return keyValuePairs!
            .Select( k => new Claim( k.Key, k.Value.ToString()! ) );
    }

    private static byte[] ParseBase64WithoutPadding( string base64 )
    {
        switch ( base64.Length %4 )
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }

        return Convert.FromBase64String( base64 );
    }
    
    public static (List<Claim>?, DateTime?) DecodeToken( string token )
    {
        if (string.IsNullOrEmpty(token))
            return ([], null);

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken( token );

            if(jsonToken is not { } tokenS)
                return (null,null);

            var claims = tokenS.Claims.ToList();
            var claimExp = claims.FirstOrDefault( f => f.Type == "exp" );
            var intExp = Convert.ToInt64(claimExp?.Value ?? "");
            var dtExp = DateTimeOffset.FromUnixTimeSeconds( intExp );
            
            return (claims, dtExp.DateTime);
        }
        catch ( Exception e )
        {
            Console.WriteLine( e );
            throw;
        }
    }
}