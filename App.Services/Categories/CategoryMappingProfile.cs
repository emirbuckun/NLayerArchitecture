using App.Repositories.Categories;
using App.Services.Categories.Create;
using App.Services.Categories.Response;
using App.Services.Categories.Update;
using AutoMapper;

namespace App.Services.Categories {
    public class CategoryMappingProfile : Profile {
        public CategoryMappingProfile() {
            CreateMap<CategoryResponse, Category>().ReverseMap();
            CreateMap<CategoryWithProductsResponse, Category>().ReverseMap();

            CreateMap<CreateCategoryRequest, Category>().ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Name.ToLowerInvariant()));

            CreateMap<UpdateCategoryRequest, Category>().ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Name.ToLowerInvariant()));
        }
    }
}