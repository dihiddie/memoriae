using AutoMapper;
using Memoriae.BAL.Core.Models;
using DbPost = Memoriae.DAL.PostgreSQL.EF.Models.Post;
using DbTag = Memoriae.DAL.PostgreSQL.EF.Models.Tag;


namespace Memoriae.BAL.PostgreSQL.Mapper
{
    public class MemoriaeProfile : Profile
    {
        public MemoriaeProfile()
        {
            CreatePostMap();
            CreateTagProfile();
        }

        private void CreatePostMap()
        {
            CreateMap<Post, DbPost>().ReverseMap();
        }

        private void CreateTagProfile()
        {
            CreateMap<Tag, DbTag>().ReverseMap();
        }
    }
}
