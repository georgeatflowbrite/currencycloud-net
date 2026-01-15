using NUnit.Framework;
using CurrencyCloud.Entity;
using CurrencyCloud.Tests.Mock.Data;
using CurrencyCloud.Entity.Pagination;
using CurrencyCloud.Tests.Mock.Http;
using CurrencyCloud.Environment;
using CurrencyCloud.Entity.List;
using System.Threading.Tasks;

namespace CurrencyCloud.Tests
{
    [TestFixture]
    class AccountsTest
    {
        Client client = new Client();
        Player player = new Player("/../../Mock/Http/Recordings/Accounts.json");

        [OneTimeSetUpAttribute]
        public void SetUp()
        {
            player.Start(ApiServer.Mock.Url);
            player.Play("SetUp");

            var credentials = Authentication.Credentials;

            client.InitializeAsync(Authentication.ApiServer, credentials.LoginId, credentials.ApiKey).Wait();
        }

        [OneTimeTearDownAttribute]
        public void TearDown()
        {
            player.Play("TearDown");

            client.CloseAsync().Wait();

            player.Close();
        }

        /// <summary>
        /// Successfully creates an account.
        /// </summary>
        [Test]
        public async Task Create()
        {
            player.Play("Create");

            var account1 = Accounts.Account1;

            Account created = await client.CreateAccountAsync(account1);

            Assert.That(created.Status, Is.Not.Null.And.Not.Empty);
            Assert.AreEqual(account1.AccountName, created.AccountName);
            Assert.AreEqual(account1.LegalEntityType, created.LegalEntityType);
            Assert.AreEqual(account1.YourReference, created.YourReference);
            Assert.AreEqual(account1.Street, created.Street);
            Assert.AreEqual(account1.City, created.City);
            Assert.AreEqual(account1.StateOrProvince, created.StateOrProvince);
            Assert.AreEqual(account1.PostalCode, created.PostalCode);
            Assert.AreEqual(account1.Country, created.Country);
            Assert.AreEqual(account1.SpreadTable, created.SpreadTable);
            Assert.AreEqual(account1.IdentificationType, created.IdentificationType);
            Assert.AreEqual(account1.Brand, created.Brand);
            Assert.AreEqual(account1.ApiTrading, created.ApiTrading);
            Assert.AreEqual(account1.OnlineTrading, created.OnlineTrading);
            Assert.AreEqual(account1.PhoneTrading, created.PhoneTrading);
            Assert.AreEqual(account1.TermsAndConditionsAccepted, created.TermsAndConditionsAccepted);
        }

        /// <summary>
        /// Successfully creates an account.
        /// </summary>
        [Test]
        public async Task CreateWithAccountCreateRequest()
        {
            player.Play("CreateWithAccountCreateRequest");

            var createRequest = Accounts.AccountCreateRequest;
            Account created = await client.CreateAccountAsync(createRequest);

            Assert.That(created.Status, Is.Not.Null.And.Not.Empty);

            Assert.AreEqual(createRequest.AccountName, created.AccountName);
            Assert.AreEqual(createRequest.LegalEntityType, created.LegalEntityType);
            Assert.AreEqual(createRequest.LegalEntitySubType, created.LegalEntitySubType);
            Assert.AreEqual(createRequest.YourReference, created.YourReference);
            Assert.AreEqual(createRequest.Street, created.Street);
            Assert.AreEqual(createRequest.City, created.City);
            Assert.AreEqual(createRequest.StateOrProvince, created.StateOrProvince);
            Assert.AreEqual(createRequest.PostalCode, created.PostalCode);
            Assert.AreEqual(createRequest.Country, created.Country);
            Assert.AreEqual(createRequest.SpreadTable, created.SpreadTable);
            Assert.AreEqual(createRequest.IdentificationType, created.IdentificationType);
            Assert.AreEqual(createRequest.IdentificationExpiration, created.IdentificationExpiration);
            Assert.AreEqual(createRequest.IdentificationIssuer, created.IdentificationIssuer);
            Assert.AreEqual(createRequest.Brand, created.Brand);
            Assert.AreEqual(createRequest.ApiTrading, created.ApiTrading);
            Assert.AreEqual(createRequest.OnlineTrading, created.OnlineTrading);
            Assert.AreEqual(createRequest.PhoneTrading, created.PhoneTrading);
            Assert.AreEqual(createRequest.TermsAndConditionsAccepted, created.TermsAndConditionsAccepted);
        }

