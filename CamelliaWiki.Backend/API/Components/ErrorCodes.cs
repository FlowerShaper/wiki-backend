namespace CamelliaWiki.Backend.API.Components;

public enum ErrorCodes
{
    None = 0,
    InternalError = 49,
    UserNotFound = 81,
    NoAuthorizationHeader = 115,
    InvalidToken = 168,
    MissingSlug = 175,
    NoPermission = 182,
    MissingContent = 185,
    InvalidParameter = 189,
    NotFound = 192,
    CommentNotFound = 198,
    MissingHeader = 201,
    MissingParameter = 204,
    MissingQuery = 210,
    InvalidQuery = 213,
}
