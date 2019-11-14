﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryCompanyAPIBackend.Models
{
    public class Street
    {
        [Key]
        public int Id { get; set; }
        public string StreetName { get; set; }

        public int RegionId { get; set; }
        public Region region { get; set; }

        public ICollection<ParcelLocker> parcelLockers { get; set; }
    }
}