        /// <summary>
        /// Successfully gets an account.
        /// </summary>
        [Test]
        public async Task Get()
        {
            player.Play("Get");

            var account1 = Accounts.Account1;

            Account created = await client.CreateAccountAsync(account1);
            Account gotten = await client.GetAccountAsync(created.Id);

            Assert.AreEqual(gotten, created);
        }

        /// <summary>
        /// Successfully updates an account.
        /// </summary>
        [Test]
        public async Task Update()
        {
            player.Play("Update");

            var account1 = Accounts.Account1;
            var account2 = Accounts.Account2;

            Account created = await client.CreateAccountAsync(account1);
            account2.Id = created.Id;
            Account updated = await client.UpdateAccountAsync(account2);
            Account gotten = await client.GetAccountAsync(created.Id);

            Assert.AreEqual(gotten, updated);
        }

        /// <summary>
        /// Successfully finds an account with search parameters.
        /// </summary>
        [Test]
        public async Task FindWithParams()
        {
            player.Play("FindWithParams");

            Account current = await client.GetCurrentAccountAsync();
            PaginatedAccounts found = await client.FindAccountsAsync(new AccountFindParameters
            {
                AccountName = current.AccountName,
                Order = "created_at",
                OrderAscDesc = FindParameters.OrderDirection.Desc,
                PerPage = 5
            });

            Assert.Contains(current, found.Accounts);
        }

        /// <summary>
        /// Successfully finds an account without search parameters.
        /// </summary>
        [Test]
        public async Task FindNoParams()
        {
            player.Play("FindNoParams");

            Account current = await client.GetCurrentAccountAsync();
            PaginatedAccounts found = await client.FindAccountsAsync();

            Assert.Contains(current, found.Accounts);
        }

        /// <summary>
        /// Successfully gets current account.
        /// </summary>
        [Test]
        public void GetCurrent()
        {
            player.Play("GetCurrent");

            Assert.DoesNotThrowAsync(async () => {
                Account current = await client.GetCurrentAccountAsync();
            });
        }

        /// <summary>
        /// Successfully gets payment charges settings for given account.
        /// </summary>
        [Test]
        public async Task GetChargesSettings()
        {
            player.Play("GetChargesSettings");

            var settings = Accounts.PaymentCharges;

            PaymentChargesSettingsList charges = await client.GetPaymentChargesSettingsAsync(settings.AccountId);
            Assert.AreEqual(charges.PaymentChargesSettings[0].AccountId, settings.AccountId);
            Assert.AreEqual(charges.PaymentChargesSettings[0].ChargeSettingsId, settings.ChargeSettingsId);
        }

