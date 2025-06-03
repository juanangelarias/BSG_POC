using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BSG.BackEnd.Services;

public interface IDateConverterService
{
    ValueConverter<DateTime, DateTime> DateConverter();
    ValueConverter<DateTime?, DateTime?> NullableDateConverter();
}