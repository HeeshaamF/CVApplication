using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVApplication.Models;

using Microsoft.AspNetCore.Identity;

public class User : IdentityUser {
    public string Nom { get; set; } = string.Empty;
}