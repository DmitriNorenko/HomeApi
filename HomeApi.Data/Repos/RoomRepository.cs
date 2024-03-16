using System.Linq;
using System.Threading.Tasks;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using Microsoft.EntityFrameworkCore;

namespace HomeApi.Data.Repos
{
    /// <summary>
    /// Репозиторий для операций с объектами типа "Room" в базе
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeApiContext _context;

        public RoomRepository(HomeApiContext context)
        {
            _context = context;
        }

        /// <summary>
        ///  Изменить Комнату
        /// </summary>
        public async Task UpdateRoom(Room room, UpdateRoomQuery newRoom)
        {
            if (newRoom.Name is not null)
            {
                room.Name = newRoom.Name;
            }
            if (newRoom.Voltage != 0)
            {
                room.Voltage = newRoom.Voltage;
            }
            if (newRoom.Area != 0)
            {
                room.Area = newRoom.Area;
            }
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
            {
                _context.Rooms.Update(room);
            }
            await _context.SaveChangesAsync();
        }

        /// <summary>
        ///  Взять все комнаты из базы
        /// </summary>
        public async Task<Room[]> GetAllRooms()
        {
            return await _context.Rooms.ToArrayAsync();
        }

        /// <summary>
        ///  Найти комнату по имени
        /// </summary>
        public async Task<Room> GetRoomByName(string name)
        {
            return await _context.Rooms.Where(r => r.Name == name).FirstOrDefaultAsync();
        }

        /// <summary>
        ///  Добавить новую комнату
        /// </summary>
        public async Task AddRoom(Room room)
        {
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                await _context.Rooms.AddAsync(room);

            await _context.SaveChangesAsync();
        }
    }
}