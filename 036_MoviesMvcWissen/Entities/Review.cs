using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using _036_MoviesMvcWissen.Validations.FluentValidations;
using FluentValidation.Attributes;

namespace _036_MoviesMvcWissen.Entities
{
    [Validator(typeof(ReviewValidator))]
    public class Review
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        
        [StringLength(200)]
        public string Reviewer { get; set; }

        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; } 
    }
}