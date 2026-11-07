
using System.ComponentModel.DataAnnotations;

namespace Warteg.Api.DTOs;

public sealed record CategoryRequest(
    [Required(ErrorMessage = "CategoryCode is required")]
    [StringLength(50, ErrorMessage = "CategoryCode maximum 50 characters")]
    string CategoryCode,

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name maximum 100 characters")]
    string Name
);