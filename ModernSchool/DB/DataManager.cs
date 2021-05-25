using Microsoft.EntityFrameworkCore;
using ModernSchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.DB
{
    public class DataManager
    {
        private DataContext db;

        public DataManager(DataContext context)
        {
            db = context;
        }

		

	}
}
