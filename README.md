# SResult
This is an "as simple as it could" result pattern library published to reuse in any clean software development. 

## Simplicity
It just a single class file and which is tested and reliable. And thats all I was needed on most  cases. But instead of copying that again and again for every project, now I can simply refer to this package and all good. 

## Structure
`Result<TGoodResult, TReasonForBad>`

Just a simple immutable object.  
You define what is result when good and how you want to deliver reason when bad.  
It has to have a **Result** when good.  
It has to have a **Reason** when bad.  
It impossible to be both or neither.  
A **Result** cannot be null when good.  
A **Reason** cannot be null when bad.  

Because of these checks it is very reliable and need not to write check and guard rails and null checks all over code.  
> It guarantees a result when good. And guarantees a reason when bad.

```csharp
public void Sample1()
{
    DummyHttpGet("http://somedomain.com")
    .WhenGood(() => { /* Do something on success */ })
    .WhenBad(() => { /* Do something on failure */ });
}

public void Sample2()
{
    DummyHttpGet("http://somedomain.com")
    .WhenGood((result) => { /* Do something on success with result */ })
    .WhenBad((reason) => { /* Do something on failure with failure */ });
}

private static Result<int, string> DummyHttpGet(string url)
{
    if (string.IsNullOrEmpty(url)) return "Url cannot be blank";
    return 200;
}
```
## Manually making result
```csharp
var result = Result.Good<int, int>(200);
```
Or
```csharp
var result = Result.Bad<int, int>(-1);
```
Note: Manual way is the only way when result and reason have same types.