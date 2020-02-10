using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _036_MoviesMvcWissen.Entities;
using FluentValidation;

namespace _036_MoviesMvcWissen.Validations.FluentValidations
{
    public class DirectorValidator:AbstractValidator<Director>
    {
        public DirectorValidator()
        {
            RuleFor(e => e.Name).NotEmpty().MaximumLength(50);
            RuleFor(e => e.Surname).NotEmpty().Length(3, 50);
        }
    }
}