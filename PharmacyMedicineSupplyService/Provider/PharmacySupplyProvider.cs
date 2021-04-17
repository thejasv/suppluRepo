using log4net;
using Newtonsoft.Json;
using PharmacyMedicineSupplyService.Models;
using PharmacyMedicineSupplyService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PharmacyMedicineSupplyService.Provider
{
    public class PharmacySupplyProvider : IPharmacySupplyProvider
    {
        private readonly ISupplyRepo supplyRepo;
        private List<PharmacyDTO> pharmacies;
        private readonly IMedicineStockProvider stockProvider;
        private readonly List<PharmacyMedicineSupply> pharmacySupply = new List<PharmacyMedicineSupply>();
        private static readonly ILog _log4net =LogManager.GetLogger(typeof(PharmacySupplyProvider));
        public PharmacySupplyProvider(ISupplyRepo repo, IMedicineStockProvider provider)
        {
            supplyRepo = repo;
            stockProvider = provider;
        }
        public async Task<List<PharmacyMedicineSupply>> GetSupply(List<MedicineDemand> medicines)
        {
            pharmacies = supplyRepo.GetPharmacies();
            _log4net.Info("Pharmacies Data retrieved");
            foreach (var m in medicines)
            {
                if (m.Count > 0)
                {
                    int stockCount = await stockProvider.GetStock(m.MedicineName);
                    if (stockCount != -1)
                    {
                        if (stockCount < m.Count)
                            m.Count = stockCount;
                        int indSupply = (m.Count) / pharmacies.Count;
                        foreach (var i in pharmacies)
                        {
                            pharmacySupply.Add(new PharmacyMedicineSupply { MedicineName = m.MedicineName, PharmacyName = i.pharmacyName, SupplyCount = indSupply });
                        }
                        if (m.Count > indSupply * pharmacies.Count)
                        {
                            pharmacySupply[pharmacySupply.Count - 1].SupplyCount += (m.Count - indSupply * pharmacies.Count);
                        }
                    }
                }

            }
            _log4net.Info("Medicines have been successfully supplied to the pharmacies");
            return pharmacySupply;

        }

    }
}