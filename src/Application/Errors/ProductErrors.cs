﻿namespace Application.Errors;

public static class ProductErrors
{
    public static Error AlreadyExists(string name) => Error.Create(ErrorType.AlreadyExists, $"Product with name {name} already exists.");

    public static Error NotFound => Error.Create(ErrorType.NotFound, "Product not found.");

    public static Error NotFoundById(int id) => Error.Create(ErrorType.NotFound, $"Product with id {id} not found.");
}