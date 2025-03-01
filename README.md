# SResult
This is an "as simple as it could" and a pure Result pattern library.

## Why another result pattern?
1. Just a boiler plate basic codes made as library for everyday use. No mvc or any bulk.
2. It has to have a **Result** when success.
3. It has to have a **Reason** when fail.
4. It impossible to be both or neither.

## Structure
You have two flavour available. 
1. `Result<TValue>` For simplest use. It returns built in `Reason` instance when failed. 
2. `Result<TValue, TReason>` is when you want to use your own Type to use for fails. 

## Making result of flavor `Result<TValue>`
```csharp
var result1 = Result.Success(200);
// Or
var result2 = Result.Fail<int>("Something is wrong");
```

## Making result of flavor `Result<TValue, TReason>`
```csharp
var result3 = Result.Success<int, int>(200);
// Or
var result4 = Result.Fail<int, int>(-1);

```

## When returning from method
```csharp
private static Result<int, string> DummyHttpGet(string url)
{
    if (string.IsNullOrEmpty(url)) 
        return Result.Fail<int, string>("Url cannot be blank");

    return Result.Success<int, string>(200);
}
```

## Mapping to return type automatically
```csharp
private static Result<int, string> DummyHttpGet(string url)
{
    if (string.IsNullOrEmpty(url)) return "Url cannot be blank";
    return 200;
}

// NOTE: Auto mapping won't work with same value and reason type. There is no way to distinguish. Make it manually for those cases. E.g.
private static Result<string, string> InnerMethod(string url)
{
    if (string.IsNullOrEmpty(url)) 
        return Result.Fail<string, string>("Parameter cannot be blank");

    return Result.Success<string, string>("All good");
}
```

## Do something before returning
```csharp
public bool Handler()
{
    var result = DummyHttpGet("http://somedomain.com")
    .OnSuccess((result) => { /* Do something on success with result */ })
    .OnFail((reason) => { /* Do something on failure with failure */ });

    return result.IsSuccess();
}

private static Result<int, string> DummyHttpGet(string url)
{
    if (string.IsNullOrEmpty(url)) return "Url cannot be blank";
    return 200;
}

```

## Built in Reason
Initially this was not part of the library. But I end up making basic reason class for all projects which is same 95% of the time.
it usually has a string message and a error type sort of thing. So I thought to make a part of it. So those 95% cases are covered. 
And free to use custom type when required. 

# Built in Reason examples
```csharp
var reason1 = new Reason("Something wrong", ReasonType.Forbidden);
var reason2 = new Reason("Something wrong");
Reason reason3 = "Something wrong";
var reason4 = Reason.Error("Something wrong");
var reason5 = Reason.InvalidArgument("Something wrong");
```
