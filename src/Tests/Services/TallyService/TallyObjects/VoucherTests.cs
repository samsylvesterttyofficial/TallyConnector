﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TallyConnector.Core.Models;

namespace Tests.Services.TallyService.TallyObjects;
internal class VoucherTests : BaseTallyServiceTest
{
    [Test]
    public async Task CheckGetAllVouchers()
    {
        RequestOptions requestOptions = new()
        {
            FromDate = new(2010, 4, 1),
            FetchList = Constants.Voucher.AccountingViewFetchList.All,
            Filters = new List<Filter>() { Constants.Voucher.Filters.ViewTypeFilters.AccountingVoucherFilter }
        };
        var ActngVchrs = await _tallyService.GetAllObjectsAsync<TCM.Voucher>(requestOptions);

        requestOptions.Filters[0] = Constants.Voucher.Filters.ViewTypeFilters.InvoiceVoucherFilter;

        ActngVchrs.AddRange(await _tallyService.GetAllObjectsAsync<TCM.Voucher>(requestOptions));

        requestOptions.Filters[0] = Constants.Voucher.Filters.ViewTypeFilters.InventoryVoucherFilter;
        // Voucher voucher = await _tallyService.GetVoucherAsync<Voucher>("52889497-5b6b-403d-8f83-224e3c7759b4-00001285", new() { LookupField = VoucherLookupField.GUID });
        //StockItem stockItem = await _tallyService.GetStockItemAsync<StockItem>("Floppy Drive");

        ActngVchrs.AddRange(await _tallyService.GetAllObjectsAsync<TCM.Voucher>(requestOptions));

        requestOptions.Filters[0] = Constants.Voucher.Filters.ViewTypeFilters.PayslipVoucherFilter;
        List<Voucher> collection = await _tallyService.GetAllObjectsAsync<TCM.Voucher>(requestOptions);
        ActngVchrs.AddRange(collection);

        requestOptions.Filters[0] = Constants.Voucher.Filters.ViewTypeFilters.MfgJournalVoucherFilter;
        ActngVchrs.AddRange(await _tallyService.GetAllObjectsAsync<TCM.Voucher>(requestOptions));

        //var value = ActngVchrs.GroupBy(c => c.VchType).ToDictionary(c => c.Key, c => c.ToList());

        //foreach (var v in value)
        //{
        //    string json = JsonSerializer.Serialize(v.Value, new JsonSerializerOptions() { WriteIndented = true,DefaultIgnoreCondition=JsonIgnoreCondition.WhenWritingDefault });
        //    await File.WriteAllTextAsync("json2/" + v.Key + ".json", json);
        //}

        Assert.That(ActngVchrs, Is.Not.Null);
        Assert.That(ActngVchrs.Count, Is.EqualTo(1107));
    }
    public class testobj
    {
        public string Vchtype { get; set; }
        public int Count { get; set; }
    }
    [Test]
    public async Task CreatePurchaseVoucher()
    {
        //string json = voucher1.GetJson();
        Voucher voucher = new()
        {
            VoucherType = "Purchase Order",
            View = VoucherViewType.InvoiceVoucherView,
            Date = new DateTime(2022, 04, 01),
            Reference = "frt",
            Ledgers = new()
            {
                new(){LedgerName="Test Party",Amount=400 },

            },
            InventoryAllocations = new()
            {
                new()
                {
                    StockItemName = "Mouse",
                    Rate = new(35,"Nos"),
                    ActualQuantity =new(10,"Nos"),
                    BilledQuantity =new(10,"Nos"),
                    Amount = -350,
                    BatchAllocations = new()
                    {
                        new(){OrderNo="frt",Amount=-350,BilledQuantity=new(10,"Nos")}
                    },
                    Ledgers = new(){ new() { LedgerName = "Purchase", Amount = -350 } }

                }
            }
        };
        TallyResult tallyResult = await _tallyService.PostVoucherAsync(voucher);
    }


    [Test]
    public async Task CheckGetAccountingVoucher()
    {

        Voucher voucher = await _tallyService.GetVoucherAsync<Voucher>("52889497-5b6b-403d-8f83-224e3c7759b4-000013d5",
                                                                       new()
                                                                       {
                                                                           LookupField = VoucherLookupField.GUID,
                                                                           FetchList = Constants.Voucher.AccountingViewFetchList.All
                                                                       });

    }
    [Test]
    public async Task CheckGetInvoiceVoucher()
    {

        Voucher voucher = await _tallyService.GetVoucherAsync<Voucher>("52889497-5b6b-403d-8f83-224e3c7759b4-000013db",
                                                                       new()
                                                                       {
                                                                           LookupField = VoucherLookupField.GUID,
                                                                           FetchList = Constants.Voucher.InvoiceViewFetchList.All
                                                                       });

        string json = voucher.GetJson();

    }


}
