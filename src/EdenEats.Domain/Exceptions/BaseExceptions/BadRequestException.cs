﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Domain.Exceptions.BaseExceptions
{
    public abstract class BadRequestException : Exception, IExceptionBase
    {
        public Dictionary<string, IEnumerable<string>> Errors { get; init; }

        public BadRequestException(string key, IEnumerable<string> errors)
            : base()
        {
            Errors = new() { { key, errors } };
        }
    }
}
