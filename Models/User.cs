namespace dotnetlearner.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


[Index(nameof(KeycloakId), IsUnique = true)]
public class User : AuditableEntity
{
    [Key]
    [Column("user_id")]
    public required Guid UserId { get; set; }

    [Column("keycloak_id")]
    public required Guid KeycloakId { get; set; }  // maps to JWT `sub`

    [Column("email")]
    public required string Email { get; set; }

    [Column("first_name")]
    public required string FirstName { get; set; }

    [Column("last_name")]
    public required string LastName { get; set; }

    [Column("profile_picture_url")]
    public string? ProfilePictureUrl { get; set; }

    [Column("theme_preference")]
    public string ThemePreference { get; set; } = "light";

}
