﻿using System;


namespace Application.Exceptions;


public sealed class NotFoundException : Exception
{
    public string EntityName { get; }
    public object Key { get; }


    public NotFoundException(string entityName, object key)
        : base($"Entity \"{entityName}\" with key ({key}) was not found.")
    {
        EntityName = entityName;
        Key = key;
    }
}
