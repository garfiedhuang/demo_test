using Hkust.Common.Application.Contracts.Enums;

namespace Hkust.Common.Application.Contracts.Interfaces;

public interface IMessageTracker
{
    TrackerKind Kind { get; }
    Task<bool> HasProcessedAsync(long eventId, string trackerName);//是否处理
    Task MarkAsProcessedAsync(long eventId, string trackerName);//标记已处理 mark by garfield 20221020
}