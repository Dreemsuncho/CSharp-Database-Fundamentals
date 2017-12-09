using AutoMapper;
using Instagraph.DataProcessor.DTO;
using Instagraph.Models;
using System.Linq;

namespace Instagraph.App
{
    public class InstagraphProfile : Profile
    {
        public InstagraphProfile()
        {
            // Imports
            CreateMap<UserImportDTO, User>()
                .ForMember(d => d.ProfilePicture, opt => opt.ResolveUsing((src, dest, prop, context) => context.Items["ProfilePicture"]));

            CreateMap<UserFollowerImportDTO, UserFollower>()
                .ForMember(d => d.User, opt => opt.ResolveUsing((src, dest, prop, context) => context.Items["User"]))
                .ForMember(d => d.UserId, opt => opt.ResolveUsing((src, dest, prop, context) => context.Items["UserId"]))
                .ForMember(d => d.Follower, opt => opt.ResolveUsing((src, dest, prop, context) => context.Items["Follower"]))
                .ForMember(d => d.FollowerId, opt => opt.ResolveUsing((src, dest, prop, context) => context.Items["FollowerId"]));

            CreateMap<PostImportDTO, Post>()
                .ForMember(d => d.Caption, opt => opt.MapFrom(pDTO => pDTO.Caption))
                .ForMember(d => d.User, opt => opt.ResolveUsing((src, dest, prop, context) => context.Items["User"]))
                .ForMember(d => d.Picture, opt => opt.ResolveUsing((src, dest, prop, context) => context.Items["Picture"]));

            CreateMap<CommentImportDTO, Comment>()
                .ForMember(d => d.Content, opt => opt.MapFrom(pDTO => pDTO.Content))
                .ForMember(d => d.User, opt => opt.ResolveUsing((src, dest, prop, context) => context.Items["User"]))
                .ForMember(d => d.UserId, opt => opt.ResolveUsing((src, dest, prop, context) => context.Items["UserId"]))
                .ForMember(d => d.Post, opt => opt.ResolveUsing((src, dest, prop, context) => context.Items["Post"]))
                .ForMember(d => d.PostId, opt => opt.ResolveUsing((src, dest, prop, context) => context.Items["PostId"]));

            // Exports
            CreateMap<Post, PostExportDTO>()
                .ForMember(dDTO => dDTO.Picture, src => src.MapFrom(p => p.Picture.Path))
                .ForMember(dDTO => dDTO.User, src => src.MapFrom(p => p.User.Username));

            CreateMap<User, UserExportDTO>()
                .ForMember(dDTO => dDTO.Followers, src => src.MapFrom(u => u.Followers.Count));

            CreateMap<User, UserCommentsOnPostExport>()
                .ForMember(dDTO => dDTO.MostComments, src => src.MapFrom(u => u.Posts.Count == 0 ? 0 : u.Posts.Max(p => p.Comments.Count)));
        }
    }
}
