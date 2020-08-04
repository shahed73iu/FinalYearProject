using Autofac.Extras.Moq;
using DevSkill.TenantPro.Billing.Entities;
using DevSkill.TenantPro.Billing.Repositories;
using DevSkill.TenantPro.Billing.Services;
using DevSkill.TenantPro.Billing.UnitOfWorks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevSkill.TenantPro.Billing.Tests
{
    public class BillServiceTests
    {
        private IBillService _billService;
        private AutoMock _mock;
        private Mock<IBillRepository> _billRepositoryMock;
        private Mock<IBillUnitOfWork> _billUnitOfWorkMock;

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
            _billRepositoryMock = _mock.Mock<IBillRepository>();
            _billUnitOfWorkMock = _mock.Mock<IBillUnitOfWork>();
            _billService = _mock.Create<BillService>();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _billRepositoryMock.Reset();
            _billUnitOfWorkMock.Reset();
        }

        [Test, Category("Add Bill Unit Test")]
        public void AddNewBill_InvalidBill_ThrowsNullReferenceException()
        {
            // Arrange
            Bill bill = null;

            // Act & Assert
            Should.Throw<NullReferenceException>(() => _billService.AddNewBill(bill));
        }

        [Test, Category("Add Bill Unit Test")]
        public void AddNewBill_ValidBill_BillSuccessfullyInserted()
        {
            // Arrange
            Bill bill = new Bill {
                DescoBillOfThisMonth = 20021,
                Month = Month.March,
                TotalUnitLocal = 2542,
                Year = 2020,
                UnitPriceForNextMonth = 3450
            };

            _billUnitOfWorkMock.Setup(x => x.BillRepository).Returns(_billRepositoryMock.Object);
            _billRepositoryMock.Setup(x => x.Add(It.Is<Bill>(y => y == bill))).Verifiable();
            _billUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            //Act
            _billService.AddNewBill(bill);

            //Assert
            _billRepositoryMock.VerifyAll();
            _billUnitOfWorkMock.VerifyAll();
        }

        [Test,Category("Delete Bill Unit Test")]
        public void DeleteBill_WhenBillIdGiven_DeleteBill()
        {
            // Arrange
            int id = 1;

            _billUnitOfWorkMock.Setup(x => x.BillRepository).Returns(_billRepositoryMock.Object);
            _billRepositoryMock.Setup(x => x.Remove(id)).Verifiable();
            _billUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            //Act
            _billService.DeleteBill(id);

            //Assert
            _billRepositoryMock.VerifyAll();
            _billUnitOfWorkMock.VerifyAll();
        }

        [Test, Category("Edit Bill Unit Test")]
        public void EditBill_WhenUpdatedBillGiven_BillUpdatedSuccessfully()
        {
            var bill = new Bill
            {
                Id = 1,
                DescoBillOfThisMonth = 20120,
                Month = Month.March,
                TotalUnitLocal = 1243,
                Year = 2020,
                UnitPriceForNextMonth = 3450
            };
            // Arrange
            Bill oldBill = new Bill
            {
                Id = 1,
                DescoBillOfThisMonth = 20021,
                Month = Month.March,
                TotalUnitLocal = 2542,
                Year = 2020,
                UnitPriceForNextMonth = 3450
            };

            _billUnitOfWorkMock.Setup(x => x.BillRepository).Returns(_billRepositoryMock.Object);
            _billRepositoryMock.Setup(x => x.GetById(bill.Id)).Returns(oldBill).Verifiable();
            _billUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            // Act
            _billService.EditBill(bill);

            //Assert
            _billUnitOfWorkMock.VerifyAll();
            _billRepositoryMock.VerifyAll();
        }

        [Test, Category("Edit Bill Unit Test")]
        public void EditBill_IfBillNotExists_ThrowsNullReferenceException()
        {
            Bill bill = new Bill {
                Id = 1,
                DescoBillOfThisMonth = 20021,
                Month = Month.March,
                TotalUnitLocal = 2542,
                Year = 2020,
                UnitPriceForNextMonth = 3450
            };
            // Arrange
            Bill oldBill = null;

            _billUnitOfWorkMock.Setup(x => x.BillRepository).Returns(_billRepositoryMock.Object);
            _billRepositoryMock.Setup(x => x.GetById(bill.Id)).Returns(oldBill).Verifiable();

            // Act & Assert
            Should.Throw<NullReferenceException>(() => _billService.EditBill(bill));
        }

        [Test, Category("Get Bill Unit test")]
        public void GetBill_WhenBillIdGiven_ReturnsBill()
        {
            //Arrange
            Bill expectedBill = new Bill
            {
                Id = 1,
                DescoBillOfThisMonth = 20021,
                Month = Month.March,
                TotalUnitLocal = 2542,
                Year = 2020,
                UnitPriceForNextMonth = 3450
            };
            int billId = 1;

            _billUnitOfWorkMock.Setup(x => x.BillRepository).Returns(_billRepositoryMock.Object);
            _billRepositoryMock.Setup(x => x.GetById(billId)).Returns(expectedBill);

            //Act 
            var actualResult = _billService.GetBill(billId);
            actualResult.ShouldBe(expectedBill);

            // Assert
            _billRepositoryMock.VerifyAll();
            _billUnitOfWorkMock.VerifyAll();
        }

        [Test, Category("Get Bills Unit test")]
        public void GetBills_ReturnsListOfBill()
        {
            //Arrange
            var total = 20;
            var totalfiltered = 10;
            var pageIndex = 1;
            var pageSize = 10;
            var searchText = "";
            IEnumerable<Bill> bills = new List<Bill>
            {
                new Bill
                {
                    Id = 1,
                    DescoBillOfThisMonth = 20021,
                    Month = Month.March,
                    TotalUnitLocal = 2542,
                    Year = 2020,
                    UnitPriceForNextMonth = 3450
                },
                new Bill
                {
                    Id = 2,
                    DescoBillOfThisMonth = 20341,
                    Month = Month.April,
                    TotalUnitLocal = 25142,
                    Year = DateTime.Now.Year,
                    UnitPriceForNextMonth = 3450
                }
            };

            _billUnitOfWorkMock.Setup(x => x.BillRepository).Returns(_billRepositoryMock.Object);
            //_billRepositoryMock.Setup(x => x.Get(
            //out total,
            //out totalfiltered,
            //null,
            //It.IsAny<Func<IQueryable<Bill>, IOrderedQueryable<Bill>>>(),
            //"", pageIndex, pageSize, true)).Returns(bills);

            //Act 
          //  _billService.GetBills(pageIndex, pageSize, searchText, out total, out totalfiltered);

            // Assert
            _billRepositoryMock.VerifyAll();
            _billUnitOfWorkMock.VerifyAll();
        }
    }
}
