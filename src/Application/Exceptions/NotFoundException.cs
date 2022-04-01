﻿using System;


namespace Application.Exceptions;


public sealed class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" with key ({key}) was not found.") { }
}
