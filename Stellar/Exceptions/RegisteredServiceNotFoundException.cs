﻿using System;
using System.Runtime.Serialization;

namespace Stellar.Exceptions;

public class RegisteredServiceNotFoundException : Exception
{
    public RegisteredServiceNotFoundException()
    {
    }

    public RegisteredServiceNotFoundException(string message)
        : base(message)
    {
    }

    public RegisteredServiceNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}