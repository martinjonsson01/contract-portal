﻿using Domain.StatusUpdates;

namespace Application.StatusUpdates;

/// <summary>
/// Represents a way of storing and retrieving <see cref="StatusUpdate"/>s.
/// </summary>
public interface IStatusUpdateRepository
{
    /// <summary>
    /// Gets all status updates.
    /// </summary>
    IEnumerable<StatusUpdate> All { get; }

    /// <summary>
    /// Adds a new status update to store.
    /// </summary>
    /// <param name="statusUpdate">The new status update instance.</param>
    void Add(StatusUpdate statusUpdate);
}
