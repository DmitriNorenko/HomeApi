using System.Threading.Tasks;
using AutoMapper;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.Controllers
{
    /// <summary>
    /// Контроллер комнат
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomRepository _repository;
        private IMapper _mapper;
        
        public RoomsController(IRoomRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Изменение комнаты
        /// </summary>
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateRoom([FromBody] UpdateRoomRequest request )
        {
            var room = _repository.GetRoomByName(request.Name).Result;
            if (room is null) 
            {
                return StatusCode(400, "Такой комнаты нет!");
            }
            var newRoom = _mapper.Map<UpdateRoomRequest,UpdateRoomQuery>(request);
            await _repository.UpdateRoom(room, newRoom);
            return StatusCode(200, "Комната была изменена");
        }

        /// <summary>
        /// Вывести все комнаты
        /// </summary>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetRooms()
        {
            var Rooms =  await _repository.GetAllRooms();
            var ViewRooms = new GetRoomsResponse
            {
                RoomAmount = Rooms.Length,
                Rooms = _mapper.Map<Room[], RoomView[]>(Rooms)
            };
            return StatusCode(200,ViewRooms);
        }

        /// <summary>
        /// Добавление комнаты
        /// </summary>
        [HttpPost] 
        [Route("")] 
        public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
        {
            var existingRoom = await _repository.GetRoomByName(request.Name);
            if (existingRoom == null)
            {
                var newRoom = _mapper.Map<AddRoomRequest, Room>(request);
                await _repository.AddRoom(newRoom);
                return StatusCode(201, $"Комната {request.Name} добавлена!");
            }
            
            return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
        }
    }
}