﻿namespace Application.Users;

/// <summary>
/// Used to send information about making a Contract a users favorite.
/// </summary>
public class FavoriteContractDto
{
    /// <summary>
    ///     Gets the id of the user.
    /// </summary>
    public Guid UserId { get; init; } = Guid.Empty;

    /// <summary>
    ///     Gets the id of the contract.
    /// </summary>
    public Guid ContractId { get; init; }

    /// <summary>
    ///     Gets a value indicating whether gets if the Contract should be a favorite or not.
    /// </summary>
    public bool IsFavorite { get; init; }
}
