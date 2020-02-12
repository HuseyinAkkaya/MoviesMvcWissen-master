using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _036_MoviesMvcWissen.Entities;
using FluentValidation;

namespace _036_MoviesMvcWissen.Validations.FluentValidations
{
    public class ReviewValidator : AbstractValidator<Review>
    {
        public ReviewValidator()
        {
            RuleFor(e => e.Content).NotEmpty().Length(3, 200).WithMessage("Yorum girme zorunludur.(3-200 karakter).");
            RuleFor(e => e.MovieId).NotEmpty().WithMessage("Film seçilmelidir.").NotNull();
            RuleFor(e => e.Reviewer).NotEmpty().Length(3, 50);
            RuleFor(e => e.Rating).NotEmpty().GreaterThan(0);
        }


    }
}