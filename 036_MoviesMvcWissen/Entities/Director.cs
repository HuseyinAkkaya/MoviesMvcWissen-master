﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using _036_MoviesMvcWissen.Validations.FluentValidations;
using FluentValidation.Attributes;

namespace _036_MoviesMvcWissen.Entities
{
    [Validator(typeof(DirectorValidator))]
    public class Director
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname { get; set; }

        public bool Retired { get; set; }

        public virtual List<MovieDirector> MovieDirectors { get; set; }
    }
}