using NUnit.Framework;
using System.Threading.Tasks;
using TallyConnector.Models;

namespace Tests
{
    public class TallyTests
    {
        TallyConnector.Tally TTally = new();
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task TallyPortCheck()
        {
            
            await TTally.Check();
            Assert.IsNotNull(TTally.Status);
            Assert.AreEqual("Running", TTally.Status);
        }
        [Test]
        public async Task TallyCheckCopanies()
        {
            await TTally.GetCompaniesList();
            Assert.IsNotNull(TTally.CompaniesInfo);
            
        }

        [Test]
        public async Task TallyGetData()
        {

            await TTally.FetchAllTallyData();
            Assert.IsNotNull(TTally.Ledgers);
        }
        [Test]
        public async Task GetGroup()
        {
            await TTally.GetObjFromTally<LedgerEnvelope>("Cash", "Ledger");
            Assert.IsNotNull(TTally.Ledgers);
        }

    }
}