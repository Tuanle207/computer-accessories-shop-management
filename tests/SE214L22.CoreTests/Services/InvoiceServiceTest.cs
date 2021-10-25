﻿
using Autofac.Extras.Moq;
using Moq;
using NUnit.Framework;
using SE214L22.Contract.DTOs;
using SE214L22.Contract.Entities;
using SE214L22.Contract.Repositories;
using SE214L22.Contract.Util;
using SE214L22.Core.Services;
using SE214L22.CoreTests.Helpers;
using SE214L22.Shared.AppConsts;
using SE214L22.Shared.Dtos;
using System;
using System.Collections.Generic;

namespace SE214L22.CoreTests.Services
{
    [TestFixture]
    public class InvoiceServiceTest
    {

        [Test]
        public void GetReportByDay_Success_ReturnReportByDayDto()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var date = new DateTime(2020, 1, 1);
                var expected = new ReportByDayDto
                {
                    ReportDay = date,
                    TotalRevenue = 300_000,
                    Products = new List<ProductReportByDayDto>
                    {
                        new ProductReportByDayDto
                        {
                            Index = 1,
                            Id = 1,
                            CategoryName = "cat 1",
                            Name = "product 1",
                            Number = 1,
                            PriceOut = 100_000,
                            Total = 100_000
                        },
                        new ProductReportByDayDto
                        {
                            Index = 2,
                            Id = 2,
                            CategoryName = "cat 1",
                            Name = "product 2",
                            Number = 2,
                            PriceOut = 100_000,
                            Total = 300_000
                        }
                    }
                };

                mock.Mock<IInvoiceRepository>()
                    .Setup(x => x.GetReportByDay(It.IsAny<DateTime>()))
                    .Returns(expected);

                var invoiceService = mock.Create<InvoiceService>();
                var actual = invoiceService.GetReportByDay(date);

