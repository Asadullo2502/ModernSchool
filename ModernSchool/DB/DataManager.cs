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

		public async Task<List<IndexesDataStatusViewModel>> IndexesStatus(int school_id)
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

		public async Task<MaxBallViewModel> MaxBallInOlympiads()
		{
			return await db.MaxBallViewModel.FromSqlRaw(@"
                with 
				db as (select *
				from Rates r
				where r.IndexId in (150,151,152)),
				db2 as (select *
				from Rates r
				where r.CriteriaId = 106
				),
				db3 as (select 
					distinct s.Id,
					s.NameUz,
					(10 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 79) +
					5 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 80) +
					3 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 81) +
	
					15 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 82) +
					13 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 83) +
					10 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 84) +
	
					20 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 85) +
					18 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 86) +
					15 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 87))/(select case when db2.ValueSchool <= 630 then 9 when db2.ValueSchool <= 945 then 18 else 27 end from db2 where db2.SchoolId = s.Id) MaxBall 

				from Schools s
				inner join db on db.SchoolId = s.Id)

				select MAX(db3.MaxBall) MaxBall
				from db3
            ").FirstAsync();
		}
	}
}
