﻿using ItServiceApp.Models.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItServiceApp.Models.Entities
{
    public class Subscription: BaseEntity
    {
        public Guid SubscriptionTypeId { get; set; }
        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime EndDate { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }
        [NotMapped]//bu kolonu mapleme yani veri tabanına atma (eşleme)
        public bool IsActive => EndDate > DateTime.UtcNow; //enddate şimdiki zamandan büyükse true veriyor. Yani üyelik aktif mi değil mi bakıyoruz.
        [ForeignKey(nameof(SubscriptionTypeId))]
        public virtual SubscriptionType SubscriptionType { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

    }
}