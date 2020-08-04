using Autofac.Extras.Moq;
using DevSkill.TenantPro.Billing.Entities;
using DevSkill.TenantPro.Billing.Repositories;
using DevSkill.TenantPro.Billing.Services;
using DevSkill.TenantPro.Billing.UnitOfWorks;
using Moq;
using NUnit.Framework;
using Autofac;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevSkill.TenantPro.Billing.Tests
{
    public class ReadingServiceTests
    {
        private IReadingService _readingService;
        private AutoMock _mock;
        private Mock<IBillRepository> _billRepositoryMock;
        private Mock<IReadingRepository> _readingRepositoryMock;
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
            _readingRepositoryMock = _mock.Mock<IReadingRepository>();
            _readingService = _mock.Create<ReadingService>();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _billRepositoryMock.Reset();
            _billUnitOfWorkMock.Reset();
            _readingRepositoryMock.Reset();
        }

        [Test, Category("Unit Test")]
        public void AddNewReading_ValidReading_SavesReading()
        {
            // Arrange
           
            var expectedUnitPrice = 300;
            var previousReading = 500;
            var presentReading = 600;
            var bill = 30000;
           
            var reading = new Reading();
           // reading.Month = currentMonth;
            reading.ReadingTakenDate = new DateTime(2020, 3, 7);
            reading.PresentReading = presentReading;
            reading.PreviousReading = previousReading;
            var previousMonth = reading.ReadingTakenDate.Month - 2;

            _billUnitOfWorkMock.Setup(x => x.BillRepository).Returns(_billRepositoryMock.Object);
            _billUnitOfWorkMock.Setup(x => x.ReadingRepository).Returns(_readingRepositoryMock.Object);
           // _billRepositoryMock.Setup(x => x.GetUnitPrice((Month)previousMonth)).Returns(expectedUnitPrice).Verifiable();
            
            _readingRepositoryMock.Setup(x => x.Add(It.Is<Reading>(y => y == reading && y.PerUnitPrice == expectedUnitPrice
                && y.TotalBillofThisMonth == bill))).Verifiable();

            _billUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            // Act
          //  _readingService.AddNewReading(reading);

            // Assert
            _billRepositoryMock.VerifyAll();
            _readingRepositoryMock.VerifyAll();
            _billUnitOfWorkMock.VerifyAll();
        }

        [Test, Category("Unit Test")]
        public void AddNewReading_InvalidReading_ThrowsException()
        {
            // Arrange
            //Reading reading = null;

            // Act & Assert
            //Should.Throw<ArgumentNullException>(() =>
            //    _readingService.AddNewReading(reading)
            //);
        }

        [Test, Category("Unit Test")]
        public void DeleteReading_GetReadingById_DeleteReading()
        {
            // Arrange
            var id = 1;
            var reading = new Reading();
            _billUnitOfWorkMock.Setup(x => x.ReadingRepository).Returns(_readingRepositoryMock.Object);
            _readingRepositoryMock.Setup(x => x.Remove(id)).Verifiable();
            _billUnitOfWorkMock.Setup(x => x.Save()).Verifiable();
            // Act 
            _readingService.DeleteReading(id);
            //Assert
            _billRepositoryMock.VerifyAll();
            _readingRepositoryMock.VerifyAll();
            _billUnitOfWorkMock.VerifyAll();
        }
        [Test, Category("Unit Test")]
        public void GetPreviousMonthReading_GiveTenantIdMonth_GetPreviousReading()
        {
            //Arrange
            var tenantid = 1;
            var previousMonthReading = 40;
            var time= new DateTime(2020, 3, 7);
            var previousMonth = time.Month - 2;
            _billUnitOfWorkMock.Setup(x => x.ReadingRepository).Returns(_readingRepositoryMock.Object);
         //   _readingRepositoryMock.Setup(x => x.GetPreviousMonthReading(tenantid, previousMonth)).Returns(previousMonthReading);

            // Act 
            _readingService.GetPreviousMonthReading(tenantid, time);
            //Assert
            _billRepositoryMock.VerifyAll();
            _readingRepositoryMock.VerifyAll();
            _billUnitOfWorkMock.VerifyAll();
        }
        [Test, Category("Unit Test")]
        public void GetReading_GiveId_GetReading()
        {
            // Arrange
            var id = 1;
            var reading = new Reading();
            _billUnitOfWorkMock.Setup(x => x.ReadingRepository).Returns(_readingRepositoryMock.Object);
            _readingRepositoryMock.Setup(x => x.GetById(id)).Returns(reading);

            // Act 
            _readingService.GetReading(id);
            //Assert
            _billRepositoryMock.VerifyAll();
            _readingRepositoryMock.VerifyAll();
            
        }
        //[Test, Category("Unit Test")]
        //public void GetReadingOfTenant_GiveTenantIdMonthYear_GetReadingByTenantId()
        //{
        //    // Arrange
        //    var tenantid = 1;
        //    Month month = Month.April;
        //    int year = 2020;
        //    var reading = new Reading();
        //    _billUnitOfWorkMock.Setup(x => x.ReadingRepository).Returns(_readingRepositoryMock.Object);
        //   // _readingRepositoryMock.Setup(x => x.GetReadingByTenantId(tenantid,month,year)).Returns(reading);
            
        //    // Act 
        //   // _readingService.GetReadingOfTenant(tenantid,month,year);
        //    //Assert
        //    _billRepositoryMock.VerifyAll();
        //    _readingRepositoryMock.VerifyAll();

        //}
        [Test, Category("Unit Test")]
        public void GetReadings_GiveTotalFilteredElementPageIndexPageSize_GetReadingList()
        {
            // Arrange
            var total = 20;
            var totalfiltered = 10;
            var pageIndex = 1;
            var pageSize = 10;
            var tenantId = 1;
             var readinglist = new List<Reading>();
            _billUnitOfWorkMock.Setup(x => x.ReadingRepository).Returns(_readingRepositoryMock.Object);
            //_readingRepositoryMock.Setup(x => x.Get(out total,out totalfiltered,null,
            //    It.IsAny<Func<IQueryable<Reading>, IOrderedQueryable<Reading>>>()
            //    , "",pageIndex,pageSize,true))
            //    .Returns(readinglist);

            // Act 
           // _readingService.GetReadings(tenantId, pageIndex, pageSize, "", out total, out totalfiltered);
            //Assert
            _billRepositoryMock.VerifyAll();
            _readingRepositoryMock.VerifyAll();

        }

        [Test, Category("Unit Test")]
        public void EditReading_ReadingObject_EditReadingObject()
        {
            // Arrange
            var previousReading = new Reading();
            var reading = new Reading();
            reading.Id = 1;
          //  reading.Month = Month.April;
            reading.PresentReading = 1000;
            reading.PreviousReading = 500;
            reading.DayOffset = 2;
            reading.ReadingTakenDate = DateTime.Today;
            reading.TenantId = 5;
            var previousMonth = reading.ReadingTakenDate.Month - 2;
            var expectedUnitPrice = 300;

            _billUnitOfWorkMock.Setup(x => x.BillRepository).Returns(_billRepositoryMock.Object);
            _billUnitOfWorkMock.Setup(x => x.ReadingRepository).Returns(_readingRepositoryMock.Object);
           _readingRepositoryMock.Setup(x => x.GetById(reading.Id)).Returns(reading).Verifiable();
          //  _billRepositoryMock.Setup(x => x.GetUnitPrice((Month)previousMonth)).Returns(expectedUnitPrice).Verifiable();
            _billUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            // Act 
            _readingService.EditReading(reading);

            //Assert
            _billRepositoryMock.VerifyAll();
            _readingRepositoryMock.VerifyAll();
            _billUnitOfWorkMock.VerifyAll();
        }
        [Test, Category("Unit Test")]
        public void GetTotalUnitLocal_GiveMonth_GetTotalUnit()
        {
            // Arrange
            Month month = Month.April;
            int totalunit = 9;
            _billUnitOfWorkMock.Setup(x => x.ReadingRepository).Returns(_readingRepositoryMock.Object);
           // _readingRepositoryMock.Setup(x => x.GetTotalUnit(month)).Returns(totalunit);

            // Act 
         //   _readingService.GetTotalUnitLocal(month);

            //Assert
            _billRepositoryMock.VerifyAll();
            _readingRepositoryMock.VerifyAll();

        }
    }
}