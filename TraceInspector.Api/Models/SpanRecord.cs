namespace TraceInspector.Api.Models;

/// <summary>
/// Telemetry data transfer object (DTO) representing an individual unit of work in a distributed trace.
/// 
/// Purpose:
/// 1. Bridges the runtime engine (.NET's native System.Diagnostics.Activity) to a JSON-serializable contract.
/// 2. Adheres to W3C TraceContext standards (TraceId, SpanId, ParentSpanId) to model nested execution hierarchies (DAGs).
/// 3. Broadcasts over WebSockets to feed frontend event buffers, virtualized logs, and Gantt waterfall timelines.
/// </summary>
/// <param name="TraceId">Global 32-character hex ID identifying the entire end-to-end request lifecycle across all services.</param>
/// <param name="SpanId">16-character hex ID uniquely identifying this specific operation (e.g., ValidateCart, ProcessPayment).</param>
/// <param name="ParentSpanId">16-character hex ID of the enclosing caller span. If null, this span is the root boundary of the trace.</param>
/// <param name="Name">The operation name (e.g., "PlaceDeliveryOrder", "CalculateETA").</param>
/// <param name="ServiceName">Logical service or subsystem producing the telemetry (e.g., "DeliveryService.Core").</param>
/// <param name="StartTimeUnixMs">Wall-clock epoch millisecond timestamp used by the frontend to compute timeline offsets.</param>
/// <param name="DurationMs">Elapsed execution time in milliseconds; used to compute bar width in the Gantt waterfall chart.</param>
/// <param name="Status">W3C/OTel activity status indicator ("Unset", "Ok", or "Error").</param>
/// <param name="StatusDescription">Error description or exception message if Status is "Error", otherwise null.</param>
/// <param name="Tags">Arbitrary key-value metadata (e.g., order ID, customer ID, items count) for filtering and debugging.</param>
public sealed record SpanRecord(
    string TraceId,
    string SpanId,
    string? ParentSpanId,
    string Name,
    string ServiceName,
    long StartTimeUnixMs,
    double DurationMs,
    string Status,
    string? StatusDescription,
    IReadOnlyDictionary<string, string?> Tags
);

/// <summary>
/// Payload to initiate a simulated delivery order flow.
/// </summary>
public sealed record PlaceOrderRequest(
    string CustomerId,
    decimal TotalAmount,
    int ItemCount,
    bool SimulateFailure = false
);

/// <summary>
/// Synchronous API response returning the generated TraceId and elapsed time to the caller.
/// </summary>
public sealed record OrderPipelineResult(
    string OrderId,
    string TraceId,
    bool Success,
    string StatusMessage,
    double TotalElapsedMs
);