using AutoMapper;
using bca.api.DTOs;
using PropertyManage.Data.Entities;
using PropertyManage.Domain.DTOs;


namespace bca.api.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            // User mapping
            CreateMap<ApplicationUser, UserDTO>()
                .ForMember(dest => dest.Roles,
                    opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)));

            CreateMap<UserCreateDTO, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            // Role mapping
            CreateMap<ApplicationRole, RoleDTO>()
                .ForMember(dest => dest.Permissions,
                    opt => opt.MapFrom(src => src.RolePermissions.Select(rp => rp.Permission.Name).ToList()));

            CreateMap<RoleCreateDTO, ApplicationRole>();

            // Permission mapping
            CreateMap<Permission, PermissionDTO>().ReverseMap();



        }
    }
}
