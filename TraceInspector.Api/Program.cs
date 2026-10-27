using System.Diagnostics;

// 1. Turn on tracing (without this, StartActivity returns null)
ActivitySource.AddActivityListener(new ActivityListener
{
    ShouldListenTo = _ => true,
    Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData
});

var source = new ActivitySource("Test");

// 2. Start parent
using (var parent = source.StartActivity("ParentStep"))
{
    Console.WriteLine($"Parent SpanId:  {parent?.SpanId}");

    // 3. Start child inside the parent block
    using (var child = source.StartActivity("ChildStep"))
    {
        Console.WriteLine($"Child SpanId:   {child?.SpanId}");
        Console.WriteLine($"Child's Parent: {child?.ParentSpanId}");
        Console.WriteLine($"Same TraceId?:  {parent?.TraceId == child?.TraceId}");
    }
}