using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyMedicineSupplyService.Provider
{
    public interface IMedicineStockProvider
    {
        public Task<int> GetStock(string medicineName);
    }
}