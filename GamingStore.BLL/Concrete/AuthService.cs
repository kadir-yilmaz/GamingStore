using AutoMapper;
using GamingStore.BLL.Abstract;
using GamingStore.EL.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingStore.BLL.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public AuthService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        public IEnumerable<IdentityRole> Roles =>
            _roleManager.Roles;

        public async Task<IdentityResult> CreateUser(UserDtoForCreation userDto)
        {
            var user = _mapper.Map<IdentityUser>(userDto);
            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
                throw new Exception("User could not be created.");

            if (userDto.Roles.Count > 0)
            {
                var roleResult = await _userManager.AddToRolesAsync(user, userDto.Roles);
                if (!roleResult.Succeeded)
                    throw new Exception("System have problems with roles.");
            }

            return result;
        }

        public async Task<IdentityResult> DeleteOneUser(string id)
        {
            var user = await GetOneUserById(id);
            return await _userManager.DeleteAsync(user);
        }

        public IEnumerable<IdentityUser> GetAllUsers()
        {
            return _userManager.Users.ToList();
        }

        public async Task<IdentityUser> GetOneUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is not null)
                return user;

            throw new Exception("User could not be found.");
        }

        public async Task<UserDtoForUpdate> GetOneUserForUpdate(string id)
        {
            var user = await GetOneUserById(id); // id ile kullanıcıyı bul

            var roles = await _roleManager.Roles.ToListAsync(); // tüm roller
            var userRoles = await _userManager.GetRolesAsync(user); // kullanıcının rolleri

            var userDto = _mapper.Map<UserDtoForUpdate>(user);
            userDto.Roles = new HashSet<string>(roles.Select(r => r.Name));
            userDto.UserRoles = new HashSet<string>(userRoles);

            return userDto;
        }


        public async Task<IdentityResult> ResetPassword(ResetPasswordDto model)
        {
            var user = await GetOneUserById(model.UserName);
            await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user, model.Password);
            return result;
        }

        //public async Task Update(UserDtoForUpdate userDto)
        //{
        //    var user = await GetOneUser(userDto.UserName);
        //    user.PhoneNumber = userDto.PhoneNumber;
        //    user.Email = userDto.Email;
        //    var result = await _userManager.UpdateAsync(user);
        //    if (userDto.Roles.Count > 0)
        //    {
        //        var userRoles = await _userManager.GetRolesAsync(user);
        //        var r1 = await _userManager.RemoveFromRolesAsync(user, userRoles);
        //        var r2 = await _userManager.AddToRolesAsync(user, userDto.Roles);
        //    }
        //    return;
        //}

        public async Task Update(UserDtoForUpdate userDto)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id); // Id ile bul
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı");

            user.UserName = userDto.UserName; // Artık ismi de güncelleyebilirsin
            user.PhoneNumber = userDto.PhoneNumber;
            user.Email = userDto.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            if (userDto.Roles != null && userDto.Roles.Count > 0)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);
                await _userManager.AddToRolesAsync(user, userDto.Roles);
            }
        }



    }
}
