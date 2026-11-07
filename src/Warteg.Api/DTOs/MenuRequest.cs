using System.ComponentModel.DataAnnotations;

namespace Warteg.Api.DTOs;

public record MenuRequest
(
    [Required, StringLength(30)]
    string MenuCode, 

    [Required, StringLength(120)]
    string Name, 

    [Required]
    decimal Price,

    [Required, Range(0, int.MaxValue)] 
    int Stock, 

    [Required]
    int CategoryId
);