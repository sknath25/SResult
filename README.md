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
But that brings additional concern on how the code is written and a proper exception handling.
But here we will have to check for Success or Failure to gain access to the Value or the Failure.
This is to reduce the concern discussed above.


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

## Built in Failure Type
Initially this was not part of the library. 
But I end up making same basic Failure class for all projects most of the time.
It usually has a string message and a failure type. And that proved sufficient for most of the cases. 
So I thought to make a part of it. So most cases will be covered. 
And to use Custom type ```IFailure``` is there.

