﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TP_ASPNET.Models;

namespace TP_ASPNET.Models
{
    public class TodoContext : IdentityDbContext<User> {

        public TodoContext (DbContextOptions<TodoContext> options)
            : base(options)
        {
        }



        public DbSet<TP_ASPNET.Models.Todo> Todo { get; set; }

        public DbSet<TP_ASPNET.Models.User> User { get; set; }

        public DbSet<TP_ASPNET.Models.Task> Task { get; set; }

        public DbSet<TP_ASPNET.Models.Label> Label { get; set; }

        public DbSet<TP_ASPNET.Models.TodoLabel> TodoLabels { get; set; }
    }
}
