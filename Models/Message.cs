using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetlearner.Models;

public class Message
{
    [Key]
    [Column("message_id")]
    public int MessageId { get; set; }

    [Required]
    [Column("sender_id")]
    public Guid SenderId { get; set; }

    [ForeignKey("SenderId")]
    public User Sender { get; set; } = null!;

    [Required]
    [Column("receiver_id")]
    public Guid ReceiverId { get; set; }

    [ForeignKey("ReceiverId")]
    public User Receiver { get; set; } = null!;

    [Column("content")]
    public required string Content { get; set; }

    [Column("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
