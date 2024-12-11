# SResult
This is an "as simple as it could" result pattern library published to reuse in any clean software development. 

## Simplicity
It just a single class file and which is tested and reliable. And thats all I was needed on most  cases. But instead of copying that again and again for every project, now I can simply refer to this package and all good. 

## Structure
`Result<TSuccessType, TFailureReason>`

Just a simple immutable object.  
You define what is result for success and how you want to deliver reason for failure.  
It has to have a **Result** when success.  
It has to have a **Reason** for failure.  
It impossible to be both or neither.  
A **Result** cannot be null when success.  
A **Reason** cannot be null when failed.  

Because of these checks it is very reliable and need not to write check and guard rails and null checks all over code.  
> It guarantees a result when success. And guarantees a reason for failure.

```csharp
public void Sample1()
{
    DummyHttpGet("http://somedomain.com")
    .OnSuccess(() => { /* Do something on success */ })
    .OnFailure(() => { /* Do something on failure */ });
}

public void Sample2()
{
    DummyHttpGet("http://somedomain.com")
    .OnSuccess((result) => { /* Do something on success with success result */ })
    .OnFailure((reason) => { /* Do something on failure with failure reason */ });
}

private static Result<int, string> DummyHttpGet(string url)
{
    if (string.IsNullOrEmpty(url)) return "Url cannot be blank";
    return 200;
}
```
## Manually making result
```csharp
var result = Result<int, int>.CreateSuccessResult(200);
```
Or
```csharp
var result = Result<int, int>.CreateFailureReason(-1);
```
Note: Manual way is the only way when result and reason have same types.