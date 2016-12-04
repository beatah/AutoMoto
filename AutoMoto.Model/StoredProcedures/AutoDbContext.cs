using AutoMoto.Model.Models;
using System.Collections.Generic;

namespace AutoMoto.Model.StoredProcedures
{
    public partial class AutoDbContext : IAutoStoredProcedures
    {
        public IEnumerable<DayUser> CustomerOrderHistory()
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IAutoStoredProcedures
    {
        IEnumerable<DayUser> CustomerOrderHistory();
    }
}
