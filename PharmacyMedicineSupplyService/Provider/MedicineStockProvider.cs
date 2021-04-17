using log4net;
using Newtonsoft.Json;
using PharmacyMedicineSupplyService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PharmacyMedicineSupplyService.Provider
{
    public class MedicineStockProvider : IMedicineStockProvider
    {
        private static readonly ILog _log4net = LogManager.GetLogger(typeof(MedicineStockProvider));
        public async Task<int> GetStock(string medicineName)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:59930")
            };
            var response = await client.GetAsync("MedicineStockInformation");
            if (!response.IsSuccessStatusCode)
            {
                _log4net.Info("No data was found in stock");
                return -1;
            }
            _log4net.Info("Retrieved data from medicine stock service");
            string stringStock = await response.Content.ReadAsStringAsync();
            var medicines = JsonConvert.DeserializeObject<List<MedicineStock>>(stringStock);
            var i = medicines.Where(x => x.Name == medicineName).FirstOrDefault();
            return i.NumberOfTabletsInStock;
        }
    }
}