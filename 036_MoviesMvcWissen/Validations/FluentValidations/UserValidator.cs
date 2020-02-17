using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _036_MoviesMvcWissen.Entities;
using FluentValidation;

namespace _036_MoviesMvcWissen.Validations.FluentValidations
{
    public class UserValidator: AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(e => e.UserName).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(e => e.Password).NotEmpty().MinimumLength(8).MaximumLength(25);
     
        }
    }
}