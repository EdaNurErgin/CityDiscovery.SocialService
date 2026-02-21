//using MediatR;
//using SocialService.Application.Interfaces;
//using SocialService.Domain.Entities;
//using CityDiscovery.SocialService.SocialServiceShared.Common.Events.Social;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace SocialService.Application.Commands.CreatePost
//{
//    // DİKKAT: Interface'iniz IRequestHandler<CreatePostCommand, Guid> ise dönüş tipini değiştirmeyin.
//    // Eğer PostDto dönüyorsa ona göre ayarlayın. Ben sizin son kodunuza göre Guid dönüşü ile devam ediyorum.
//    public class CreatePostHandler : IRequestHandler<CreatePostCommand, Guid>
//    {
//        private readonly IPostRepository _postRepository;
//        private readonly IVenueServiceClient _venueServiceClient;
//        private readonly IMediator _mediator;
//        private readonly IMessageBus _messageBus;
//        // Eğer ICurrentUserService kullanıyorsanız ekleyin, yoksa request.UserId'den devam edin.
//        // private readonly ICurrentUserService _currentUserService; 

//        public CreatePostHandler(
//            IPostRepository postRepository,
//            IVenueServiceClient venueServiceClient,
//            IMediator mediator,
//            IMessageBus messageBus)
//        {
//            _postRepository = postRepository;
//            _venueServiceClient = venueServiceClient;
//            _mediator = mediator;
//            _messageBus = messageBus;
//        }

//        public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
//        {
//            // 1. ADIM: MEKAN BİLGİLERİNİ ÇEK (Eskiden sadece Exists kontrolü vardı)
//            var venueDto = await _venueServiceClient.GetVenueAsync(request.VenueId);

//            if (venueDto == null)
//            {
//                throw new Exception($"Venue not found with id: {request.VenueId}");
//            }

//            // 2. Post Nesnesini Oluştur
//            var newPost = new Post
//            {
//                UserId = request.UserId,
//                VenueId = request.VenueId,
//                Content = request.Content,
//                CreatedDate = DateTime.UtcNow,

//                // --- İŞTE EKSİK OLAN KISIM BURASIYDI ---
//                // Mekan bilgilerini Venue Service'den alıp buraya kaydediyoruz
//                VenueName = venueDto.Name,

//                VenueImageUrl = venueDto.ProfilePictureUrl ?? "",
//                // ---------------------------------------

//                // Kullanıcı bilgileri (Authentication yapısına göre burayı dinamik yapmanız gerekebilir)
//                // Şimdilik isteği atan ID'ye göre bir servis çağrılabilir veya token'dan alınabilir.
//                // Eğer elinizde yoksa geçici değer atayabilirsiniz ama ideali ICurrentUserService kullanmaktır.
//                AuthorUserName = "TempUser", // TODO: IIdentityService veya Token'dan alınmalı
//                AuthorAvatarUrl = ""         // TODO: IIdentityService veya Token'dan alınmalı
//            };

//            // 3. Fotoğrafları Ekle
//            if (request.PhotoUrls != null && request.PhotoUrls.Any())
//            {
//                foreach (var url in request.PhotoUrls)
//                {
//                    newPost.Photos.Add(new PostPhoto
//                    {
//                        ImageUrl = url,
//                        Post = newPost // İlişkiyi kur
//                    });
//                }
//            }

//            // 4. Veritabanına Kaydet
//            await _postRepository.AddAsync(newPost);

//            // 5. Event Yayınla (Mevcut kodunuzu korudum)
//            var postCreatedEvent = new PostCreatedEvent(
//                postId: newPost.Id,
//                venueId: newPost.VenueId, // Constructora venueId eklenmesi iyi olur
//                userId: newPost.UserId,
//                content: newPost.Content,
//                createdDate: newPost.CreatedDate);

//            // ... (Event yayınlama kodlarınız aynı kalabilir) ...

//            return newPost.Id;
//        }
//    }
//}

using MediatR;
using SocialService.Application.Interfaces;
using SocialService.Domain.Entities;
using CityDiscovery.SocialService.SocialServiceShared.Common.Events.Social;


