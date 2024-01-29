namespace CamelliaWiki.Backend.API.Components;

public enum ErrorCodes
{
    None = 0,
    InternalError = 49,
    UserNotFound = 81,
    NoAuthorizationHeader = 115,
    InvalidToken = 168,
    MissingSlug = 175,
    MissingContent = 185,
    NotFound = 192
}
