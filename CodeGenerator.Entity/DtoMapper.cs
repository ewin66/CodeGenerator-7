using AutoMapper;
using CodeGenerator.Entity.Dto;
using CodeGenerator.Entity.Scm;
using CodeGenerator.Entity.Base_SysManage;
using CodeGenerator.Entity.Crm;
using CodeGenerator.Entity.Oms;

namespace CodeGenerator.Entity
{
    public class DtoMapper : Profile
    {

        //protected override void Configure()
        public DtoMapper()
        {
            CreateMap<Base_User, Base_UserDto>();
            CreateMap<Base_UserDto, Base_User>();


            CreateMap<Scm_Product, Scm_ProductDto>();
            CreateMap<Scm_ProductDto, Scm_Product>();


            CreateMap<Crm_CusGroup, Crm_CusGroupDto>();
            CreateMap<Crm_CusGroupDto, Crm_CusGroup>();
            CreateMap<Crm_CusGroDetail, Crm_CusGroDetailDto>();
            CreateMap<Crm_CusGroDetailDto, Crm_CusGroDetail>();


            CreateMap<Oms_Order, Oms_OrderDto>();
            CreateMap<Oms_OrderDto, Oms_Order>();
            CreateMap<Oms_OrdDetail, Oms_OrdDetailDto>();
            CreateMap<Oms_OrdDetailDto, Oms_OrdDetail>();


            CreateMap<Crm_Customer, Crm_CustomerDto>();
            CreateMap<Crm_CustomerDto, Crm_Customer>();
        }

    }
}
