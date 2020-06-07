using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using Core.Entities.Identity;
using Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using AutoMapper;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
        ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await _userManager.FindByEmailFromClaimsPrincipal(HttpContext.User);

        return Ok(
            new UserDto
            {
                Token = _tokenService.CreateToken(user),
                Email = user.Email,
                DisplayName = user.DisplayName
            }
        );
    }

    [HttpGet("emailexists")]
    public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    [HttpGet("address")]
    [Authorize]
    public async Task<ActionResult<AddressDto>> GetUerAddress()
    {
        var user = await _userManager.FindUserByClaimsPrincipalWithAddressAsync(HttpContext.User);

        return _mapper.Map<AddressDto>(user.Address);
    }
    
    [HttpPut("address")]
    [Authorize]
    public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto)
    {
        var user = await _userManager.FindUserByClaimsPrincipalWithAddressAsync(HttpContext.User);

        var address = _mapper.Map<Address>(addressDto);

        user.Address = address;

        var result = await _userManager.UpdateAsync(user);

        if(result.Succeeded) return Ok(_mapper.Map<Address,AddressDto>(user.Address));

        return BadRequest("Problem updating the user");
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null) return Unauthorized(new ApiResponse(401));

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

        return Ok(
            new UserDto
            {
                Token = _tokenService.CreateToken(user),
                Email = user.Email,
                DisplayName = user.DisplayName
            }
        );
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(UserRegisterDto userRegisterDto)
    {
        if(CheckEmailExistsAsync(userRegisterDto.Email).Result.Value){
            return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors = new []
            {"Email address is in use" }});
        }

        var user = new AppUser
        {
            Email = userRegisterDto.Email,
            UserName = userRegisterDto.Email,
            DisplayName = userRegisterDto.DisplayName
        };

        var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

        if (!result.Succeeded) return BadRequest(new ApiResponse(400));

        return Ok(
            new UserDto
            {
                Token = "Token for now",
                Email = user.Email,
                DisplayName = user.DisplayName
            }
        );
    }
}
}