                Assert.IsInstanceOf<ReportByDayDto>(actual);
            }
        }

        [Test]
        public void GetReportByMonth_Success_ReportByMonthDto()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var month = new DateTime(2020, 1, 1);
                var expected = new ReportByMonthDto
                {
                    MonthReport = month,
                    TotalRevenue = 300_000,
                    TotalProfit = 100_000,
                    DayStatistics = new List<ItemReportByMonthDto>
                    {
                        new ItemReportByMonthDto
                        {
                            Index = 1,
                            Day = new DateTime(2000, 1, 2),
                            TotalRevenue = 200_000,
                            TotalProfit = 50_000
                        },
                        new ItemReportByMonthDto
                        {
                            Index = 2,
                            Day = new DateTime(2000, 1, 28),
                            TotalRevenue = 100_000,
                            TotalProfit = 50_000
                        }
                    }
                };

                mock.Mock<IInvoiceRepository>()
                    .Setup(x => x.GetReportByMonth(It.IsAny<DateTime>()))
                    .Returns(expected);

                var invoiceService = mock.Create<InvoiceService>();
                var actual = invoiceService.GetReportByMonth(month);

                Assert.IsInstanceOf<ReportByMonthDto>(actual);
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetRevenue_Success_ReturnRevenueDto(bool timeTypeIsUndefined)
        {
            using (var mock = AutoMock.GetLoose())
            {
                var date = new DateTime(2021, 1, 1);
                var expected = new RevenueDto
                {
                    Revenue = 300_000,
                    Sales = 3
                };
                var timeType = TimeType.Day;

                mock.Mock<IInvoiceRepository>()
                    .Setup(x => x.GetSalesByDay(It.IsAny<DateTime>()))
                    .Returns(3);

                mock.Mock<IInvoiceRepository>()
                    .Setup(x => x.GetRevenueByDay(It.IsAny<DateTime>()))
                    .Returns(300_000);

                var invoiceService = mock.Create<InvoiceService>();

                var actual = timeTypeIsUndefined
                    ? invoiceService.GetRevenue(date)
                    : invoiceService.GetRevenue(date, timeType);

                Assert.IsInstanceOf<RevenueDto>(actual);
            }
        }

        [Test]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void AddInvoice_Fail_ThrowException(bool invoiceIsNull, bool detailIsNull)
        {
            using (var mock = AutoMock.GetLoose())
            {

                var invoice = invoiceIsNull ? null : GetSampleInvoiceForCreation();
                var detail = detailIsNull ? null : GetSampleInvoiceDetails();
                var customer = DataSample.GetSampleCustomer();
                var savedInvoice = GetSampleInvoice();
                var invoiceProducts = GetSampleInvoiceProducts();
                User currentUser = DataSample.GetSampleCurrentUser();

                mock.Mock<ICustomerRepository>()
                   .Setup(x => x.GetCustomByPhoneNumber(It.IsAny<string>()))
                   .Returns(customer);

                mock.Mock<ISession>()
                    .Setup(x => x.CurrentUser)
                    .Returns(currentUser);

                mock.Mock<IInvoiceRepository>()
                    .Setup(x => x.Create(It.IsAny<Invoice>()))
                    .Returns(savedInvoice);

                mock.Mock<IInvoiceProductRepository>()
                    .Setup(x => x.Create(It.IsAny<InvoiceProduct>()))
                    .Returns(It.IsAny<InvoiceProduct>());

                mock.Mock<IProductRepository>()
                   .Setup(x => x.UpdateNumberById(It.IsAny<int>(), It.IsAny<int>()));

                var invoiceService = mock.Create<InvoiceService>();

                Assert.Throws<NullReferenceException>(() => invoiceService.AddInvoice(invoice, detail));
            }
        }

        [Test]
        public void AddInvoice_Success_NotThrowException()
        {
            using (var mock = AutoMock.GetLoose())
            {

                var invoice = GetSampleInvoiceForCreation();
                var detail = GetSampleInvoiceDetails();
                var customer = DataSample.GetSampleCustomer();
                var savedInvoice = GetSampleInvoice();
                var invoiceProducts = GetSampleInvoiceProducts();
                var currentUser = DataSample.GetSampleCurrentUser();

                mock.Mock<ICustomerRepository>()
                   .Setup(x => x.GetCustomByPhoneNumber(It.IsAny<string>()))
                   .Returns(customer);

                mock.Mock<ISession>()
                    .Setup(x => x.CurrentUser)
                    .Returns(currentUser);

                mock.Mock<IInvoiceRepository>()
                    .Setup(x => x.Create(It.IsAny<Invoice>()))
                    .Returns(savedInvoice);

                mock.Mock<IInvoiceProductRepository>()
                    .Setup(x => x.Create(It.IsAny<InvoiceProduct>()))
                    .Returns(It.IsAny<InvoiceProduct>());

                mock.Mock<IProductRepository>()
                   .Setup(x => x.UpdateNumberById(It.IsAny<int>(), It.IsAny<int>()));

                var invoiceService = mock.Create<InvoiceService>();

                Assert.DoesNotThrow(() => invoiceService.AddInvoice(invoice, detail));
            }
        }

        private InvoiceForCreationDto GetSampleInvoiceForCreation()
        {
            return new InvoiceForCreationDto
            {
                CustomerName = "Lê Anh Tuấn",
                CustomerLevel = "Thường",
                PhoneNumber = "01224578226",
                Total = 100_000,
                Discount = 0,
                Price = 100_000
            };
        }

        private Invoice GetSampleInvoice()
        {
            return new Invoice
            {
                Id = 1,
                CustomerId = 1,
                CreationTime = DateTime.Now,
                Total = 100_000,
                Discount = 0,
                Price = 100_000,
                UserId = 1
            };
        }

        private List<SelectingProductForSellDto> GetSampleInvoiceDetails()
        {
            return new List<SelectingProductForSellDto>
            {
                new SelectingProductForSellDto
                {
                    Id = 1,
                    Name = "Sản phẩm 1",
                    PriceOut = 50_000,
                    SelectedNumber = 1
                },
                new SelectingProductForSellDto
                {
                    Id = 2,
                    Name = "Sản phẩm 2",
                    PriceOut = 50_000,
                    SelectedNumber = 1
                }
            };
        }

        private List<InvoiceProduct> GetSampleInvoiceProducts()
        {
            return new List<InvoiceProduct>
            {
                new InvoiceProduct
                {
                    ProductId = 1,
                    InvoiceId = 1,
                    Number = 1
                },
                new InvoiceProduct
                {
                    ProductId = 2,
                    InvoiceId = 1,
                    Number = 1
                }
            };
        }
    }
}
