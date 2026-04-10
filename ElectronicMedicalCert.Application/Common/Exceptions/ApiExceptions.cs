namespace ElectronicMedicalCert.Application.Common.Exceptions;

public abstract class ApiException(string message) : Exception(message);

public sealed class NotFoundException(string message) : ApiException(message);

public sealed class ForbiddenException(string message) : ApiException(message);

public sealed class ConflictException(string message) : ApiException(message);

