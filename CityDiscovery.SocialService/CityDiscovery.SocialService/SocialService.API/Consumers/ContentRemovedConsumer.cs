using MassTransit;
using CityDiscovery.AdminNotificationService.Shared.Common.Events.AdminNotification;
using SocialService.Infrastructure.Data;


namespace CityDiscovery.SocialService.SocialService.API.Consumers
{
    public class ContentRemovedConsumer : IConsumer<ContentRemovedEvent>
    {
        private readonly SocialDbContext _context;

        public ContentRemovedConsumer(SocialDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<ContentRemovedEvent> context)
        {
            var message = context.Message;

            // DİKKAT: Admin panelinden gönderirken ContentType olarak "PostComment" veya "Comment" gönderdiğinden emin ol.
            // Buradaki string kontrolü (message.ContentType) ile Admin'in gönderdiği string aynı olmalı.
            if (message.ContentType == "Comment" || message.ContentType == "PostComment")
            {
                // DÜZELTME BURADA: _context.Comments yerine _context.PostComments
                var comment = await _context.PostComments.FindAsync(message.ContentId);

                if (comment != null)
                {
                    _context.PostComments.Remove(comment);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"[SocialService] Yorum silindi: {message.ContentId}");
                }
            }
            else if (message.ContentType == "Post")
            {
                var post = await _context.Posts.FindAsync(message.ContentId);

                if (post != null)
                {
                    _context.Posts.Remove(post);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"[SocialService] Post silindi: {message.ContentId}");
                }
            }
        }
    }
}