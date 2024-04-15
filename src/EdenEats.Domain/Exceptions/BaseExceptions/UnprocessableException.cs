﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Domain.Exceptions.BaseExceptions
{
    public abstract class UnprocessableException : Exception
    {
        public Dictionary<string, IEnumerable<string>> Errors { get; init; }

        public UnprocessableException(string key, IEnumerable<string> errors)
            : base(errors.FirstOrDefault())
        {
            Errors = new() { { key, errors } };
        }
    }
}