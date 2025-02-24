namespace webtap_agent.Exceptions;

public class NotFoundException(string ressourceType, string ressourceIdentifier)
     : Exception($"{ressourceType}: {ressourceIdentifier} not found")
{
}