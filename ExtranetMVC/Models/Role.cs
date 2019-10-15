﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetMVC.Models
{
    public class Role
    {

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
    }
}