namespace SocialService.Application.Commands.CreatePost
{
    // DİKKAT: Interface'iniz IRequestHandler<CreatePostCommand, Guid> ise dönüş tipini değiştirmeyin.
    // Eğer PostDto dönüyorsa ona göre ayarlayın. Ben sizin son kodunuza göre Guid dönüşü ile devam ediyorum.
    public class CreatePostHandler : IRequestHandler<CreatePostCommand, Guid>
    {
        private readonly IPostRepository _postRepository;
        private readonly IVenueServiceClient _venueServiceClient;
        private readonly IIdentityServiceClient _identityServiceClient; // EKLENDİ
        private readonly IMediator _mediator;
        private readonly IMessageBus _messageBus;
        // Eğer ICurrentUserService kullanıyorsanız ekleyin, yoksa request.UserId'den devam edin.
        // private readonly ICurrentUserService _currentUserService; 

        public CreatePostHandler(
            IPostRepository postRepository,
            IVenueServiceClient venueServiceClient,
            IIdentityServiceClient identityServiceClient, // EKLENDİ
            IMediator mediator,
            IMessageBus messageBus)
        {
            _postRepository = postRepository;
            _venueServiceClient = venueServiceClient;
            _identityServiceClient = identityServiceClient; // EKLENDİ
            _mediator = mediator;
            _messageBus = messageBus;
        }

        public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            // 1. ADIM: MEKAN BİLGİLERİNİ ÇEK (Eskiden sadece Exists kontrolü vardı)
            var venueDto = await _venueServiceClient.GetVenueAsync(request.VenueId);

            if (venueDto == null)
            {
                throw new Exception($"Venue not found with id: {request.VenueId}");
            }

            // 2. ADIM: KULLANICI BİLGİLERİNİ ÇEK (EKLENDİ)
            var userDto = await _identityServiceClient.GetUserAsync(request.UserId);

            if (userDto == null)
            {
                throw new Exception($"User not found with id: {request.UserId}");
            }

            // 3. Post Nesnesini Oluştur
            var newPost = new Post
            {
                UserId = request.UserId,
                VenueId = request.VenueId,
                Content = request.Content,
                CreatedDate = DateTime.UtcNow,

                // --- İŞTE EKSİK OLAN KISIM BURASIYDI ---
                // Mekan bilgilerini Venue Service'den alıp buraya kaydediyoruz
                VenueName = venueDto.Name,

                VenueImageUrl = venueDto.ProfilePictureUrl ?? "",
                // ---------------------------------------

                // Kullanıcı bilgileri (Authentication yapısına göre burayı dinamik yapmanız gerekebilir)
                // Şimdilik isteği atan ID'ye göre bir servis çağrılabilir veya token'dan alınabilir.
                // Eğer elinizde yoksa geçici değer atayabilirsiniz ama ideali ICurrentUserService kullanmaktır.
                AuthorUserName = userDto.UserName, // DEĞİŞTİRİLDİ
                AuthorAvatarUrl = userDto.AvatarUrl ?? "" // DEĞİŞTİRİLDİ (Eğer IIdentityServiceClient'taki UserDto içerisinde fotoğraf alanı farklı isimlendirildiyse burayı ona göre güncelleyin)
            };

            // 4. Fotoğrafları Ekle
            if (request.PhotoUrls != null && request.PhotoUrls.Any())
            {
                foreach (var url in request.PhotoUrls)
                {
                    newPost.Photos.Add(new PostPhoto
                    {
                        ImageUrl = url,
                        Post = newPost // İlişkiyi kur
                    });
                }
            }

            // 5. Veritabanına Kaydet
            await _postRepository.AddAsync(newPost);

            // 6. Event Yayınla (Mevcut kodunuzu korudum)
            var postCreatedEvent = new PostCreatedEvent(
                postId: newPost.Id,
                venueId: newPost.VenueId, // Constructora venueId eklenmesi iyi olur
                userId: newPost.UserId,
                content: newPost.Content,
                createdDate: newPost.CreatedDate);

            // ... (Event yayınlama kodlarınız aynı kalabilir) ...

            return newPost.Id;
        }
    }
}