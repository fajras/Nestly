namespace Nestly.Model.DTOObjects
{
    public class CreateChatRoomDto
    {
        public string Name { get; set; } = default!;
        public bool IsPrivate { get; set; }
    }

    public class CreateChatMemberDto
    {
        public long RoomId { get; set; }
        public long UserId { get; set; }
    }

    public class CreateChatMessageDto
    {
        public long RoomId { get; set; }
        public long UserId { get; set; }
        public string Content { get; set; } = default!;
    }
}
