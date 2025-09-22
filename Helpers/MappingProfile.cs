using AutoMapper;
using bca.api.DTOs;
using PropertyManage.Data.Entities;
using PropertyManage.Data.MasterEntities;
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

            CreateMap<PropertyType, PropertyTypeDTO>().ReverseMap();
            CreateMap<CreatePropertyTypeDTO, PropertyType>();


            // CreatePropertyDto -> Property
            CreateMap<PropertyCreateDTO, Propertiy>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())  // Auto-generate GUID in BaseEntity
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // handled in BaseEntity
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ClientId, opt => opt.Ignore()) // Set manually in Service
                .ForMember(dest => dest.Buildings, opt => opt.Ignore())
                .ForMember(dest => dest.Units, opt => opt.Ignore());

            // UpdatePropertyDto -> Property (Partial Update)
            CreateMap<PropertyUpdateDTO, Propertiy>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Property -> PropertyDto
            CreateMap<Propertiy, PropertyDTO>();

            // Unit mappings
            CreateMap<UnitCreateDTO, Unit>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Tenants, opt => opt.Ignore());

            CreateMap<UnitUpdateDTO, Unit>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Unit, UnitDTO>();

            //Tenant Mappings
            CreateMap<TenantCreateDTO, Tenant>();
            CreateMap<TenantUpdateDTO, Tenant>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Tenant, TenantDTO>();
        }
    }
}
