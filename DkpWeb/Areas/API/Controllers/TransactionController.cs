﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web.Http;
using Austin.DkpLib;

namespace DkpWeb.Areas.API.Controllers
{
    [DataContract]
    public class ApiTransaction
    {
        public ApiTransaction(Transaction t)
        {
            this.ID = t.ID;
            this.Amount = t.Amount;
            this.Created = t.Created;
            this.DebtorId = t.DebtorID;
            this.CreditorId = t.CreditorID;
            this.Description = t.Description;
        }

        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public int Amount { get; set; }
        [DataMember]
        public DateTime Created { get; set; }
        [DataMember]
        public int DebtorId { get; set; }
        [DataMember]
        public int CreditorId { get; set; }
        [DataMember, StringLength(50)]
        public string Description { get; set; }

        public Transaction ToTransaction(BillSplit bill)
        {
            var t = new Transaction();
            t.ID = Guid.NewGuid();
            t.Created = DateTime.Now;
            t.Description = this.Description;
            t.CreditorID = this.CreditorId;
            t.DebtorID = this.DebtorId;
            t.Amount = this.Amount;
            t.BillSplit = bill;
            return t;
        }
    }
    public class TransactionController : ApiController
    {
        // GET api/transaction
        public IEnumerable<ApiTransaction> Get()
        {
            using (var db = new DkpDataContext())
            {
                return db.Transactions.Select(t => new ApiTransaction(t)).ToList();
            }
        }

        // GET api/transaction/5
        public ApiTransaction Get(Guid id)
        {
            using (var db = new DkpDataContext())
            {
                var ret = db.Transactions.Where(t => t.ID == id).Single();
                return new ApiTransaction(ret);
            }
        }

        // POST api/transaction
        public Guid Post([FromBody]ApiTransaction value)
        {
            using (var db = new DkpDataContext())
            {
                var t = value.ToTransaction(null);
                db.Transactions.InsertOnSubmit(t);
                db.SubmitChanges();
                return t.ID;
            }
        }

        // PUT api/transaction/5
        public void Put(Guid id, [FromBody]ApiTransaction value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/transaction/5
        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
