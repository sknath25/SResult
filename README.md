# SResult
This is an "as simple as it could" and a pure Result pattern library.

## Why another result pattern?
1. Just a boiler plate basic codes made as library for everyday use. No mvc or any bulk.
2. It has to have a **Result** when success.
3. It has to have a **Failure** when fail.
4. It impossible to be both or neither.

## Structure
1. `Result<TValue>` For simplest use. It returns built in `Reason` instance when failed or `TValue` when succeeded. 

## How to return a Result from a function in most basic way.
```csharp
    public static Result<int> TryParseToInteger(string value)
    {
        if (int.TryParse(value, out var val))
        {
            return val;
        }
        else
        {
            return "The value is not any integer.";
        }
    }
```
Hint: It will automatically transform to Result<int> from a failure string and an integer value on success.

## How to read a Result
In continuation of the example above
```csharp

var result = TryParseToInteger("101");
if(result.IsSuccess(out var value, out var failure)) Console.WriteLine($"The integer value is {value}");
else Console.WriteLine(failure.Message);

```
The usual approach is to throw an exception when trying to access the Value when it Failed.
But that brings additional concern on how and when the Result or the Value being accessed. 
Here we will not get Value or Result without checking success or failure. 
So, to me that should make code more descriptive. 

## Do something before returning
```csharp
public bool Handler()
{
    var result = TryParseToInteger("125")
    .OnSuccess((value) => { /* Do something on success with value */ })
    .OnFailure((failure) => { /* Do something on failure with failure */ });

    return result.IsSuccess();
}

```

## Failure Level 
Some time type isn't enough and a another layer required to feel the context. 
The caller gets a failure for a task in series. But for some operation, the nature of the failure could by confirm failure or may be failure. 
In this case caller has to make a choice if it would like to proceed or abort.
For this we have the additional layer in which we can set a Failure as *Warning* or *Fatal*. 
Use the ```AsWarning()``` or ```AsFatal()``` method to decorate Failure accordingly.
For example: 
```csharp
return Result.Unavailable("The payment service timed out. The final status of the payment is unknown.").AsWarning();
```
or
```csharp
return Result.Conflict("Please resolve the conflict before proceeding further!").AsFatal();
```
The Warning is the default.
And to check use ```IsWarning()``` or ```IsFatal()``` extension method of the Failure. 
For example:
```csharp
var result = await MakePaymentAsync(args);
if(result.IsFailure(out var failure))
{
    if(failure.Level.IsWarning())
    {
        // Continue making purchase..
    }
    else
    {
        // Abort. 
    }
}
```

## Built in Failure class
Initially this was not part of the library. 
But I end up making same basic Failure class for all projects most of the time.
It usually has a string message and a failure type. And that proved sufficient for most of the cases. 
So I thought to make a part of it. So most cases will be covered. 
And to use Custom type ```IFailure``` is there.

## Failure type (Only for builtin Failure class):
Sometime we want to send additional information with Failure message like the Failure type. 
It could be a Duplicate when saving to database or a Conflict when dealing with file or an Unauthorize when dealing with login etc. 
Eor example, ```Failure.Forbidden("..."), Failure.Conflict("...")``` etc. 
Note: This is a feature of Builtin Failure class only. Not a rocket science, we can have our own ways for out custom types. 
```csharp
    public async Task<Result<string>> Login(string username, string password)
    {
        // Code to authenticate.
        // authSuccess and authToken are imaginary variables here to explain the usages.

        if(authSuccess)
        {
           return authToken;
        }
        else
        {
            return Failure.Forbidden("Access denied! Invalid username or password.");
        }

        // Code to save the entity

        return 1;
    }
```
### Here is the list of types available: 
    Error (Default),
    NotFound,
    Unavailable,
    NoContent,
    Forbidden,
    Unauthorized,
    Invalid,
    InvalidArgument,
    Conflict,
    Duplicate,
    Inconsistent






