﻿using Domain.StatusUpdates;

namespace Application.StatusUpdates;

/// <summary>
/// A way of interacting with and manipulating status updates.
/// </summary>
public interface IStatusUpdateService
{
    /// <summary>
    /// Fetches all of the status updates.
    /// </summary>
    /// <returns>All status updates.</returns>
    IEnumerable<StatusUpdate> FetchAll();

    /// <summary>
    /// Adds a new status update.
    /// </summary>
    /// <param name="statusUpdate">The new status update.</param>
    void Add(StatusUpdate statusUpdate);
}
