using Database.Models.Account;
using Database.Models.Bank;
using Database.Models.Door;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Services
{
	public static class DoorService
	{
		public static List<DoorModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.Doors.ToList();
		}

		public static void Add(DoorModel model)
		{
			using var ctx = new Context();
			ctx.Doors.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(DoorModel model)
		{
			using var ctx = new Context();
			ctx.Doors.Remove(model);
			ctx.SaveChanges();
		}

		public static DoorModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Doors.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(DoorModel model)
		{
			using var ctx = new Context();
			ctx.Doors.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<DoorModel> models)
		{
			using var ctx = new Context();
			ctx.Doors.UpdateRange(models);
			ctx.SaveChanges();
		}

		// entites
		public static List<DoorEntityModel> GetEntities(int doorId)
		{
			using var ctx = new Context();
			return ctx.DoorEntities.Where(x => x.DoorId == doorId).ToList();
		}

		public static void AddEntity(DoorEntityModel model)
		{
			using var ctx = new Context();
			ctx.DoorEntities.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveEntity(DoorEntityModel model)
		{
			using var ctx = new Context();
			ctx.DoorEntities.Remove(model);
			ctx.SaveChanges();
		}

		public static DoorEntityModel? GetEntity(int id)
		{
			using var ctx = new Context();
			return ctx.DoorEntities.FirstOrDefault(x => x.Id == id);
		}

		// access
		public static List<DoorAccessModel> GetAccesses(int doorId)
		{
			using var ctx = new Context();
			return ctx.DoorAccesses.Where(x => x.DoorId == doorId).ToList();
		}

		public static void AddAccess(DoorAccessModel model)
		{
			using var ctx = new Context();
			ctx.DoorAccesses.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveAccess(DoorAccessModel model)
		{
			using var ctx = new Context();
			ctx.DoorAccesses.Remove(model);
			ctx.SaveChanges();
		}

		public static DoorAccessModel? GetAccess(int id)
		{
			using var ctx = new Context();
			return ctx.DoorAccesses.FirstOrDefault(x => x.Id == id);
		}
	}
}