        /// <summary>
        /// Successfully manages given Account's Payment Charge Settings.
        /// </summary>
        [Test]
        public async Task ManageChargesSettings()
        {
            player.Play("ManageChargesSettings");

            var settings = Accounts.PaymentCharges;

            PaymentChargesSettings charges = await client.ManageAccountPaymentChargesSettingsAsync(settings);
            Assert.AreEqual(settings, charges);
        }
        /// <summary>
        /// Successfully gets payment charges settings for given account.
        /// </summary>
        [Test]
        public async Task GetComplianceSettings()
        {
            player.Play("GetComplianceSettings");

            var expected = Accounts.ComplianceSettings;

            AccountComplianceSettings actual = await client.GetComplianceSettingsAsync(expected.AccountId);

            Assert.AreEqual(actual.AccountId, expected.AccountId);
            Assert.AreEqual(actual.IndustryType, expected.IndustryType);
            Assert.AreEqual(actual.CountryOfIncorporation, expected.CountryOfIncorporation);
            Assert.AreEqual(actual.DateOfIncorporation, expected.DateOfIncorporation);
            Assert.AreEqual(actual.BusinessWebsiteUrl, expected.BusinessWebsiteUrl);
            Assert.AreEqual(actual.ExpectedTransactionCountries, expected.ExpectedTransactionCountries);
            Assert.AreEqual(actual.ExpectedTransactionCurrencies, expected.ExpectedTransactionCurrencies);
            Assert.AreEqual(actual.ExpectedMonthlyActivityVolume, expected.ExpectedMonthlyActivityVolume);
            Assert.AreEqual(actual.ExpectedMonthlyActivityValue, expected.ExpectedMonthlyActivityValue);
            Assert.AreEqual(actual.TaxIdentification, expected.TaxIdentification);
            Assert.AreEqual(actual.NationalIdentification, expected.NationalIdentification);
            Assert.AreEqual(actual.CountryOfCitizenship, expected.CountryOfCitizenship);
            Assert.AreEqual(actual.TradingAddressStreet, expected.TradingAddressStreet);
            Assert.AreEqual(actual.TradingAddressCity, expected.TradingAddressCity);
            Assert.AreEqual(actual.TradingAddressState, expected.TradingAddressState);
            Assert.AreEqual(actual.TradingAddressPostalcode, expected.TradingAddressPostalcode);
            Assert.AreEqual(actual.TradingAddressCountry, expected.TradingAddressCountry);
            Assert.AreEqual(actual.CustomerRisk, expected.CustomerRisk);
        }

        /// <summary>
        /// Successfully manages given Account's Compliance Settings.
        /// </summary>
        [Test]
        public async Task ManageComplianceSettings()
        {
            player.Play("ManageComplianceSettings");

            var expected = Accounts.ComplianceSettings;

            AccountComplianceSettings actual = await client.ManageComplianceSettingsAsync(expected);

            Assert.AreEqual(actual.AccountId, expected.AccountId);
            Assert.AreEqual(actual.IndustryType, expected.IndustryType);
            Assert.AreEqual(actual.CountryOfIncorporation, expected.CountryOfIncorporation);
            Assert.AreEqual(actual.DateOfIncorporation, expected.DateOfIncorporation);
            Assert.AreEqual(actual.BusinessWebsiteUrl, expected.BusinessWebsiteUrl);
            Assert.AreEqual(actual.ExpectedTransactionCountries, expected.ExpectedTransactionCountries);
            Assert.AreEqual(actual.ExpectedTransactionCurrencies, expected.ExpectedTransactionCurrencies);
            Assert.AreEqual(actual.ExpectedMonthlyActivityVolume, expected.ExpectedMonthlyActivityVolume);
            Assert.AreEqual(actual.ExpectedMonthlyActivityValue, expected.ExpectedMonthlyActivityValue);
            Assert.AreEqual(actual.TaxIdentification, expected.TaxIdentification);
            Assert.AreEqual(actual.NationalIdentification, expected.NationalIdentification);
            Assert.AreEqual(actual.CountryOfCitizenship, expected.CountryOfCitizenship);
            Assert.AreEqual(actual.TradingAddressStreet, expected.TradingAddressStreet);
            Assert.AreEqual(actual.TradingAddressCity, expected.TradingAddressCity);
            Assert.AreEqual(actual.TradingAddressState, expected.TradingAddressState);
            Assert.AreEqual(actual.TradingAddressPostalcode, expected.TradingAddressPostalcode);
            Assert.AreEqual(actual.TradingAddressCountry, expected.TradingAddressCountry);
            Assert.AreEqual(actual.CustomerRisk, expected.CustomerRisk);
        }
    }
}
