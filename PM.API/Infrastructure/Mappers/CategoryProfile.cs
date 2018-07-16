using AutoMapper;
using PM.API.Models.Request;
using PM.Data.Models;
using PM.Domain.Models;

namespace PM.API.Infrastructure.Mappers
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<PostCategory, CategoryDTO>()
                .ForMember(dest => dest.Parent,
                           opts => opts.ResolveUsing(
                               src =>
                               {
                                   if (src.ParentID.HasValue)
                                   {
                                       return new Category
                                       {
                                           ID = src.ParentID.Value
                                       };
                                   }
                                   return null;
                               }
                          ));

            CreateMap<PutCategory, CategoryDTO>()
                .ForMember(dest => dest.Parent,
                           opts => opts.ResolveUsing(
                               src =>
                               {
                                   if (src.ParentID.HasValue)
                                   {
                                       return new Category
                                       {
                                           ID = src.ParentID.Value
                                       };
                                   }
                                   return null;
                               }
                          ));

            CreateMap<CategoryDTO, Category>()
                .ForMember(dest => dest.ParentID,
                           opts => opts.ResolveUsing(
                               src =>
                               {
                                   if (src.Parent != null)
                                   {
                                       return src.Parent.ID;
                                   }
                                   return (int?)null;
                               }
                          ))
                .ForMember(dest => dest.Parent,
                           opts => opts.Ignore());
        }
    }
}
