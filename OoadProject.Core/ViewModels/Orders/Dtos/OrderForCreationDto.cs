﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OoadProject.Core.ViewModels.Orders.Dtos
{
    public class OrderForCreationDto
    {
        public DateTime CreationTime { get; set; }
        public int UserId { get; set; }
        public int ProviderId { get; set; }
    }
}