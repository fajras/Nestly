using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nestly.Model.Entity
{
    public class ChatConversation
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(AppUser))]
        public long User1Id { get; set; }
        [ForeignKey(nameof(AppUser))]
        public long User2Id { get; set; }

        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public AppUser User1 { get; set; }
        [JsonIgnore]
        public AppUser User2 { get; set; }

        public ICollection<ChatMessage> Messages { get; set; }
    }


}
