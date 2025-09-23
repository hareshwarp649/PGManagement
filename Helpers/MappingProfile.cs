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

            // Country mappings
            // Input → Entity
            CreateMap<CountryInput, Country>()
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Name));

            CreateMap<Country, CountryDetails>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CountryName));

            // State mappings
            CreateMap<StateInput, State>()
                .ForMember(dest => dest.StateName, opt => opt.MapFrom(src => src.Name));

            CreateMap<State, StateDetails>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.StateName));

            //district mappings
            CreateMap<DistrictInput, District>();
            CreateMap<District, DistrictDetails>();

            //client mappings
            CreateMap<ClientCreateDTO, Client>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.UpdatedAt, opt => opt.Ignore())
            .ForMember(d => d.CreatedBy, opt => opt.Ignore())
            .ForMember(d => d.UpdatedBy, opt => opt.Ignore())
            .ForMember(d => d.Properties, opt => opt.Ignore())
            .ForMember(d => d.Subscriptions, opt => opt.Ignore())
            .ForMember(d => d.PaymentTransactions, opt => opt.Ignore());

            // For partial update - we will not rely on AutoMapper to decide nulls; we do manual checks in service.
            CreateMap<Client, ClientDTO>();

            //ClientSubscription Mappings
            CreateMap<ClientSubscription, ClientSubscriptionDTO>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.ClientName))
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.SubscriptionPlan.PlanName));

            CreateMap<CreateClientSubscriptionDTO, ClientSubscription>();
            CreateMap<UpdateClientSubscriptionDTO, ClientSubscription>();

            //SubscriptionPlan Mappings
            CreateMap<SubscriptionPlan, SubscriptionPlanDTO>();

            CreateMap<SubscriptionPlanCreateDTO, SubscriptionPlan>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
            // UpdateDTO -> Entity (Partial Update)
            CreateMap<SubscriptionPlanUpdateDTO, SubscriptionPlan>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));



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
