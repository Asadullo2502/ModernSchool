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

		public async Task<List<IndexesDataStatusViewModel>> IndexesStatusValueSchool(int school_id, int year)
        {
            return await db.IndexesDataStatuses.FromSqlRaw(@"
                with 
				DB as (
					select 
						i.Id,

						isnull((select top (1) 1
						from Rates r
						where r.ValueSchool is not null and r.IndexId = i.Id and r.SchoolId = " + school_id + @" and r.Year = " + year + @"),0) +

						isnull((select top (1) 1
						from Rates r
						where r.ValueSchool is not null and r.IndexId = i2.Id and r.SchoolId = " + school_id + @" and r.Year = " + year + @"),0) +

						isnull((select top (1) 1
						from Rates r
						where r.ValueSchool is not null and r.IndexId = i3.Id and r.SchoolId = " + school_id + @" and r.Year = " + year + @"),0) +

						isnull((select top (1) 1
						from Rates r
						where r.ValueSchool is not null and r.IndexId = i4.Id and r.SchoolId = " + school_id + @" and r.Year = " + year + @"),0) +

						isnull((select top (1) 1
						from Rates r
						where r.ValueSchool is not null and r.IndexId = i5.Id and r.SchoolId = " + school_id + @" and r.Year = " + year + @"),0) as Selected

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

		public async Task<List<IndexesDataStatusViewModel>> IndexesStatusValueInspektor(int school_id, int year)
		{
			return await db.IndexesDataStatuses.FromSqlRaw(@"
                with 
				DB as (
					select 
						i.Id,

						isnull((select top (1) 1
						from Rates r
						where r.ValueInspektor is not null and r.IndexId = i.Id and r.SchoolId = " + school_id + @" and r.Year = " + year + @"),0) +

						isnull((select top (1) 1
						from Rates r
						where r.ValueInspektor is not null and r.IndexId = i2.Id and r.SchoolId = " + school_id + @" and r.Year = " + year + @"),0) +

						isnull((select top (1) 1
						from Rates r
						where r.ValueInspektor is not null and r.IndexId = i3.Id and r.SchoolId = " + school_id + @" and r.Year = " + year + @"),0) +

						isnull((select top (1) 1
						from Rates r
						where r.ValueInspektor is not null and r.IndexId = i4.Id and r.SchoolId = " + school_id + @" and r.Year = " + year + @"),0) +

						isnull((select top (1) 1
						from Rates r
						where r.ValueInspektor is not null and r.IndexId = i5.Id and r.SchoolId = " + school_id + @" and r.Year = " + year + @"),0) as Selected

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

		public double MaxBallInRepublicOlympiadsSchool(int year)
		{
			double result = 0;
			var data = db.MaxBallViewModel.FromSqlRaw(@"
                with 
				db as (
					select *
					from Rates r
					where r.IndexId in (150,151,152) and r.Year = " + year + @" and r.ValueSchool is not null
				),
				db2 as (
					select *
					from Rates r
					where r.CriteriaId = 106 and r.Year = " + year + @" and r.ValueSchool is not null
				),
				db3 as (
					select 
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
						15 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 87)) /
						(select case when db2.ValueSchool <= 630 then 9 when db2.ValueSchool <= 945 then 18 else 27 end from db2 where db2.SchoolId = s.Id) MaxBall 
					from Schools s
					inner join db on db.SchoolId = s.Id
				)

				select
					NEWID() Id,
					MAX(db3.MaxBall) MaxBall
				from db3
            ").ToList();
			result = (double)data.FirstOrDefault().MaxBall;
			return result;
		}

		public double MaxBallInInternationalOlympiadsSchool(int year)
		{
			double result = 0;
			var data = db.MaxBallViewModel.FromSqlRaw(@"
                with 
				db as (
					select *
					from Rates r
					where r.IndexId = 85 and r.Year = " + year + @" and r.ValueSchool is not null
				),
				db3 as (
					select 
						distinct s.Id,
						s.NameUz,
						5 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 89) +
						10 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 90) +
						20 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 91) +
						25 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 92) MaxBall
					from Schools s
					inner join db on db.SchoolId = s.Id
				)

				select
					NEWID() Id,
					MAX(db3.MaxBall) MaxBall
				from db3
            ").ToList();
			result = (double)data.FirstOrDefault().MaxBall;
			return result;
		}

		public double MaxBallInAbiturSchool(int year)
		{
			double result = 0;
			var data = db.MaxBallViewModel.FromSqlRaw(@"
                with 
				db as (
					select *
					from Rates r
					where r.IndexId = 86 and r.Year = " + year + @" and r.ValueSchool is not null
				),
				db3 as (
					select 
						distinct s.Id,
						s.NameUz,
						(select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 93) /
						(select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 94) MaxBall
					from Schools s
					inner join db on db.SchoolId = s.Id
				)

				select
					NEWID() Id,
					MAX(db3.MaxBall) MaxBall
				from db3
            ").ToList();
			result = (double)data.FirstOrDefault().MaxBall;
			return result;
		}

		public double MaxBallInBandlikSchool(int year)
		{
			double result = 0;
			var data = db.MaxBallViewModel.FromSqlRaw(@"
                with 
				db as (
					select *
					from Rates r
					where r.IndexId = 87 and r.Year = " + year + @" and r.ValueSchool is not null
				),
				db3 as (
					select 
						distinct s.Id,
						s.NameUz,
						(25 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 95) +
						20 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 96) +
						5 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 97) +
						30 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 98) +
						10 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 99) +
						5 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 100) +
						5 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 101)) /
						(select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 102) MaxBall 
					from Schools s
					inner join db on db.SchoolId = s.Id
				)

				select
					NEWID() Id,
					MAX(db3.MaxBall) MaxBall
				from db3
            ").ToList();
			result = (double)data.FirstOrDefault().MaxBall;
			return result;
		}

		public double MaxBallInRespublikaTanlovSchool(int year)
		{
			double result = 0;
			var data = db.MaxBallViewModel.FromSqlRaw(@"
                with 
				db as (
					select *
					from Rates r
					where r.IndexId = 88 and r.Year = " + year + @" and r.ValueSchool is not null
				),
				db3 as (
					select 
						distinct s.Id,
						s.NameUz,
						(5 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 103) +
						10 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 104) +
						15 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 105)) /
						(select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 106) MaxBall 
					from Schools s
					inner join db on db.SchoolId = s.Id
				)

				select
					NEWID() Id,
					MAX(db3.MaxBall) MaxBall
				from db3
            ").ToList();
			result = (double)data.FirstOrDefault().MaxBall;
			return result;
		}

		public double MaxBallInXalqaroTanlovSchool(int year)
		{
			double result = 0;
			var data = db.MaxBallViewModel.FromSqlRaw(@"
                with 
				db as (
					select *
					from Rates r
					where r.IndexId = 89 and r.Year = " + year + @" and r.ValueSchool is not null
				),
				db3 as (
					select 
						distinct s.Id,
						s.NameUz,
						(10 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 107) +
						20 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 108) +
						25 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 109)) /
						(select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 110) MaxBall 
					from Schools s
					inner join db on db.SchoolId = s.Id
				)

				select
					NEWID() Id,
					MAX(db3.MaxBall) MaxBall
				from db3
            ").ToList();
			result = (double)data.FirstOrDefault().MaxBall;
			return result;
		}

		public double MaxBallInKompTaminnotSchool(int year)
		{
			double result = 0;
			var data = db.MaxBallViewModel.FromSqlRaw(@"
                with 
				db as (
					select *
					from Rates r
					where r.IndexId = 101 and r.Year = " + year + @" and r.ValueSchool is not null
				),
				db3 as (
					select 
						distinct s.Id,
						s.NameUz,
						20 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 178) / (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 179) +
						5 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 180) / (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 181) +
						5 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 182) / (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 183) MaxBall 
					from Schools s
					inner join db on db.SchoolId = s.Id
				)

				select
					NEWID() Id,
					MAX(db3.MaxBall) MaxBall
				from db3
            ").ToList();
			result = (double)data.FirstOrDefault().MaxBall;
			return result;
		}

		public double MaxBallInChetTiliSchool(int year)
		{
			double result = 0;
			var data = db.MaxBallViewModel.FromSqlRaw(@"
                with 
				db as (
					select *
					from Rates r
					where r.IndexId = 106 and r.Year = " + year + @" and r.ValueSchool is not null
				),
				db3 as (
					select 
						distinct s.Id,
						s.NameUz,
						(30 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 161) +
						20 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 162) +
						10 * (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 163)) / (select db.ValueSchool from db where db.SchoolId = s.Id and db.CriteriaId = 164) MaxBall 
					from Schools s
					inner join db on db.SchoolId = s.Id
				)

				select
					NEWID() Id,
					MAX(db3.MaxBall) MaxBall
				from db3
            ").ToList();
			result = (double)data.FirstOrDefault().MaxBall;
			return result;
		}

		public double MaxBallInRepublicOlympiadsInspektor(int year)
		{
			double result = 0;
			var data = db.MaxBallViewModel.FromSqlRaw(@"
                with 
				db as (
					select *
					from Rates r
					where r.IndexId in (150,151,152) and r.Year = " + year + @" and r.ValueInspektor is not null
				),
				db2 as (
					select *
					from Rates r
					where r.CriteriaId = 106 and r.Year = " + year + @" and r.ValueInspektor is not null
				),
				db3 as (
					select 
						distinct s.Id,
						s.NameUz,
						(10 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 79) +
						5 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 80) +
						3 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 81) +
						15 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 82) +
						13 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 83) +
						10 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 84) +
						20 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 85) +
						18 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 86) +
						15 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 87)) /
						(select case when db2.ValueInspektor <= 630 then 9 when db2.ValueInspektor <= 945 then 18 else 27 end from db2 where db2.SchoolId = s.Id) MaxBall 
					from Schools s
					inner join db on db.SchoolId = s.Id
				)

				select
					NEWID() Id,
					MAX(db3.MaxBall) MaxBall
				from db3
            ").ToList();
			result = (double)data.FirstOrDefault().MaxBall;
			return result;
		}

		public double MaxBallInInternationalOlympiadsInspektor(int year)
		{
			double result = 0;
			var data = db.MaxBallViewModel.FromSqlRaw(@"
                with 
				db as (
					select *
					from Rates r
					where r.IndexId = 85 and r.Year = " + year + @" and r.ValueInspektor is not null
				),
				db3 as (
					select 
						distinct s.Id,
						s.NameUz,
						5 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 89) +
						10 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 90) +
						20 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 91) +
						25 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 92) MaxBall
					from Schools s
					inner join db on db.SchoolId = s.Id
				)

				select
					NEWID() Id,
					MAX(db3.MaxBall) MaxBall
				from db3
            ").ToList();
			result = (double)data.FirstOrDefault().MaxBall;
			return result;
		}

		public double MaxBallInAbiturInspektor(int year)
		{
			double result = 0;
			var data = db.MaxBallViewModel.FromSqlRaw(@"
                with 
				db as (
					select *
					from Rates r
					where r.IndexId = 86 and r.Year = " + year + @" and r.ValueInspektor is not null
				),
				db3 as (
					select 
						distinct s.Id,
						s.NameUz,
						(select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 93) /
						(select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 94) MaxBall
					from Schools s
					inner join db on db.SchoolId = s.Id
				)

				select
					NEWID() Id,
					MAX(db3.MaxBall) MaxBall
				from db3
            ").ToList();
			result = (double)data.FirstOrDefault().MaxBall;
			return result;
		}

		public double MaxBallInBandlikInspektor(int year)
		{
			double result = 0;
			var data = db.MaxBallViewModel.FromSqlRaw(@"
                with 
				db as (
					select *
					from Rates r
					where r.IndexId = 87 and r.Year = " + year + @" and r.ValueInspektor is not null
				),
				db3 as (
					select 
						distinct s.Id,
						s.NameUz,
						(25 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 95) +
						20 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 96) +
						5 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 97) +
						30 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 98) +
						10 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 99) +
						5 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 100) +
						5 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 101)) /
						(select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 102) MaxBall 
					from Schools s
					inner join db on db.SchoolId = s.Id
				)

				select
					NEWID() Id,
					MAX(db3.MaxBall) MaxBall
				from db3
            ").ToList();
			result = (double)data.FirstOrDefault().MaxBall;
			return result;
		}

		public double MaxBallInRespublikaTanlovInspektor(int year)
		{
			double result = 0;
			var data = db.MaxBallViewModel.FromSqlRaw(@"
                with 
				db as (
					select *
					from Rates r
					where r.IndexId = 88 and r.Year = " + year + @" and r.ValueInspektor is not null
				),
				db3 as (
					select 
						distinct s.Id,
						s.NameUz,
						(5 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 103) +
						10 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 104) +
						15 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 105)) /
						(select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 106) MaxBall 
					from Schools s
					inner join db on db.SchoolId = s.Id
				)

				select
					NEWID() Id,
					MAX(db3.MaxBall) MaxBall
				from db3
            ").ToList();
			result = (double)data.FirstOrDefault().MaxBall;
			return result;
		}

		public double MaxBallInXalqaroTanlovInspektor(int year)
		{
			double result = 0;
			var data = db.MaxBallViewModel.FromSqlRaw(@"
                with 
				db as (
					select *
					from Rates r
					where r.IndexId = 89 and r.Year = " + year + @" and r.ValueInspektor is not null
				),
				db3 as (
					select 
						distinct s.Id,
						s.NameUz,
						(10 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 107) +
						20 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 108) +
						25 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 109)) /
						(select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 110) MaxBall 
					from Schools s
					inner join db on db.SchoolId = s.Id
				)

				select
					NEWID() Id,
					MAX(db3.MaxBall) MaxBall
				from db3
            ").ToList();
			result = (double)data.FirstOrDefault().MaxBall;
			return result;
		}

		public double MaxBallInKompTaminnotInspektor(int year)
		{
			double result = 0;
			var data = db.MaxBallViewModel.FromSqlRaw(@"
                with 
				db as (
					select *
					from Rates r
					where r.IndexId = 101 and r.Year = " + year + @" and r.ValueInspektor is not null
				),
				db3 as (
					select 
						distinct s.Id,
						s.NameUz,
						20 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 178) / (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 179) +
						5 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 180) / (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 181) +
						5 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 182) / (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 183) MaxBall 
					from Schools s
					inner join db on db.SchoolId = s.Id
				)

				select
					NEWID() Id,
					MAX(db3.MaxBall) MaxBall
				from db3
            ").ToList();
			result = (double)data.FirstOrDefault().MaxBall;
			return result;
		}

		public double MaxBallInChetTiliInspektor(int year)
		{
			double result = 0;
			var data = db.MaxBallViewModel.FromSqlRaw(@"
                with 
				db as (
					select *
					from Rates r
					where r.IndexId = 106 and r.Year = " + year + @" and r.ValueInspektor is not null
				),
				db3 as (
					select 
						distinct s.Id,
						s.NameUz,
						(30 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 161) +
						20 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 162) +
						10 * (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 163)) / (select db.ValueInspektor from db where db.SchoolId = s.Id and db.CriteriaId = 164) MaxBall 
					from Schools s
					inner join db on db.SchoolId = s.Id
				)

				select
					NEWID() Id,
					MAX(db3.MaxBall) MaxBall
				from db3
            ").ToList();
			result = (double)data.FirstOrDefault().MaxBall;
			return result;
		}
	}
}
