using AutoMapper;
using Contracts.Common;
using Contracts.DTOs;
using Contracts.Managers;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManager<User> _userManager;
        public UsersController(IUserManager<User> userManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Register a user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result<UserDto>), 200)]

        public async Task<IActionResult> Create([FromBody] CreateUserDto request)
        {
            var user = new User
            {
                Email = request.EmailAddress,
                PhoneNumber = request.PhoneNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.FirstName
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded) return ApiBad(errors: result.Errors.Select(x => x.Description).ToList(), message: result.Errors.First().Description);

            var createdUser = await _userManager.FindByEmailAsync(user.Email);

            return ApiOk(_mapper.Map<UserDto>(createdUser));
        }

        /// <summary>
        /// Get users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _unitOfWork.Users.GetAll().ToList();

            return ApiOk(_mapper.Map<List<UserDto>>(users));
        }
    }
}
