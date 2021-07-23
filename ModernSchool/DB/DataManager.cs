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

		public async Task<IEnumerable<IndexesDataStatusViewModel>> IndexesStatus(int school_id)
        {
            return await db.IndexesDataStatuses.FromSqlRaw(@"
                with 
				DB as (
					select 
						i.Id,

						isnull((select top (1) 1
						from Rates r
						where r.IndexId = i.Id and r.SchoolId = " + school_id + @"),0) +

						isnull((select top (1) 1
						from Rates r
						where r.IndexId = i2.Id and r.SchoolId = " + school_id + @"),0) +

						isnull((select top (1) 1
						from Rates r
						where r.IndexId = i3.Id and r.SchoolId = " + school_id + @"),0) +

						isnull((select top (1) 1
						from Rates r
						where r.IndexId = i4.Id and r.SchoolId = " + school_id + @"),0) +

						isnull((select top (1) 1
						from Rates r
						where r.IndexId = i5.Id and r.SchoolId = " + school_id + @"),0) as Selected

					from [Indexes] i
					left join [Indexes] i2 on i.Id = i2.ParentId
					left join [Indexes] i3 on i2.Id = i3.ParentId
					left join [Indexes] i4 on i3.Id = i4.ParentId
					left join [Indexes] i5 on i4.Id = i5.ParentId
				)

				select 
					i.Id, 
					(select sum(db.Selected) from DB db where db.Id = i.Id) selected,
					count(*) [count]
				from [Indexes] i
				left join [Indexes] i2 on i.Id = i2.ParentId
				left join [Indexes] i3 on i2.Id = i3.ParentId
				left join [Indexes] i4 on i3.Id = i4.ParentId
				left join [Indexes] i5 on i4.Id = i5.ParentId
				group by i.Id
            ").ToListAsync();
		}

	}
}
