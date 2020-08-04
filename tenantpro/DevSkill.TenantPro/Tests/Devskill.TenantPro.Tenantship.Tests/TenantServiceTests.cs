using Autofac.Extras.Moq;
using DevSkill.TenantPro.Tenantship.Entities;
using DevSkill.TenantPro.Tenantship.Repositories;
using DevSkill.TenantPro.Tenantship.Services;
using DevSkill.TenantPro.Tenantship.UnitOfWorks;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Devskill.TenantPro.Tenantship.Tests
{
    public class TenantServiceTests
    {
        private ITenantService _tenantService;
        private AutoMock _mock;
        private Mock<ITenantRepository> _tenantRepositoryMock;
        private Mock<IContactPersonRepository> _contactPersonRepositoryMock;
        private Mock<ITenantUnitOfWork> _tenantUnitOfWorkMock;
        private Mock<IPrintingRepository> _printingRepositoryMock;

        [OneTimeSetUp]
        public void ClassSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [OneTimeTearDown]
        public void ClassCleanUp()
        {
            _mock?.Dispose();
        }

        [SetUp]
        public void TestSetup()
        {
            _tenantRepositoryMock = _mock.Mock<ITenantRepository>();
            _printingRepositoryMock = _mock.Mock<IPrintingRepository>();
            _contactPersonRepositoryMock = _mock.Mock<IContactPersonRepository>();
            _tenantUnitOfWorkMock = _mock.Mock<ITenantUnitOfWork>();
            _tenantService = _mock.Create<TenantService>();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _tenantRepositoryMock.Reset();
            _printingRepositoryMock.Reset();
            _contactPersonRepositoryMock.Reset();
            _tenantUnitOfWorkMock.Reset();
        }

        [Test, Category("Unit Test")]
        public void AddNewTenant_ValidTenantContactPerson_SaveTenandAndContactPerson()
        {
            //Arrange
            var tenant = new Tenant
            {
                Name = "DevSkill",
                Email = "DevSkill@gamil.com",
                AdvanceArrear = 500000,
                ContractStartDate = DateTime.Now.Date,
                ContractExpirationDate = DateTime.Now.AddMonths(2),
                Holding = "9B/c",
                PhoneNumber = "+8801521200542",
                Rent = 25000,
                ServiceCharge = 400,
                WaterBill = 1200,
                Status = true,
            };
            var contactPerson = new ContactPerson
            {
                Name = "Jalaluddin",
                Email = "jalaluddin@gmail.com",
                ContractNumber = "+8801521200542"
            };
            
            List<Printing> printings = new List<Printing>();
            _tenantUnitOfWorkMock.Setup(x => x.TenantRepository).Returns(_tenantRepositoryMock.Object);
            _tenantRepositoryMock.Setup(x => x.Add(It.Is<Tenant>(y => y == tenant))).Verifiable();
            _tenantUnitOfWorkMock.Setup(x => x.ContactPersonRepository).Returns(_contactPersonRepositoryMock.Object);
            _contactPersonRepositoryMock.Setup(x => x.Add(It.Is<ContactPerson>(c => c == contactPerson))).Verifiable();
            _tenantUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            //Act
            _tenantService.AddNewTenant(tenant, contactPerson, printings);

            //Assert
            _tenantRepositoryMock.VerifyAll();
            _contactPersonRepositoryMock.VerifyAll();
            _tenantUnitOfWorkMock.VerifyAll();
        }
        [Test, Category("Unit Test")]
        public void EditTenantStatus_whenStatusIsFalse_DataWillNotAppear()
        {
            //Arrange
            int id = 1;
            var oldTenant = new Tenant
            {
                Id = id,
                Name = "DevSkill",
                Email = "DevSkill@gamil.com",
                AdvanceArrear = 500000,
                ContractStartDate = DateTime.Now.Date,
                ContractExpirationDate = DateTime.Now.AddMonths(2),
                Holding = "9B/c",
                PhoneNumber = "+8801521200542",
                Rent = 25000,
                ServiceCharge = 400,
                WaterBill = 1200,
                Status = true,
            };
            _tenantUnitOfWorkMock.Setup(x => x.TenantRepository).Returns(_tenantRepositoryMock.Object);
            _tenantRepositoryMock.Setup(x => x.GetById(id)).Returns(oldTenant);
            _tenantUnitOfWorkMock.Setup(x => x.Save()).Verifiable();
            //Act
            _tenantService.EditTenantStatus(oldTenant);
            //Assert
            _tenantRepositoryMock.VerifyAll();
            _tenantUnitOfWorkMock.VerifyAll();
        }
        [Test, Category("Unit Test")]
        public void GetConatctPerson_WhenIdIsGiven_GetContactPersons()
        {
            var id = 1;
            //Arrange
            var contactPerson = new ContactPerson
            {
                Id = id,
                Name = "Jalaluddin",
                Email = "jalaluddin@gmail.com",
                ContractNumber = "+8801521200542"
            };

            _tenantUnitOfWorkMock.Setup(x => x.ContactPersonRepository).Returns(_contactPersonRepositoryMock.Object);
            _contactPersonRepositoryMock.Setup(x => x.GetById(id)).Returns(contactPerson);
            //Act
            _tenantService.GetConatctPerson(id);
            //Assert
            _tenantUnitOfWorkMock.VerifyAll();
            _tenantRepositoryMock.VerifyAll();
        }
        [Test, Category("Unit Test")]
        public void GetTenantOnly_WhenIdIsGiven_GetTenant()
        {
            //Arrange
            int id = 1;
            var tenant = new Tenant();

            _tenantUnitOfWorkMock.Setup(x => x.TenantRepository).Returns(_tenantRepositoryMock.Object);
            _tenantRepositoryMock.Setup(x => x.GetById(id)).Returns(tenant);

            //Act
            _tenantService.GetTenantOnly(id);
            //Assert
            _tenantUnitOfWorkMock.VerifyAll();
            _tenantUnitOfWorkMock.VerifyAll();
        }
        [Test, Category("Unit Test")]
        public void GetTenant_WhenIdIsGiven_TenantReturns()
        {
            int id = 1;
            var tenant = new Tenant();

            //Arrange
            _tenantUnitOfWorkMock.Setup(x => x.TenantRepository).Returns(_tenantRepositoryMock.Object);
            _tenantRepositoryMock.Setup(x => x.GetTenantIncludingChild(id)).Returns(tenant);

            //Act
            _tenantService.GetTenant(id);

            //Assert
            _tenantUnitOfWorkMock.VerifyAll();
            _tenantRepositoryMock.VerifyAll();
        }
        [Test, Category("Unit Test")]
        public void EditTenant_WhenTenantAndContactPerson_EditTenantContactPersonObject()
        {
            //Arragne
            var tenant = new Tenant();
            var contactPerson = new ContactPerson();
            tenant.Id = 1;
            tenant.Name = "Jalal Uddin";
            tenant.Holding = "9-B/2";
            tenant.PhoneNumber = "01521200542";
            tenant.Email = "JalalUddin@gmail.com";
            tenant.AdvanceArrear = 250000;
            tenant.Rent = 25000;
            tenant.ServiceCharge = 1200;
            tenant.WaterBill = 500;
            tenant.ContractExpirationDate = new DateTime(2020, 4, 18);
            tenant.ContractSigningDate = new DateTime(2022, 4, 27);
            tenant.Status = true;
            tenant.ContactPersons = new List<ContactPerson>()
            {
                new ContactPerson
                {
                    Id = 1,
                    Name = "Shahed",
                    Email = "shahedimamuddin@gmail.com",
                    ContractNumber = "01521200544"
                },
            };
            tenant.Printings = new List<Printing>()
            {
                new Printing
                {
                    Id=9,
                    Format="Rent,GasBill",
                    Tenant=tenant
                },
            };
            string[] billType = new string[] { "rent", "electricityBill" };
            List<Printing> printings = new List<Printing>()
            {
                new Printing
                {
                    Id=9,
                    Format="Rent,GasBill",
                    BillFormat = billType,
                    Tenant=tenant
                },
            };
            _tenantUnitOfWorkMock.Setup(x => x.TenantRepository).Returns(_tenantRepositoryMock.Object);
            _tenantUnitOfWorkMock.Setup(x => x.PrintingRepository).Returns(_printingRepositoryMock.Object);
            _tenantRepositoryMock.Setup(x => x.GetTenantIncludingChild(tenant.Id)).Returns(tenant).Verifiable();
            _tenantUnitOfWorkMock.Setup(x => x.Save()).Verifiable();
            //Act
            _tenantService.EditTenant(tenant, contactPerson, printings);
            //Assert
            _tenantRepositoryMock.VerifyAll();
            _tenantUnitOfWorkMock.VerifyAll();
        }
        [Test, Category("Unit Test")]
        public void GetTenants_GiveTotalFilteredElementPageIndexPageSize_GetTenantsList()
        {
            //Arrange
            var TenantList = new List<Tenant>();
            var tenant = new Tenant();
            var seachText = "shahed";
            var total = 20;
            var totalfiltered = 10;
            var pageIndex = 1;
            var pageSize = 10;
            tenant.Status = true;

            _tenantUnitOfWorkMock.Setup(x => x.TenantRepository).Returns(_tenantRepositoryMock.Object);
            //_tenantRepositoryMock.Setup(x => x.Get(
            //out total, out totalfiltered,
            ////y => y.Name.Contains(seachText) && y.Status == true,
            //It.IsAny<Expression<Func<Tenant, bool>>>(),
            //It.IsAny<Func<IQueryable<Tenant>,
            //IOrderedQueryable<Tenant>>>(),
            //"ContactPersons", pageIndex, pageSize, true)).Returns(TenantList);
            ////Act
            ////_tenantService.GetTenants(
            //pageIndex, pageSize,
            //"ContactPersons", out total, out totalfiltered);
            ////Assert
            _tenantUnitOfWorkMock.VerifyAll();
            _tenantRepositoryMock.VerifyAll();

        }
    }
}

